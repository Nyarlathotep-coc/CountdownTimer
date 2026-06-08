using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CountdownTimer.Models;
using CountdownTimer.Services;

namespace CountdownTimer.Forms
{
    public partial class HisPage : UserControl
    {
        private MainForm _father;
        private ListView _lv;
        private TextBox _searchBox;
        private Button _btnSearch, _btnDel, _btnClear, _btnRef;
        private Label _cnt;
        private List<HistoryRecord> _data;

        public HisPage(MainForm father)
        {
            _father = father;
            _data = new List<HistoryRecord>();
            MakeUI();
            LoadData();
        }

        private void MakeUI()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Padding = new Padding(20);

            Label title = new Label();
            title.Text = "历史记录";
            title.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(50, 50, 70);
            title.Location = new Point(20, 15);
            title.Size = new Size(300, 40);

            var topP = new Panel();
            topP.Location = new Point(20, 65);
            topP.Size = new Size(680, 45);
            topP.BackColor = Color.White;

            _searchBox = new TextBox();
            _searchBox.Location = new Point(15, 10);
            _searchBox.Size = new Size(250, 25);
            _searchBox.BorderStyle = BorderStyle.FixedSingle;
            _searchBox.ForeColor = Color.Gray;
            _searchBox.Text = "搜一下任务名...";

            _searchBox.Enter += (s, e) =>
            {
                if (_searchBox.Text == "搜一下任务名...")
                { _searchBox.Text = ""; _searchBox.ForeColor = Color.Black; }
            };
            _searchBox.Leave += (s, e) =>
            {
                if (_searchBox.Text.Trim() == "")
                { _searchBox.Text = "搜一下任务名..."; _searchBox.ForeColor = Color.Gray; }
            };

            _btnSearch = MakeBtn("搜索", 275, Color.FromArgb(33, 150, 243));
            _btnRef = MakeBtn("刷新", 365, Color.FromArgb(0, 150, 136));
            _btnDel = MakeBtn("删选中", 455, Color.FromArgb(244, 67, 54));
            _btnClear = MakeBtn("全清", 565, Color.FromArgb(100, 100, 120));

            topP.Controls.Add(_searchBox);
            topP.Controls.Add(_btnSearch);
            topP.Controls.Add(_btnRef);
            topP.Controls.Add(_btnDel);
            topP.Controls.Add(_btnClear);

            _cnt = new Label();
            _cnt.Text = "共 0 条";
            _cnt.Font = new Font("Microsoft YaHei UI", 9F);
            _cnt.ForeColor = Color.Gray;
            _cnt.Location = new Point(20, 115);
            _cnt.Size = new Size(300, 20);

            _lv = new ListView();
            _lv.Location = new Point(20, 140);
            _lv.Size = new Size(680, 380);
            _lv.View = View.Details;
            _lv.FullRowSelect = true;
            _lv.GridLines = true;
            _lv.BackColor = Color.White;
            _lv.Font = new Font("Microsoft YaHei UI", 9F);
            _lv.MultiSelect = false;

            _lv.Columns.Add("ID", 40, HorizontalAlignment.Center);
            _lv.Columns.Add("名字", 140);
            _lv.Columns.Add("类型", 70, HorizontalAlignment.Center);
            _lv.Columns.Add("设定", 80, HorizontalAlignment.Center);
            _lv.Columns.Add("实际", 80, HorizontalAlignment.Center);
            _lv.Columns.Add("开始", 150);
            _lv.Columns.Add("状态", 60, HorizontalAlignment.Center);
            _lv.Columns.Add("备注", 100);

            _lv.ItemSelectionChanged += (s, e) =>
            {
                _btnDel.Enabled = _lv.SelectedItems.Count > 0;
            };
            _btnDel.Enabled = false;

            _btnSearch.Click += (s, e) => DoSearch();
            _btnRef.Click += (s, e) => LoadData();
            _btnDel.Click += (s, e) => DelOne();
            _btnClear.Click += (s, e) => ClearAll();

            _lv.DoubleClick += (s, e) =>
            {
                if (_lv.SelectedItems.Count > 0)
                {
                    int id = int.Parse(_lv.SelectedItems[0].Text);
                    var r = _data.Find(x => x.Id == id);
                    if (r != null)
                    {
                        string info = "名字：" + r.Name + "\n" +
                            "类型：" + r.ShowType + "\n" +
                            "设定：" + r.ShowTime + "\n" +
                            "实际：" + r.ShowUsed + "\n" +
                            "开始：" + r.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "\n" +
                            "结束：" + r.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "\n" +
                            "状态：" + (r.IsFinish ? "完成" : "中断") + "\n" +
                            "备注：" + (r.Notes == null ? "无" : r.Notes);
                        MessageBox.Show(info, "详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            };

            this.Controls.Add(title); this.Controls.Add(topP);
            this.Controls.Add(_cnt); this.Controls.Add(_lv);
        }

        private Button MakeBtn(string t, int x, Color c)
        {
            Button b = new Button();
            b.Text = t; b.Location = new Point(x, 8); b.Size = new Size(80, 28);
            b.BackColor = c; b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            return b;
        }

        public void LoadData()
        {
            _data = TimerService.GetHis();
            RefList(_data);
        }

        private void DoSearch()
        {
            string w = _searchBox.Text;
            if (w == "搜一下任务名...") w = "";
            RefList(TimerService.SearchHis(w));
        }

        private void RefList(List<HistoryRecord> list)
        {
            _lv.Items.Clear();
            foreach (var r in list)
            {
                var item = new ListViewItem(r.Id.ToString());
                item.SubItems.Add(r.Name);
                item.SubItems.Add(r.ShowType);
                item.SubItems.Add(r.ShowTime);
                item.SubItems.Add(r.ShowUsed);
                item.SubItems.Add(r.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(r.IsFinish ? "完成" : "中断");
                item.SubItems.Add(r.Notes == null ? "" : r.Notes);
                item.Tag = r.Id;
                _lv.Items.Add(item);
            }
            _cnt.Text = "共 " + list.Count + " 条";
        }

        private void DelOne()
        {
            if (_lv.SelectedItems.Count == 0) return;
            int id = (int)_lv.SelectedItems[0].Tag;
            var r = MessageBox.Show("删掉这条记录？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
            {
                TimerService.DelHis(id);
                LoadData();
            }
        }

        private void ClearAll()
        {
            if (_data.Count == 0) return;
            var r = MessageBox.Show("确定删光 " + _data.Count + " 条？找不回来的哦",
                "小心", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
            {
                TimerService.ClearHis();
                LoadData();
            }
        }
    }
}
