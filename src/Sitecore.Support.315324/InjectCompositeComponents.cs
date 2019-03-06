namespace Sitecore.Support.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition
{
  using Sitecore.Data;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml;
  using System.Xml.Linq;

  public class InjectCompositeComponents : Sitecore.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition.InjectCompositeComponents
  {
    public override IList<XElement> GetDevices(XElement layoutXml, ID contextDeviceId)
    {
      List<XElement> devices = layoutXml.Descendants("d").ToList();
      IList<XElement> list = FilterDevicesByDeviceId(devices, contextDeviceId);
      if (!list.Any() && Context.Device.FallbackDevice != null)
      {
        return GetDevices(layoutXml, Context.Device.FallbackDevice.ID);
      }
      return list;
    }
  }
}