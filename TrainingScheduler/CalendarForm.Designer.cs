namespace TrainingScheduler
{
    partial class CalendarForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.calendar = new System.Windows.Forms.TableLayoutPanel();
            this.monthSelector = new System.Windows.Forms.Panel();
            this.curMonthLabel = new System.Windows.Forms.Label();
            this.prevMonthBtn = new System.Windows.Forms.Button();
            this.nextMonthBtn = new System.Windows.Forms.Button();
            this.monthSelector.SuspendLayout();
            this.SuspendLayout();
            // 
            // calendar
            // 
            this.calendar.ColumnCount = 8;
            this.calendar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.calendar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.calendar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.calendar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.calendar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.calendar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.calendar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.calendar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.calendar.Location = new System.Drawing.Point(12, 61);
            this.calendar.Margin = new System.Windows.Forms.Padding(0);
            this.calendar.Name = "calendar";
            this.calendar.RowCount = 1;
            this.calendar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 700F));
            this.calendar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692307F));
            this.calendar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 700F));
            this.calendar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 700F));
            this.calendar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 700F));
            this.calendar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 700F));
            this.calendar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 700F));
            this.calendar.Size = new System.Drawing.Size(771, 700);
            this.calendar.TabIndex = 0;
            // 
            // monthSelector
            // 
            this.monthSelector.Controls.Add(this.curMonthLabel);
            this.monthSelector.Controls.Add(this.prevMonthBtn);
            this.monthSelector.Controls.Add(this.nextMonthBtn);
            this.monthSelector.Location = new System.Drawing.Point(12, 17);
            this.monthSelector.Name = "monthSelector";
            this.monthSelector.Size = new System.Drawing.Size(240, 41);
            this.monthSelector.TabIndex = 2;
            // 
            // curMonthLabel
            // 
            this.curMonthLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curMonthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.curMonthLabel.Location = new System.Drawing.Point(29, 0);
            this.curMonthLabel.Name = "curMonthLabel";
            this.curMonthLabel.Size = new System.Drawing.Size(182, 41);
            this.curMonthLabel.TabIndex = 3;
            this.curMonthLabel.Text = "week";
            this.curMonthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // prevMonthBtn
            // 
            this.prevMonthBtn.AutoSize = true;
            this.prevMonthBtn.Dock = System.Windows.Forms.DockStyle.Left;
            this.prevMonthBtn.FlatAppearance.BorderSize = 0;
            this.prevMonthBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prevMonthBtn.Location = new System.Drawing.Point(0, 0);
            this.prevMonthBtn.Name = "prevMonthBtn";
            this.prevMonthBtn.Size = new System.Drawing.Size(29, 41);
            this.prevMonthBtn.TabIndex = 1;
            this.prevMonthBtn.Text = "◀ ";
            this.prevMonthBtn.UseVisualStyleBackColor = true;
            this.prevMonthBtn.Click += new System.EventHandler(this.prevWeekBtn_Click);
            // 
            // nextMonthBtn
            // 
            this.nextMonthBtn.AutoSize = true;
            this.nextMonthBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.nextMonthBtn.FlatAppearance.BorderSize = 0;
            this.nextMonthBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextMonthBtn.Location = new System.Drawing.Point(211, 0);
            this.nextMonthBtn.Name = "nextMonthBtn";
            this.nextMonthBtn.Size = new System.Drawing.Size(29, 41);
            this.nextMonthBtn.TabIndex = 0;
            this.nextMonthBtn.Text = "▶ ";
            this.nextMonthBtn.UseVisualStyleBackColor = true;
            this.nextMonthBtn.Click += new System.EventHandler(this.nextWeekBtn_Click);
            // 
            // CalendarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(795, 772);
            this.Controls.Add(this.monthSelector);
            this.Controls.Add(this.calendar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CalendarForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Training Scheduler";
            this.monthSelector.ResumeLayout(false);
            this.monthSelector.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel calendar;
        private System.Windows.Forms.Panel monthSelector;
        private System.Windows.Forms.Button nextMonthBtn;
        private System.Windows.Forms.Label curMonthLabel;
        private System.Windows.Forms.Button prevMonthBtn;
    }
}

