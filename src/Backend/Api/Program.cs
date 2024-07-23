using Api;
using Api.Abstractions;
using Api.Persistence;
using Api.Services;
using EasyNetQ;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
{
	opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
    });
});

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddDbContext<ApplicationDbContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDateOnlyTimeOnlyStringConverters();
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureSwagger();

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.RegisterEasyNetQ("host=rabbitmq;username=rabbitmq;password=rabbitmq", register =>
{
	register.EnableConsoleLogger();
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
