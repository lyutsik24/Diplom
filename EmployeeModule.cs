using Diplom.Classess;
using System;
using System.Data;
using System.Windows.Forms;

namespace Diplom
{
    public partial class EmployeeModule : Form
    {
        private EmployeeListAdminForm employeeList;

        FormDraggable formDraggable = new FormDraggable();

        public EmployeeModule(EmployeeListAdminForm form)
        {
            InitializeComponent();

            employeeList = form;

            FillComboBoxes();

            formDraggable.Attach(lblName);
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

        private void FillComboBoxes()
        {
            FillComboBox(cmbGender, "gender_id", "gender_name", Database.GetGenders());
            FillComboBox(cmbEducation, "education_id", "education_name", Database.GetEducations());
            FillComboBox(cmbDepartment, "department_id", "department_name", Database.GetDepartments());
            FillComboBox(cmbPosition, "position_id", "position_name", Database.GetPositions());
        }

        private void FillComboBox(ComboBox comboBox, string valueMember, string displayMember, DataTable data)
        {
            comboBox.DataSource = data;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
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
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string lastName = txtLastName.Text;
            string firstName = txtFirstName.Text;
            string middleName = txtMiddleName.Text;
            object gender = cmbGender.SelectedValue;
            object department = cmbDepartment.SelectedValue;
            object position = cmbPosition.SelectedValue;
            object education = cmbEducation.SelectedValue;
            DateTime dateOfBirth = dtpBirth.Value;
            DateTime hireDate = dtpHire.Value;
            string address = txtAddress.Text;

            if (Validator.ValidateInputEmployee(lastName, firstName, middleName, gender, department, position, education, dateOfBirth, hireDate, address))
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите обновить данные?", "Подтверждение обновления данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int employeeId = Convert.ToInt32(pcode.Text);
                    int genderId = Convert.ToInt32(gender);
                    int departmentId = Convert.ToInt32(department);
                    int positionId = Convert.ToInt32(position);
                    int educationId = Convert.ToInt32(education);

                    bool success = Database.UpdateEmployee(employeeId, lastName, firstName, middleName, genderId, dateOfBirth, departmentId, positionId, hireDate, address, educationId);

                    if (success)
                    {
                        MessageBox.Show("Данные успешно обновлены.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Database.FillDataGridViewEmployees(employeeList.dgvEmployees);
                        Close();
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
            string lastName = txtLastName.Text;
            string firstName = txtFirstName.Text;
            string middleName = txtMiddleName.Text;
            object gender = cmbGender.SelectedValue;
            object department = cmbDepartment.SelectedValue;
            object position = cmbPosition.SelectedValue;
            object education = cmbEducation.SelectedValue;
            DateTime dateOfBirth = dtpBirth.Value;
            DateTime hireDate = dtpHire.Value;
            string address = txtAddress.Text;

            if (Validator.ValidateInputEmployee(lastName, firstName, middleName, gender, department, position, education, dateOfBirth, hireDate, address))
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите добавить данные?", "Подтверждение добавления данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int genderId = Convert.ToInt32(gender);
                    int departmentId = Convert.ToInt32(department);
                    int positionId = Convert.ToInt32(position);
                    int educationId = Convert.ToInt32(education);

                    bool success = Database.InsertEmployee(lastName, firstName, middleName, genderId, dateOfBirth, departmentId, positionId, hireDate, address, educationId);

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
