using Amazon.Runtime;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

var envName = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

// Add services to the container.
builder.Services.AddControllers();

// Add secrets manager for AWS and filter to only include secrets for the environment and application
builder.Configuration.AddSecretsManager(configurator: config =>
{
    config.SecretFilter = entry => entry.Name.StartsWith($"{envName}/{appName}/");
    config.KeyGenerator = (_, name) => name
    .Replace($"{envName}/{appName}/", string.Empty)
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

app.MapGet("/", () => $"Welcome! to this demo running ASP.NET Core Minimal API on AWS Lambda in UAT! in the environment: {environment}.");

app.Run();
