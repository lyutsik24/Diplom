using System;
using System.Linq;
using System.Text.RegularExpressions;
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
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || txtFullName.Text == "Введите Фамилию Имя Отчество" ||
                string.IsNullOrWhiteSpace(txtLogin.Text) || txtLogin.Text == "Введите логин" ||
                string.IsNullOrWhiteSpace(txtEmail.Text) || txtEmail.Text == "Введите почту" ||
                string.IsNullOrWhiteSpace(txtPassword_1.Text) || txtPassword_1.Text == "Введите пароль" ||
                string.IsNullOrWhiteSpace(txtPassword_2.Text) || txtPassword_2.Text == "Повторите пароль" ||
                string.IsNullOrWhiteSpace(txtSecret.Text) || txtSecret.Text == "Придумайте секретное слово")
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (!Regex.IsMatch(txtFullName.Text, "^[а-яА-Я\\s]+$"))
            {
                MessageBox.Show("Фамилия Имя Отчество должно содержать только русские буквы и пробелы", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (!Regex.IsMatch(txtLogin.Text, "^[a-zA-Z]+$"))
            {
                MessageBox.Show("Логин должен содержать только латинские буквы", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (!Regex.IsMatch(txtEmail.Text, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Некорректный email", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword_1.Text) || txtPassword_1.Text == "Введите пароль")
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButtons.OK);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword_2.Text) || txtPassword_2.Text == "Повторите пароль")
            {
                MessageBox.Show("Повторите пароль", "Ошибка", MessageBoxButtons.OK);
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

            try
            {
                Database.Registration(txtFullName, txtLogin, txtEmail, txtPassword_1,txtSecret);
                MessageBox.Show("Пользователь успешно создан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка создания пользователя: " + ex.Message);
            }
        }

        private void txtPassword_1_Enter(object sender, EventArgs e)
        {
            txtPassword_1.UseSystemPasswordChar = true;
        }

        private void txtPassword_1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword_1.Text) || txtPassword_1.Text == "Введите пароль")
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
            if (string.IsNullOrWhiteSpace(txtPassword_2.Text) || txtPassword_2.Text == "Повторите пароль")
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
            if (string.IsNullOrWhiteSpace(txtSecret.Text) || txtSecret.Text == "Придумайте секретное слово")
            {
                txtSecret.UseSystemPasswordChar = false;
            }
        }
    }
}
