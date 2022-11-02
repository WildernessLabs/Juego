using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Juego
{
    public static class Settings
    {
        static Dictionary<string, int> settings = new Dictionary<string, int>();

        static string filename = "/meadow0/settings.xml";

        public static void LoadSettings()
        {
            if(File.Exists(filename) == false)
            {
                return;
            }

            settings = XElement.Parse(File.ReadAllText(filename))
                .Elements()
                .ToDictionary(k => k.Name.ToString(), v => int.Parse(v.Value.ToString()));
        }

        public static void SaveSettings()
        {
             new XElement("root", settings.Select(kv => new XElement(kv.Key, kv.Value)))
                .Save(filename, SaveOptions.OmitDuplicateNamespaces);
        }

        public static int GetInteger(string key, int defaultValue = 0)
        {
            if(settings.ContainsKey(key))
            {
                return settings[key];
            }
            return defaultValue;
        }

        public static bool GetBoolean(string key, bool defaultValue = false)
        {
            return GetInteger(key, defaultValue ? 1 : 0) == 1 ? true : false;
        }

        public static byte GetByte(string key, byte defaultValue = 0)
        {
            return (byte)GetInteger(key, defaultValue);
        }

        public static void SetInteger(string key, int value)
        {
            settings.Add(key, value);
        }

        public static void SetByte(string key, byte value)
        {
            settings.Add(key, value);
        }

        public static void SetBoolean(string key, bool value)
        {
            settings.Add(key, value?1:0);
        }
    }
}