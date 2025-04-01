using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace kursach_2_0
{
    public class TableItem
    {
        public string TableName { get; set; }
        public string DisplayName { get; set; }

        public TableItem(string tableName, string displayName)
        {
            TableName = tableName;
            DisplayName = displayName;
        }
    }

    public partial class DirectoriesControl : UserControl
    {
        private string _role;
        private string _login;

        private ComboBox comboBoxTableList;
        private DataGridView dataGridView1;
        private Button btnSaveChanges;
        private Button btnDelete;

        private MySqlDataAdapter adapter;
        private DataTable currentTable;

        private List<TableItem> tableMapping;

        public DirectoriesControl(string login, string role)
        {
            _role = role;
            _login = login;
            InitializeComponent();
            InitializeTableMapping();
            LoadTableList();
            CheckPermissions();
        }

        private void InitializeComponent()
        {
            this.comboBoxTableList = new ComboBox();
            this.dataGridView1 = new DataGridView();
            this.btnSaveChanges = new Button();
            this.btnDelete = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            this.comboBoxTableList.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxTableList.Location = new Point(20, 20);
            this.comboBoxTableList.Size = new Size(250, 24);
            this.comboBoxTableList.SelectedIndexChanged += new EventHandler(this.ComboBoxTableList_SelectedIndexChanged);

            this.dataGridView1.Location = new Point(20, 72);
            this.dataGridView1.Size = new Size(800, 400);

            this.btnSaveChanges.Location = new Point(290, 20);
            this.btnSaveChanges.Size = new Size(150, 30);
            this.btnSaveChanges.Text = "Сохранить";
            this.btnSaveChanges.Click += new EventHandler(this.btnSaveChanges_Click);

            this.btnDelete.Location = new Point(450, 20);
            this.btnDelete.Size = new Size(150, 30);
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            this.Controls.Add(this.comboBoxTableList);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnSaveChanges);
            this.Controls.Add(this.btnDelete);
            this.Size = new Size(900, 500);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }

        private void CheckPermissions()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=advertising_agency"))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT permissions FROM user_registration WHERE login = @login", connection);
                command.Parameters.AddWithValue("@login", _login);
                string permissions = command.ExecuteScalar()?.ToString();

                if (permissions == "0000")
                {
                    this.Enabled = false;
                }
                else if (permissions == "1000") // Только чтение
                {
                    this.btnSaveChanges.Enabled = false;
                    this.btnDelete.Enabled = false;
                    this.dataGridView1.ReadOnly = true;
                }
                else if (permissions == "1100") // Чтение + Добавление
                {
                    this.btnSaveChanges.Enabled = true;
                    this.btnDelete.Enabled = false;
                    this.dataGridView1.ReadOnly = false;
                    this.dataGridView1.AllowUserToDeleteRows = false;
                }
                else if (permissions == "1110") // Чтение + Добавление + Изменение
                {
                    this.btnSaveChanges.Enabled = true;
                    this.btnDelete.Enabled = false;
                    this.dataGridView1.ReadOnly = false;
                    this.dataGridView1.AllowUserToDeleteRows = false;
                    this.dataGridView1.AllowUserToAddRows = true;
                }
                else if (permissions == "1111") // Полный доступ
                {
                    this.btnSaveChanges.Enabled = true;
                    this.btnDelete.Enabled = true;
                    this.dataGridView1.ReadOnly = false;
                    this.dataGridView1.AllowUserToDeleteRows = true;
                    this.dataGridView1.AllowUserToAddRows = true;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления!");
            }
        }
        private void LoadTableData(string tableName)
        {
            try
            {
                string connString = "server=localhost;port=3306;username=root;password=root;database=advertising_agency";
                string query = $"SELECT * FROM {tableName}";

                adapter = new MySqlDataAdapter(query, connString);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);

                currentTable = new DataTable();
                adapter.Fill(currentTable);

                dataGridView1.DataSource = currentTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке таблицы '{tableName}': {ex.Message}");
            }
        }
        private void InitializeTableMapping()
        {
            tableMapping = new List<TableItem>
            {
                new TableItem("street", "Улицы"),
                new TableItem("area", "Районы"),
                new TableItem("billboards", "Рекламные щиты"),
                new TableItem("contract", "Договоры"),
                new TableItem("employee", "Сотрудники"),
                new TableItem("optional", "Доп. опции"),
                new TableItem("`order`", "Заказы"),
                new TableItem("post", "Должности"),
                new TableItem("pricelist", "Прайс-лист"),
                new TableItem("rental", "Аренда"),
                new TableItem("typepay", "Тип оплаты"),
                new TableItem("user_registration", "Пользователи"),
                new TableItem("`work record`", "Трудовая книжка")
            };
        }

        private void LoadTableList()
        {
            if (_role != "admin")
            {
                tableMapping.RemoveAll(t => t.TableName == "user_registration" || t.TableName == "employee");
            }
            comboBoxTableList.DataSource = tableMapping;
            comboBoxTableList.DisplayMember = "DisplayName";
            comboBoxTableList.ValueMember = "TableName";
            if (comboBoxTableList.Items.Count > 0)
            {
                comboBoxTableList.SelectedIndex = 0;
            }
        }

        private void ComboBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTableList.SelectedValue is string tableName)
            {
                LoadTableData(tableName);
            }
        }
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (adapter == null || currentTable == null)
            {
                MessageBox.Show("Нет данных для сохранения!");
                return;
            }

            try
            {
                int rowsUpdated = adapter.Update(currentTable);
                MessageBox.Show($"Изменения сохранены! Обновлено строк: {rowsUpdated}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }
    }
}
