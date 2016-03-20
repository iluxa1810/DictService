namespace FormClient
{
    partial class MainForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Dictionaries = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dgvDict = new System.Windows.Forms.DataGridView();
            this.Dictionary_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FriendlyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dgvTask = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnVersionControl = new System.Windows.Forms.Button();
            this.btnRefreshData = new System.Windows.Forms.Button();
            this.btnEditDict = new System.Windows.Forms.Button();
            this.btnAddDict = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.Links = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.dgvTasks = new System.Windows.Forms.DataGridView();
            this.LinkTaskId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTaskProjectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTaskServerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTaskState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.dgvProjects = new System.Windows.Forms.DataGridView();
            this.Project_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectServerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnLinkDict = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.Permissions = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.Dictionaries.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDict)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTask)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.Links.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjects)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Dictionaries);
            this.tabControl1.Controls.Add(this.Links);
            this.tabControl1.Controls.Add(this.Permissions);
            this.tabControl1.Location = new System.Drawing.Point(-1, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1251, 460);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // Dictionaries
            // 
            this.Dictionaries.Controls.Add(this.groupBox6);
            this.Dictionaries.Controls.Add(this.groupBox3);
            this.Dictionaries.Controls.Add(this.groupBox2);
            this.Dictionaries.Controls.Add(this.groupBox1);
            this.Dictionaries.Location = new System.Drawing.Point(4, 22);
            this.Dictionaries.Name = "Dictionaries";
            this.Dictionaries.Padding = new System.Windows.Forms.Padding(3);
            this.Dictionaries.Size = new System.Drawing.Size(1243, 434);
            this.Dictionaries.TabIndex = 0;
            this.Dictionaries.Text = "Словари";
            this.Dictionaries.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dgvDict);
            this.groupBox6.Location = new System.Drawing.Point(6, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(892, 353);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Словари";
            // 
            // dgvDict
            // 
            this.dgvDict.AllowUserToAddRows = false;
            this.dgvDict.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDict.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDict.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Dictionary_id,
            this.FriendlyName,
            this.CategoryName,
            this.Description,
            this.State});
            this.dgvDict.Location = new System.Drawing.Point(3, 19);
            this.dgvDict.MultiSelect = false;
            this.dgvDict.Name = "dgvDict";
            this.dgvDict.ReadOnly = true;
            this.dgvDict.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvDict.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDict.Size = new System.Drawing.Size(883, 331);
            this.dgvDict.TabIndex = 3;
            // 
            // Dictionary_id
            // 
            this.Dictionary_id.HeaderText = "Идентификатор";
            this.Dictionary_id.Name = "Dictionary_id";
            this.Dictionary_id.ReadOnly = true;
            // 
            // FriendlyName
            // 
            this.FriendlyName.HeaderText = "Название";
            this.FriendlyName.Name = "FriendlyName";
            this.FriendlyName.ReadOnly = true;
            // 
            // CategoryName
            // 
            this.CategoryName.HeaderText = "Категория";
            this.CategoryName.Name = "CategoryName";
            this.CategoryName.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.HeaderText = "Описание";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // State
            // 
            this.State.HeaderText = "Состояние";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.dgvTask);
            this.groupBox3.Location = new System.Drawing.Point(904, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(333, 426);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Фоновые задачи";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(20, 379);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "Удалить";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(138, 379);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Очистить все";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(252, 379);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Подробнее";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // dgvTask
            // 
            this.dgvTask.AllowUserToAddRows = false;
            this.dgvTask.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTask.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTask.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.TaskName,
            this.Status});
            this.dgvTask.Location = new System.Drawing.Point(6, 19);
            this.dgvTask.Name = "dgvTask";
            this.dgvTask.ReadOnly = true;
            this.dgvTask.Size = new System.Drawing.Size(321, 334);
            this.dgvTask.TabIndex = 1;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // TaskName
            // 
            this.TaskName.HeaderText = "Название";
            this.TaskName.Name = "TaskName";
            this.TaskName.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.HeaderText = "Статус";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbCategory);
            this.groupBox2.Location = new System.Drawing.Point(669, 362);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(229, 67);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Навигация";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Категория";
            // 
            // cbCategory
            // 
            this.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.Items.AddRange(new object[] {
            "Все"});
            this.cbCategory.Location = new System.Drawing.Point(102, 28);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(121, 21);
            this.cbCategory.TabIndex = 3;
            this.cbCategory.SelectedIndexChanged += new System.EventHandler(this.categoryBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnVersionControl);
            this.groupBox1.Controls.Add(this.btnRefreshData);
            this.groupBox1.Controls.Add(this.btnEditDict);
            this.groupBox1.Controls.Add(this.btnAddDict);
            this.groupBox1.Controls.Add(this.btnDownload);
            this.groupBox1.Location = new System.Drawing.Point(3, 356);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(632, 73);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Управление";
            // 
            // btnVersionControl
            // 
            this.btnVersionControl.Location = new System.Drawing.Point(324, 16);
            this.btnVersionControl.Name = "btnVersionControl";
            this.btnVersionControl.Size = new System.Drawing.Size(145, 23);
            this.btnVersionControl.TabIndex = 8;
            this.btnVersionControl.Text = "Контроль версий";
            this.btnVersionControl.UseVisualStyleBackColor = true;
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.Location = new System.Drawing.Point(108, 49);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(194, 23);
            this.btnRefreshData.TabIndex = 3;
            this.btnRefreshData.Text = "Обновить форму";
            this.btnRefreshData.UseVisualStyleBackColor = true;
            this.btnRefreshData.Click += new System.EventHandler(this.refreshBtnData_Click);
            // 
            // btnEditDict
            // 
            this.btnEditDict.Location = new System.Drawing.Point(108, 16);
            this.btnEditDict.Name = "btnEditDict";
            this.btnEditDict.Size = new System.Drawing.Size(194, 23);
            this.btnEditDict.TabIndex = 2;
            this.btnEditDict.Text = "Редактировать информацию";
            this.btnEditDict.UseVisualStyleBackColor = true;
            this.btnEditDict.Click += new System.EventHandler(this.editDictButton_Click);
            // 
            // btnAddDict
            // 
            this.btnAddDict.Location = new System.Drawing.Point(6, 16);
            this.btnAddDict.Name = "btnAddDict";
            this.btnAddDict.Size = new System.Drawing.Size(75, 23);
            this.btnAddDict.TabIndex = 0;
            this.btnAddDict.Text = "Добавить";
            this.btnAddDict.UseVisualStyleBackColor = true;
            this.btnAddDict.Click += new System.EventHandler(this.btnAddDict_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(6, 49);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "Скачать";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.bDownload_Click);
            // 
            // Links
            // 
            this.Links.Controls.Add(this.groupBox8);
            this.Links.Controls.Add(this.groupBox7);
            this.Links.Controls.Add(this.groupBox5);
            this.Links.Controls.Add(this.groupBox4);
            this.Links.Location = new System.Drawing.Point(4, 22);
            this.Links.Name = "Links";
            this.Links.Padding = new System.Windows.Forms.Padding(3);
            this.Links.Size = new System.Drawing.Size(1243, 434);
            this.Links.TabIndex = 1;
            this.Links.Text = "Привязки";
            this.Links.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.dgvTasks);
            this.groupBox8.Location = new System.Drawing.Point(594, 6);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(548, 348);
            this.groupBox8.TabIndex = 9;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Задачи";
            // 
            // dgvTasks
            // 
            this.dgvTasks.AllowUserToAddRows = false;
            this.dgvTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LinkTaskId,
            this.LinkTaskProjectId,
            this.LinkTaskServerId,
            this.LinkTaskName,
            this.LinkTaskState});
            this.dgvTasks.Location = new System.Drawing.Point(6, 16);
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.ReadOnly = true;
            this.dgvTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTasks.Size = new System.Drawing.Size(536, 327);
            this.dgvTasks.TabIndex = 1;
            // 
            // LinkTaskId
            // 
            this.LinkTaskId.HeaderText = "Id задачи";
            this.LinkTaskId.Name = "LinkTaskId";
            this.LinkTaskId.ReadOnly = true;
            this.LinkTaskId.Visible = false;
            // 
            // LinkTaskProjectId
            // 
            this.LinkTaskProjectId.HeaderText = "LinkProjectId";
            this.LinkTaskProjectId.Name = "LinkTaskProjectId";
            this.LinkTaskProjectId.ReadOnly = true;
            this.LinkTaskProjectId.Visible = false;
            // 
            // LinkTaskServerId
            // 
            this.LinkTaskServerId.HeaderText = "LinkTaskServerId";
            this.LinkTaskServerId.Name = "LinkTaskServerId";
            this.LinkTaskServerId.ReadOnly = true;
            this.LinkTaskServerId.Visible = false;
            // 
            // LinkTaskName
            // 
            this.LinkTaskName.HeaderText = "Название задачи";
            this.LinkTaskName.Name = "LinkTaskName";
            this.LinkTaskName.ReadOnly = true;
            // 
            // LinkTaskState
            // 
            this.LinkTaskState.HeaderText = "Состояние";
            this.LinkTaskState.Name = "LinkTaskState";
            this.LinkTaskState.ReadOnly = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.dgvProjects);
            this.groupBox7.Location = new System.Drawing.Point(8, 3);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(544, 351);
            this.groupBox7.TabIndex = 8;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Проекты";
            // 
            // dgvProjects
            // 
            this.dgvProjects.AllowUserToAddRows = false;
            this.dgvProjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Project_id,
            this.ProjectServerId,
            this.ProjectName,
            this.ServerName});
            this.dgvProjects.Location = new System.Drawing.Point(6, 16);
            this.dgvProjects.MultiSelect = false;
            this.dgvProjects.Name = "dgvProjects";
            this.dgvProjects.ReadOnly = true;
            this.dgvProjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProjects.Size = new System.Drawing.Size(532, 330);
            this.dgvProjects.TabIndex = 0;
            this.dgvProjects.SelectionChanged += new System.EventHandler(this.dgvProjects_SelectionChanged);
            // 
            // Project_id
            // 
            this.Project_id.HeaderText = "Id проекта";
            this.Project_id.Name = "Project_id";
            this.Project_id.ReadOnly = true;
            this.Project_id.Visible = false;
            // 
            // ProjectServerId
            // 
            this.ProjectServerId.HeaderText = "ProjectServerId";
            this.ProjectServerId.Name = "ProjectServerId";
            this.ProjectServerId.ReadOnly = true;
            this.ProjectServerId.Visible = false;
            // 
            // ProjectName
            // 
            this.ProjectName.HeaderText = "Название проекта";
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            // 
            // ServerName
            // 
            this.ServerName.HeaderText = "Название сервера";
            this.ServerName.Name = "ServerName";
            this.ServerName.ReadOnly = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnLinkDict);
            this.groupBox5.Location = new System.Drawing.Point(248, 360);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(261, 71);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Действия";
            // 
            // btnLinkDict
            // 
            this.btnLinkDict.Location = new System.Drawing.Point(6, 19);
            this.btnLinkDict.Name = "btnLinkDict";
            this.btnLinkDict.Size = new System.Drawing.Size(176, 23);
            this.btnLinkDict.TabIndex = 2;
            this.btnLinkDict.Text = "Редактировать привязки";
            this.btnLinkDict.UseVisualStyleBackColor = true;
            this.btnLinkDict.Click += new System.EventHandler(this.btnLinkDict_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Controls.Add(this.comboBox2);
            this.groupBox4.Location = new System.Drawing.Point(9, 360);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(233, 71);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Навигация";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Проект";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Сервер";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(87, 44);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 4;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(87, 17);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 5;
            // 
            // Permissions
            // 
            this.Permissions.Location = new System.Drawing.Point(4, 22);
            this.Permissions.Name = "Permissions";
            this.Permissions.Padding = new System.Windows.Forms.Padding(3);
            this.Permissions.Size = new System.Drawing.Size(1243, 434);
            this.Permissions.TabIndex = 2;
            this.Permissions.Text = "Права";
            this.Permissions.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 465);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "Управление сервисом";
            this.tabControl1.ResumeLayout(false);
            this.Dictionaries.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDict)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTask)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.Links.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjects)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Dictionaries;
        private System.Windows.Forms.DataGridView dgvDict;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEditDict;
        private System.Windows.Forms.Button btnAddDict;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TabPage Links;
        private System.Windows.Forms.TabPage Permissions;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.DataGridView dgvProjects;
        private System.Windows.Forms.Button btnLinkDict;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dictionary_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn FriendlyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.Button btnRefreshData;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvTask;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnVersionControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTaskId;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTaskProjectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTaskServerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTaskState;
        private System.Windows.Forms.DataGridViewTextBoxColumn Project_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectServerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServerName;
    }
}

