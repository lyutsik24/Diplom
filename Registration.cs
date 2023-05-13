using Diplom.Classess;
using System;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
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
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var authorizationForm = new Authorization();
            authorizationForm.Show();
            Hide();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (Validator.ValidateRegistration(txtFullName.Text, txtLogin.Text, txtEmail.Text, txtPassword_1.Text, txtPassword_2.Text, txtSecret.Text))
            {
                try
                {
                    Database.Registration(txtFullName, txtLogin, txtEmail, txtPassword_1, txtSecret);
                    MessageBox.Show("Пользователь успешно создан!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка создания пользователя: " + ex.Message);
                }
            }
        }

        private void txtPassword_1_Enter(object sender, EventArgs e)
        {
            txtPassword_1.UseSystemPasswordChar = true;
        }

        private void txtPassword_1_Leave(object sender, EventArgs e)
        {
            Validator.TogglePasswordVisibility(txtPassword_1, "Введите пароль");
        }

        private void txtPassword_2_Enter(object sender, EventArgs e)
        {
            txtPassword_2.UseSystemPasswordChar = true;
        }

        private void txtPassword_2_Leave(object sender, EventArgs e)
        {
            Validator.TogglePasswordVisibility(txtPassword_2, "Повторите пароль");
        }

        private void txtSecret_Enter(object sender, EventArgs e)
        {
            txtSecret.UseSystemPasswordChar = true;
        }

        private void txtSecret_Leave(object sender, EventArgs e)
        {
            Validator.TogglePasswordVisibility(txtSecret, "Придумайте секретное слово");
        }

    }
}
