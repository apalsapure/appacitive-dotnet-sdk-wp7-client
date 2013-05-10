using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Appacitive.Mobile.WindowsPhone7.Todo
{
    static class ConfigurationManager
    {
        private const string FILE_NAME = "AppConfiguration.xml";
        private static Dictionary<string, string> _items = new Dictionary<string, string>();

        static ConfigurationManager()
        {
            StreamResourceInfo xml = Application.GetResourceStream(new Uri(FILE_NAME, UriKind.Relative));
            var xElement = XElement.Load(xml.Stream);
            var settings = from setting in xElement.Descendants("add")
                           select new { Key = setting.Attribute("key").Value, Value = setting.Attribute("value").Value };

            foreach (var setting in settings) _items[setting.Key] = setting.Value;
        }
        public static string AppSettings(string key)
        {
            if (_items.ContainsKey(key)) return _items[key];
            return string.Empty;
        }
    }
}
