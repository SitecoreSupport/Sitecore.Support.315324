using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Pipelines.Response.GetXmlBasedLayoutDefinition;
using Sitecore.XA.Feature.Composites;
using Sitecore.XA.Feature.Composites.Services;

namespace Sitecore.Support.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition
{
  public class SetOnPageEditingContext : InjectCompositeComponents
  {
    /// <inheritdoc />
    /// <summary>
    /// Starts the processing - extracts composite components from layout field and then expands them recursively
    /// </summary>
    public override void Process(GetXmlBasedLayoutDefinitionArgs args)
    {
      if (!ServiceLocator.ServiceProvider.GetService<ICompositesConfiguration>().OnPageEditingEnabled)
      {
        return;
      }
      var layoutXml = args.Result;

      if (layoutXml == null)
      {
        return;
      }

      var compositeComponents = GetCompositeComponents(layoutXml);

      if (!compositeComponents.Any())
      {
        return;
      }
      ServiceLocator.ServiceProvider.GetService<IOnPageEditingContextService>().XmlLayoutDefinition = args.Result;
    }
  }
}