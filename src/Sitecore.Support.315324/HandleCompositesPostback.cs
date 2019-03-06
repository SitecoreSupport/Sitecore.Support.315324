namespace Sitecore.Support.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition
{
  using Microsoft.Extensions.DependencyInjection;
  using Sitecore.Data;
  using Sitecore.DependencyInjection;
  using Sitecore.Mvc.Pipelines.Response.GetXmlBasedLayoutDefinition;
  using Sitecore.XA.Feature.Composites;
  using Sitecore.XA.Feature.Composites.Extensions;
  using Sitecore.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition;
  using Sitecore.XA.Feature.Composites.Services;
  using Sitecore.XA.Foundation.Abstractions.Configuration;
  using Sitecore.XA.Foundation.Presentation.Layout;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web;
  using System.Xml.Linq;

  public class HandleCompositesPostback : Sitecore.Support.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition.InjectCompositeComponents
  {
    public override void Process(GetXmlBasedLayoutDefinitionArgs args)
    {
      if (ServiceLocator.ServiceProvider.GetService<IConfiguration<CompositesConfiguration>>().GetConfiguration().OnPageEditingEnabled)
      {
        XElement result = args.Result;
        if (result != null)
        {
          IEnumerable<XElement> compositeComponents = GetCompositeComponents(result);
          if (compositeComponents.Any())
          {
            string experienceEditorAction = ServiceLocator.ServiceProvider.GetService<IOnPageEditingContextService>().ExperienceEditorAction;
            if (!(experienceEditorAction == "insert") && (!(experienceEditorAction == "activatecondition") || ActionExecutedForComposite(compositeComponents)))
            {
              foreach (DeviceModel item in new LayoutModel(args.Result.ToString()).Devices.DevicesCollection)
              {
                List<RenderingModel> renderings = item.Renderings.RenderingsCollection.ToList();
                IList<RenderingModel> list = GetStaleInjectedRenderings(renderings);
                bool flag = experienceEditorAction == "preview";
                bool flag2 = experienceEditorAction == "activatecondition" && ActionExecutedForComposite(compositeComponents);
                if ((flag | flag2) && !list.Any((RenderingModel model) => renderings.Any(delegate (RenderingModel r)
                {
                  if (r.Placeholder == model.Placeholder)
                  {
                    return r.XmlNode.Attributes?["cmps"] != null;
                  }
                  return false;
                })))
                {
                  list = new List<RenderingModel>();
                }
                if (list.Any())
                {
                  foreach (RenderingModel item2 in list)
                  {
                    (from x in args.Result.Descendants()
                     where x.Name == (XName)"r"
                     where x.Attribute(XName.Get("uid"))?.Value == item2.UniqueId.ToString()
                     where x.Attribute(XName.Get("ph"))?.Value == item2.Placeholder
                     select x).Remove();
                  }
                  ServiceLocator.ServiceProvider.GetService<IOnPageEditingContextService>().ExperienceEditorAction = "conditionchange";
                }
              }
            }
          }
        }
      }
    }

    protected virtual bool ActionExecutedForComposite(IEnumerable<XElement> compositeComponents)
    {
      Guid result;
      if (Guid.TryParse(HttpContext.Current.Request.QueryString["sc_ruid"], out result))
      {
        ID renderinUniqueId = new ID(result);
        return compositeComponents.Any((XElement e) => MatchesID(e, renderinUniqueId));
      }
      return false;
    }

    protected virtual bool MatchesID(XElement e, ID renderinUniqueId)
    {
      ID result;
      if (ID.TryParse(e.Attribute("uid").Value, out result))
      {
        return result.Equals(renderinUniqueId);
      }
      return false;
    }

    protected virtual IList<RenderingModel> GetStaleInjectedRenderings(IList<RenderingModel> renderings)
    {
      List<RenderingModel> list = new List<RenderingModel>();
      foreach (RenderingModel item in renderings.ToList())
      {
        if (item.XmlNode.Attributes?["cmps"] == null && new Placeholder(item.Placeholder).GetCompositePlaceholderName() != null)
        {
          list.Add(item);
        }
      }
      return list;
    }
  }
}