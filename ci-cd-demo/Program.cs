using Amazon.Runtime;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

// Add services to the container.
builder.Services.AddControllers();

// Add secrets manager for AWS
builder.Configuration.AddSecretsManager(configurator: config =>
{
    config.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}_");
    config.KeyGenerator = (_, name) => name
    .Replace($"{env}_{appName}_", string.Empty)
    .Replace("__", ":");
});

// Add the Secrets class to model secrets from AWS
builder.Services.AddOptions<SecretsList>()
    .BindConfiguration("Secrets");

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

var app = builder.Build();

// Get config service
ConfigurationManager config = builder.Configuration;

// Get environment name from appsettings file
string environment = config.GetValue<string>("Environment");


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => $"Welcome to running ASP.NET Core Minimal API on AWS Lambda! in the environment: {environment}.");

app.Run();
