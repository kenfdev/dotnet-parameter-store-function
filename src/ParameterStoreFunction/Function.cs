using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using ParameterStoreFunction.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ParameterStoreFunction;

public class Settings
{
  public string Secret { get; set; } = "";
}

public class Function
{

  private readonly Service service;

  public Function()
  {
    var stage = Environment.GetEnvironmentVariable("STAGE") ?? "test";

    var settings = this.CreateConfig(stage);

    this.service = new Service(settings);
  }

  public string FunctionHandler(object input, ILambdaContext context)
  {
    return this.service.Execute();
  }

  private Settings CreateConfig(string stage)
  {

    var builder = new ConfigurationBuilder();

    if (stage == "test")
    {
      builder.AddJsonFile(Path.Join(Directory.GetCurrentDirectory(), "appsettings.json"));
    }
    else
    {
      builder.AddSystemsManager("/MyAppA").Build();
    }

    var config = builder
        .Build()
        .GetSection("Config")
        .Get<Settings>();

    if (config == null) throw new ApplicationException("failed to load settings");

    return config;
  }
}
