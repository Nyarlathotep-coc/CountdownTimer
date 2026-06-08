using System;
using System.Drawing;
using System.Windows.Forms;
using CountdownTimer.Models;
using CountdownTimer.Services;

namespace CountdownTimer.Forms
{
    public partial class SingleTimerPage : UserControl
    {
        private CountdownEngine _my;
        private MainForm _father;
        private Label _showTime;
        private ProgressBar _pBar;
        private TextBox _inName;
        private NumericUpDown _inHour, _inMin, _inSec;
        private Button _btnGo, _btnPause, _btnStop, _btnAdd;
        private Panel _pSet;
        private Panel _pRun;
        private Label _tip;
        private DateTime _begin;

        public SingleTimerPage(MainForm father)
        {
            _father = father;
            _my = new CountdownEngine(0, "单次倒计时");
            MakeUI();
            LinkEvents();
        }

        private void MakeUI()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Padding = new Padding(20);

            Label title = new Label();
            title.Text = "单次倒计时";
            title.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(50, 50, 70);
            title.Location = new Point(20, 15);
            title.Size = new Size(300, 40);

            // 设置面板
            _pSet = new Panel();
            _pSet.Location = new Point(20, 70);
            _pSet.Size = new Size(450, 160);
            _pSet.BackColor = Color.White;

            Label lab1 = new Label();
            lab1.Text = "设置倒计时";
            lab1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            lab1.ForeColor = Color.FromArgb(60, 60, 80);
            lab1.Location = new Point(20, 15);
            lab1.Size = new Size(200, 25);

            Label lab2 = new Label();
            lab2.Text = "名称：";
            lab2.Location = new Point(20, 50);
            lab2.Size = new Size(60, 25);
            lab2.ForeColor = Color.FromArgb(80, 80, 100);

            _inName = new TextBox();
            _inName.Text = "倒计时";
            _inName.Location = new Point(80, 48);
            _inName.Size = new Size(200, 25);
            _inName.BorderStyle = BorderStyle.FixedSingle;

            Label lh = new Label(); lh.Text = "时"; lh.Location = new Point(20, 85); lh.Size = new Size(20, 25); lh.ForeColor = Color.FromArgb(80, 80, 100);
            _inHour = new NumericUpDown(); _inHour.Location = new Point(40, 83); _inHour.Size = new Size(55, 25); _inHour.Maximum = 99; _inHour.Value = 0;
            Label lm = new Label(); lm.Text = "分"; lm.Location = new Point(100, 85); lm.Size = new Size(20, 25); lm.ForeColor = Color.FromArgb(80, 80, 100);
            _inMin = new NumericUpDown(); _inMin.Location = new Point(120, 83); _inMin.Size = new Size(55, 25); _inMin.Maximum = 59; _inMin.Value = 5;
            Label ls = new Label(); ls.Text = "秒"; ls.Location = new Point(180, 85); ls.Size = new Size(20, 25); ls.ForeColor = Color.FromArgb(80, 80, 100);
            _inSec = new NumericUpDown(); _inSec.Location = new Point(200, 83); _inSec.Size = new Size(55, 25); _inSec.Maximum = 59; _inSec.Value = 0;

            _btnGo = new Button();
            _btnGo.Text = "开始";
            _btnGo.Location = new Point(20, 120);
            _btnGo.Size = new Size(100, 35);
            _btnGo.BackColor = Color.FromArgb(76, 175, 80);
            _btnGo.ForeColor = Color.White;
            _btnGo.FlatStyle = FlatStyle.Flat;
            _btnGo.FlatAppearance.BorderSize = 0;
            _btnGo.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            _btnGo.Cursor = Cursors.Hand;

            _pSet.Controls.Add(lab1); _pSet.Controls.Add(lab2); _pSet.Controls.Add(_inName);
            _pSet.Controls.Add(lh); _pSet.Controls.Add(_inHour);
            _pSet.Controls.Add(lm); _pSet.Controls.Add(_inMin);
            _pSet.Controls.Add(ls); _pSet.Controls.Add(_inSec);
            _pSet.Controls.Add(_btnGo);

            // 运行面板
            _pRun = new Panel();
            _pRun.Location = new Point(20, 250);
            _pRun.Size = new Size(450, 220);
            _pRun.BackColor = Color.White;
            _pRun.Visible = false;

            _showTime = new Label();
            _showTime.Text = "05:00";
            _showTime.Font = new Font("Consolas", 52F, FontStyle.Bold);
            _showTime.ForeColor = Color.FromArgb(50, 50, 70);
            _showTime.TextAlign = ContentAlignment.MiddleCenter;
            _showTime.Location = new Point(0, 20);
            _showTime.Size = new Size(450, 80);

            _pBar = new ProgressBar();
            _pBar.Location = new Point(30, 105);
            _pBar.Size = new Size(390, 20);
            _pBar.Style = ProgressBarStyle.Continuous;
            _pBar.ForeColor = Color.FromArgb(76, 175, 80);
            _pBar.BackColor = Color.FromArgb(220, 220, 230);

            _tip = new Label();
            _tip.Text = "运行中...";
            _tip.Font = new Font("Microsoft YaHei UI", 9F);
            _tip.ForeColor = Color.Gray;
            _tip.TextAlign = ContentAlignment.MiddleCenter;
            _tip.Location = new Point(0, 130);
            _tip.Size = new Size(450, 20);

            _btnPause = new Button(); _btnPause.Text = "暂停"; _btnPause.Size = new Size(90, 35);
            _btnPause.BackColor = Color.FromArgb(255, 152, 0); _btnPause.ForeColor = Color.White;
            _btnPause.FlatStyle = FlatStyle.Flat; _btnPause.FlatAppearance.BorderSize = 0;
            _btnPause.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            _btnPause.Cursor = Cursors.Hand; _btnPause.Location = new Point(30, 155);

            _btnStop = new Button(); _btnStop.Text = "停止"; _btnStop.Size = new Size(90, 35);
            _btnStop.BackColor = Color.FromArgb(244, 67, 54); _btnStop.ForeColor = Color.White;
            _btnStop.FlatStyle = FlatStyle.Flat; _btnStop.FlatAppearance.BorderSize = 0;
            _btnStop.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            _btnStop.Cursor = Cursors.Hand; _btnStop.Location = new Point(130, 155);

            _btnAdd = new Button(); _btnAdd.Text = "+1分"; _btnAdd.Size = new Size(70, 35);
            _btnAdd.BackColor = Color.FromArgb(33, 150, 243); _btnAdd.ForeColor = Color.White;
            _btnAdd.FlatStyle = FlatStyle.Flat; _btnAdd.FlatAppearance.BorderSize = 0;
            _btnAdd.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            _btnAdd.Cursor = Cursors.Hand; _btnAdd.Location = new Point(230, 155);

            _pRun.Controls.Add(_showTime); _pRun.Controls.Add(_pBar);
            _pRun.Controls.Add(_tip); _pRun.Controls.Add(_btnPause);
            _pRun.Controls.Add(_btnStop); _pRun.Controls.Add(_btnAdd);

            this.Controls.Add(title); this.Controls.Add(_pSet); this.Controls.Add(_pRun);
        }

        private void LinkEvents()
        {
            _btnGo.Click += (s, e) => Go();

            _btnPause.Click += (s, e) =>
            {
                if (_my.IsPause)
                {
                    _my.DoResume();
                    _btnPause.Text = "暂停";
                    _tip.Text = "运行中...";
                }
                else
                {
                    _my.DoPause();
                    _btnPause.Text = "继续";
                    _tip.Text = "暂停了";
                }
            };

            _btnStop.Click += (s, e) => DoStop();

            _btnAdd.Click += (s, e) =>
            {
                _my.AddSec(60);
                UpShow();
            };

            _my.OnTick += (s) =>
            {
                if (this.IsHandleCreated)
                    this.BeginInvoke(new Action(() => UpShow()));
            };

            _my.OnDone += () =>
            {
                if (this.IsHandleCreated)
                    this.BeginInvoke(new Action(() => WhenDone()));
            };
        }

        private void Go()
        {
            int sec = ((int)_inHour.Value * 3600) + ((int)_inMin.Value * 60) + (int)_inSec.Value;
            if (sec <= 0)
            {
                MessageBox.Show("时间要大于0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _my.MyName = _inName.Text;
            _my.SetTime(sec);
            _my.MyType = TimerType.Single;
            _begin = DateTime.Now;

            _pSet.Visible = false;
            _pRun.Visible = true;
            _showTime.ForeColor = Color.FromArgb(50, 50, 70);
            _btnPause.Text = "暂停";

            UpShow();
            _my.Go();
        }

        private void DoStop()
        {
            _my.DoStop();
            _pRun.Visible = false;
            _pSet.Visible = true;
        }

        private void WhenDone()
        {
            _showTime.ForeColor = Color.FromArgb(244, 67, 54);
            _showTime.Text = "00:00";
            _pBar.Value = 100;
            _tip.Text = "完成了！";

            HistoryRecord r = new HistoryRecord();
            r.Name = _my.MyName;
            r.Type = TimerType.Single;
            r.TotalSec = _my.TotalSec;
            r.UsedSec = _my.TotalSec;
            r.StartTime = _begin;
            r.EndTime = DateTime.Now;
            r.IsFinish = true;
            TimerService.AddHis(r);

            string msg = _my.MyName + " 倒计时完成！再来一次不？";
            var ans = MessageBox.Show(msg, "完成了", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ans == DialogResult.Yes)
            {
                this.BeginInvoke(new Action(() =>
                {
                    _pRun.Visible = false;
                    _pSet.Visible = true;
                }));
            }
            else
            {
                this.BeginInvoke(new Action(() => DoStop()));
            }
        }

        private void UpShow()
        {
            _showTime.Text = _my.ShowTime;
            _pBar.Value = (int)(_my.GetPercent * 100);
            _tip.Text = "还剩 " + _my.ShowTime;

            if (_my.LeftSec <= 10 && _my.LeftSec > 0)
                _showTime.ForeColor = Color.FromArgb(244, 67, 54);
        }
    }
}

