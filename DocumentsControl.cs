using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Word = Microsoft.Office.Interop.Word;

namespace kursach_2_0
{
    public partial class DocumentsControl : UserControl
    {
        // Текстовые поля для ввода данных
        private TextBox RentalText;    // "Арендатор" – вводится числовой ID арендатора (из таблицы rental)
        private TextBox EmployeeText;  // "Сотрудник" – вводится числовой ID сотрудника (из таблицы employee)
        private TextBox AddText;       // "Дополнительно" – числовой ID дополнительной услуги (из таблицы optional)
        private TextBox PayText;       // "Вид оплаты" – числовой ID типа оплаты (из таблицы typepay)

        // DataGridView для отображения записей (список контрактов)
        private DataGridView dataGridView1;
        // Кнопка "Экспорт" – вставляет запись в таблицу contract и создаёт документ Word с чеком
        private Button buttonExport;

        public DocumentsControl()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Загрузка данных в DataGridView. 
        /// Подтягиваем поля из contract + связанные таблицы rental, employee, typepay, optional.
        /// </summary>
        private void LoadData()
        {
            try
            {
                DB db = new DB();
                db.openConnection();

                string query = @"
                    SELECT
                        c.id              AS 'Договор',
                        c.dateorder       AS 'Дата подписания',
                        c.cost            AS 'Стоимость итого',
                        
                        r.name            AS 'Название арендатора',
                        r.status          AS 'Статус арендатора',

                        e.name            AS 'Имя сотрудника',
                        e.surname         AS 'Фамилия сотрудника',

                        t.type            AS 'Вид оплаты',

                        o.service         AS 'Доп. услуга'
                    FROM `contract` c
                    LEFT JOIN `rental`   r ON c.`id renter`   = r.id
                    LEFT JOIN `employee` e ON c.`id employee` = e.id
                    LEFT JOIN `typepay`  t ON c.`idtypepay`   = t.id
                    LEFT JOIN `optional` o ON c.`id optional` = o.id
                ";

                MySqlCommand command = new MySqlCommand(query, db.getConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

      
        private void InitializeComponent()
        {
            this.RentalText = new System.Windows.Forms.TextBox();
            this.EmployeeText = new System.Windows.Forms.TextBox();
            this.AddText = new System.Windows.Forms.TextBox();
            this.PayText = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // RentalText
            // 
            this.RentalText.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.RentalText.Location = new System.Drawing.Point(49, 90);
            this.RentalText.Name = "RentalText";
            this.RentalText.Size = new System.Drawing.Size(150, 22);
            this.RentalText.TabIndex = 0;
            this.RentalText.Text = "Арендатор";
            this.RentalText.Enter += new System.EventHandler(this.RentalText_Enter);
            this.RentalText.Leave += new System.EventHandler(this.RentalText_Leave);
            // 
            // EmployeeText
            // 
            this.EmployeeText.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.EmployeeText.Location = new System.Drawing.Point(49, 132);
            this.EmployeeText.Name = "EmployeeText";
            this.EmployeeText.Size = new System.Drawing.Size(150, 22);
            this.EmployeeText.TabIndex = 1;
            this.EmployeeText.Text = "Сотрудник";
            this.EmployeeText.Enter += new System.EventHandler(this.EmployeeText_Enter);
            this.EmployeeText.Leave += new System.EventHandler(this.EmployeeText_Leave);
            // 
            // AddText
            // 
            this.AddText.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.AddText.Location = new System.Drawing.Point(49, 173);
            this.AddText.Name = "AddText";
            this.AddText.Size = new System.Drawing.Size(150, 22);
            this.AddText.TabIndex = 2;
            this.AddText.Text = "Дополнительно";
            this.AddText.Enter += new System.EventHandler(this.AddText_Enter);
            this.AddText.Leave += new System.EventHandler(this.AddText_Leave);
            // 
            // PayText
            // 
            this.PayText.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.PayText.Location = new System.Drawing.Point(49, 217);
            this.PayText.Name = "PayText";
            this.PayText.Size = new System.Drawing.Size(150, 22);
            this.PayText.TabIndex = 3;
            this.PayText.Text = "Вид оплаты";
            this.PayText.Enter += new System.EventHandler(this.PayText_Enter);
            this.PayText.Leave += new System.EventHandler(this.PayText_Leave);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeight = 29;
            this.dataGridView1.Location = new System.Drawing.Point(241, 44);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(893, 390);
            this.dataGridView1.TabIndex = 4;
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(49, 265);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(142, 37);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "Экспорт";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // DocumentsControl
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.PayText);
            this.Controls.Add(this.AddText);
            this.Controls.Add(this.EmployeeText);
            this.Controls.Add(this.RentalText);
            this.Name = "DocumentsControl";
            this.Size = new System.Drawing.Size(1174, 524);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // Обработчики плейсхолдеров
        private void RentalText_Enter(object sender, EventArgs e)
        {
            if (RentalText.Text == "Арендатор")
            {
                RentalText.Text = "";
                RentalText.ForeColor = Color.Black;
            }
        }
        private void RentalText_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RentalText.Text))
            {
                RentalText.Text = "Арендатор";
                RentalText.ForeColor = Color.Gray;
            }
        }

        private void EmployeeText_Enter(object sender, EventArgs e)
        {
            if (EmployeeText.Text == "Сотрудник")
            {
                EmployeeText.Text = "";
                EmployeeText.ForeColor = Color.Black;
            }
        }
        private void EmployeeText_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmployeeText.Text))
            {
                EmployeeText.Text = "Сотрудник";
                EmployeeText.ForeColor = Color.Gray;
            }
        }

        private void AddText_Enter(object sender, EventArgs e)
        {
            if (AddText.Text == "Дополнительно")
            {
                AddText.Text = "";
                AddText.ForeColor = Color.Black;
            }
        }
        private void AddText_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddText.Text))
            {
                AddText.Text = "Дополнительно";
                AddText.ForeColor = Color.Gray;
            }
        }

        private void PayText_Enter(object sender, EventArgs e)
        {
            if (PayText.Text == "Вид оплаты")
            {
                PayText.Text = "";
                PayText.ForeColor = Color.Black;
            }
        }
        private void PayText_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PayText.Text))
            {
                PayText.Text = "Вид оплаты";
                PayText.ForeColor = Color.Gray;
            }
        }

     
        private void buttonExport_Click(object sender, EventArgs e)
        {
            //Вставляем запись (cost=0)
            int contractId = InsertContract();
            if (contractId == -1)
            {
                MessageBox.Show("Ошибка при добавлении записи в таблицу contract.");
                return;
            }

            //Получаем суммарную стоимость заказов + доп. услуги
            int totalOrderCost = GetOrderCost(contractId);
            int optionalCost = GetOptionalServiceCost(Convert.ToInt32(AddText.Text));
            int totalCost = totalOrderCost + optionalCost;

            //Обновляем поле cost
            if (!UpdateContractCost(contractId, totalCost))
            {
                MessageBox.Show("Ошибка при обновлении стоимости в таблице contract.");
                return;
            }

            //Создаём документ Word
            
            string orderDetails = GetOrderInfoForContract(contractId);
            ExportReceiptToWord(contractId, orderDetails, totalCost);
            LoadData(); 
        }

        /// <summary>
        /// Вставляем запись в contract (дата подписания = сегодня, cost=0).
        /// </summary>
        private int InsertContract()
        {
            int newId = -1;
            try
            {
                string connString = @"server=localhost;port=3306;username=root;password=root;database=advertising_agency";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO `contract`
                        (`id renter`, `dateorder`, `id employee`, `id optional`, `cost`, `idtypepay`)
                        VALUES
                        (@rental, @now, @emp, @opt, 0, @pay);
                        SELECT LAST_INSERT_ID();
                    ";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@rental", Convert.ToInt32(RentalText.Text));
                    cmd.Parameters.AddWithValue("@now", DateTime.Now);
                    cmd.Parameters.AddWithValue("@emp", Convert.ToInt32(EmployeeText.Text));
                    cmd.Parameters.AddWithValue("@opt", Convert.ToInt32(AddText.Text));
                    cmd.Parameters.AddWithValue("@pay", Convert.ToInt32(PayText.Text));

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        newId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при вставке в contract: " + ex.Message);
            }
            return newId;
        }

        /// <summary>
        /// Получаем суммарную стоимость заказов (таблица `order`) для данного контракта,
        /// суммируя (count * cost) для каждой записи.
        /// </summary>
        private int GetOrderCost(int contractId)
        {
            int total = 0;
            try
            {
                string connString = @"server=localhost;port=3306;username=root;password=root;database=advertising_agency";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
                        SELECT `count`, `cost`
                        FROM `order`
                        WHERE contract_id = @cid
                    ";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cid", contractId);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int cnt = rdr.GetInt32("count");
                            int cst = rdr.GetInt32("cost");
                            total += (cnt * cst);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении стоимости заказов: " + ex.Message);
            }
            return total;
        }
        /// <summary>
        /// Получает название арендатора из таблицы rental по заданному ID.
        /// </summary>
        private string GetRenterName(int renterId)
        {
            string name = "";
            try
            {
                string connString = @"server=localhost;port=3306;username=root;password=root;database=advertising_agency";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT name FROM rental WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", renterId);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        name = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении названия арендатора: " + ex.Message);
            }
            return name;
        }

        /// <summary>
        /// Получает имя сотрудника (с учетом фамилии) из таблицы employee по заданному ID.
        /// </summary>
        private string GetEmployeeName(int employeeId)
        {
            string name = "";
            try
            {
                string connString = @"server=localhost;port=3306;username=root;password=root;database=advertising_agency";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    
                    string query = "SELECT CONCAT(name, ' ', surname) FROM employee WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", employeeId);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        name = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении имени сотрудника: " + ex.Message);
            }
            return name;
        }

        /// <summary>
        /// Получает название типа оплаты из таблицы typepay по заданному ID.
        /// </summary>
        private string GetPayTypeName(int payTypeId)
        {
            string name = "";
            try
            {
                string connString = @"server=localhost;port=3306;username=root;password=root;database=advertising_agency";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT type FROM typepay WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", payTypeId);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        name = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении типа оплаты: " + ex.Message);
            }
            return name;
        }

        /// <summary>
        /// Получает название дополнительной услуги из таблицы optional по заданному ID.
        /// </summary>
        private string GetOptionalServiceName(int optionalId)
        {
            string name = "";
            try
            {
                string connString = @"server=localhost;port=3306;username=root;password=root;database=advertising_agency";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT service FROM optional WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", optionalId);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        name = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении дополнительной услуги: " + ex.Message);
            }
            return name;
        }

        /// <summary>
        /// Получаем стоимость дополнительной услуги (поле price или cost) из таблицы optional
        /// по ID (AddText.Text).
        /// </summary>
        private int GetOptionalServiceCost(int optId)
        {
            int cost = 0;
            try
            {
                string connString = @"server=localhost;port=3306;username=root;password=root;database=advertising_agency";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    // Предположим, в optional есть столбец price (или cost)
                    string query = "SELECT price FROM `optional` WHERE id = @oid";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@oid", optId);
                    object res = cmd.ExecuteScalar();
                    if (res != null)
                        cost = Convert.ToInt32(res);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении стоимости доп. услуги: " + ex.Message);
            }
            return cost;
        }

        /// <summary>
        /// Обновляем поле cost в таблице contract.
        /// </summary>
        private bool UpdateContractCost(int contractId, int totalCost)
        {
            bool ok = false;
            try
            {
                string connString = @"server=localhost;port=3306;username=root;password=root;database=advertising_agency";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE `contract` SET `cost` = @cost WHERE `id` = @cid";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cost", totalCost);
                    cmd.Parameters.AddWithValue("@cid", contractId);
                    int rows = cmd.ExecuteNonQuery();
                    ok = (rows == 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении поля cost: " + ex.Message);
            }
            return ok;
        }

        /// <summary>
        /// Формируем строку с деталями заказов для вывода в чек (необязательно).
        /// </summary>
        private string GetOrderInfoForContract(int contractId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                string connString = @"server=localhost;port=3306;username=root;password=root;database=advertising_agency";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
                        SELECT billboard_id, startdate, enddate, `count`, cost, pictures
                        FROM `order`
                        WHERE contract_id = @cid
                    ";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cid", contractId);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int billboard = rdr.GetInt32("billboard_id");
                            DateTime st = rdr.GetDateTime("startdate");
                            DateTime en = rdr.GetDateTime("enddate");
                            int cnt = rdr.GetInt32("count");
                            int cst = rdr.GetInt32("cost");
                            string pics = rdr["pictures"].ToString();

                            int totalOrder = cnt * cst;
                            sb.AppendLine($"Рекламный щит: {billboard}");
                            sb.AppendLine($"Начало аренды: {st:dd-MM-yyyy}");
                            sb.AppendLine($"Конец аренды: {en:dd-MM-yyyy}");
                            sb.AppendLine($"Количество (дней): {cnt}");
                            sb.AppendLine($"Цена за единицу: {cst}");
                            sb.AppendLine($"Итоговая стоимость заказа: {totalOrder}");
                            sb.AppendLine($"Дополнительно: {pics}");
                            sb.AppendLine("----------------------------");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении заказов: " + ex.Message);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Создаём документ Word (чек), используя итоговую стоимость и данные внешних ключей.
        /// </summary>
        private void ExportReceiptToWord(int contractId, string orderDetails, int totalContractCost)
        {
            try
            {
                // Получаем понятные названия из ID
                int renterId = Convert.ToInt32(RentalText.Text);
                int employeeId = Convert.ToInt32(EmployeeText.Text);
                int payTypeId = Convert.ToInt32(PayText.Text);
                int optionalId = Convert.ToInt32(AddText.Text);

                string renterName = GetRenterName(renterId);
                string employeeName = GetEmployeeName(employeeId);
                string payTypeName = GetPayTypeName(payTypeId);

                // Доп. услуга (название)
                string optionalService = GetOptionalServiceName(optionalId);

                // Дата подписания = сегодня
                string todayDate = DateTime.Now.ToString("dd-MM-yyyy");

                // Формируем чек
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ЧЕК ОБ ОПЛАТЕ");
                sb.AppendLine("----------------------------");
                sb.AppendLine($"Номер контракта: {contractId}");
                sb.AppendLine($"Дата подписания: {todayDate}");
                sb.AppendLine($"Арендатор: {renterName}");
                sb.AppendLine($"Сотрудник: {employeeName}");
                sb.AppendLine($"Вид оплаты: {payTypeName}");
                sb.AppendLine($"Дополнительно: {optionalService}");
                sb.AppendLine("----------------------------");
                sb.AppendLine("Информация по заказам:");
                sb.AppendLine(orderDetails);
                sb.AppendLine("----------------------------");
                sb.AppendLine($"Итоговая стоимость по договору: {totalContractCost}");
                sb.AppendLine("----------------------------");
                sb.AppendLine("Спасибо за оплату!");

                // Создаем Word
                Word.Application wordApp = new Word.Application();
                wordApp.Visible = false;
                Word.Document doc = wordApp.Documents.Add();

                // Вставляем текст
                Word.Paragraph para = doc.Content.Paragraphs.Add();
                para.Range.Text = sb.ToString();
                para.Range.Font.Name = "Arial";
                para.Range.Font.Size = 12;
                para.Range.InsertParagraphAfter();

                // Сохраняем
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Word Document (*.docx)|*.docx";
                sfd.FileName = "Чек.docx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    doc.SaveAs2(sfd.FileName);
                    MessageBox.Show("Документ успешно создан!");
                }
                else
                {
                    MessageBox.Show("Сохранение документа отменено.");
                }

                // Закрываем Word
                doc.Close();
                wordApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при экспорте в Word: " + ex.Message);
            }
        }
    }
}
