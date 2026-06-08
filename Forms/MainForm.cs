using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CountdownTimer.Services;

namespace CountdownTimer.Forms
{
    public partial class MainForm : Form
    {
        private Panel _left;
        private Panel _right;
        private Label _bottomTitle;
        private UserControl _nowPage;

        // 每个页面只创建一次，存起来复用
        private SingleTimerPage _page0;
        private PomoPage _page1;
        private MultiPage _page2;
        private HisPage _page3;
        private SetPage _page4;
        private AboutPage _page5;

        public MainForm()
        {
            InitForm();
            OpenPage(0);
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

            // 底部标题 - 置底
            _bottomTitle = new Label();
            _bottomTitle.Text = "倒计时器";
            _bottomTitle.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold);
            _bottomTitle.ForeColor = Color.FromArgb(140, 140, 160);
            _bottomTitle.TextAlign = ContentAlignment.MiddleCenter;
            _bottomTitle.Height = 60;
            _bottomTitle.Dock = DockStyle.Bottom;
            _bottomTitle.Padding = new Padding(0, 0, 0, 10);
            _left.Controls.Add(_bottomTitle);

            // 按钮容器 - 占满中间
            Panel btnPanel = new Panel();
            btnPanel.Dock = DockStyle.Fill;

            // 先创建所有页面
            _page0 = new SingleTimerPage(this);
            _page1 = new PomoPage(this);
            _page2 = new MultiPage(this);
            _page3 = new HisPage(this);
            _page4 = new SetPage(this);
            _page5 = new AboutPage();

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

                int idx = i;
                b.Click += (s, e) => OpenPage(idx);

                btnPanel.Controls.Add(b);
            }

            _left.Controls.Add(btnPanel);

            // 右边内容区
            _right = new Panel();
            _right.Dock = DockStyle.Fill;
            _right.BackColor = Color.FromArgb(245, 245, 250);
            _right.Padding = new Padding(15);

            this.Controls.Add(_right);
            this.Controls.Add(_left);
        }

        public void OpenPage(int idx)
        {
            if (_nowPage != null)
            {
                _right.Controls.Remove(_nowPage);
                _nowPage.Visible = false;
            }

            switch (idx)
            {
                case 0: _nowPage = _page0; break;
                case 1: _nowPage = _page1; break;
                case 2: _nowPage = _page2; break;
                case 3: _nowPage = _page3; break;
                case 4: _nowPage = _page4; break;
                case 5: _nowPage = _page5; break;
            }

            _nowPage.Visible = true;
            _nowPage.Dock = DockStyle.Fill;
            _right.Controls.Add(_nowPage);
        }
    }
}
