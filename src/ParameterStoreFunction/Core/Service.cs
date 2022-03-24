namespace ParameterStoreFunction.Core;

public class Service
{

  private readonly Settings settings;

  public Service(Settings settings)
  {
    this.settings = settings;
  }

  public string Execute()
  {
    return this.settings.Secret;
  }
}