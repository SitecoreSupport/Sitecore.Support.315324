namespace Sitecore.Support.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition
{
  using Microsoft.Extensions.DependencyInjection;
  using Sitecore.DependencyInjection;
  using Sitecore.Mvc.Pipelines.Response.GetXmlBasedLayoutDefinition;
  using Sitecore.XA.Feature.Composites;
  using Sitecore.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition;
  using Sitecore.XA.Feature.Composites.Services;
  using Sitecore.XA.Foundation.Abstractions.Configuration;
  using System.Linq;
  using System.Xml.Linq;

  public class SetOnPageEditingContext : Sitecore.Support.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition.InjectCompositeComponents
  {
    public override void Process(GetXmlBasedLayoutDefinitionArgs args)
    {
      if (ServiceLocator.ServiceProvider.GetService<IConfiguration<CompositesConfiguration>>().GetConfiguration().OnPageEditingEnabled)
      {
        XElement result = args.Result;
        if (result != null && GetCompositeComponents(result).Any())
        {
          ServiceLocator.ServiceProvider.GetService<IOnPageEditingContextService>().XmlLayoutDefinition = args.Result;
        }
      }
    }
  }
}