using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using CountdownTimer.Models;

namespace CountdownTimer.Services
{
    public static class TimerService
    {
        private static DataStore _db = new DataStore();
        private static SettingsStore _setDb = new SettingsStore();
        private static AppSettings _set;

        static TimerService()
        {
            _set = _setDb.Read();
        }

        public static AppSettings GetSet()
        {
            return _set;
        }

        public static void SaveSet(AppSettings s)
        {
            _set = s;
            _setDb.Write(s);
        }

        public static List<HistoryRecord> GetHis()
        {
            return _db.GetAll();
        }

        public static void AddHis(HistoryRecord r)
        {
            if (_set.AutoSave) _db.Add(r);
        }

        public static void DelHis(int id)
        {
            _db.Delete(id);
        }

        public static void ClearHis()
        {
            _db.Clear();
        }

        public static List<HistoryRecord> SearchHis(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return GetHis();
            var all = GetHis();
            var ret = new List<HistoryRecord>();
            foreach (var r in all)
            {
                if (r.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                    ret.Add(r);
            }
            return ret;
        }

        public static void Beep()
        {
            SystemSounds.Beep.Play();
        }

        public static void PopMsg(string title, string msg)
        {
            Beep();
            System.Windows.Forms.MessageBox.Show(msg, title,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}

