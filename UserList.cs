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
    public partial class UserList : Form
    {
        private MainMenu mainMenu;

        public UserList(MainMenu form)
        {
            InitializeComponent();

            mainMenu = form;

            Database.FillDataGridViewUsers(dgvUsers);

            timerUpdate.Start();
        }

        private void txtphbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtphbSearch.Text.Trim();

            if (searchTerm == "Поиск...")
            {
                return;
            }

            DataTable dataTable = (DataTable)dgvUsers.DataSource;

            string filter = string.Format("Логин LIKE '%{0}%' OR " +
                                          "[Электронная почта] LIKE '%{0}%' OR " +
                                          "[Полное имя] LIKE '%{0}%' OR " +
                                          "role_name LIKE '%{0}%'", searchTerm.Replace("'", "''"));

            dataTable.DefaultView.RowFilter = filter;

            if (dataTable.DefaultView.Count == 0)
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

        public static void SetupDataGridView(DataGridView dataGridView, DataTable dataTable)
        {
            // Проверяем наличие столбца "Роль" в DataGridView
            DataGridViewColumn existingRoleColumn = dataGridView.Columns["role_name"];

            if (existingRoleColumn == null)
            {
                // Создание столбца DataGridViewComboBoxColumn для редактирования значения роли
                DataGridViewComboBoxColumn roleColumn = new DataGridViewComboBoxColumn
                {
                    Name = "role_name",
                    DataPropertyName = "role_name",
                    HeaderText = "Роль",
                    DataSource = Database.GetRoleList(),
                    ValueMember = "role_name",
                    DisplayMember = "role_name",
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox,
                    SortMode = DataGridViewColumnSortMode.Automatic,
                    FlatStyle = FlatStyle.Flat,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        BackColor = Color.FromArgb(46, 51, 73),
                        ForeColor = Color.FromArgb(0, 126, 249)
                    },
                    HeaderCell = {
                        Style = {
                            Alignment = DataGridViewContentAlignment.MiddleCenter
                        }
                    }
                    };


                dataGridView.Columns.Add(roleColumn);
            }

            dataGridView.DataSource = dataTable;

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Columns["№"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            // Обновленный индекс столбцов
            dataGridView.Columns["№"].DisplayIndex = 0;
            dataGridView.Columns["Полное имя"].DisplayIndex = 1;
            dataGridView.Columns["Логин"].DisplayIndex = 2;
            dataGridView.Columns["Электронная почта"].DisplayIndex = 3;
            dataGridView.Columns["Номер телефона"].DisplayIndex = 4;
            dataGridView.Columns["role_name"].DisplayIndex = 5;

            // Установка начального выбранного значения в соответствии с данными в таблице
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)row.Cells["role_name"];
                string roleName = comboBoxCell.Value?.ToString();
                comboBoxCell.Value = roleName;
            }

            // Установка свойства ReadOnly для всех столбцов, кроме столбца "Роль"
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name != "role_name")
                {
                    column.ReadOnly = true;
                }
            }

            // Обработчик события CellPainting для изменения цвета шапки таблицы
            dataGridView.CellPainting += (sender, e) =>
            {
                if (e.RowIndex == -1 && e.ColumnIndex >= 0)
                {
                    // Шапка таблицы
                    using (Brush backColorBrush = new SolidBrush(Color.FromArgb(24, 30, 54))) // Цвет фона шапки
                    using (Brush foreColorBrush = new SolidBrush(Color.FromArgb(0, 126, 249))) // Цвет текста шапки
                    {
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                        e.Graphics.DrawString(e.Value?.ToString(), dataGridView.ColumnHeadersDefaultCellStyle.Font, foreColorBrush, e.CellBounds, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        e.Handled = true;
                    }
                }
            };
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Получение измененных данных из DataGridView
            DataTable dataTable = (DataTable)dgvUsers.DataSource;
            DataTable changes = dataTable.GetChanges();

            if (changes != null)
            {
                // Отправка изменений в базу данных
                Database.UpdateUserData(changes);

                // Обновление данных в DataGridView
                Database.FillDataGridViewUsers(dgvUsers);
                dataTable.AcceptChanges();
                dgvUsers.Refresh();

                MessageBox.Show("Данные успешно обновлены.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Нет измененных данных для обновления.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            Database.FillDataGridViewUsers(dgvUsers);
        }
    }
}
