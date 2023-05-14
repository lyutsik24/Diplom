using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
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

            Database.FillDataGridViewVacations(dgvShedule);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Хотите сохранить данные в Excel файл?", "Подтверждение сохранения", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ExportToExcel(dgvShedule);
            }

        }

        private void ExportToExcel(DataGridView dgv)
        {
            // Путь к шаблону Excel файла
            string templatePath = @"F:\Учеба\Diplom\Diplom\example.xlsx";

            // Открытие диалогового окна сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "FORM T-7 Copy.xlsx";
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
                    int startColumn1 = 1;
                    int startColumn2 = 21;

                    // Запись данных из DGV в Excel
                    for (int row = 0; row < dgv.Rows.Count; row++)
                    {
                        // Запись значения первой колонки в Excel
                        object cellValue1 = dgv.Rows[row].Cells[0].Value;
                        worksheet.Cells[startRow + row, startColumn1].Value = cellValue1;

                        // Запись значения второй колонки в Excel
                        object cellValue2 = dgv.Rows[row].Cells[1].Value;
                        worksheet.Cells[startRow + row, startColumn2].Value = cellValue2;
                    }

                    // Сохранение изменений в новом Excel файле
                    excelPackage.Save();
                }

                MessageBox.Show("Данные сохранены в новый Excel файл: " + newFilePath, "Сохранено");
            }
        }
    }
}
