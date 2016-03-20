namespace FormClient.Forms
{
    partial class EditDictionaryForm
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
            this.addCategoryBtn = new System.Windows.Forms.Button();
            this.resfreshBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.DescriptionRtb = new System.Windows.Forms.RichTextBox();
            this.categoryCb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FrendlyNameTb = new System.Windows.Forms.TextBox();
            this.uploadChkBx = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonFileDialog = new System.Windows.Forms.Button();
            this.tbFilePath = new System.Windows.Forms.TextBox();
            this.tbDictionaryId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // addCategoryBtn
            // 
            this.addCategoryBtn.Location = new System.Drawing.Point(433, 122);
            this.addCategoryBtn.Name = "addCategoryBtn";
            this.addCategoryBtn.Size = new System.Drawing.Size(21, 23);
            this.addCategoryBtn.TabIndex = 21;
            this.addCategoryBtn.Text = "+";
            this.addCategoryBtn.UseMnemonic = false;
            this.addCategoryBtn.UseVisualStyleBackColor = true;
            // 
            // resfreshBtn
            // 
            this.resfreshBtn.Location = new System.Drawing.Point(272, 271);
            this.resfreshBtn.Name = "resfreshBtn";
            this.resfreshBtn.Size = new System.Drawing.Size(75, 23);
            this.resfreshBtn.TabIndex = 20;
            this.resfreshBtn.Text = "Обновить";
            this.resfreshBtn.UseVisualStyleBackColor = true;
            this.resfreshBtn.Click += new System.EventHandler(this.resfreshBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Описание";
            // 
            // DescriptionRtb
            // 
            this.DescriptionRtb.Location = new System.Drawing.Point(189, 169);
            this.DescriptionRtb.Name = "DescriptionRtb";
            this.DescriptionRtb.Size = new System.Drawing.Size(238, 96);
            this.DescriptionRtb.TabIndex = 18;
            this.DescriptionRtb.Text = "";
            // 
            // categoryCb
            // 
            this.categoryCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.categoryCb.FormattingEnabled = true;
            this.categoryCb.Location = new System.Drawing.Point(189, 122);
            this.categoryCb.Name = "categoryCb";
            this.categoryCb.Size = new System.Drawing.Size(238, 21);
            this.categoryCb.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Категория";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Имя словаря";
            // 
            // FrendlyNameTb
            // 
            this.FrendlyNameTb.Location = new System.Drawing.Point(189, 86);
            this.FrendlyNameTb.Name = "FrendlyNameTb";
            this.FrendlyNameTb.Size = new System.Drawing.Size(238, 20);
            this.FrendlyNameTb.TabIndex = 14;
            // 
            // uploadChkBx
            // 
            this.uploadChkBx.AutoSize = true;
            this.uploadChkBx.Location = new System.Drawing.Point(470, 19);
            this.uploadChkBx.Name = "uploadChkBx";
            this.uploadChkBx.Size = new System.Drawing.Size(120, 17);
            this.uploadChkBx.TabIndex = 22;
            this.uploadChkBx.Text = "Обновить словарь";
            this.uploadChkBx.UseVisualStyleBackColor = true;
            this.uploadChkBx.CheckStateChanged += new System.EventHandler(this.uploadChkBx_CheckStateChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Путь к словарю";
            // 
            // buttonFileDialog
            // 
            this.buttonFileDialog.AutoEllipsis = true;
            this.buttonFileDialog.Enabled = false;
            this.buttonFileDialog.Location = new System.Drawing.Point(433, 15);
            this.buttonFileDialog.Name = "buttonFileDialog";
            this.buttonFileDialog.Size = new System.Drawing.Size(21, 23);
            this.buttonFileDialog.TabIndex = 24;
            this.buttonFileDialog.Text = "...";
            this.buttonFileDialog.UseVisualStyleBackColor = true;
            this.buttonFileDialog.Click += new System.EventHandler(this.buttonFileDialog_Click);
            // 
            // tbFilePath
            // 
            this.tbFilePath.Enabled = false;
            this.tbFilePath.Location = new System.Drawing.Point(189, 15);
            this.tbFilePath.Name = "tbFilePath";
            this.tbFilePath.Size = new System.Drawing.Size(238, 20);
            this.tbFilePath.TabIndex = 23;
            // 
            // tbDictionaryId
            // 
            this.tbDictionaryId.Enabled = false;
            this.tbDictionaryId.Location = new System.Drawing.Point(189, 52);
            this.tbDictionaryId.Name = "tbDictionaryId";
            this.tbDictionaryId.Size = new System.Drawing.Size(79, 20);
            this.tbDictionaryId.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "ID словаря";
            // 
            // EditDictionaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 306);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbDictionaryId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonFileDialog);
            this.Controls.Add(this.tbFilePath);
            this.Controls.Add(this.uploadChkBx);
            this.Controls.Add(this.addCategoryBtn);
            this.Controls.Add(this.resfreshBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DescriptionRtb);
            this.Controls.Add(this.categoryCb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FrendlyNameTb);
            this.Name = "EditDictionaryForm";
            this.Text = "Редактирование словаря";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addCategoryBtn;
        private System.Windows.Forms.Button resfreshBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox DescriptionRtb;
        private System.Windows.Forms.ComboBox categoryCb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FrendlyNameTb;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox uploadChkBx;
        private System.Windows.Forms.TextBox tbFilePath;
        private System.Windows.Forms.Button buttonFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDictionaryId;
        private System.Windows.Forms.Label label5;
    }
}