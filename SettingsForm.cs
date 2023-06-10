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

            InitializeTextBoxes();

            mainMenu = form;

            SetPasswordCharForTextBoxes();

            txtNewPassword_1.Enabled = false;
            txtNewPassword_2.Enabled = false;
        }

        private void InitializeTextBoxes()
        {
            int userId = User.UserID;

            txtLogin.Text = Database.GetLogin(userId);
            txtEmail.Text = Database.GetEmail(userId);
            txtPhoneNumber.Text = Database.GetPhoneNumber(userId);
        }

        private void SetPasswordCharForTextBoxes()
        {
            txtPassword.UseSystemPasswordChar = true;
            txtNewPassword_1.UseSystemPasswordChar = true;
            txtNewPassword_2.UseSystemPasswordChar = true;
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
                    InitializeTextBoxes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления данных: " + ex.Message);
                }
            }
        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            int userId = User.UserID;

            string currentPasswordHash = Database.GetPasswordHash(userId);

            string newPassword = txtNewPassword_1.Text;
            string newPasswordConfirm = txtNewPassword_2.Text;

            bool isPasswordChanged = !string.IsNullOrEmpty(newPassword) && newPassword != currentPasswordHash;

            if (!isPasswordChanged)
            {
                MessageBox.Show("Нет изменений для обновления.", "Внимание", MessageBoxButtons.OK);
                return;
            }

            if (!Validator.ValidatePassword(newPassword, newPasswordConfirm))
            {
                return;
            }

            try
            {
                if (isPasswordChanged)
                {
                    string newPasswordHash = HashHelper.HashPassword(newPassword);
                    Database.UpdatePassword(newPasswordHash, userId);
                    txtPassword.Clear();
                    txtNewPassword_1.Clear();
                    txtNewPassword_2.Clear();
                }

                MessageBox.Show("Данные успешно обновлены!");
                InitializeTextBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления данных: " + ex.Message);
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            int userId = User.UserID;
            string currentPasswordHash = Database.GetPasswordHash(userId);
            bool isCurrentPasswordValid = HashHelper.VerifyHash(txtPassword.Text, currentPasswordHash);

            bool isPasswordEntered = !string.IsNullOrEmpty(txtPassword.Text);

            if (isPasswordEntered)
            {
                txtNewPassword_1.Enabled = isCurrentPasswordValid;
                txtNewPassword_2.Enabled = isCurrentPasswordValid;
            }
            else
            {
                txtNewPassword_1.Enabled = false;
                txtNewPassword_2.Enabled = false;
            }
        }
    }
}
