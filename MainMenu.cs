﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Diplom
{
    public partial class MainMenu : Form
    {
        private Form activeChildForm = null;
        FormDraggable formDraggable = new FormDraggable();

        public MainMenu()
        {
            InitializeComponent();

            InitializeUI();

            btnCalendar.Hide();

            formDraggable.Attach(pnlControl);
        }

        private void InitializeUI()
        {
            UpdateNavigation(btnUsers);

            lblFullName.Text = $"{User.UserFullName}\n({User.UserRole})";

            if (User.UserRole == "Пользователь")
            {
                btnUsers.Visible = false;
                btnEmployees.Text = "Мои данные";
                OpenChildForm(new EmployeeListUserForm(this));
            }
            else
            {
                btnEmployees.Text = "Сотрудники";
                OpenChildForm(new UserList(this));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть программу?", "Подтверждение закрытия", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            WindowState = WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти из учетной записи?", "Подтверждение выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
                new Authorization().Show();
            }
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            UpdateNavigation(btnUsers);
            OpenChildForm(new UserList(this));
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            UpdateNavigation(btnEmployees);
            if (User.UserRole == "Пользователь")
            {
                OpenChildForm(new EmployeeListUserForm(this));
            }
            else
            {
                OpenChildForm(new EmployeeListAdminForm(this));
            }
        }

        private void btnVacationSchedule_Click(object sender, EventArgs e)
        {
            UpdateNavigation(btnVacationSchedule);
            OpenChildForm(new VacationSchedule(this));
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            UpdateNavigation(btnCalendar);
            OpenChildForm(new Calendar(this));
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            UpdateNavigation(btnSettings);
            OpenChildForm(new SettingsForm(this));
        }

        private void UpdateNavigation(Button button)
        {
            pnlNav.Height = button.Height;
            pnlNav.Top = button.Top;
            pnlNav.Left = button.Left;

            btnUsers.BackColor = btnEmployees.BackColor = btnVacationSchedule.BackColor = btnCalendar.BackColor = btnSettings.BackColor = Color.FromArgb(24, 30, 54);
            button.BackColor = Color.FromArgb(46, 51, 73);
        }

        private void OpenChildForm(Form childForm)
        {
            if (activeChildForm != null)
            {
                activeChildForm.Close();
            }

            activeChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
    }
}
