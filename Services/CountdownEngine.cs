using System;
using System.Timers;
using CountdownTimer.Models;

namespace CountdownTimer.Services
{
    public class CountdownEngine : IDisposable
    {
        private Timer _timer;
        private int _leftSec;
        private int _totalSec;
        private object _lock = new object();
        private bool _hasDispose = false;

        public event Action<int> OnTick;
        public event Action OnDone;
        public event Action OnPause;
        public event Action OnResume;

        public int LeftSec
        {
            get { lock (_lock) { return _leftSec; } }
            private set { lock (_lock) { _leftSec = value; } }
        }

        public int TotalSec
        {
            get { lock (_lock) { return _totalSec; } }
            private set { lock (_lock) { _totalSec = value; } }
        }

        public bool IsRun { get; private set; }
        public bool IsPause { get; private set; }
        public string MyName { get; set; }
        public TimerType MyType { get; set; }

        public string ShowTime
        {
            get
            {
                var ts = TimeSpan.FromSeconds(LeftSec);
                if (ts.TotalHours >= 1) return ts.ToString(@"hh\:mm\:ss");
                return ts.ToString(@"mm\:ss");
            }
        }

        public double GetPercent
        {
            get
            {
                if (TotalSec > 0) return (double)(TotalSec - LeftSec) / TotalSec;
                return 0;
            }
        }

        public CountdownEngine(int sec, string name)
        {
            TotalSec = sec;
            LeftSec = sec;
            MyName = name;
            MyType = TimerType.Single;
        }

        public void SetTime(int sec)
        {
            if (IsRun) return;
            TotalSec = Math.Max(1, sec);
            LeftSec = TotalSec;
        }

        public void Go()
        {
            if (LeftSec <= 0) return;
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimerGo;
            _timer.AutoReset = true;
            IsRun = true;
            IsPause = false;
            _timer.Start();
        }

        public void DoPause()
        {
            if (!IsRun || IsPause) return;
            IsPause = true;
            if (_timer != null) _timer.Stop();
            if (OnPause != null) OnPause();
        }

        public void DoResume()
        {
            if (!IsRun || !IsPause) return;
            IsPause = false;
            if (_timer != null) _timer.Start();
            if (OnResume != null) OnResume();
        }

        public void DoStop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
            IsRun = false;
            IsPause = false;
            LeftSec = TotalSec;
        }

        public void AddSec(int s)
        {
            lock (_lock)
            {
                _leftSec += s;
                _totalSec += s;
            }
        }

        private void OnTimerGo(object o, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                if (_leftSec > 0) _leftSec--;
            }
            if (OnTick != null) OnTick(LeftSec);
            if (LeftSec <= 0)
            {
                if (_timer != null) _timer.Stop();
                IsRun = false;
                if (OnDone != null) OnDone();
            }
        }

        public void Dispose()
        {
            if (_hasDispose) return;
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
            _hasDispose = true;
        }
    }
}
