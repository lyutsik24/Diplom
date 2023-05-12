using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            cmbGender.DisplayMember = "gender";
            cmbGender.ValueMember = "id";
        }

        private void FillEducationComboBox()
        {
            DataTable educationData = Database.GetEducations();
            cmbEducation.DataSource = educationData;
            cmbEducation.DisplayMember = "education_name";
            cmbEducation.ValueMember = "id";
        }

        private void FillDepartmentComboBox()
        {
            DataTable departmentData = Database.GetDepartments();
            cmbDepartment.DataSource = departmentData;
            cmbDepartment.DisplayMember = "department_name";
            cmbDepartment.ValueMember = "id";
        }

        private void FillPositionComboBox()
        {
            DataTable positionData = Database.GetPositions();
            cmbPosition.DataSource = positionData;
            cmbPosition.DisplayMember = "position_name";
            cmbPosition.ValueMember = "id";
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedDepartmentId = Convert.ToInt32(cmbDepartment.SelectedValue);

            DataView positionView = new DataView(Database.GetPositions());
            positionView.RowFilter = "id_department = " + selectedDepartmentId;

            cmbPosition.DataSource = positionView;
            cmbPosition.DisplayMember = "position_name";
            cmbPosition.ValueMember = "id";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int employeeId = Convert.ToInt32(pcode.Text);
            string LastName = txtLastName.Text;
            string FirstName = txtFirstName.Text;
            string MiddleName = txtMiddleName.Text; 
            int gender = (int)cmbGender.SelectedValue;
            DateTime dateOfBirth = dtpBirth.Value;
            int department = (int)cmbDepartment.SelectedValue;
            int position = (int)cmbPosition.SelectedValue;
            DateTime hireDate = dtpHire.Value;
            string address = txtAddress.Text;
            int education = (int)cmbEducation.SelectedValue;

            bool success = Database.UpdateEmployee(employeeId, LastName, FirstName, MiddleName, gender, dateOfBirth, department, position, hireDate, address, education);

            if (success)
            {
                MessageBox.Show("Данные успешно обновлены.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Database.FillDataGridViewEmployees(employeeList.dgvEmployees);
            }
            else
            {
                MessageBox.Show("Ошибка при обновлении данных.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string LastName = txtLastName.Text;
            string FirstName = txtFirstName.Text;
            string MiddleName = txtMiddleName.Text;
            int gender = (int)cmbGender.SelectedValue;
            DateTime dateOfBirth = dtpBirth.Value;
            int department = (int)cmbDepartment.SelectedValue;
            int position = (int)cmbPosition.SelectedValue;
            DateTime hireDate = dtpHire.Value;
            string address = txtAddress.Text;
            int education = (int)cmbEducation.SelectedValue;

            bool success = Database.InsertEmployee(LastName, FirstName, MiddleName, gender, dateOfBirth, department, position, hireDate, address, education);

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
