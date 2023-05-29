using System;
using System.Data;
using System.Windows.Forms;

namespace Diplom
{
    public partial class VacationSheduleModule : Form
    {
        private VacationSchedule vacationSchedule;

        public VacationSheduleModule(VacationSchedule form)
        {
            InitializeComponent();

            FillComboBoxes();

            vacationSchedule = form;

            txtReason.Enabled = false;
            cmbStatus.Visible = false;
            label5.Visible = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите отправить запрос?", "Подтверждение добавления данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int idEmployee = User.EmployeeID;
                int duration = Convert.ToInt32(txtDuration.Text);
                DateTime startVacation = dtpStartVacation.Value;
                int vacationType = (int)cmbVacationType.SelectedValue;
                string reason = txtReason.Text;

                bool success = Database.InsertVacation(idEmployee, duration, startVacation, vacationType, reason);

                if (success)
                {
                    MessageBox.Show("Запрос успешно отправлен.", "Добавление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Database.FillDataGridViewVacations(vacationSchedule.dgvShedule);
                    Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при отправке запроса.", "Добавление данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FillComboBoxes()
        {
            FillComboBox(cmbStatus, "status_id", "status_name", Database.GetStatusList());
            FillComboBox(cmbVacationType, "vacation_type_id", "vacation_type_name", Database.GetVacationType());
        }

        private void FillComboBox(ComboBox comboBox, string valueMember, string displayMember, DataTable data)
        {
            comboBox.DataSource = data;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
        }

        private void cmbVacationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtReason.Enabled = cmbVacationType.SelectedIndex == 1;
        }

        public void SetVacationData(int vacationId, string duration, DateTime dateOfStart, string reason)
        {
            pcode.Text = vacationId.ToString();
            txtDuration.Text = duration;
            dtpStartVacation.Value = dateOfStart;
            txtReason.Text = reason;
        }

        private void txtDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите изменить статус?", "Подтверждение Обновления данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int vacationId = Convert.ToInt32(pcode.Text);
                int status = (int)cmbStatus.SelectedValue;

                bool success = Database.UpdateVacationStatus(vacationId, status);

                if (success)
                {
                    MessageBox.Show("Статус успешно обновлен.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Database.FillDataGridViewVacations(vacationSchedule.dgvShedule);
                    Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при обновлении статуса.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
