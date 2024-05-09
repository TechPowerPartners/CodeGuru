using Eval.Api;
using Eval.Api.Exceptions;
using Eval.Api.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<CodeExecutor>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

var exitTimer = new Timer(_ => app.Lifetime.StopApplication(), null, Timeout.Infinite, Timeout.Infinite);
app.Use(async (context, next) =>
{
	if (app.Environment.IsProduction() && !context.Response.HasStarted && context.Request.Path.Equals("/eval", StringComparison.OrdinalIgnoreCase))
	{
		// terminate the process after 30 seconds whether the request is done or not (infinite loops, long sleeps, etc.)
		exitTimer.Change(TimeSpan.FromSeconds(30), Timeout.InfiniteTimeSpan);

		// terminate the process when the request finishes (assume the code is malicious. 
		// Should be hosted in a container/host system that destroys/re-builds the container)
		context.Response.OnCompleted(() =>
		{
			return Task.Run(async () =>
			{
				await Task.Delay(10000);
				app.Lifetime.StopApplication();
			});
		});
	}

	await next();
});

app.MapPost("/eval", async (
		[FromBody] Request? request,
		[FromServices] IValidator<Request> validator,
		[FromServices] CodeExecutor codeExecutor) =>
	{
		await ApiException.ValidateAsync(validator, request);

		try
		{
			var result = await codeExecutor.ExecuteCode(request!.Code!);
			var options = CodeExecutor.CreateJsonSerializerOptions();

			return Results.Json(new
			{
				Status = 200,
				Error = "",
				Result = result,
				SerializedReturnValue = result.ReturnValue is not null
					? JsonSerializer.Serialize(result.ReturnValue, options)
					: null
			});
		}
		catch (Exception)
		{
			return Results.Json(new
			{
				Status = 400,
				Error = "Таймаут, спам или неправильный запрос"
			});
		}
	})
	.WithName("Evaluate C# script")
	.WithOpenApi();

app.Run();

public class Request
{
	public string? Code { get; set; }

	public class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(b => b.Code).NotEmpty();
		}
	}
}