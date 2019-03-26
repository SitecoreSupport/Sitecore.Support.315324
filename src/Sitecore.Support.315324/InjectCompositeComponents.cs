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
      var allDevices = layoutXml.Descendants("d").ToList();

      #region fix
      if (!allDevices.Any())
      {
        return new List<XElement>();
      }
      #endregion

      var filteredDevices = FilterDevicesByDeviceId(allDevices, contextDeviceId);
      if (!filteredDevices.Any() && Context.Device.FallbackDevice != null)
      {
        return GetDevices(layoutXml, Context.Device.FallbackDevice.ID);
      }
      return filteredDevices;
    }
  }
}