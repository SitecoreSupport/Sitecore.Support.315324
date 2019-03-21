namespace Sitecore.Support.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition
{
  using Sitecore.Data;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  public class InjectCompositeComponents : Sitecore.XA.Feature.Composites.Pipelines.GetXmlBasedLayoutDefinition.InjectCompositeComponents
  {
    public override List<XElement> GetDevices(XElement layoutXml, ID contextDeviceId)
    {
      var devices = layoutXml.Descendants("d").ToList();

      #region Sitecore.Support.315324
      if (!devices.Any())
      {
        return new List<XElement>();
      }
      #endregion

      devices = FilterDevicesByDeviceId(devices, contextDeviceId);
      if (!devices.Any() && Context.Device.FallbackDevice != null)
      {
        return GetDevices(layoutXml, Context.Device.FallbackDevice.ID);
      }
      return devices;
    }
  }
}