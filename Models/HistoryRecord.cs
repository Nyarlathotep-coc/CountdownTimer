using System;
using System.Xml.Serialization;

namespace CountdownTimer.Models
{
    public enum TimerType
    {
        Single,
        Pomodoro,
        MultiTask
    }

    [Serializable]
    public class HistoryRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimerType Type { get; set; }
        public int TotalSec { get; set; }
        public int UsedSec { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsFinish { get; set; }
        public string Notes { get; set; }

        public string ShowTime
        {
            get { return TimeSpan.FromSeconds(TotalSec).ToString(@"hh\:mm\:ss"); }
        }

        public string ShowUsed
        {
            get { return TimeSpan.FromSeconds(UsedSec).ToString(@"hh\:mm\:ss"); }
        }

        public string ShowType
        {
            get
            {
                if (Type == TimerType.Single) return "单次";
                if (Type == TimerType.Pomodoro) return "番茄";
                if (Type == TimerType.MultiTask) return "多任务";
                return "未知";
            }
        }
    }
}
