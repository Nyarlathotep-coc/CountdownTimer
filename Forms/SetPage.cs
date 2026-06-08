using System;
using System.Drawing;
using System.Windows.Forms;
using CountdownTimer.Models;
using CountdownTimer.Services;

namespace CountdownTimer.Forms
{
    public partial class SetPage : UserControl
    {
        private MainForm _father;
        private AppSettings _s;
        private NumericUpDown _defMin;
        private NumericUpDown _wMin, _bMin, _lMin, _cyc;
        private CheckBox _chkSound, _chkTop, _chkSave;
        private ComboBox _theme;
        private Button _btnSave, _btnDef;

        public SetPage(MainForm father)
        {
            _father = father;
            _s = TimerService.GetSet();
            MakeUI();
            LoadSet();
        }

        private void MakeUI()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Padding = new Padding(20);
            this.AutoScroll = true;

            Label title = new Label();
            title.Text = "系统设置";
            title.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(50, 50, 70);
            title.Location = new Point(20, 15);
            title.Size = new Size(300, 40);

            int cx = 20, cw = 520, y = 70;

            // 通用
            var c1 = NewCard("通用", cx, y, cw, 110); y += 115;
            AddNum(c1, "默认分钟：", 20, out _defMin, 1, 999, 25);
            AddChk(c1, "声音提醒", 50, out _chkSound);
            AddChk(c1, "窗口置顶", 75, out _chkTop);

            // 番茄钟
            var c2 = NewCard("番茄钟", cx, y, cw, 170); y += 175;
            AddNum(c2, "工作分钟：", 20, out _wMin, 1, 120, 25);
            AddNum(c2, "短歇分钟：", 50, out _bMin, 1, 60, 5);
            AddNum(c2, "长歇分钟：", 80, out _lMin, 1, 60, 15);
            AddNum(c2, "几轮长歇：", 110, out _cyc, 1, 10, 4);

            // 外观
            var c3 = NewCard("外观", cx, y, cw, 80); y += 85;
            Label tl = new Label(); tl.Text = "主题："; tl.Location = new Point(20, 15); tl.Size = new Size(180, 25); tl.ForeColor = Color.FromArgb(80, 80, 100);
            _theme = new ComboBox(); _theme.Location = new Point(200, 13); _theme.Size = new Size(150, 25); _theme.DropDownStyle = ComboBoxStyle.DropDownList;
            _theme.Items.Add("经典蓝"); _theme.Items.Add("暗黑"); _theme.Items.Add("森林绿"); _theme.Items.Add("魅紫");
            c3.Controls.Add(tl); c3.Controls.Add(_theme);
            AddChk(c3, "自动保存记录", 45, out _chkSave);

            _btnSave = new Button(); _btnSave.Text = "保存"; _btnSave.Location = new Point(cx, y + 10); _btnSave.Size = new Size(100, 38);
            _btnSave.BackColor = Color.FromArgb(76, 175, 80); _btnSave.ForeColor = Color.White;
            _btnSave.FlatStyle = FlatStyle.Flat; _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold); _btnSave.Cursor = Cursors.Hand;

            _btnDef = new Button(); _btnDef.Text = "恢复默认"; _btnDef.Location = new Point(cx + 110, y + 10); _btnDef.Size = new Size(100, 38);
            _btnDef.BackColor = Color.FromArgb(158, 158, 158); _btnDef.ForeColor = Color.White;
            _btnDef.FlatStyle = FlatStyle.Flat; _btnDef.FlatAppearance.BorderSize = 0;
            _btnDef.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold); _btnDef.Cursor = Cursors.Hand;

            _btnSave.Click += (s, e) => SaveIt();
            _btnDef.Click += (s, e) => ResetIt();

            this.Controls.Add(title); this.Controls.Add(c1); this.Controls.Add(c2);
            this.Controls.Add(c3); this.Controls.Add(_btnSave); this.Controls.Add(_btnDef);
        }

        private Panel NewCard(string t, int x, int y, int w, int h)
        {
            var p = new Panel(); p.Location = new Point(x, y); p.Size = new Size(w, h); p.BackColor = Color.White;
            Label l = new Label(); l.Text = t; l.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            l.ForeColor = Color.FromArgb(60, 60, 80); l.Location = new Point(15, 8); l.Size = new Size(200, 22);
            Label line = new Label(); line.Location = new Point(15, 30); line.Size = new Size(w - 30, 1); line.BackColor = Color.FromArgb(220, 220, 230);
            p.Controls.Add(l); p.Controls.Add(line);
            return p;
        }

        private void AddNum(Panel p, string lab, int y, out NumericUpDown nud, int min, int max, int def)
        {
            Label l = new Label(); l.Text = lab; l.Location = new Point(20 + 10, y); l.Size = new Size(140, 25);
            l.ForeColor = Color.FromArgb(80, 80, 100); l.TextAlign = ContentAlignment.MiddleLeft;
            nud = new NumericUpDown(); nud.Location = new Point(150, y); nud.Size = new Size(70, 25);
            nud.Minimum = min; nud.Maximum = max; nud.Value = def;
            p.Controls.Add(l); p.Controls.Add(nud);
        }

        private void AddChk(Panel p, string t, int y, out CheckBox chk)
        {
            chk = new CheckBox(); chk.Text = t; chk.Location = new Point(30, y);
            chk.Size = new Size(200, 25); chk.ForeColor = Color.FromArgb(80, 80, 100); chk.Checked = true;
            p.Controls.Add(chk);
        }

        private void LoadSet()
        {
            _defMin.Value = _s.DefMin;
            _wMin.Value = _s.PomoWork;
            _bMin.Value = _s.PomoBreak;
            _lMin.Value = _s.PomoLong;
            _cyc.Value = _s.PomoCycle;
            _chkSound.Checked = _s.PlaySound;
            _chkTop.Checked = _s.TopWin;
            _chkSave.Checked = _s.AutoSave;

            if (_s.Theme == "Blue") _theme.SelectedIndex = 0;
            else if (_s.Theme == "Dark") _theme.SelectedIndex = 1;
            else if (_s.Theme == "Green") _theme.SelectedIndex = 2;
            else if (_s.Theme == "Purple") _theme.SelectedIndex = 3;
            else _theme.SelectedIndex = 0;
        }

        private void SaveIt()
        {
            string[] ts = { "Blue", "Dark", "Green", "Purple" };
            _s.DefMin = (int)_defMin.Value;
            _s.PomoWork = (int)_wMin.Value;
            _s.PomoBreak = (int)_bMin.Value;
            _s.PomoLong = (int)_lMin.Value;
            _s.PomoCycle = (int)_cyc.Value;
            _s.PlaySound = _chkSound.Checked;
            _s.TopWin = _chkTop.Checked;
            _s.AutoSave = _chkSave.Checked;
            _s.Theme = ts[_theme.SelectedIndex];

            TimerService.SaveSet(_s);
            _father.RefSet();
            MessageBox.Show("搞定了！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ResetIt()
        {
            var r = MessageBox.Show("恢复默认设置？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                var d = new AppSettings();
                _s.DefMin = d.DefMin; _s.PomoWork = d.PomoWork; _s.PomoBreak = d.PomoBreak;
                _s.PomoLong = d.PomoLong; _s.PomoCycle = d.PomoCycle;
                _s.PlaySound = d.PlaySound; _s.TopWin = d.TopWin; _s.AutoSave = d.AutoSave;
                _s.Theme = d.Theme;
                LoadSet();
                TimerService.SaveSet(_s);
                _father.RefSet();
                MessageBox.Show("恢复好了", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
