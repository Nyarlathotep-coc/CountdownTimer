using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CountdownTimer.Models;
using CountdownTimer.Services;

namespace CountdownTimer.Forms
{
    public partial class MultiPage : UserControl
    {
        private MainForm _father;
        private List<OneTask> _tasks = new List<OneTask>();
        private FlowLayoutPanel _list;
        private Panel _addP;
        private TextBox _inName;
        private NumericUpDown _inMin;
        private Button _btnAdd, _btnClear;
        private Label _title;

        public MultiPage(MainForm father)
        {
            _father = father;
            MakeUI();
        }

        private void MakeUI()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Padding = new Padding(20);
            this.AutoScroll = true;

            _title = new Label();
            _title.Text = "多任务倒计时";
            _title.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            _title.ForeColor = Color.FromArgb(50, 50, 70);
            _title.Location = new Point(20, 15);
            _title.Size = new Size(300, 40);

            _addP = new Panel();
            _addP.Location = new Point(20, 65);
            _addP.Size = new Size(600, 55);
            _addP.BackColor = Color.White;

            Label la = new Label(); la.Text = "任务名："; la.Location = new Point(15, 16); la.Size = new Size(55, 25); la.ForeColor = Color.FromArgb(80, 80, 100);
            _inName = new TextBox(); _inName.Text = "新任务"; _inName.Location = new Point(70, 14); _inName.Size = new Size(120, 25); _inName.BorderStyle = BorderStyle.FixedSingle;
            Label lb = new Label(); lb.Text = "分钟："; lb.Location = new Point(200, 16); lb.Size = new Size(40, 25); lb.ForeColor = Color.FromArgb(80, 80, 100);
            _inMin = new NumericUpDown(); _inMin.Location = new Point(240, 14); _inMin.Size = new Size(60, 25); _inMin.Maximum = 999; _inMin.Minimum = 1; _inMin.Value = 5;

            _btnAdd = new Button(); _btnAdd.Text = "加任务"; _btnAdd.Location = new Point(315, 11); _btnAdd.Size = new Size(90, 32);
            _btnAdd.BackColor = Color.FromArgb(76, 175, 80); _btnAdd.ForeColor = Color.White;
            _btnAdd.FlatStyle = FlatStyle.Flat; _btnAdd.FlatAppearance.BorderSize = 0;
            _btnAdd.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold); _btnAdd.Cursor = Cursors.Hand;

            _btnClear = new Button(); _btnClear.Text = "清空"; _btnClear.Location = new Point(415, 11); _btnClear.Size = new Size(70, 32);
            _btnClear.BackColor = Color.FromArgb(244, 67, 54); _btnClear.ForeColor = Color.White;
            _btnClear.FlatStyle = FlatStyle.Flat; _btnClear.FlatAppearance.BorderSize = 0;
            _btnClear.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold); _btnClear.Cursor = Cursors.Hand;

            _addP.Controls.Add(la); _addP.Controls.Add(_inName); _addP.Controls.Add(lb);
            _addP.Controls.Add(_inMin); _addP.Controls.Add(_btnAdd); _addP.Controls.Add(_btnClear);

            _list = new FlowLayoutPanel();
            _list.Location = new Point(20, 130);
            _list.Size = new Size(620, 400);
            _list.AutoScroll = true;
            _list.FlowDirection = FlowDirection.TopDown;
            _list.WrapContents = false;

            _btnAdd.Click += (s, e) => AddOne();
            _btnClear.Click += (s, e) => ClearAll();

            this.Controls.Add(_title); this.Controls.Add(_addP); this.Controls.Add(_list);
        }

        private void AddOne()
        {
            string n = _inName.Text.Trim();
            if (n == "") n = "任务";
            int m = (int)_inMin.Value;

            OneTask t = new OneTask(this, n, m);
            _tasks.Add(t);
            _list.Controls.Add(t.MakeUI());

            _inMin.Value = 5;
            _inName.Text = "新任务";
        }

        private void ClearAll()
        {
            if (_tasks.Count == 0) return;
            var r = MessageBox.Show("清空所有任务？正在跑的也会停哦", "确认",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
            {
                foreach (var t in _tasks) t.Dispose();
                _tasks.Clear();
                _list.Controls.Clear();
            }
        }

        internal void DelTask(OneTask t)
        {
            _tasks.Remove(t);
        }

        internal void SaveHis(HistoryRecord r)
        {
            TimerService.AddHis(r);
        }
    }

    internal class OneTask : IDisposable
    {
        private MultiPage _owner;
        private CountdownEngine _e;
        private string _name;
        private Panel _p;
        private Label _lName, _lTime;
        private ProgressBar _pBar;
        private Button _btnGo, _btnPause, _btnDel;
        private DateTime _tStart;

        public OneTask(MultiPage owner, string name, int min)
        {
            _owner = owner;
            _name = name;
            _e = new CountdownEngine(min * 60, name);
            _e.OnTick += (s) =>
            {
                if (_p != null && _p.IsHandleCreated)
                    _p.BeginInvoke(new Action(() => UpShow()));
            };
            _e.OnDone += () =>
            {
                if (_p != null && _p.IsHandleCreated)
                    _p.BeginInvoke(new Action(() => WhenDone()));
            };
        }

        public Panel MakeUI()
        {
            _p = new Panel();
            _p.Size = new Size(580, 65);
            _p.BackColor = Color.White;
            _p.Margin = new Padding(0, 0, 0, 8);

            _lName = new Label();
            _lName.Text = _name;
            _lName.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            _lName.ForeColor = Color.FromArgb(60, 60, 80);
            _lName.Location = new Point(12, 6);
            _lName.Size = new Size(180, 22);

            _lTime = new Label();
            _lTime.Text = _e.ShowTime;
            _lTime.Font = new Font("Consolas", 22F, FontStyle.Bold);
            _lTime.ForeColor = Color.FromArgb(50, 50, 70);
            _lTime.Location = new Point(200, 6);
            _lTime.Size = new Size(110, 35);
            _lTime.TextAlign = ContentAlignment.MiddleCenter;

            _pBar = new ProgressBar();
            _pBar.Location = new Point(320, 12);
            _pBar.Size = new Size(100, 18);
            _pBar.Style = ProgressBarStyle.Continuous;
            _pBar.ForeColor = Color.FromArgb(76, 175, 80);
            _pBar.BackColor = Color.FromArgb(220, 220, 230);

            _btnGo = new Button(); _btnGo.Text = "开始"; _btnGo.Size = new Size(50, 28);
            _btnGo.BackColor = Color.FromArgb(76, 175, 80); _btnGo.ForeColor = Color.White;
            _btnGo.FlatStyle = FlatStyle.Flat; _btnGo.FlatAppearance.BorderSize = 0;
            _btnGo.Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);
            _btnGo.Location = new Point(435, 18); _btnGo.Cursor = Cursors.Hand;

            _btnPause = new Button(); _btnPause.Text = "II"; _btnPause.Size = new Size(40, 28);
            _btnPause.BackColor = Color.FromArgb(255, 152, 0); _btnPause.ForeColor = Color.White;
            _btnPause.FlatStyle = FlatStyle.Flat; _btnPause.FlatAppearance.BorderSize = 0;
            _btnPause.Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);
            _btnPause.Location = new Point(490, 18); _btnPause.Enabled = false; _btnPause.Cursor = Cursors.Hand;

            _btnDel = new Button(); _btnDel.Text = "X"; _btnDel.Size = new Size(35, 28);
            _btnDel.BackColor = Color.FromArgb(244, 67, 54); _btnDel.ForeColor = Color.White;
            _btnDel.FlatStyle = FlatStyle.Flat; _btnDel.FlatAppearance.BorderSize = 0;
            _btnDel.Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);
            _btnDel.Location = new Point(535, 18); _btnDel.Cursor = Cursors.Hand;

            _btnGo.Click += (s, e) => Go();
            _btnPause.Click += (s, e) => PauseOrNot();
            _btnDel.Click += (s, e) => Del();

            _p.Controls.Add(_lName); _p.Controls.Add(_lTime); _p.Controls.Add(_pBar);
            _p.Controls.Add(_btnGo); _p.Controls.Add(_btnPause); _p.Controls.Add(_btnDel);
            return _p;
        }

        private void Go()
        {
            _tStart = DateTime.Now;
            _e.Go();
            _btnGo.Enabled = false;
            _btnPause.Enabled = true;
            _btnDel.Enabled = false;
        }

        private void PauseOrNot()
        {
            if (_e.IsPause)
            {
                _e.DoResume();
                _btnPause.Text = "II";
            }
            else
            {
                _e.DoPause();
                _btnPause.Text = ">";
            }
        }

        private void Del()
        {
            _e.DoStop();
            _owner.DelTask(this);
            if (_p != null && _p.Parent != null) _p.Parent.Controls.Remove(_p);
            Dispose();
        }

        private void WhenDone()
        {
            TimerService.Beep();
            _lTime.ForeColor = Color.FromArgb(244, 67, 54);
            _lTime.Text = "OK";
            _pBar.Value = 100;

            HistoryRecord r = new HistoryRecord();
            r.Name = _name; r.Type = TimerType.MultiTask;
            r.TotalSec = _e.TotalSec; r.UsedSec = _e.TotalSec;
            r.StartTime = _tStart; r.EndTime = DateTime.Now; r.IsFinish = true;
            _owner.SaveHis(r);

            MessageBox.Show(_name + " 搞定了！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpShow()
        {
            _lTime.Text = _e.ShowTime;
            _pBar.Value = (int)(_e.GetPercent * 100);
        }

        public void Dispose()
        {
            if (_e != null) _e.Dispose();
        }
    }
}

