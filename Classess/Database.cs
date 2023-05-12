using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplom
{
    public class Database
    {
        public static bool Authorization(TextBox txtLogin, TextBox txtPassword)
        {
            using (var cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            using (var cm = new MySqlCommand())
            {
                cm.Connection = cn;
                cm.CommandText = @"SELECT CONCAT(e.last_name, ' ', e.first_name, ' ', e.middle_name) AS full_name, r.role_name AS role, u.id AS user_id
                                   FROM users u
                                   JOIN roles r ON u.role_id = r.id
                                   JOIN employees e ON u.employee_id = e.id
                                   WHERE (u.login = @p_login OR u.email = @p_login) AND u.password = @p_password";

                cm.Parameters.AddWithValue("@p_login", txtLogin.Text);
                cm.Parameters.AddWithValue("@p_password", txtPassword.Text);

                var da = new MySqlDataAdapter(cm);
                var dt = new DataTable();

                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count != 1)
                    {
                        return false;
                    }

                    User.UserID = Convert.ToInt32(dt.Rows[0]["user_id"]);
                    User.UserFullName = dt.Rows[0]["full_name"].ToString();
                    User.UserRole = dt.Rows[0]["role"].ToString();
                    return true;
                }
                finally
                {
                    da.Dispose();
                    dt.Dispose();
                }
            }
        }

        public static void Registration(TextBox txtFullName, TextBox txtLogin, TextBox txtEmail, TextBox txtPassword_1, TextBox txtSecret)
        {
            if (IsLoginExists(txtLogin.Text))
            {
                throw new Exception("Данный логин уже занят.");
            }

            if (IsEmailExists(txtEmail.Text))
            {
                throw new Exception("Данный адрес электронной почты уже занят.");
            }

            if (txtPassword_1.Text.Length < 8)
            {
                throw new Exception("Пароль должен состоять минимум из 8 символов.");
            }

            string hashedPassword = HashHelper.HashPassword(txtPassword_1.Text);
            string hashedSecret = HashHelper.HashSecretWord(txtSecret.Text);

            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = @"INSERT INTO users (login, password, email, employee_id, secret_word)
                                 VALUES (@p_login, @p_password, @p_email, @p_employee, @p_secretword)";

                using (MySqlCommand cm = new MySqlCommand(query, cn))
                {
                    try
                    {
                        int employeeId = GetEmployeeId(txtFullName.Text);

                        cm.Parameters.AddWithValue("@p_login", txtLogin.Text);
                        cm.Parameters.AddWithValue("@p_password", hashedPassword);
                        cm.Parameters.AddWithValue("@p_email", txtEmail.Text);
                        cm.Parameters.AddWithValue("@p_employee", employeeId);
                        cm.Parameters.AddWithValue("@p_secretword", hashedSecret);

                        cn.Open();

                        cm.ExecuteNonQuery();
                    }
                    catch (ArgumentException ex)
                    {
                        throw ex;
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception($"Ошибка при выполнении запроса. {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Ошибка при регистрации сотрудника. {ex.Message}");
                    }
                }
            }
        }

        private static int GetEmployeeId(string fullName)
        {
            string[] fio = fullName.Split(' ');

            if (fio.Length != 3)
            {
                throw new ArgumentException("ФИО сотрудника введено некорректно.");
            }

            string lastName = fio[0];
            string firstName = fio[1];
            string middleName = fio[2];

            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                try
                {
                    cn.Open();

                    string accountQuery = @"SELECT COUNT(*) FROM users WHERE employee_id IN (SELECT id FROM employees WHERE first_name = @p_first_name AND middle_name = @p_middle_name AND last_name = @p_last_name)";
                    using (MySqlCommand accountCmd = new MySqlCommand(accountQuery, cn))
                    {
                        accountCmd.Parameters.AddWithValue("@p_first_name", firstName);
                        accountCmd.Parameters.AddWithValue("@p_middle_name", middleName);
                        accountCmd.Parameters.AddWithValue("@p_last_name", lastName);

                        int accountCount = Convert.ToInt32(accountCmd.ExecuteScalar());

                        if (accountCount >= 1)
                        {
                            throw new ArgumentException("У сотрудника уже есть аккаунт. Если вы забыли данные для входа перейдите на форму 'Восстановление пароля'");
                        }
                    }

                    string query = @"SELECT id FROM employees WHERE first_name = @p_first_name AND middle_name = @p_middle_name AND last_name = @p_last_name";
                    using (MySqlCommand cm = new MySqlCommand(query, cn))
                    {
                        cm.Parameters.AddWithValue("@p_first_name", firstName);
                        cm.Parameters.AddWithValue("@p_middle_name", middleName);
                        cm.Parameters.AddWithValue("@p_last_name", lastName);

                        object employeeId = cm.ExecuteScalar();

                        if (employeeId == null)
                        {
                            throw new ArgumentException("Сотрудник не найден.");
                        }

                        return (int)employeeId;
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception($"Ошибка при выполнении запроса. {ex.Message}");
                }
            }
        }

        private static bool IsLoginExists(string login)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = "SELECT COUNT(*) FROM users WHERE login = @p_login";
                using (MySqlCommand cm = new MySqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@p_login", login);
                    cn.Open();
                    int count = Convert.ToInt32(cm.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private static bool IsEmailExists(string email)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = "SELECT COUNT(*) FROM users WHERE email = @p_email";
                using (MySqlCommand cm = new MySqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@p_email", email);
                    cn.Open();
                    int count = Convert.ToInt32(cm.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public static void ResetPassword(TextBox loginOrEmail, TextBox secretWord, TextBox newPassword)
        {
            if (!IsUserExists(loginOrEmail.Text))
            {
                throw new Exception("Пользователь с таким логином или электронной почтой не найден.");
            }
            if (!IsSecretWordCorrect(loginOrEmail.Text, secretWord.Text))
            {
                throw new Exception("Неверное секретное слово.");
            }
            if (newPassword.Text.Length < 8)
            {
                throw new Exception("Пароль должен состоять минимум из 8 символов.");
            }

            string hashedPassword = HashHelper.HashPassword(newPassword.Text);

            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = @"UPDATE users SET password = @p_password WHERE login = @p_login OR email = @p_email";

                using (MySqlCommand cm = new MySqlCommand(query, cn))
                {
                    try
                    {
                        cm.Parameters.AddWithValue("@p_login", loginOrEmail.Text);
                        cm.Parameters.AddWithValue("@p_email", loginOrEmail.Text);
                        cm.Parameters.AddWithValue("@p_password", hashedPassword);

                        cn.Open();

                        cm.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception($"Ошибка при выполнении запроса. {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Ошибка при обновлении пароля. {ex.Message}");
                    }
                }
            }
        }

        private static bool IsUserExists(string loginOrEmail)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = @"SELECT COUNT(*) FROM users WHERE login = @p_login OR email = @p_email";

                using (MySqlCommand cm = new MySqlCommand(query, cn))
                {
                    try
                    {
                        cm.Parameters.AddWithValue("@p_login", loginOrEmail);
                        cm.Parameters.AddWithValue("@p_email", loginOrEmail);

                        cn.Open();

                        int count = Convert.ToInt32(cm.ExecuteScalar());

                        return count > 0;
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception($"Ошибка при выполнении запроса. {ex.Message}");
                    }
                }
            }
        }

        private static bool IsSecretWordCorrect(string loginOrEmail, string secretWord)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = @"SELECT secret_word FROM users WHERE login = @p_login OR email = @p_email";

                using (MySqlCommand cm = new MySqlCommand(query, cn))
                {
                    try
                    {
                        cm.Parameters.AddWithValue("@p_login", loginOrEmail);
                        cm.Parameters.AddWithValue("@p_email", loginOrEmail);

                        cn.Open();

                        string hashedSecret = Convert.ToString(cm.ExecuteScalar());

                        return HashHelper.VerifyHash(secretWord, hashedSecret);
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception($"Ошибка при выполнении запроса. {ex.Message}");
                    }
                }
            }
        }

        public static void FillDataGridViewUsers(DataGridView dataGridView)
        {
            string query = @"SELECT u.id AS '№', u.login AS 'Логин', u.email AS 'Электронная почта', u.phone_number AS 'Номер телефона',
                             CONCAT(e.last_name, ' ', e.first_name, ' ', e.middle_name) AS 'Полное имя', r.role_name
                             FROM users u
                             INNER JOIN employees e ON u.employee_id = e.id
                             INNER JOIN roles r ON u.role_id = r.id
                             ORDER BY u.id";

            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(query, cn))
                {
                    DataSet ds = new DataSet();

                    da.Fill(ds);

                    DataTable dt = ds.Tables[0];

                    UserList.SetupDataGridView(dataGridView, dt);
                }
            }
        }

        public static DataTable GetRoleList()
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = "SELECT role_name FROM roles";
                using (MySqlCommand command = new MySqlCommand(query, cn))
                {
                    cn.Open();
                    DataTable dataTable = new DataTable();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
        }

        public static void UpdateUserData(DataTable changes)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                cn.Open();

                using (MySqlDataAdapter da = new MySqlDataAdapter())
                {
                    da.UpdateCommand = new MySqlCommand("", cn);

                    foreach (DataRow row in changes.Rows)
                    {
                        da.UpdateCommand.CommandText = @"UPDATE users
                                                 SET role_id = (SELECT id FROM roles WHERE role_name = @role_name)
                                                 WHERE id = @id";

                        da.UpdateCommand.Parameters.Clear();
                        da.UpdateCommand.Parameters.Add("@role_name", MySqlDbType.VarChar, 50).Value = row["role_name"];
                        da.UpdateCommand.Parameters.Add("@id", MySqlDbType.Int32).Value = row["№"];

                        da.UpdateCommand.ExecuteNonQuery();
                    }
                }

                cn.Close();
            }
        }

        public static void FillDataGridViewEmployees(DataGridView dataGridView)
        {
            string query = @"SELECT e.id AS '№',
                                    e.last_name AS 'Фамилия',
                                    e.first_name AS 'Имя',
                                    e.middle_name AS 'Отчество',
                                    g.gender AS 'Пол',
                                    e.date_of_birth AS 'Дата рождения',
                                    d.department_name AS 'Отдел',
                                    p.position_name AS 'Должность',
                                    e.hire_date AS 'Дата приема',
                                    e.address AS 'Адрес',
                                    ed.education_name AS 'Образование'
                             FROM employees e
                             INNER JOIN genders g ON e.gender_id = g.id
                             INNER JOIN departments d ON e.department_id = d.id
                             INNER JOIN positions p ON e.position_id = p.id
                             INNER JOIN educations ed ON e.education_id = ed.id
                             ORDER BY e.id";

            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(query, cn))
                {
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView.DataSource = dt;

                    EmployeeList.SetupDataGridView(dataGridView, dt);
                }
            }
        }

        public static DataTable GetGenders()
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = "SELECT * FROM genders";
                using (MySqlCommand command = new MySqlCommand(query, cn))
                {
                    cn.Open();
                    DataTable dataTable = new DataTable();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
        }

        public static DataTable GetEducations()
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = "SELECT * FROM educations";
                using (MySqlCommand command = new MySqlCommand(query, cn))
                {
                    cn.Open();
                    DataTable dataTable = new DataTable();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
        }

        public static DataTable GetDepartments()
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = "SELECT * FROM departments";
                using (MySqlCommand command = new MySqlCommand(query, cn))
                {
                    cn.Open();
                    DataTable dataTable = new DataTable();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
        }

        public static DataTable GetPositions()
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
            {
                string query = "SELECT * FROM positions";
                using (MySqlCommand command = new MySqlCommand(query, cn))
                {
                    cn.Open();
                    DataTable dataTable = new DataTable();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
        }

        public static bool UpdateEmployee(int employeeId, string LastName, string FirstName, string MiddleName, int gender, DateTime dateOfBirth, int department, int position, DateTime hireDate, string address, int education)
        {
            bool success = false;

            try
            {
                using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
                {
                    cn.Open();

                    string query = @"UPDATE employees 
                                     SET last_name = @LastName, 
                                         first_name = @FirstName, 
                                         middle_name = @MiddleName, 
                                         gender_id = @GenderId, 
                                         date_of_birth = @DateOfBirth, 
                                         department_id = @DepartmentId, 
                                         position_id = @PositionId, 
                                         hire_date = @HireDate, 
                                         address = @Address, 
                                         education_id = @EducationId 
                                     WHERE id = @EmployeeId";

                    MySqlCommand command = new MySqlCommand(query, cn);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@MiddleName", MiddleName);
                    command.Parameters.AddWithValue("@GenderId", gender);
                    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    command.Parameters.AddWithValue("@DepartmentId", department);
                    command.Parameters.AddWithValue("@PositionId", position);
                    command.Parameters.AddWithValue("@HireDate", hireDate);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@EducationId", education);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных: " + ex.Message);
            }

            return success;
        }

        public static bool InsertEmployee(string LastName, string FirstName, string MiddleName, int gender, DateTime dateOfBirth, int department, int position, DateTime hireDate, string address, int education)
        {
            bool success = false;

            try
            {
                using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
                {
                    cn.Open();

                    string query = @"INSERT INTO employees 
                                        (last_name, first_name, middle_name, gender_id, date_of_birth, department_id, position_id, hire_date, address, education_id)
                                     VALUES 
                                        (@LastName, @FirstName, @MiddleName, @GenderId, @DateOfBirth, @DepartmentId, @PositionId, @HireDate, @Address, @EducationId)";

                    MySqlCommand command = new MySqlCommand(query, cn);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@MiddleName", MiddleName);
                    command.Parameters.AddWithValue("@GenderId", gender);
                    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    command.Parameters.AddWithValue("@DepartmentId", department);
                    command.Parameters.AddWithValue("@PositionId", position);
                    command.Parameters.AddWithValue("@HireDate", hireDate);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@EducationId", education);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных: " + ex.Message);
            }

            return success;
        }
    }
}
