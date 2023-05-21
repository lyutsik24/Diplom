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
    public partial class VacationSheduleModule : Form
    {
        private VacationSchedule vacationSchedule;

        public VacationSheduleModule(VacationSchedule form)
        {
            InitializeComponent();

            FillVacationType();

            vacationSchedule = form;

            txtReason.Enabled = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите отправить запрос?", "Подтверждение добавления данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int IdEmployee = User.EmployeeID;
                int Duration = Convert.ToInt32(txtDuration.Text);
                DateTime StartVacation = dtpStartVacation.Value;
                int VacationType = (int)cmbVacationType.SelectedValue;
                string Reason = txtReason.Text;

                bool success = Database.InsertVacation(IdEmployee, Duration, StartVacation, VacationType, Reason);

                if (success)
                {
                    MessageBox.Show("Запрос успешно отправлен.", "Добавление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Database.FillDataGridViewVacations(vacationSchedule.dgvShedule);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при отправке запроса.", "Добавление данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FillVacationType()
        {
            DataTable vacationtypeData = Database.GetVacationType();
            cmbVacationType.DataSource = vacationtypeData;
            cmbVacationType.DisplayMember = "vacation_type_name";
            cmbVacationType.ValueMember = "vacation_type_id";
        }

        private void cmbVacationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVacationType.SelectedIndex == 1)
            {
                txtReason.Enabled = true;
            }
            else
            {
                txtReason.Enabled = false;
            }
        }
    }
}
