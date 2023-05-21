using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;
using System.Data;
using System.Drawing;
using MySqlX.XDevAPI.Relational;

namespace Diplom
{
    public partial class VacationSchedule : Form
    {
        private MainMenu mainMenu;

        public VacationSchedule(MainMenu form)
        {
            InitializeComponent();

            mainMenu = form;

            Database.FillDataGridViewVacations(dgvShedule);

            if (User.UserRole == "Пользователь")
            {
                btnExport.Visible = false;
            }
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

        private void ExportToExcel(DataGridView dgv)
        {
            // Путь к шаблону Excel файла
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string templatePath = Path.Combine(currentDirectory, "example.xlsx");

            // Открытие диалогового окна сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "FORM T-7.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string newFilePath = saveFileDialog.FileName;

                // Копирование шаблона в новый файл
                File.Copy(templatePath, newFilePath, true);

                // Открытие нового файла для записи данных
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(newFilePath)))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.First();

                    // Начальные позиции строк и столбцов в шаблоне Excel файла
                    int startRow = 19;
                    int dateRow = 12;
                    int startColumnA = 1;
                    int startColumnU = 21;
                    int startColumnAO = 41;
                    int startColumnCC = 81;
                    int currentDateColumn = 87;
                    int startColumnCN = 92;
                    int startColumnDA = 105;
                    int currentGodColumn = 106;

                    // Запись данных из DGV в Excel
                    for (int row = 0; row < dgv.Rows.Count; row++)
                    {
                        object cellValue1 = dgv.Rows[row].Cells["Отдел"].Value;
                        worksheet.Cells[startRow + row, startColumnA].Value = cellValue1;

                        object cellValue2 = dgv.Rows[row].Cells["Должность"].Value;
                        worksheet.Cells[startRow + row, startColumnU].Value = cellValue2;

                        object cellValue3 = dgv.Rows[row].Cells["Полное имя"].Value;
                        worksheet.Cells[startRow + row, startColumnAO].Value = cellValue3;

                        object cellValue4 = dgv.Rows[row].Cells["Табельный номер"].Value;
                        worksheet.Cells[startRow + row, startColumnCC].Value = cellValue4;

                        object cellValue5 = dgv.Rows[row].Cells["Кол-во дней отпуска"].Value;
                        worksheet.Cells[startRow + row, startColumnCN].Value = cellValue5;

                        object cellValue6 = dgv.Rows[row].Cells["Дата начала отпуска"].Value;
                        worksheet.Cells[startRow + row, startColumnDA].Value = cellValue6;
                    }

                    object cellValue7 = DateTime.Now;
                    worksheet.Cells[dateRow, currentDateColumn].Value = cellValue7;

                    object cellValue8 = DateTime.Now.Year;
                    worksheet.Cells[dateRow, currentGodColumn].Value = cellValue8;
                    // Сохранение изменений в новом Excel файле
                    excelPackage.Save();
                }

                MessageBox.Show("Данные сохранены в новый Excel файл: " + newFilePath, "Сохранено");
            }
        }

        private void txtphbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtphbSearch.Text.Trim();

            if (searchTerm == "Поиск...")
            {
                return;
            }

            DataTable dataTable = (DataTable)dgvShedule.DataSource;

            string filter = string.Format("Отдел LIKE '%{0}%' OR " +
                                          "Должность LIKE '%{0}%' OR " +
                                          "[Тип отпуска] LIKE '%{0}%' OR " +
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
            employeeModule.ShowDialog();
        }

        private void dgvShedule_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
