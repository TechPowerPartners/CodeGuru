using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("");

using CancellationTokenSource cts = new();
Dictionary<string, string> questions = new Dictionary<string, string>
{
   
    { "b1", @"
{
    ""question"": ""What is C#?"",
    ""options"": [""A programming language"", ""A type of coffee"", ""A type of chair"", ""A city""],
    ""correct"": 0
}" },
{ "b2", @"
{
    ""question"": ""What does the 'using' keyword do in C#?"",
    ""options"": [""It imports namespaces"", ""It defines a loop"", ""It creates a class"", ""It declares a variable""],
    ""correct"": 0
}" },
{ "b3", @"
{
    ""question"": ""What is the syntax for declaring a variable in C#?"",
    ""options"": [""var variableName;"", ""int variableName;"", ""string variableName;"", ""variableName = value;""],
    ""correct"": 0
}" },

    
    { "i1", @"
{
    ""question"": ""What is the difference between '==' and '===' in C#?"",
    ""options"": [""They are the same"", ""'==' compares value and '===' compares reference"", ""'===' compares value and '==' compares reference"", ""C# has no '===' operator""],
    ""correct"": 1
}" },
{ "i2", @"
{
    ""question"": ""What is the purpose of the 'async' and 'await' keywords in C#?"",
    ""options"": [""To perform asynchronous operations"", ""To create a loop"", ""To handle exceptions"", ""To define a class""],
    ""correct"": 0
}" },
{ "i3", @"
{
    ""question"": ""What does 'IEnumerable' represent in C#?"",
    ""options"": [""A collection of items"", ""A single item"", ""A method"", ""A property""],
    ""correct"": 0
}" },

    
    { "a1", @"
{
    ""question"": ""What is a delegate in C#?"",
    ""options"": [""A type that represents references to methods with a particular parameter list and return type"", ""A keyword to define a class"", ""A keyword to create an object"", ""A built-in function""],
    ""correct"": 0
}" },
{ "a2", @"
{
    ""question"": ""What is the purpose of 'using' statement in C#?"",
    ""options"": [""To ensure that IDisposable objects are correctly disposed after use"", ""To define namespaces"", ""To create a loop"", ""To handle exceptions""],
    ""correct"": 0
}" },
{ "a3", @"
{
    ""question"": ""What is the purpose of the 'readonly' keyword in C#?"",
    ""options"": [""To make a field immutable once it's assigned a value"", ""To define a constant"", ""To create an array"", ""To make a method readonly""],
    ""correct"": 0
}" }
};

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot


async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message)
        return;
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    if (messageText == "/test")
    {
        var randomKey = GetRandomKey(questions);
        var questionObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(questions[randomKey]);
        var options = ((JArray)questionObject["options"]).ToObject<string[]>();

        // Combine options into a single string
        string optionsString = string.Join("\n", options);

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: optionsString,
            cancellationToken: cancellationToken);
    }
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

static string GetRandomKey<T>(Dictionary<string,T> dictionary)
{
    var random = new Random();
    var keys = dictionary.Keys.ToList();
    return keys[random.Next(keys.Count)];
}