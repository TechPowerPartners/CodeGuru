using Api;
using Api.Persistence;
using EasyNetQ;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
{
	opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddDbContext<ApplicationDbContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDateOnlyTimeOnlyStringConverters();
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureSwagger();

builder.Services.AddEndpointsApiExplorer();

builder.Services.RegisterEasyNetQ("host=rabbitmq;username=rabbitmq;password=rabbitmq", register =>
{
	register.EnableConsoleLogger();
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
