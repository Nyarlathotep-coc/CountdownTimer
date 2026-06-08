using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CountdownTimer.Models
{
    public class DataStore
    {
        private string _path;
        private XmlSerializer _xs;

        public DataStore(string path = null)
        {
            if (path == null)
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                dir = Path.Combine(dir, "MyTimer123");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                _path = Path.Combine(dir, "history.xml");
            }
            else
            {
                _path = path;
            }
            _xs = new XmlSerializer(typeof(List<HistoryRecord>));
        }

        public List<HistoryRecord> GetAll()
        {
            if (!File.Exists(_path)) return new List<HistoryRecord>();
            try
            {
                using (var fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
                {
                    return (List<HistoryRecord>)_xs.Deserialize(fs);
                }
            }
            catch
            {
                return new List<HistoryRecord>();
            }
        }

        public void SaveAll(List<HistoryRecord> list)
        {
            string dir = Path.GetDirectoryName(_path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            lock (_xs)
            {
                using (var fs = new FileStream(_path, FileMode.Create, FileAccess.Write))
                {
                    _xs.Serialize(fs, list);
                }
            }
        }

        public void Add(HistoryRecord r)
        {
            var list = GetAll();
            if (list.Count > 0) r.Id = list.Max(x => x.Id) + 1;
            else r.Id = 1;
            list.Add(r);
            SaveAll(list);
        }

        public void Delete(int id)
        {
            var list = GetAll();
            list.RemoveAll(x => x.Id == id);
            SaveAll(list);
        }

        public void Clear()
        {
            SaveAll(new List<HistoryRecord>());
        }
    }
}
