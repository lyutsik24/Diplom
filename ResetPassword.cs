using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Diplom
{
    public partial class ResetPassword : Form
    {
        public ResetPassword()
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
            if (string.IsNullOrWhiteSpace(txtLoginOrEmail.Text) || txtLoginOrEmail.Text == "Введите логин или почту" ||
                string.IsNullOrWhiteSpace(txtSecret.Text) || txtSecret.Text == "Введите секретное слово" ||
                string.IsNullOrWhiteSpace(txtPassword_1.Text) || txtPassword_1.Text == "Введите новый пароль" ||
                string.IsNullOrWhiteSpace(txtPassword_2.Text) || txtPassword_2.Text == "Повторите новый пароль")
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (!Regex.IsMatch(txtLoginOrEmail.Text, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$") &&
                !Regex.IsMatch(txtLoginOrEmail.Text, "^[a-zA-Z]+$"))
            {
                MessageBox.Show("Некорректный логин или почта", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword_1.Text) || txtPassword_1.Text == "Введите пароль")
            {
                MessageBox.Show("Введите новый пароль", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword_2.Text) || txtPassword_2.Text == "Повторите пароль")
            {
                MessageBox.Show("Повторите новый пароль", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (txtPassword_1.Text != txtPassword_2.Text)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (txtPassword_1.Text.Any(Char.IsWhiteSpace))
            {
                MessageBox.Show("Пароль не может содержать пробелы", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (!Regex.IsMatch(txtPassword_1.Text, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Пароль может содержать только латинские буквы и цифры", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSecret.Text) || txtSecret.Text == "Введите секретное слово")
            {
                MessageBox.Show("Введите секретное слово", "Ошибка", MessageBoxButtons.OK);
                return;
            }

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
