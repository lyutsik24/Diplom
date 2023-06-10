using Diplom.Classess;
using System;
using System.Windows.Forms;

namespace Diplom
{
    public partial class ResetPassword : Form
    {
        FormDraggable formDraggable = new FormDraggable();

        public ResetPassword()
        {
            InitializeComponent();

            formDraggable.Attach(pnlControl);
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            var formAuth = new Authorization();
            formAuth.Show();
        }

        private void txtPassword_1_Enter(object sender, EventArgs e)
        {
            txtPassword_1.UseSystemPasswordChar = true;
        }

        private void txtPassword_1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword_1.Text) || txtPassword_1.Text == "Введите новый пароль")
            {
                txtPassword_1.UseSystemPasswordChar = false;
            }
        }

        private void txtPassword_2_Enter(object sender, EventArgs e)
        {
            txtPassword_2.UseSystemPasswordChar = true;
        }

        private void txtPassword_2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword_2.Text) || txtPassword_2.Text == "Повторите новый пароль")
            {
                txtPassword_2.UseSystemPasswordChar = false;
            }
        }

        private void txtSecret_Enter(object sender, EventArgs e)
        {
            txtSecret.UseSystemPasswordChar = true;
        }

        private void txtSecret_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSecret.Text) || txtSecret.Text == "Введите секретное слово")
            {
                txtSecret.UseSystemPasswordChar = false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (Validator.ValidatePasswordReset(txtLoginOrEmail.Text, txtSecret.Text, txtPassword_1.Text, txtPassword_2.Text))
            {
                try
                {
                    Database.ResetPassword(txtLoginOrEmail, txtSecret, txtPassword_1);
                    MessageBox.Show("Пароль успешно изменен!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка изменения пароля: " + ex.Message);
                }
            }
        }
    }
}
