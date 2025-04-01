using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace kursach_2_0
{
    public partial class EmployeesControl : UserControl
    {
        private DataGridView dataGridView1;
        private CheckBox readCheckBox;
        private CheckBox addCheckBox;
        private CheckBox editCheckBox;
        private CheckBox deleteCheckBox;
        private Button saveButton;

        public EmployeesControl()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(
                    "server=localhost;port=3306;username=root;password=root;database=advertising_agency"))
                {
                    connection.Open();
                    string query = @"
                        SELECT
                            id AS 'ID',
                            login AS 'Логин',
                            surname AS 'Фамилия',
                            username AS 'Имя',
                            role AS 'Роль',
                            permissions AS 'Права доступа',
                            number AS 'Телефон',
                            created_at AS 'Дата регистрации'
                        FROM user_registration";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "user_registration");
                    dataGridView1.DataSource = ds.Tables["user_registration"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.readCheckBox = new System.Windows.Forms.CheckBox();
            this.addCheckBox = new System.Windows.Forms.CheckBox();
            this.editCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(50, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(800, 400);
            this.dataGridView1.TabIndex = 0;
            // 
            // readCheckBox
            // 
            this.readCheckBox.Location = new System.Drawing.Point(50, 450);
            this.readCheckBox.Name = "readCheckBox";
            this.readCheckBox.Size = new System.Drawing.Size(104, 24);
            this.readCheckBox.TabIndex = 1;
            this.readCheckBox.Text = "Чтение";
            // 
            // addCheckBox
            // 
            this.addCheckBox.Location = new System.Drawing.Point(150, 450);
            this.addCheckBox.Name = "addCheckBox";
            this.addCheckBox.Size = new System.Drawing.Size(104, 24);
            this.addCheckBox.TabIndex = 2;
            this.addCheckBox.Text = "Добавление";
            // 
            // editCheckBox
            // 
            this.editCheckBox.Location = new System.Drawing.Point(250, 450);
            this.editCheckBox.Name = "editCheckBox";
            this.editCheckBox.Size = new System.Drawing.Size(104, 24);
            this.editCheckBox.TabIndex = 3;
            this.editCheckBox.Text = "Изменение";
            // 
            // deleteCheckBox
            // 
            this.deleteCheckBox.Location = new System.Drawing.Point(350, 450);
            this.deleteCheckBox.Name = "deleteCheckBox";
            this.deleteCheckBox.Size = new System.Drawing.Size(104, 24);
            this.deleteCheckBox.TabIndex = 4;
            this.deleteCheckBox.Text = "Удаление";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(500, 450);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(112, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Сохранить права доступа";
            this.saveButton.Click += new System.EventHandler(this.SavePermissions_Click);
            // 
            // EmployeesControl
            // 
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.readCheckBox);
            this.Controls.Add(this.addCheckBox);
            this.Controls.Add(this.editCheckBox);
            this.Controls.Add(this.deleteCheckBox);
            this.Controls.Add(this.saveButton);
            this.Name = "EmployeesControl";
            this.Size = new System.Drawing.Size(912, 502);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        private void SavePermissions_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                try
                {
                    string selectedLogin = dataGridView1.SelectedRows[0].Cells["Логин"].Value.ToString();
                    string permissions = (readCheckBox.Checked ? "1" : "0") +
                                         (addCheckBox.Checked ? "1" : "0") +
                                         (editCheckBox.Checked ? "1" : "0") +
                                         (deleteCheckBox.Checked ? "1" : "0");

                    using (MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=advertising_agency"))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("UPDATE user_registration SET permissions = @permissions WHERE login = @login", connection);
                        command.Parameters.AddWithValue("@permissions", permissions);
                        command.Parameters.AddWithValue("@login", selectedLogin);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Права доступа обновлены");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя");
            }
        }
    }
}
