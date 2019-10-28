namespace TrainingScheduler
{
    partial class TrainingDetailsForm
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
            this.addTrainingBtn = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameEdit = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.coachLabel = new System.Windows.Forms.Label();
            this.traineesList = new System.Windows.Forms.ListBox();
            this.traineesLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // addTrainingBtn
            // 
            this.addTrainingBtn.AutoSize = true;
            this.addTrainingBtn.Location = new System.Drawing.Point(128, 295);
            this.addTrainingBtn.Name = "addTrainingBtn";
            this.addTrainingBtn.Size = new System.Drawing.Size(75, 49);
            this.addTrainingBtn.TabIndex = 0;
            this.addTrainingBtn.Text = "Создать\r\nтренировку\r\n\r\n";
            this.addTrainingBtn.UseVisualStyleBackColor = true;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(268, 16);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(57, 13);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Название";
            // 
            // nameEdit
            // 
            this.nameEdit.Enabled = false;
            this.nameEdit.Location = new System.Drawing.Point(12, 32);
            this.nameEdit.Name = "nameEdit";
            this.nameEdit.Size = new System.Drawing.Size(313, 20);
            this.nameEdit.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(12, 82);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(313, 20);
            this.textBox1.TabIndex = 4;
            // 
            // coachLabel
            // 
            this.coachLabel.AutoSize = true;
            this.coachLabel.Location = new System.Drawing.Point(281, 66);
            this.coachLabel.Name = "coachLabel";
            this.coachLabel.Size = new System.Drawing.Size(44, 13);
            this.coachLabel.TabIndex = 3;
            this.coachLabel.Text = "Тренер";
            // 
            // traineesList
            // 
            this.traineesList.FormattingEnabled = true;
            this.traineesList.Location = new System.Drawing.Point(12, 135);
            this.traineesList.Name = "traineesList";
            this.traineesList.Size = new System.Drawing.Size(313, 147);
            this.traineesList.TabIndex = 5;
            // 
            // traineesLabel
            // 
            this.traineesLabel.AutoSize = true;
            this.traineesLabel.Location = new System.Drawing.Point(264, 119);
            this.traineesLabel.Name = "traineesLabel";
            this.traineesLabel.Size = new System.Drawing.Size(61, 13);
            this.traineesLabel.TabIndex = 6;
            this.traineesLabel.Text = "Участники";
            // 
            // TrainingDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 356);
            this.Controls.Add(this.traineesLabel);
            this.Controls.Add(this.traineesList);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.coachLabel);
            this.Controls.Add(this.nameEdit);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.addTrainingBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TrainingDetailsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Информация о тренировке";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addTrainingBtn;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameEdit;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label coachLabel;
        private System.Windows.Forms.ListBox traineesList;
        private System.Windows.Forms.Label traineesLabel;
    }
}