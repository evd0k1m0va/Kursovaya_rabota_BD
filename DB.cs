using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows.Forms;

namespace kursach_2_0
{
    internal class DB
    {
        // Основное подключение к БД (используется во всех модулях)
        private MySqlConnection connection = new MySqlConnection(
            "server=localhost;port=3306;user=root;password=root;database=advertising_agency;AllowPublicKeyRetrieval=True;SslMode=none;");

        // Открытие соединения
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        // Закрытие соединения
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        // Получить объект соединения
        public MySqlConnection getConnection()
        {
            return connection;
        }

        // Автоматическая установка базы данных из SQL-файла
        public void InstallDatabaseIfNotExists()
        {
            try
            {
                // Подключение без указания базы (чтобы можно было создать базу)
                string masterConnStr = "server=localhost;port=3306;user=root;password=root;AllowPublicKeyRetrieval=True;SslMode=none;";
                using (MySqlConnection conn = new MySqlConnection(masterConnStr))
                {
                    conn.Open();

                    if (File.Exists("advertising_agency.sql"))
                    {
                        string sql = File.ReadAllText("advertising_agency.sql");
                        MySqlScript script = new MySqlScript(conn, sql);
                        script.Execute();
                    }
                    else
                    {
                        MessageBox.Show("Файл advertising_agency.sql не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при установке БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
