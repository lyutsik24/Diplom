
namespace Diplom
{
    partial class Registration
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.txtEmail = new Diplom.TextBoxWithPlaceholder();
            this.txtFullName = new Diplom.TextBoxWithPlaceholder();
            this.txtPassword_2 = new Diplom.TextBoxWithPlaceholder();
            this.txtLogin = new Diplom.TextBoxWithPlaceholder();
            this.txtPassword_1 = new Diplom.TextBoxWithPlaceholder();
            this.txtSecret = new Diplom.TextBoxWithPlaceholder();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnMinimize);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 32);
            this.panel1.TabIndex = 25;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Image = global::Diplom.Properties.Resources.minimize_window_32px;
            this.btnMinimize.Location = new System.Drawing.Point(536, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(32, 32);
            this.btnMinimize.TabIndex = 2;
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::Diplom.Properties.Resources.close_window_32px;
            this.btnClose.Location = new System.Drawing.Point(568, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(32, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(595, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 368);
            this.panel2.TabIndex = 26;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(5, 368);
            this.panel3.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.label1.Location = new System.Drawing.Point(5, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(590, 50);
            this.label1.TabIndex = 28;
            this.label1.Text = "РЕГИСТРАЦИЯ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCreate
            // 
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnCreate.FlatAppearance.BorderSize = 0;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnCreate.Location = new System.Drawing.Point(67, 336);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(138, 41);
            this.btnCreate.TabIndex = 7;
            this.btnCreate.Text = "Создать";
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.btnBack.Location = new System.Drawing.Point(401, 336);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(138, 41);
            this.btnBack.TabIndex = 8;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(79)))), ((int)(((byte)(99)))));
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmail.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtEmail.Location = new System.Drawing.Point(67, 147);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtEmail.PlaceholderText = "Введите почту";
            this.txtEmail.Size = new System.Drawing.Size(472, 22);
            this.txtEmail.TabIndex = 2;
            // 
            // txtFullName
            // 
            this.txtFullName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(79)))), ((int)(((byte)(99)))));
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFullName.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtFullName.Location = new System.Drawing.Point(67, 100);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtFullName.PlaceholderText = "Введите Фамилию Имя Отчество";
            this.txtFullName.Size = new System.Drawing.Size(472, 22);
            this.txtFullName.TabIndex = 1;
            // 
            // txtPassword_2
            // 
            this.txtPassword_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(79)))), ((int)(((byte)(99)))));
            this.txtPassword_2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword_2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtPassword_2.Location = new System.Drawing.Point(311, 241);
            this.txtPassword_2.Name = "txtPassword_2";
            this.txtPassword_2.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtPassword_2.PlaceholderText = "Повторите пароль";
            this.txtPassword_2.Size = new System.Drawing.Size(228, 22);
            this.txtPassword_2.TabIndex = 5;
            this.txtPassword_2.Enter += new System.EventHandler(this.txtPassword_2_Enter);
            this.txtPassword_2.Leave += new System.EventHandler(this.txtPassword_2_Leave);
            // 
            // txtLogin
            // 
            this.txtLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(79)))), ((int)(((byte)(99)))));
            this.txtLogin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLogin.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtLogin.Location = new System.Drawing.Point(67, 194);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtLogin.PlaceholderText = "Введите логин";
            this.txtLogin.Size = new System.Drawing.Size(472, 22);
            this.txtLogin.TabIndex = 3;
            // 
            // txtPassword_1
            // 
            this.txtPassword_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(79)))), ((int)(((byte)(99)))));
            this.txtPassword_1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword_1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtPassword_1.Location = new System.Drawing.Point(67, 241);
            this.txtPassword_1.Name = "txtPassword_1";
            this.txtPassword_1.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtPassword_1.PlaceholderText = "Введите пароль";
            this.txtPassword_1.Size = new System.Drawing.Size(228, 22);
            this.txtPassword_1.TabIndex = 4;
            this.txtPassword_1.Enter += new System.EventHandler(this.txtPassword_1_Enter);
            this.txtPassword_1.Leave += new System.EventHandler(this.txtPassword_1_Leave);
            // 
            // txtSecret
            // 
            this.txtSecret.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(79)))), ((int)(((byte)(99)))));
            this.txtSecret.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSecret.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtSecret.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtSecret.Location = new System.Drawing.Point(67, 288);
            this.txtSecret.Name = "txtSecret";
            this.txtSecret.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtSecret.PlaceholderText = "Придумайте секретное слово";
            this.txtSecret.Size = new System.Drawing.Size(472, 22);
            this.txtSecret.TabIndex = 6;
            this.txtSecret.Enter += new System.EventHandler(this.txtSecret_Enter);
            this.txtSecret.Leave += new System.EventHandler(this.txtSecret_Leave);
            // 
            // Registration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.txtPassword_2);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.txtPassword_1);
            this.Controls.Add(this.txtSecret);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Registration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registration";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private TextBoxWithPlaceholder txtSecret;
        private TextBoxWithPlaceholder txtPassword_1;
        private TextBoxWithPlaceholder txtLogin;
        private TextBoxWithPlaceholder txtPassword_2;
        private TextBoxWithPlaceholder txtFullName;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnBack;
        private TextBoxWithPlaceholder txtEmail;
    }
}