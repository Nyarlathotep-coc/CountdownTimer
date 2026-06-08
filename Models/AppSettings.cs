using System;

namespace CountdownTimer.Models
{
    public class AppSettings
    {
        public AppSettings()
        {
            DefMin = 25;
            PomoWork = 25;
            PomoBreak = 5;
            PomoLong = 15;
            PomoCycle = 4;
            PlaySound = true;
            TopWin = false;
            Theme = "Blue";
            AutoSave = true;
        }

        public int DefMin { get; set; }
        public int PomoWork { get; set; }
        public int PomoBreak { get; set; }
        public int PomoLong { get; set; }
        public int PomoCycle { get; set; }
        public bool PlaySound { get; set; }
        public bool TopWin { get; set; }
        public string Theme { get; set; }
        public bool AutoSave { get; set; }
    }
}
