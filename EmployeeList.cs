using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Diplom
{
    public partial class EmployeeList : Form
    {
        private MainMenu mainMenu;

        public EmployeeList(MainMenu form)
        {
            InitializeComponent();

            mainMenu = form;

            UpdateDataGridView();

            timerUpdate.Start();
        }

        public static void SetupDataGridView(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Columns["№"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Обработчик события CellPainting для изменения цвета шапки таблицы
            dataGridView.CellPainting += (sender, e) =>
            {
                if (e.RowIndex == -1 && e.ColumnIndex >= 0)
                {
                    using (Brush backColorBrush = new SolidBrush(Color.FromArgb(24, 30, 54)))
                    using (Brush foreColorBrush = new SolidBrush(Color.FromArgb(0, 126, 249)))
                    {
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                        e.Graphics.DrawString(e.Value?.ToString(), dataGridView.ColumnHeadersDefaultCellStyle.Font, foreColorBrush, e.CellBounds, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        e.Handled = true;
                    }
                }
            };
        }

        private void txtphbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtphbSearch.Text.Trim();

            if (searchTerm == "Поиск...")
            {
                return;
            }

            DataTable dataTable = (DataTable)dgvEmployees.DataSource;

            string filter = string.Format("Фамилия LIKE '%{0}%' OR " +
                                          "Имя LIKE '%{0}%' OR " +
                                          "Отчество LIKE '%{0}%' OR " +
                                          "Пол LIKE '%{0}%' OR " +
                                          "Должность LIKE '%{0}%' OR " +
                                          "Образование LIKE '%{0}%' OR " +
                                          "Адрес LIKE '%{0}%' OR " +
                                          "Отдел LIKE '%{0}%'", searchTerm.Replace("'", "''"));

            dataTable.DefaultView.RowFilter = filter;

            if (dataTable.DefaultView.Count == 0)
            {
                MessageBox.Show("Ничего не найдено.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void txtphbSearch_Enter(object sender, EventArgs e)
        {
            if (txtphbSearch.Text == "Поиск...")
            {
                txtphbSearch.Text = "";
            }
        }

        private void txtphbSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtphbSearch.Text))
            {
                txtphbSearch.Text = "Поиск...";
            }
        }

        private void dgvEmployees_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvEmployees.Rows[e.RowIndex];

                EmployeeModule employeeModule = new EmployeeModule(this);

                int employeeId = Convert.ToInt32(selectedRow.Cells["№"].Value);
                string lastName = selectedRow.Cells["Фамилия"].Value.ToString();
                string firstName = selectedRow.Cells["Имя"].Value.ToString();
                string middleName = selectedRow.Cells["Отчество"].Value.ToString();
                employeeModule.cmbGender.SelectedIndex = employeeModule.cmbGender.FindStringExact(selectedRow.Cells["Пол"].Value.ToString());
                DateTime dateOfBirth = Convert.ToDateTime(selectedRow.Cells["Дата рождения"].Value);
                employeeModule.cmbDepartment.SelectedIndex = employeeModule.cmbDepartment.FindStringExact(selectedRow.Cells["Отдел"].Value.ToString());
                employeeModule.cmbPosition.SelectedIndex = employeeModule.cmbPosition.FindStringExact(selectedRow.Cells["Должность"].Value.ToString());
                DateTime hireDate = Convert.ToDateTime(selectedRow.Cells["Дата приема"].Value);
                string address = selectedRow.Cells["Адрес"].Value.ToString();
                employeeModule.cmbEducation.SelectedIndex = employeeModule.cmbEducation.FindStringExact(selectedRow.Cells["Образование"].Value.ToString());

                employeeModule.SetEmployeeData(employeeId, lastName, firstName, middleName, dateOfBirth, hireDate, address);

                employeeModule.btnInsert.Visible = false;

                employeeModule.ShowDialog();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EmployeeModule employeeModule = new EmployeeModule(this);
            employeeModule.btnUpdate.Visible = false;
            employeeModule.ShowDialog();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }

        private void UpdateDataGridView()
        {
            Database.FillDataGridViewEmployees(dgvEmployees);
        }
    }
}
