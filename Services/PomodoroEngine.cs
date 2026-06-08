using System;
using CountdownTimer.Models;

namespace CountdownTimer.Services
{
    public enum PomoState
    {
        Work,
        Break,
        LongBreak
    }

    public class PomodoroEngine : IDisposable
    {
        private CountdownEngine _work;
        private CountdownEngine _rest;
        private PomoState _now = PomoState.Work;
        private int _doneNum = 0;
        private bool _hasDispose = false;

        public int WorkMin { get; set; }
        public int RestMin { get; set; }
        public int LongMin { get; set; }
        public int CycleNum { get; set; }

        public PomoState NowState { get { return _now; } }
        public int DoneNum { get { return _doneNum; } }
        public CountdownEngine WorkTimer { get { return _work; } }
        public CountdownEngine RestTimer { get { return _rest; } }

        public event Action<PomoState> OnChange;
        public event Action<int> OnTick;

        public PomodoroEngine()
        {
            WorkMin = 25;
            RestMin = 5;
            LongMin = 15;
            CycleNum = 4;

            _work = new CountdownEngine(0, "工作时间");
            _rest = new CountdownEngine(0, "休息时间");

            _work.OnDone += WhenWorkDone;
            _rest.OnDone += WhenRestDone;
            _work.OnTick += (s) => { if (OnTick != null) OnTick(s); };
            _rest.OnTick += (s) => { if (OnTick != null) OnTick(s); };
        }

        public void Go()
        {
            _doneNum = 0;
            _now = PomoState.Work;
            _work.SetTime(WorkMin * 60);
            _work.Go();
            if (OnChange != null) OnChange(_now);
        }

        public void DoPause()
        {
            if (_work.IsRun && !_work.IsPause) _work.DoPause();
            else if (_rest.IsRun && !_rest.IsPause) _rest.DoPause();
        }

        public void DoResume()
        {
            if (_work.IsPause) _work.DoResume();
            else if (_rest.IsPause) _rest.DoResume();
        }

        public void DoStop()
        {
            _work.DoStop();
            _rest.DoStop();
            _now = PomoState.Work;
            _doneNum = 0;
        }

        public bool IsRun
        {
            get { return _work.IsRun || _rest.IsRun; }
        }

        public bool IsPause
        {
            get { return _work.IsPause || _rest.IsPause; }
        }

        private void WhenWorkDone()
        {
            _doneNum++;
            if (_doneNum >= CycleNum)
            {
                _now = PomoState.LongBreak;
                _rest.SetTime(LongMin * 60);
                _doneNum = 0;
            }
            else
            {
                _now = PomoState.Break;
                _rest.SetTime(RestMin * 60);
            }
            _rest.Go();
            if (OnChange != null) OnChange(_now);
        }

        private void WhenRestDone()
        {
            _now = PomoState.Work;
            _work.SetTime(WorkMin * 60);
            _work.Go();
            if (OnChange != null) OnChange(_now);
        }

        public void Dispose()
        {
            if (_hasDispose) return;
            if (_work != null) _work.Dispose();
            if (_rest != null) _rest.Dispose();
            _hasDispose = true;
        }
    }
}
