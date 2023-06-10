using Diplom.Classess;
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
    public partial class EmployeeListUserForm : Form
    {
        private MainMenu mainMenu;

        public EmployeeListUserForm(MainMenu form)
        {
            InitializeComponent();

            mainMenu = form;

            FillComboBoxes();

            FillDetails();
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

        private void FillDetails()
        {
            int employeeID = User.EmployeeID;
            string lastName, firstName, middleName, gender, department, position, address, education;
            DateTime dateOfBirth, hireDate;

            Database.GetEmployeeDetails(employeeID, out lastName, out firstName, out middleName, out gender, out dateOfBirth,
                                        out department, out position, out hireDate, out address, out education);

            txtLastName.Text = lastName;
            txtFirstName.Text = firstName;
            txtMiddleName.Text = middleName;
            cmbGender.SelectedIndex = cmbGender.FindStringExact(gender);
            dtpBirth.Value = dateOfBirth;
            cmbDepartment.SelectedIndex = cmbDepartment.FindStringExact(department);
            cmbPosition.SelectedIndex = cmbPosition.FindStringExact(position);
            dtpHire.Value = hireDate;
            txtAddress.Text = address;
            cmbEducation.SelectedIndex = cmbEducation.FindStringExact(education);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int employeeID = User.EmployeeID;
            string lastName = txtLastName.Text;
            string firstName = txtFirstName.Text;
            string middleName = txtMiddleName.Text;
            int genderID = Convert.ToInt32(cmbGender.SelectedValue);
            DateTime dateOfBirth = dtpBirth.Value;
            int departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
            int positionID = Convert.ToInt32(cmbPosition.SelectedValue);
            DateTime hireDate = dtpHire.Value;
            string address = txtAddress.Text;
            int educationID = Convert.ToInt32(cmbEducation.SelectedValue);

            if (Validator.ValidateInputEmployee(lastName, firstName, middleName, genderID.ToString(), departmentID.ToString(), positionID.ToString(), educationID.ToString(), dateOfBirth, hireDate, address))
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите обновить данные?", "Подтверждение обновления данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = Database.UpdateEmployeeDetails(employeeID, lastName, firstName, middleName, genderID, dateOfBirth,
                                                                 departmentID, positionID, hireDate, address, educationID);

                    if (success)
                    {
                        MessageBox.Show("Данные успешно обновлены.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении данных.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
