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
    public partial class VacationSchedule : Form
    {
        private MainMenu mainMenu;

        public VacationSchedule(MainMenu form)
        {
            InitializeComponent();

            mainMenu = form;

            Database.FillDataGridViewVacations(dgvShedule);
        }
    }
}
