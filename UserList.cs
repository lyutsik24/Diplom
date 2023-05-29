using System;
using System.Data;
using System.Drawing;
using System.Linq;
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

            DataTable dataTable = dgvUsers.DataSource as DataTable;

            string filter = string.Format("Логин LIKE '%{0}%' OR " +
                                          "[Электронная почта] LIKE '%{0}%' OR " +
                                          "[Полное имя] LIKE '%{0}%' OR " +
                                          "role_name LIKE '%{0}%'", searchTerm.Replace("'", "''"));

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

        public static void SetupDataGridView(DataGridView dataGridView, DataTable dataTable)
        {
            if (dataGridView.Columns["role_name"] == null)
            {
                var roleColumn = new DataGridViewComboBoxColumn
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
                    HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
                };

                dataGridView.Columns.Add(roleColumn);
            }

            dataGridView.DataSource = dataTable;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Columns["№"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            SetColumnDisplayIndex(dataGridView, "№", 0);
            SetColumnDisplayIndex(dataGridView, "Полное имя", 1);
            SetColumnDisplayIndex(dataGridView, "Логин", 2);
            SetColumnDisplayIndex(dataGridView, "Электронная почта", 3);
            SetColumnDisplayIndex(dataGridView, "Номер телефона", 4);
            SetColumnDisplayIndex(dataGridView, "role_name", 5);

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                ((DataGridViewComboBoxCell)row.Cells["role_name"]).Value = ((DataGridViewComboBoxCell)row.Cells["role_name"]).Value?.ToString();
            }

            SetReadOnlyColumns(dataGridView, "role_name");

            dataGridView.CellPainting += DataGridView_CellPainting;
        }

        private static void SetColumnDisplayIndex(DataGridView dataGridView, string columnName, int displayIndex)
        {
            if (dataGridView.Columns.Contains(columnName))
                dataGridView.Columns[columnName].DisplayIndex = displayIndex;
        }

        private static void SetReadOnlyColumns(DataGridView dataGridView, params string[] excludedColumns)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (!excludedColumns.Contains(column.Name))
                    column.ReadOnly = true;
            }
        }

        private static void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                using (Brush backColorBrush = new SolidBrush(Color.FromArgb(24, 30, 54)))
                using (Brush foreColorBrush = new SolidBrush(Color.FromArgb(0, 126, 249)))
                {
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                    e.Graphics.DrawString(e.Value?.ToString(), ((DataGridView)sender).ColumnHeadersDefaultCellStyle.Font, foreColorBrush, e.CellBounds, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    e.Handled = true;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DataTable dataTable = (DataTable)dgvUsers.DataSource;
            DataTable changes = dataTable.GetChanges();

            if (changes == null)
            {
                MessageBox.Show("Нет измененных данных для обновления.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Вы уверены, что хотите обновить данные?", "Подтверждение обновления данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Database.UpdateUserData(changes);

                Database.FillDataGridViewUsers(dgvUsers);
                dataTable.AcceptChanges();
                dgvUsers.Refresh();

                MessageBox.Show("Данные успешно обновлены.", "Обновление данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            Database.FillDataGridViewUsers(dgvUsers);
        }
    }
}
