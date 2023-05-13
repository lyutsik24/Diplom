using System.Collections.Generic;
using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Diplom
{
    public partial class Calendar : Form
    {
        private MainMenu mainMenu;

        
        public Calendar(MainMenu form)
        {
            InitializeComponent();

            LoadDataAndHighlightDates(monthCalendar1);

            mainMenu = form;
        }

        private static void LoadDataAndHighlightDates(MonthCalendar monthCalendar)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.DiplomConnectionString))
                {
                    string query = @"SELECT e.employee_id, e.last_name, e.first_name, v.start_date, v.duration
                                     FROM employees e
                                     INNER JOIN vacations v ON e.employee_id = v.id_employee";

                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<DateTime> highlightedDates = new List<DateTime>();

                            while (reader.Read())
                            {
                                DateTime startDate = reader.GetDateTime("start_date");
                                int duration = reader.GetInt32("duration");

                                DateTime endDate = startDate.AddDays(duration - 1);
                                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                                {
                                    highlightedDates.Add(date);
                                }
                            }

                            monthCalendar.BoldedDates = highlightedDates.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }
    }
}
