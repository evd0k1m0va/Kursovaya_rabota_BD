using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace kursach_2_0
{
    public partial class mainForm : Form
    {
        private TabPage EmployeesPage;
        private EmployeesControl employeesControl;
        //private DirectoriesControl directoriesControl;
        private DocumentsControl documentsControl;
        private string _login;
        private string _role;

        // Конструктор с передачей параметров login и role
        public mainForm(string login, string role)
        {
            _login = login;
            _role = role;
            
            InitializeComponent();
            directoriesControl = new DirectoriesControl(_login, _role);
            directoriesControl.Dock = DockStyle.Fill;
            DirectoriesPage.Controls.Add(directoriesControl);
            InitializeRoleSpecificControls();
        }

        private void InitializeRoleSpecificControls()
        {
            if (_role == "admin")
            {
                employeesControl = new EmployeesControl();
                employeesControl.Dock = DockStyle.Fill;

                EmployeesPage = new TabPage("Сотрудники")
                {
                    BackColor = System.Drawing.Color.Gray
                };
                EmployeesPage.Controls.Add(employeesControl);
                Fullcontrol.Controls.Add(EmployeesPage);
            }
            else
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;user=root;password=root;database=advertising_agency"))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT permissions FROM user_registration WHERE login = @login", connection);
                    command.Parameters.AddWithValue("@login", _login);
                    string permissions = command.ExecuteScalar()?.ToString();

                    if (permissions != null && permissions == "0000")
                    {
                        EmployeesPage.Enabled = false;
                    }
                }
            }
        }
    }
}