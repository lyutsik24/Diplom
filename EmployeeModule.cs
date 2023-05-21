using Diplom.Classess;
using System;
using System.Data;
using System.Windows.Forms;

namespace Diplom
{
    public partial class EmployeeModule : Form
    {
        private EmployeeList employeeList;

        public EmployeeModule(EmployeeList form)
        {
            InitializeComponent();

            employeeList = form;

            FillGenderComboBox();
            FillEducationComboBox();
            FillDepartmentComboBox();
            FillPositionComboBox();
        }

        public void SetEmployeeData(int employeeId, string lastName, string firstName, string middleName, DateTime dateOfBirth, DateTime hireDate, string address)
        {
            pcode.Text = employeeId.ToString();
            txtLastName.Text = lastName;
            txtFirstName.Text = firstName;
            txtMiddleName.Text = middleName;
            dtpBirth.Value = dateOfBirth;
            dtpHire.Value = hireDate;
            txtAddress.Text = address;
        }

        private void FillGenderComboBox()
        {
            DataTable genderData = Database.GetGenders();
            cmbGender.DataSource = genderData;
            cmbGender.DisplayMember = "gender_name";
            cmbGender.ValueMember = "gender_id";
        }

        private void FillEducationComboBox()
        {
            DataTable educationData = Database.GetEducations();
            cmbEducation.DataSource = educationData;
            cmbEducation.DisplayMember = "education_name";
            cmbEducation.ValueMember = "education_id";
        }

        private void FillDepartmentComboBox()
        {
            DataTable departmentData = Database.GetDepartments();
            cmbDepartment.DataSource = departmentData;
            cmbDepartment.DisplayMember = "department_name";
            cmbDepartment.ValueMember = "department_id";
        }

        private void FillPositionComboBox()
        {
            DataTable positionData = Database.GetPositions();
            cmbPosition.DataSource = positionData;
            cmbPosition.DisplayMember = "position_name";
            cmbPosition.ValueMember = "position_id";
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedDepartmentId = Convert.ToInt32(cmbDepartment.SelectedValue);

            DataView positionView = new DataView(Database.GetPositions());
            positionView.RowFilter = "id_department = " + selectedDepartmentId;

            cmbPosition.DataSource = positionView;
            cmbPosition.DisplayMember = "position_name";
            cmbPosition.ValueMember = "position_id";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Validator.ValidateInputEmployee(txtLastName.Text, txtFirstName.Text, txtMiddleName.Text, cmbGender.SelectedValue, cmbDepartment.SelectedValue, cmbPosition.SelectedValue, cmbEducation.SelectedValue))
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите обновить данные?", "Подтверждение обновления данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int employeeId = Convert.ToInt32(pcode.Text);
                    string lastName = txtLastName.Text;
                    string firstName = txtFirstName.Text;
                    string middleName = txtMiddleName.Text;
                    int gender = (int)cmbGender.SelectedValue;
                    DateTime dateOfBirth = dtpBirth.Value;
                    int department = (int)cmbDepartment.SelectedValue;
                    int position = (int)cmbPosition.SelectedValue;
                    DateTime hireDate = dtpHire.Value;
                    string address = txtAddress.Text;
                    int education = (int)cmbEducation.SelectedValue;

                    bool success = Database.UpdateEmployee(employeeId, lastName, firstName, middleName, gender, dateOfBirth, department, position, hireDate, address, education);

                    if (success)
                    {
                        MessageBox.Show("Данные успешно обновлены.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Database.FillDataGridViewEmployees(employeeList.dgvEmployees);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении данных.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (Validator.ValidateInputEmployee(txtLastName.Text, txtFirstName.Text, txtMiddleName.Text, cmbGender.SelectedValue, cmbDepartment.SelectedValue, cmbPosition.SelectedValue, cmbEducation.SelectedValue))
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите добавить данные?", "Подтверждение добавления данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string lastName = txtLastName.Text;
                    string firstName = txtFirstName.Text;
                    string middleName = txtMiddleName.Text;
                    int gender = (int)cmbGender.SelectedValue;
                    DateTime dateOfBirth = dtpBirth.Value;
                    int department = (int)cmbDepartment.SelectedValue;
                    int position = (int)cmbPosition.SelectedValue;
                    DateTime hireDate = dtpHire.Value;
                    string address = txtAddress.Text;
                    int education = (int)cmbEducation.SelectedValue;

                    bool success = Database.InsertEmployee(lastName, firstName, middleName, gender, dateOfBirth, department, position, hireDate, address, education);

                    if (success)
                    {
                        MessageBox.Show("Данные успешно добавлены.", "Добавление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Database.FillDataGridViewEmployees(employeeList.dgvEmployees);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении данных.", "Добавление данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
