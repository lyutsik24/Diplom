using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Diplom.Classess
{
    public class Validator
    {
        public static bool ValidateInputEmployee(string lastName, string firstName, string middleName, object gender, object department, object position, object education)
        {
            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(middleName)
                || gender == null || department == null || position == null || education == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public static bool ValidateLogin(string txtlogin, string txtPassword)
        {
            if (string.IsNullOrWhiteSpace(txtlogin) || string.IsNullOrWhiteSpace(txtPassword))
            {
                MessageBox.Show("Пожалуйста, заполните логин и пароль.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public static bool ValidateRegistration(string fullName, string login, string email, string password1, string password2, string secret)
        {
            if (string.IsNullOrWhiteSpace(fullName) || fullName == "Введите Фамилию Имя Отчество" ||
                string.IsNullOrWhiteSpace(login) || login == "Введите логин" ||
                string.IsNullOrWhiteSpace(email) || email == "Введите почту" ||
                string.IsNullOrWhiteSpace(password1) || password1 == "Введите пароль" ||
                string.IsNullOrWhiteSpace(password2) || password2 == "Повторите пароль" ||
                string.IsNullOrWhiteSpace(secret) || secret == "Придумайте секретное слово")
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(fullName, "^[а-яА-Я\\s]+$"))
            {
                MessageBox.Show("Фамилия Имя Отчество должно содержать только русские буквы и пробелы", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(login, "^[a-zA-Z]+$"))
            {
                MessageBox.Show("Логин должен содержать только латинские буквы", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Некорректный email", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (string.IsNullOrWhiteSpace(password1) || password1 == "Введите пароль")
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (string.IsNullOrWhiteSpace(password2) || password2 == "Повторите пароль")
            {
                MessageBox.Show("Повторите пароль", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (password1 != password2)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (password1.Any(Char.IsWhiteSpace))
            {
                MessageBox.Show("Пароль не может содержать пробелы", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(password1, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Пароль может содержать только латинские буквы и цифры", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(password1, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                MessageBox.Show("Пароль должен содержать хотя бы одну прописную букву, одну заглавную букву и одну цифру", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (Database.IsLoginExists(login))
            {
                throw new Exception("Данный логин уже занят.");
            }

            if (Database.IsEmailExists(email))
            {
                throw new Exception("Данный адрес электронной почты уже занят.");
            }

            if (password1.Length < 8)
            {
                throw new Exception("Пароль должен состоять минимум из 8 символов.");
            }

            return true;
        }

        public static void TogglePasswordVisibility(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == placeholder)
            {
                textBox.UseSystemPasswordChar = false;
            }
        }

        public static bool ValidatePasswordReset(string loginOrEmail, string secret, string password1, string password2)
        {
            if (string.IsNullOrWhiteSpace(loginOrEmail) || loginOrEmail == "Введите логин или почту" ||
                string.IsNullOrWhiteSpace(secret) || secret == "Введите секретное слово" ||
                string.IsNullOrWhiteSpace(password1) || password1 == "Введите новый пароль" ||
                string.IsNullOrWhiteSpace(password2) || password2 == "Повторите новый пароль")
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(loginOrEmail, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$") &&
                !Regex.IsMatch(loginOrEmail, "^[a-zA-Z]+$"))
            {
                MessageBox.Show("Некорректный логин или почта", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (password1 != password2)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (password1.Any(Char.IsWhiteSpace))
            {
                MessageBox.Show("Пароль не может содержать пробелы", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(password1, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Пароль может содержать только латинские буквы и цифры", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(password1, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                MessageBox.Show("Пароль должен содержать хотя бы одну прописную букву, одну заглавную букву и одну цифру", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (Database.IsUserExists(loginOrEmail))
            {
                throw new Exception("Пользователь с таким логином или электронной почтой не найден.");
            }
            if (Database.IsSecretWordCorrect(loginOrEmail, secret))
            {
                throw new Exception("Неверное секретное слово.");
            }
            if (password1.Length < 8)
            {
                throw new Exception("Пароль должен состоять минимум из 8 символов.");
            }

            return true;
        }
    }
}
