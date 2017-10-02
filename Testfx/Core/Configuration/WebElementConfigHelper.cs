using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TestFx.Core.Configuration
{
    /// <summary>
    /// This is a helper class for reading Web elements' data
    /// </summary>
    internal static class WebElementConfigHelper
    {
        public static XElement ReadConfigXElement(string environment)
        {
            var configFullPath = @"\Configuration\" + environment + @"\WebElements.config";
            XElement xElement = XElement.Load(configFullPath);

            return xElement;
        }

        public static Dictionary<string, string> ReadElementSettings(string section, string environment)
        {
            var currentFolder = Directory.GetCurrentDirectory();

            var configFullPath = currentFolder + @"\Configuration\" + environment + @"\WebElements.config";
            XElement xElement = XElement.Load(configFullPath);
            Dictionary<string, string> pageElements =
                xElement.Descendants(section)
                    .Elements("add")
                    .ToDictionary(child => (string)child.Attribute("key"), child => (string)child.Attribute("value"));
            return pageElements;
        }

        public static bool ContainsSection(string section, string environment)
        {
            var currentFolder = Directory.GetCurrentDirectory();

            var configFullPath = currentFolder + @"\Configuration\" + environment + @"\WebElements.config";
            XElement xElement = XElement.Load(configFullPath);

            return xElement.Descendants(section).Any();
        }
    }
}
