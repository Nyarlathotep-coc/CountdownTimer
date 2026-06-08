using System;
using System.IO;
using System.Xml.Serialization;

namespace CountdownTimer.Models
{
    public class SettingsStore
    {
        private string _path;
        private XmlSerializer _xs;

        public SettingsStore(string path = null)
        {
            if (path == null)
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                dir = Path.Combine(dir, "MyTimer123");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                _path = Path.Combine(dir, "settings.xml");
            }
            else
            {
                _path = path;
            }
            _xs = new XmlSerializer(typeof(AppSettings));
        }

        public AppSettings Read()
        {
            if (!File.Exists(_path)) return new AppSettings();
            try
            {
                using (var fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
                    return (AppSettings)_xs.Deserialize(fs);
            }
            catch
            {
                return new AppSettings();
            }
        }

        public void Write(AppSettings s)
        {
            string dir = Path.GetDirectoryName(_path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            using (var fs = new FileStream(_path, FileMode.Create, FileAccess.Write))
                _xs.Serialize(fs, s);
        }
    }
}
