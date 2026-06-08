using System;
using System.Drawing;
using System.Windows.Forms;

namespace CountdownTimer.Forms
{
    public partial class AboutPage : UserControl
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Padding = new Padding(20);

            var titleLabel = new Label();
            titleLabel.Text = "关于";
            titleLabel.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(50, 50, 70);
            titleLabel.Location = new Point(20, 15);
            titleLabel.Size = new Size(300, 40);

            var card = new Panel();
            card.Location = new Point(20, 70);
            card.Size = new Size(500, 320);
            card.BackColor = Color.White;

            var nameLabel = new Label();
            nameLabel.Text = "多功能倒计时器";
            nameLabel.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            nameLabel.ForeColor = Color.FromArgb(50, 50, 70);
            nameLabel.TextAlign = ContentAlignment.MiddleCenter;
            nameLabel.Location = new Point(0, 30);
            nameLabel.Size = new Size(500, 35);

            var verLabel = new Label();
            verLabel.Text = "版本 1.0.0";
            verLabel.Font = new Font("Microsoft YaHei UI", 10F);
            verLabel.ForeColor = Color.Gray;
            verLabel.TextAlign = ContentAlignment.MiddleCenter;
            verLabel.Location = new Point(0, 65);
            verLabel.Size = new Size(500, 25);

            var descLabel = new Label();
            descLabel.Text = "一个功能丰富的倒计时器系统，支持单次倒计时、\n" +
                           "番茄钟工作法、多任务并行倒计时，\n" +
                           "并记录完整的操作历史。\n\n" +
                           "项目类型：C# Windows Forms 课程设计\n" +
                           "开发环境：.NET Framework 4.8\n" +
                           "开发语言：C#\n\n" +
                           "(c) 2026 学生课程设计作品";
            descLabel.Font = new Font("Microsoft YaHei UI", 10F);
            descLabel.ForeColor = Color.FromArgb(80, 80, 100);
            descLabel.TextAlign = ContentAlignment.MiddleCenter;
            descLabel.Location = new Point(20, 100);
            descLabel.Size = new Size(460, 200);

            card.Controls.Add(nameLabel);
            card.Controls.Add(verLabel);
            card.Controls.Add(descLabel);

            this.Controls.Add(titleLabel);
            this.Controls.Add(card);
        }
    }
}
