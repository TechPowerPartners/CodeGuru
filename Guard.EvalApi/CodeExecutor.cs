using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AngouriMath;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Scripting;
using TypeInfo = System.Reflection.TypeInfo;

namespace Guard.EvalApi;

public class CodeExecutor
{
    private static readonly ImmutableArray<DiagnosticAnalyzer> Analyzers =
        [new BlacklistedTypesAnalyzer()];
    
    public JsonSerializerOptions CreateJsonSerializerOptions()
    {
        var serializerOptions = new JsonSerializerOptions();
        serializerOptions.MaxDepth = 10240;
        serializerOptions.IncludeFields = true;
        serializerOptions.PropertyNameCaseInsensitive = true;
        serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        serializerOptions.NumberHandling |= JsonNumberHandling.AllowNamedFloatingPointLiterals;
        serializerOptions.Converters.Add(new TypeJsonConverter());
        serializerOptions.Converters.Add(new TypeInfoJsonConverter());
        serializerOptions.Converters.Add(new RuntimeTypeHandleJsonConverter());
        serializerOptions.Converters.Add(new TypeJsonConverterFactory());
        serializerOptions.Converters.Add(new AssemblyJsonConverter());
        serializerOptions.Converters.Add(new ModuleJsonConverter());
        serializerOptions.Converters.Add(new AssemblyJsonConverterFactory());
        serializerOptions.Converters.Add(new DirectoryInfoJsonConverter());
        serializerOptions.Converters.Add(new IntPtrJsonConverter());
        return serializerOptions;
    }
    
    public async Task<EvalResult> ExecuteCode(string code)
    {
        var sb = new StringBuilder();
        await using var textWr = new ConsoleLikeStringWriter(sb);

        var sw = Stopwatch.StartNew();

        var context = new ScriptExecutionContext(code);

        var eval = CSharpScript.Create(context.Code, context.Options, typeof(Globals));

        var compilation = eval.GetCompilation().WithAnalyzers(Analyzers);

        var compileResult = await compilation.GetAllDiagnosticsAsync();
        var compileErrors = compileResult.Where(a => a.Severity == DiagnosticSeverity.Error).ToImmutableArray();
        sw.Stop();

        var compileTime = sw.Elapsed;
        if (!compileErrors.IsEmpty)
        {
            return EvalResult.CreateErrorResult(code, sb.ToString(), sw.Elapsed, compileErrors);
        }

        var globals = new Globals();
        Globals.Random = new Random();
        Globals.Console = textWr;

        sw.Restart();
        ScriptState<object> result;

        try
        {
            result = await eval.RunAsync(globals, _ => true);
        }
        catch (CompilationErrorException ex)
        {
            return EvalResult.CreateErrorResult(code, sb.ToString(), sw.Elapsed, ex.Diagnostics);
        }

        sw.Stop();

        var evalResult = new EvalResult(code, result, sb.ToString(), sw.Elapsed, compileTime)
        {
            Code = code
        };
        //this hack is to test if we're about to send an object that can't be serialized back to the caller.
        //if the object can't be serialized, return a failure instead.
        try
        {
            JsonSerializer.Serialize(value: evalResult, options: CreateJsonSerializerOptions());
        }
        catch (Exception ex)
        {
            evalResult = new EvalResult
            {
                Code = code,
                CompileTime = compileTime,
                ConsoleOut = sb.ToString(),
                ExecutionTime = sw.Elapsed,
                Exception = $"An exception occurred when serializing the response: {ex.GetType().Name}: {ex.Message}",
                ExceptionType = ex.GetType().Name
            };
        }

        return evalResult;
    }
}


public class EvalResult
{
    public EvalResult()
    {
    }

    public EvalResult(string code, ScriptState<object> state, string consoleOut, TimeSpan executionTime,
        TimeSpan compileTime)
    {
        state = state ?? throw new ArgumentNullException(nameof(state));

        ReturnValue = state.ReturnValue;
        var type = state.ReturnValue?.GetType();

        if (type?.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEnumerator)) ?? false)
        {
            var genericParams = type.GetGenericArguments();

            if (genericParams.Length == 2)
            {
                type = typeof(List<>).MakeGenericType(genericParams[1]);

                ReturnValue = Activator.CreateInstance(type, ReturnValue);
            }
        }

        ExecutionTime = executionTime;
        CompileTime = compileTime;
        ConsoleOut = consoleOut;
        Code = code;
        Exception = state.Exception?.Message;
        ExceptionType = state.Exception?.GetType().Name;
    }

    public static EvalResult CreateErrorResult(string code, string consoleOut, TimeSpan compileTime,
        ImmutableArray<Diagnostic> compileErrors)
    {
        var ex = new CompilationErrorException(string.Join("\n", compileErrors.Select(a => a.GetMessage())),
            compileErrors);
        var errorResult = new EvalResult
        {
            Code = code,
            CompileTime = compileTime,
            ConsoleOut = consoleOut,
            Exception = ex.Message,
            ExceptionType = ex.GetType().Name,
            ExecutionTime = TimeSpan.FromMilliseconds(0),
            ReturnValue = null,
        };
        return errorResult;
    }

    public object? ReturnValue { get; set; }

    public string? Exception { get; set; }

    public string? ExceptionType { get; set; }

    public string? Code { get; set; }

    public string? ConsoleOut { get; set; }

    public TimeSpan ExecutionTime { get; set; }

    public TimeSpan CompileTime { get; set; }
}

public class ScriptExecutionContext(string code)
{
    private static readonly List<string> DefaultImports =
    [
        "System",
        "System.Collections",
        "System.Collections.Concurrent",
        "System.Collections.Immutable",
        "System.Collections.Generic",
        "System.Diagnostics",
        "System.Dynamic",
        "System.Security.Cryptography",
        "System.Globalization",
        "System.IO",
        "System.Linq",
        "System.Linq.Expressions",
        "System.Net",
        "System.Net.Http",
        "System.Numerics",
        "System.Reflection",
        "System.Reflection.Emit",
        "System.Runtime.CompilerServices",
        "System.Runtime.InteropServices",
        "System.Runtime.Intrinsics",
        "System.Runtime.Intrinsics.X86",
        "System.Text",
        "System.Text.RegularExpressions",
        "System.Threading",
        "System.Threading.Tasks",
        "System.Text.Json",
        "AngouriMath",
        "AngouriMath.Extensions"
    ];

    private static readonly List<Assembly> DefaultReferences =
    [
        typeof(Enumerable).GetTypeInfo().Assembly,
        typeof(HttpClient).GetTypeInfo().Assembly,
        typeof(List<>).GetTypeInfo().Assembly,
        typeof(string).GetTypeInfo().Assembly,
        typeof(ValueTuple).GetTypeInfo().Assembly,
        typeof(Globals).GetTypeInfo().Assembly,
        typeof(Memory<>).GetTypeInfo().Assembly,
        typeof(Entity).GetTypeInfo().Assembly
    ];

    public ScriptOptions Options =>
        ScriptOptions.Default
            .WithLanguageVersion(LanguageVersion.Preview)
            .WithOptimizationLevel(OptimizationLevel.Release)
            .WithImports(Imports)
            .WithReferences(References);

    public HashSet<Assembly> References { get; private set; } = [..DefaultReferences];

    public HashSet<string> Imports { get; private set; } = [..DefaultImports];

    public string Code { get; set; } = code;

    public void AddImport(string import)
    {
        if (string.IsNullOrEmpty(import))
        {
            return;
        }

        Imports.Add(import);
    }

    public bool TryAddReferenceAssembly(Assembly assembly)
    {
        if (assembly is null)
        {
            return false;
        }

        References.Add(assembly);
        return true;
    }
}

public class Globals
{
    public static Random Random { get; set; } = null!;
    public static ConsoleLikeStringWriter Console { get; internal set; } = null!;

    public void Cmd(string name, string args = "")
    {
        var psi = new ProcessStartInfo(name)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            Arguments = args
        };

        var p = Process.Start(psi);
        p!.WaitForExit();
        Console.WriteLine(p.StandardOutput.ReadToEnd());
        Console.WriteLine(p.StandardError.ReadToEnd());
    }
}

public sealed class ConsoleLikeStringWriter : StringWriter
{
    private ConsoleKeyInfo _readKeyValue = new('a', ConsoleKey.A, false, false, false);
    private string _readLineValue = $"line{Environment.NewLine}";


    public ConsoleColor BackgroundColor { get; set; } = Console.BackgroundColor;
    public int BufferHeight { get; set; }
    public int BufferWidth { get; set; }

    [SupportedOSPlatform("windows")] public bool CapsLock => Console.CapsLock;
    public int CursorLeft { get; set; }
    public int CursorSize { get; set; }
    public int CursorTop { get; set; }
    [SupportedOSPlatform("windows")] public bool CursorVisible { get; set; }
    public TextWriter Error { get; set; }
    public ConsoleColor ForegroundColor { get; set; } = Console.ForegroundColor;
    public TextWriter In { get; set; } = new StringWriter();
    public Encoding InputEncoding { get; set; } = Encoding.UTF8;
    public TextWriter Out { get; set; }
    public Encoding OutEncoding { get; set; }
    [SupportedOSPlatform("windows")] public string Title { get; set; } = "This is a Console, I promise!";
    public bool TreatControlCAsInput { get; set; } = Console.TreatControlCAsInput;
    public int WindowHeight { get; set; }
    public int WindowLeft { get; set; }
    public int WindowTop { get; set; }
    public int WindowWidth { get; set; }

    public ConsoleLikeStringWriter(StringBuilder builder) : base(builder)
    {
        Error = this;
        Out = this;
        OutEncoding = this.Encoding;
    }

    public void SetReadKeyValue(ConsoleKeyInfo value)
    {
        _readKeyValue = value;
    }

    public void SetReadLineValue(string line)
    {
        _readLineValue = $"{line}{Environment.NewLine}";
    }

    public void Beep()
    {
    }

    public void Beep(int a, int b)
    {
    }

    public void Clear()
    {
        GetStringBuilder().Clear();
    }

    public (int Left, int Top) GetCursorPosition()
    {
        return (CursorLeft, CursorTop);
    }

    public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft,
        int targetTop)
    {
    }

    public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft,
        int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
    {
    }

    public Stream OpenStandardError() => new MemoryStream();

    public Stream OpenStandardError(int bufferSize) => new MemoryStream(bufferSize);

    public Stream OpenStandardInput() => new MemoryStream();

    public Stream OpenStandardInput(int bufferSize) => new MemoryStream(bufferSize);

    public Stream OpenStandardOutput() => new MemoryStream();

    public Stream OpenStandardOutput(int bufferSize) => new MemoryStream(bufferSize);

    public int Read() => -1;

    public ConsoleKeyInfo ReadKey() => _readKeyValue;

    public ConsoleKeyInfo ReadKey(bool intercept)
    {
        if (intercept)
        {
            Write(_readKeyValue.KeyChar);
        }

        return ReadKey();
    }

    public string ReadLine() => _readLineValue;

    public void ResetColor()
    {
        ForegroundColor = Console.ForegroundColor;
        BackgroundColor = Console.BackgroundColor;
    }

    public void SetBufferSize(int width, int height)
    {
        BufferWidth = width;
        BufferHeight = height;
    }

    public void SetCursorPosition(int left, int top)
    {
        CursorLeft = left;
        CursorTop = top;
    }

    public void SetError(TextWriter wr)
    {
        Error = wr;
    }

    public void SetIn(TextWriter wr)
    {
        In = wr;
        InputEncoding = wr.Encoding;
    }

    public void SetOut(TextWriter wr)
    {
        Out = wr;
        OutEncoding = wr.Encoding;
    }

    public void SetWindowPosition(int left, int top)
    {
        WindowLeft = left;
        WindowTop = top;
    }

    public void SetWindowSize(int width, int height)
    {
        WindowWidth = width;
        WindowHeight = height;
    }
}

[DiagnosticAnalyzer(LanguageNames.CSharp)]
#pragma warning disable RS1036
public class BlacklistedTypesAnalyzer : DiagnosticAnalyzer
#pragma warning restore RS1036
{
    public const string DiagnosticId = "MOD0001";
    private static readonly LocalizableString Title = "Prohibited API";
    private static readonly LocalizableString MessageFormat = "Usage of this API is prohibited";
    private const string Category = "Discord";

    internal static readonly DiagnosticDescriptor Rule =
#pragma warning disable RS2008 // Enable analyzer release tracking
        new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true);
#pragma warning restore RS2008 // Enable analyzer release tracking

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();
        context.RegisterSemanticModelAction(AnalyzeSymbol);
    }

    private static void AnalyzeSymbol(SemanticModelAnalysisContext context)
    {
        var model = context.SemanticModel;

        var tree = model.SyntaxTree;
        var nodes = tree.GetRoot()
            .DescendantNodes(_ => true)
            .Where(n => n is IdentifierNameSyntax || n is ExpressionSyntax);

        foreach (var node in nodes)
        {
            var symbol = node is IdentifierNameSyntax
                ? model.GetSymbolInfo(node).Symbol
                : model.GetTypeInfo(node).Type;

            if (symbol is INamedTypeSymbol { Name: nameof(Environment) })
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation()));
            }
        }
    }
}

public class TypeJsonConverter : JsonConverter<Type>
{
    public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Type.GetType(reader.GetString()!)!;
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.AssemblyQualifiedName);
    }
}

public class RuntimeTypeJsonConverter : JsonConverter<object>
{
    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Type.GetType(reader.GetString()!)!;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        writer.WriteStringValue((value as Type)!.AssemblyQualifiedName);
    }
}

public class IntPtrJsonConverter : JsonConverter<IntPtr>
{
    public override IntPtr Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new IntPtr(reader.GetInt64());
    }

    public override void Write(Utf8JsonWriter writer, IntPtr value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.ToInt64());
    }
}

public class TypeInfoJsonConverter : JsonConverter<TypeInfo>
{
    public override TypeInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (TypeInfo)Type.GetType(reader.GetString()!)!;
    }

    public override void Write(Utf8JsonWriter writer, TypeInfo value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.AssemblyQualifiedName);
    }
}

public class AssemblyJsonConverter : JsonConverter<Assembly>
{
    public override Assembly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return default!;
    }

    public override void Write(Utf8JsonWriter writer, Assembly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.FullName);
    }
}

public class RuntimeAssemblyJsonConverter : JsonConverter<object>
{
    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return default!;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        writer.WriteStringValue((value as Assembly)!.FullName);
    }
}

public class ModuleJsonConverter : JsonConverter<Module>
{
    public override Module Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return default!;
    }

    public override void Write(Utf8JsonWriter writer, Module value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.FullyQualifiedName);
    }
}

public class RuntimeTypeHandleJsonConverter : JsonConverter<RuntimeTypeHandle>
{
    public override RuntimeTypeHandle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return default;
    }

    public override void Write(Utf8JsonWriter writer, RuntimeTypeHandle value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value.ToInt32());
    }
}

public class TypeJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert == Type.GetType("System.RuntimeType");

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new RuntimeTypeJsonConverter();
    }
}

public class AssemblyJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert == Type.GetType("System.Reflection.RuntimeAssembly");

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new RuntimeAssemblyJsonConverter();
    }
}

public class DirectoryInfoJsonConverter : JsonConverter<DirectoryInfo>
{
    public override DirectoryInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new DirectoryInfo(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, DirectoryInfo value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.FullName);
    }
}