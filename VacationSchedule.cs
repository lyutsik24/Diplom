using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;

namespace Diplom
{
    public partial class VacationSchedule : Form
    {
        private MainMenu mainMenu;

        public VacationSchedule(MainMenu form)
        {
            InitializeComponent();

            mainMenu = form;

            string role = User.UserRole;
            Database.FillDataGridViewVacations(dgvShedule, role);

            dgvShedule.Columns["Причина"].Visible = false;

            btnExport.Visible = (User.UserRole != "Пользователь");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Хотите сохранить данные в Excel файл?", "Подтверждение сохранения", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ExportToExcel(dgvShedule);
            }
        }

        public static void SetupDataGridView(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.CellPainting += DataGridView_CellPainting;
        }

        private static void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                using (Brush backColorBrush = new SolidBrush(Color.FromArgb(24, 30, 54)))
                using (Brush foreColorBrush = new SolidBrush(Color.FromArgb(0, 126, 249)))
                {
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                    e.Graphics.DrawString(e.Value?.ToString(), e.CellStyle.Font, foreColorBrush, e.CellBounds, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    e.Handled = true;
                }
            }
        }

        private void ExportToExcel(DataGridView dgv)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "example.xlsx");

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx",
                FileName = "FORM T-7.xlsx"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            string newFilePath = saveFileDialog.FileName;

            File.Copy(templatePath, newFilePath, true);

            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(newFilePath)))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.First();

                int startRow = 19;
                int dateRow = 12;
                int startColumnA = 1;
                int startColumnU = 21;
                int startColumnAO = 41;
                int startColumnCC = 81;
                int currentDateColumn = 87;
                int startColumnCN = 92;
                int startColumnDA = 105;
                int currentYearColumn = 106;

                int rowIndex = 0;

                foreach (DataGridViewRow dataGridViewRow in dgv.Rows)
                {
                    string status = dataGridViewRow.Cells["Статус"].Value.ToString();
                    string vacationType = dataGridViewRow.Cells["Тип отпуска"].Value.ToString();

                    if (status == "Одобрен" && (vacationType == "Основной отпуск" || vacationType == "Дополнительный отпуск" || vacationType == "Социальный отпуск"))
                    {
                        worksheet.Cells[startRow + rowIndex, startColumnA].Value = dataGridViewRow.Cells["Отдел"].Value;
                        worksheet.Cells[startRow + rowIndex, startColumnU].Value = dataGridViewRow.Cells["Должность"].Value;
                        worksheet.Cells[startRow + rowIndex, startColumnAO].Value = dataGridViewRow.Cells["Полное имя"].Value;
                        worksheet.Cells[startRow + rowIndex, startColumnCC].Value = dataGridViewRow.Cells["Табельный номер"].Value;
                        worksheet.Cells[startRow + rowIndex, startColumnCN].Value = dataGridViewRow.Cells["Кол-во дней отпуска"].Value;
                        worksheet.Cells[startRow + rowIndex, startColumnDA].Value = dataGridViewRow.Cells["Дата начала отпуска"].Value;

                        rowIndex++;
                    }
                }

                worksheet.Cells[dateRow, currentDateColumn].Value = DateTime.Now;
                worksheet.Cells[dateRow, currentYearColumn].Value = DateTime.Now.Year;

                excelPackage.Save();
            }

            MessageBox.Show("Данные сохранены в новый Excel файл: " + newFilePath, "Сохранено");
        }

        private void txtphbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtphbSearch.Text.Trim();

            if (searchTerm == "Поиск...")
                return;

            DataTable dataTable = dgvShedule.DataSource as DataTable;

            string filter = string.Format("Отдел LIKE '%{0}%' OR " +
                                          "Должность LIKE '%{0}%' OR " +
                                          "[Тип отпуска] LIKE '%{0}%' OR " +
                                          "Статус LIKE '%{0}%' OR " +
                                          "[Полное имя] LIKE '%{0}%'", searchTerm.Replace("'", "''"));

            dataTable.DefaultView.RowFilter = filter;

            if (searchTerm.Length >= 3 && dataTable.DefaultView.Count == 0)
            {
                MessageBox.Show("Ничего не найдено.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtphbSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtphbSearch.Text))
            {
                txtphbSearch.Text = "Поиск...";
            }
        }

        private void txtphbSearch_Enter(object sender, EventArgs e)
        {
            if (txtphbSearch.Text == "Поиск...")
            {
                txtphbSearch.Text = "";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            VacationSheduleModule employeeModule = new VacationSheduleModule(this);
            employeeModule.btnUpdate.Visible = false;
            employeeModule.ShowDialog();
        }

        private void dgvShedule_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow selectedRow = dgvShedule.Rows[e.RowIndex];
            int vacationId = Convert.ToInt32(selectedRow.Cells["№"].Value);
            string duration = selectedRow.Cells["Кол-во дней отпуска"].Value.ToString();
            DateTime dateOfStart = Convert.ToDateTime(selectedRow.Cells["Дата начала отпуска"].Value);
            string reason = selectedRow.Cells["Причина"].Value.ToString();

            VacationSheduleModule vacationModule = new VacationSheduleModule(this);
            vacationModule.SetVacationData(vacationId, duration, dateOfStart, reason);

            vacationModule.cmbStatus.SelectedIndex = vacationModule.cmbStatus.FindStringExact(selectedRow.Cells["Статус"].Value.ToString());
            vacationModule.cmbVacationType.SelectedIndex = vacationModule.cmbVacationType.FindStringExact(selectedRow.Cells["Тип отпуска"].Value.ToString());

            string role = User.UserRole;

            if (role == "Пользователь")
            {
                vacationModule.cmbStatus.Enabled = false;
                vacationModule.btnUpdate.Visible = false;
            }

            vacationModule.cmbStatus.Visible = true;
            vacationModule.label5.Visible = true;
            vacationModule.cmbVacationType.Enabled = false;
            vacationModule.txtDuration.Enabled = false;
            vacationModule.txtReason.Enabled = false;
            vacationModule.dtpStartVacation.Enabled = false;
            vacationModule.btnSend.Visible = false;

            string vacationNumber = selectedRow.Cells["№"].Value.ToString();
            vacationModule.lblName.Text = "ЗАЯВКА НА ОТПУСК № " + vacationNumber;

            vacationModule.ShowDialog();
        }
    }
}
