using Diplom.Classess;
using System;
using System.Windows.Forms;

namespace Diplom
{
    public partial class SettingsForm : Form
    {
        private MainMenu mainMenu;

        public SettingsForm(MainMenu form)
        {
            InitializeComponent();

            FillTextBoxes();

            mainMenu = form;

            txtPassword.UseSystemPasswordChar = true;
            txtNewPassword_1.UseSystemPasswordChar = true;
            txtNewPassword_2.UseSystemPasswordChar = true;
            txtSecret.UseSystemPasswordChar = true;
        }

        private void FillTextBoxes()
        {
            int userId = User.UserID;

            txtLogin.Text = Database.GetLogin(userId);
            txtEmail.Text = Database.GetEmail(userId);
            txtPhoneNumber.Text = Database.GetPhoneNumber(userId);
            txtPassword.Text = Database.GetPasswordHash(userId);
            txtSecret.Text = Database.GetSecretWordHash(userId);
        }

        private void btnUpdateData_Click(object sender, EventArgs e)
        {
            int userId = User.UserID;

            string currentLogin = Database.GetLogin(userId);
            string currentEmail = Database.GetEmail(userId);
            string currentPhoneNumber = Database.GetPhoneNumber(userId);

            string newLogin = txtLogin.Text;
            string newEmail = txtEmail.Text;
            string newPhoneNumber = txtPhoneNumber.Text;

            bool isLoginChanged = !string.Equals(currentLogin, newLogin);
            bool isEmailChanged = !string.Equals(currentEmail, newEmail);
            bool isPhoneNumberChanged = !string.Equals(currentPhoneNumber, newPhoneNumber);

            if (!isLoginChanged && !isEmailChanged && !isPhoneNumberChanged)
            {
                MessageBox.Show("Нет изменений для обновления.", "Внимание", MessageBoxButtons.OK);
                return;
            }

            if (Validator.ValidateUserData(newLogin, newEmail, newPhoneNumber))
            {
                try
                {
                    if (isLoginChanged)
                    {
                        Database.UpdateLogin(newLogin, userId);
                    }

                    if (isEmailChanged)
                    {
                        Database.UpdateEmail(newEmail, userId);
                    }

                    if (isPhoneNumberChanged)
                    {
                        Database.UpdatePhoneNumber(newPhoneNumber, userId);
                    }

                    MessageBox.Show("Данные успешно обновлены!");
                    FillTextBoxes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления данных: " + ex.Message);
                }
            }
        }

        private void btnUpdatePasswordAndSecret_Click(object sender, EventArgs e)
        {
            int userId = User.UserID;

            string currentPasswordHash = Database.GetPasswordHash(userId);
            string currentSecretHash = Database.GetSecretWordHash(userId);

            string newPassword = txtNewPassword_1.Text;
            string newPasswordConfirm = txtNewPassword_2.Text;
            string newSecret = txtSecret.Text;

            bool isPasswordChanged = !string.IsNullOrEmpty(newPassword) && newPassword != currentPasswordHash;
            bool isSecretChanged = !string.IsNullOrEmpty(newSecret) && newSecret != currentSecretHash;

            if (!isPasswordChanged && !isSecretChanged)
            {
                MessageBox.Show("Нет изменений для обновления.", "Внимание", MessageBoxButtons.OK);
                return;
            }

            if (!Validator.ValidatePasswordAndSecret(newPassword, newPasswordConfirm, newSecret))
            {
                return;
            }

            try
            {
                if (isPasswordChanged)
                {
                    string newPasswordHash = HashHelper.HashPassword(newPassword);
                    Database.UpdatePassword(newPasswordHash, userId);
                    txtNewPassword_1.Clear();
                    txtNewPassword_2.Clear();
                }

                if (isSecretChanged)
                {
                    string newSecretHash = HashHelper.HashSecretWord(newSecret);
                    Database.UpdateSecretWord(newSecretHash, userId);
                }

                MessageBox.Show("Данные успешно обновлены!");
                FillTextBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления данных: " + ex.Message);
            }
        }
    }
}
