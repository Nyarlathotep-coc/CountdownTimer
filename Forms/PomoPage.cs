using System;
using System.Drawing;
using System.Windows.Forms;
using CountdownTimer.Models;
using CountdownTimer.Services;

namespace CountdownTimer.Forms
{
    public partial class PomoPage : UserControl
    {
        private PomodoroEngine _p;
        private MainForm _father;
        private Label _showTime;
        private Label _phase;
        private Label _numTip;
        private ProgressBar _pBar;
        private Button _btnGo, _btnPause, _btnStop;
        private DateTime _begin;

        public PomoPage(MainForm father)
        {
            _father = father;
            _p = new PomodoroEngine();
            MakeUI();
            LinkEv();
            LoadSet();
        }

        private void MakeUI()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Padding = new Padding(20);

            Label title = new Label();
            title.Text = "番茄钟";
            title.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(50, 50, 70);
            title.Location = new Point(20, 15);
            title.Size = new Size(300, 40);

            var card = new Panel();
            card.Location = new Point(20, 70);
            card.Size = new Size(500, 360);
            card.BackColor = Color.White;

            _phase = new Label();
            _phase.Text = "准备就绪";
            _phase.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold);
            _phase.ForeColor = Color.FromArgb(76, 175, 80);
            _phase.TextAlign = ContentAlignment.MiddleCenter;
            _phase.Location = new Point(0, 30);
            _phase.Size = new Size(500, 30);

            _showTime = new Label();
            _showTime.Text = _p.WorkMin.ToString("D2") + ":00";
            _showTime.Font = new Font("Consolas", 56F, FontStyle.Bold);
            _showTime.ForeColor = Color.FromArgb(50, 50, 70);
            _showTime.TextAlign = ContentAlignment.MiddleCenter;
            _showTime.Location = new Point(0, 70);
            _showTime.Size = new Size(500, 80);

            _pBar = new ProgressBar();
            _pBar.Location = new Point(40, 165);
            _pBar.Size = new Size(420, 15);
            _pBar.Style = ProgressBarStyle.Continuous;
            _pBar.ForeColor = Color.FromArgb(76, 175, 80);
            _pBar.BackColor = Color.FromArgb(220, 220, 230);

            var infoP = new Panel();
            infoP.Location = new Point(40, 190);
            infoP.Size = new Size(420, 50);
            infoP.BackColor = Color.FromArgb(240, 240, 248);

            _numTip = new Label();
            _numTip.Text = "已经搞了 0 轮";
            _numTip.Font = new Font("Microsoft YaHei UI", 10F);
            _numTip.ForeColor = Color.FromArgb(100, 100, 120);
            _numTip.TextAlign = ContentAlignment.MiddleCenter;
            _numTip.Location = new Point(0, 0);
            _numTip.Size = new Size(420, 50);
            infoP.Controls.Add(_numTip);

            _btnGo = new Button(); _btnGo.Text = "开始干活"; _btnGo.Size = new Size(120, 40);
            _btnGo.BackColor = Color.FromArgb(76, 175, 80); _btnGo.ForeColor = Color.White;
            _btnGo.FlatStyle = FlatStyle.Flat; _btnGo.FlatAppearance.BorderSize = 0;
            _btnGo.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            _btnGo.Location = new Point(40, 250); _btnGo.Cursor = Cursors.Hand;

            _btnPause = new Button(); _btnPause.Text = "暂停"; _btnPause.Size = new Size(100, 40);
            _btnPause.BackColor = Color.FromArgb(255, 152, 0); _btnPause.ForeColor = Color.White;
            _btnPause.FlatStyle = FlatStyle.Flat; _btnPause.FlatAppearance.BorderSize = 0;
            _btnPause.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            _btnPause.Location = new Point(170, 250); _btnPause.Enabled = false; _btnPause.Cursor = Cursors.Hand;

            _btnStop = new Button(); _btnStop.Text = "停止"; _btnStop.Size = new Size(100, 40);
            _btnStop.BackColor = Color.FromArgb(244, 67, 54); _btnStop.ForeColor = Color.White;
            _btnStop.FlatStyle = FlatStyle.Flat; _btnStop.FlatAppearance.BorderSize = 0;
            _btnStop.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            _btnStop.Location = new Point(280, 250); _btnStop.Enabled = false; _btnStop.Cursor = Cursors.Hand;

            card.Controls.Add(_phase); card.Controls.Add(_showTime);
            card.Controls.Add(_pBar); card.Controls.Add(infoP);
            card.Controls.Add(_btnGo); card.Controls.Add(_btnPause); card.Controls.Add(_btnStop);

            // 说明
            var tipBox = new GroupBox();
            tipBox.Text = "说明";
            tipBox.Location = new Point(20, 445);
            tipBox.Size = new Size(500, 120);
            tipBox.ForeColor = Color.FromArgb(80, 80, 100);
            tipBox.Font = new Font("Microsoft YaHei UI", 9F);

            var tips = new Label();
            tips.Text = "番茄钟就是让你干一会儿歇一会儿\n" +
                       "干25分 -> 歇5分，4轮后来个15分大歇\n" +
                       "专注干活，效率更高";
            tips.Font = new Font("Microsoft YaHei UI", 9F);
            tips.ForeColor = Color.FromArgb(100, 100, 120);
            tips.Location = new Point(15, 20);
            tips.Size = new Size(470, 90);
            tipBox.Controls.Add(tips);

            this.Controls.Add(title); this.Controls.Add(card); this.Controls.Add(tipBox);
        }

        private void LoadSet()
        {
            var s = TimerService.GetSet();
            _p.WorkMin = s.PomoWork;
            _p.RestMin = s.PomoBreak;
            _p.LongMin = s.PomoLong;
            _p.CycleNum = s.PomoCycle;
        }

        private void LinkEv()
        {
            _btnGo.Click += (s, e) => DoGo();
            _btnPause.Click += (s, e) => DoPause();
            _btnStop.Click += (s, e) => DoStop();

            _p.OnTick += (sec) =>
            {
                if (this.IsHandleCreated)
                    this.BeginInvoke(new Action(() => UpNum()));
            };

            _p.OnChange += (st) =>
            {
                if (this.IsHandleCreated)
                    this.BeginInvoke(new Action(() => WhenChange(st)));
            };
        }

        private void DoGo()
        {
            _begin = DateTime.Now;
            LoadSet();
            _p.Go();
            _btnGo.Enabled = false;
            _btnPause.Enabled = true;
            _btnStop.Enabled = true;
            _btnPause.Text = "暂停";
            WhenChange(PomoState.Work);
        }

        private void DoPause()
        {
            if (_p.IsPause)
            {
                _p.DoResume();
                _btnPause.Text = "暂停";
                if (_p.NowState == PomoState.Work) _phase.Text = "干活中";
                else _phase.Text = "歇着呢";
            }
            else
            {
                _p.DoPause();
                _btnPause.Text = "继续";
                _phase.Text = "暂停了";
            }
        }

        private void DoStop()
        {
            _p.DoStop();
            BackUI();
        }

        private void WhenChange(PomoState st)
        {
            string t;
            Color c;
            if (st == PomoState.Work) { t = "干活中！专心！"; c = Color.FromArgb(244, 67, 54); }
            else if (st == PomoState.Break) { t = "小歇一会儿"; c = Color.FromArgb(76, 175, 80); }
            else { t = "大歇！好好放松"; c = Color.FromArgb(33, 150, 243); }

            _phase.Text = t;
            _phase.ForeColor = c;
            _pBar.ForeColor = c;
            _showTime.ForeColor = c;

            _numTip.Text = "已经搞了 " + _p.DoneNum + " 轮";

            if (st != PomoState.Work) // 完成一个工作时段 -> 存历史
            if (st == PomoState.Break || st == PomoState.LongBreak)
            {
                Models.HistoryRecord r = new Models.HistoryRecord();
                r.Name = "番茄钟";
                r.Type = Models.TimerType.Pomodoro;
                r.TotalSec = _p.WorkMin * 60;
                r.UsedSec = _p.WorkMin * 60;
                r.StartTime = _begin;
                r.EndTime = DateTime.Now;
                r.IsFinish = true;
                r.Notes = "第 " + _p.DoneNum + " 轮番茄";
                TimerService.AddHis(r);
            }
            UpNum();
        }

        private void UpNum()
        {
            CountdownEngine cur;
            if (_p.NowState == PomoState.Work) cur = _p.WorkTimer;
            else cur = _p.RestTimer;
            _showTime.Text = cur.ShowTime;
            _pBar.Value = (int)(cur.GetPercent * 100);
        }

        private void BackUI()
        {
            _showTime.Text = _p.WorkMin.ToString("D2") + ":00";
            _showTime.ForeColor = Color.FromArgb(50, 50, 70);
            _phase.Text = "准备就绪";
            _phase.ForeColor = Color.FromArgb(76, 175, 80);
            _pBar.Value = 0;
            _pBar.ForeColor = Color.FromArgb(76, 175, 80);
            _numTip.Text = "已经搞了 0 轮";
            _btnGo.Enabled = true;
            _btnPause.Enabled = false;
            _btnStop.Enabled = false;
            _btnPause.Text = "暂停";
        }
    }
}


