using System;
using System.Drawing;
using System.Windows.Forms;
using CountdownTimer.Services;

namespace CountdownTimer.Forms
{
    public partial class MainForm : Form
    {
        private Panel _left;
        private Panel _right;
        private Label _topTitle;
        private UserControl _nowPage;

        public MainForm()
        {
            InitForm();
            OpenPage(new SingleTimerPage(this));
        }

        private void InitForm()
        {
            this.Text = "倒计时器";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(960, 640);
            this.MinimumSize = new Size(800, 500);
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Font = new Font("Microsoft YaHei UI", 9F);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.DoubleBuffered = true;

            // 左边栏
            _left = new Panel();
            _left.Dock = DockStyle.Left;
            _left.Width = 190;
            _left.BackColor = Color.FromArgb(30, 33, 48);

            // 顶部标题 - 大字固定，不能改
            _topTitle = new Label();
            _topTitle.Text = "倒计时器";
            _topTitle.Font = new Font("Microsoft YaHei UI", 20F, FontStyle.Bold);
            _topTitle.ForeColor = Color.White;
            _topTitle.TextAlign = ContentAlignment.MiddleCenter;
            _topTitle.Height = 80;
            _topTitle.Dock = DockStyle.Top;
            _topTitle.Padding = new Padding(0, 15, 0, 0);
            _left.Controls.Add(_topTitle);

            // 导航按钮
            string[] btns = { "单次倒计时", "番茄钟", "多任务", "历史记录", "系统设置", "关于" };
            for (int i = 0; i < btns.Length; i++)
            {
                Button b = new Button();
                b.Text = "  " + btns[i];
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(30, 33, 48);
                b.ForeColor = Color.FromArgb(200, 200, 210);
                b.Font = new Font("Microsoft YaHei UI", 11F);
                b.TextAlign = ContentAlignment.MiddleLeft;
                b.Padding = new Padding(25, 0, 0, 0);
                b.Height = 44;
                b.Dock = DockStyle.Top;
                b.Cursor = Cursors.Hand;

                b.MouseEnter += (s, e) => { b.BackColor = Color.FromArgb(45, 48, 68); };
                b.MouseLeave += (s, e) => { b.BackColor = Color.FromArgb(30, 33, 48); };

                switch (i)
                {
                    case 0: b.Click += (s, e) => OpenPage(new SingleTimerPage(this)); break;
                    case 1: b.Click += (s, e) => OpenPage(new PomoPage(this)); break;
                    case 2: b.Click += (s, e) => OpenPage(new MultiPage(this)); break;
                    case 3: b.Click += (s, e) => OpenPage(new HisPage(this)); break;
                    case 4: b.Click += (s, e) => OpenPage(new SetPage(this)); break;
                    case 5: b.Click += (s, e) => OpenPage(new AboutPage()); break;
                }
                _left.Controls.Add(b);
            }

            // 右边内容区
            _right = new Panel();
            _right.Dock = DockStyle.Fill;
            _right.BackColor = Color.FromArgb(245, 245, 250);
            _right.Padding = new Padding(15);

            this.Controls.Add(_right);
            this.Controls.Add(_left);
        }

        public void OpenPage(UserControl page)
        {
            if (_nowPage != null)
            {
                _right.Controls.Remove(_nowPage);
                // 不Dispose，只隐藏，这样多任务切回来还在
                _nowPage.Visible = false;
            }
            _nowPage = page;
            page.Visible = true;
            page.Dock = DockStyle.Fill;
            _right.Controls.Add(page);
        }
    }
}
