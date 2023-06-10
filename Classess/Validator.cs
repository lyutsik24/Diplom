using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Diplom.Classess
{
    public class Validator
    {
        public static bool ValidateInputEmployee(string lastName, string firstName, string middleName, object gender, object department, object position, object education, DateTime dateOfBirth, DateTime hireDate, string address)
        {
            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(middleName) || string.IsNullOrWhiteSpace(address)
                || gender == null || department == null || position == null || education == null || dateOfBirth == null || hireDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string namePattern = @"^[а-яА-ЯёЁ\s]+$";
            if (!Regex.IsMatch(lastName, namePattern) || !Regex.IsMatch(firstName, namePattern) || !Regex.IsMatch(middleName, namePattern))
            {
                MessageBox.Show("Пожалуйста, введите ФИО на русском языке без цифр и специальных символов.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string addressPattern = @"^[а-яА-ЯёЁ0-9\s.,/-]+$";
            if (!Regex.IsMatch(address, addressPattern))
            {
                MessageBox.Show("Пожалуйста, введите адрес на русском языке без специальных символов, кроме .,/-", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (IsEmail(txtlogin))
            {
                if (IsValidEmail(txtlogin) == false)
                {
                    MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                if (IsValidLogin(txtlogin) == false)
                {
                    MessageBox.Show("Пожалуйста, введите логин, содержащий только английские буквы и цифры.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private static bool IsEmail(string input)
        {
            return input.Contains("@");
        }

        private static bool IsValidEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private static bool IsValidLogin(string login)
        {
            string loginPattern = @"^[a-zA-Z0-9]+$";
            return Regex.IsMatch(login, loginPattern);
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

            if (!Regex.IsMatch(login, "^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Логин должен содержать только латинские буквы и цифры", "Ошибка", MessageBoxButtons.OK);
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
                MessageBox.Show("Данный логин уже занят.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (Database.IsEmailExists(email))
            {
                MessageBox.Show("Данный адрес электронной почты уже занят.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (password1.Length < 8)
            {
                MessageBox.Show("Пароль должен состоять минимум из 8 символов.", "Ошибка", MessageBoxButtons.OK);
                return false;
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
                !Regex.IsMatch(loginOrEmail, "^[a-zA-Z0-9]+$"))
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

            if (!Database.IsUserExists(loginOrEmail))
            {
                MessageBox.Show("Пользователь с таким логином или электронной почтой не найден.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Database.IsSecretWordCorrect(loginOrEmail, secret))
            {
                MessageBox.Show("Неверное секретное слово.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (password1.Length < 8)
            {
                MessageBox.Show("Пароль должен состоять минимум из 8 символов.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        public static bool ValidateUserData(string login, string email, string phoneNumber)
        {
            if (!Regex.IsMatch(email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Некорректный адрес электронной почты", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(login, "^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Некорректный логин", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            int userId = User.UserID;

            if (Database.IsLoginExists(login) && Database.GetLogin(userId) != login)
            {
                MessageBox.Show("Данный логин уже занят.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (Database.IsEmailExists(email) && Database.GetEmail(userId) != email)
            {
                MessageBox.Show("Данный адрес электронной почты уже занят.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        public static bool ValidatePassword(string password, string password2)
        {
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пароль не может быть пустым.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (string.IsNullOrEmpty(password2))
            {
                MessageBox.Show("Подтверждение пароля не может быть пустым.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Пароль должен содержать не менее 8 символов.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(password, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Пароль может содержать только латинские буквы и цифры", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                MessageBox.Show("Пароль должен содержать хотя бы одну прописную букву, одну заглавную букву и одну цифру", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (password.Contains(" "))
            {
                MessageBox.Show("Пароль не может содержать пробелы.", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            if (password != password2)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        public static bool ValidateInputVacation(string duration, DateTime startVacation, object vacationType, string reason)
        {
            if (string.IsNullOrWhiteSpace(duration) || startVacation == null || vacationType == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(duration, out int parsedDuration) || parsedDuration <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректную продолжительность отпуска.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int vacationTypeId = (int)vacationType;
            if (vacationTypeId == 2 && (string.IsNullOrWhiteSpace(reason) || !IsRussianLanguage(reason)))
            {
                MessageBox.Show("Пожалуйста, укажите причину отпуска без сохранения заработной платы на русском языке.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private static bool IsRussianLanguage(string text)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(text, @"^[\p{IsCyrillic}\d\s\-.,?!]+$");
        }
    }
}
