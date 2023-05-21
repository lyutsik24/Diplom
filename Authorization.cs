using Diplom.Classess;
using System;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();

            txtPassword.UseSystemPasswordChar = true;

            txtLogin.Focus();

            txtLogin.Text = "lyuts24@mail.ru";
            txtPassword.Text = "350z0NADcR";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (Validator.ValidateLogin(txtLogin.Text, txtPassword.Text))
            {
                if (Database.Authorization(txtLogin, txtPassword))
                {
                    MainMenu formMain = new MainMenu();
                    formMain.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Входные данные некорректны. Пожалуйста, проверьте правильность введенного логина и пароля и повторите попытку.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void chbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chbShowPassword.Checked;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите закрыть программу?", "Подтверждение закрытия", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnRegistration_Click(object sender, EventArgs e)
        {
            Registration formReg = new Registration();
            formReg.Show();
            Hide();
        }

        private void lLblResetPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetPassword formRes = new ResetPassword();
            formRes.Show();
            Hide();
        }

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin.PerformClick();
                e.Handled = true;
            }
        }
    }
}
