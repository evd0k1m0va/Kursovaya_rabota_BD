using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Security.Policy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace kursach_2_0
{
    public partial class change_pass : Form
    {
        private TextBox new_pass;
        private Button ChangeButton;
        private Button backbutton;

        public change_pass()
        {
            InitializeComponent();

        }
        
        private void InitializeComponent()
        {
            this.new_pass = new System.Windows.Forms.TextBox();
            this.backbutton = new System.Windows.Forms.Button();
            this.ChangeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // new_pass
            // 
            this.new_pass.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.new_pass.Location = new System.Drawing.Point(207, 127);
            this.new_pass.Name = "new_pass";
            this.new_pass.Size = new System.Drawing.Size(157, 22);
            this.new_pass.TabIndex = 0;
            this.new_pass.Text = "Новый пароль";
            this.new_pass.TextChanged += new System.EventHandler(this.new_pass_TextChanged);
            this.new_pass.Enter += new System.EventHandler(this.new_pass_Enter);
            this.new_pass.Leave += new System.EventHandler(this.new_pass_Leave);
            // 
            // backbutton
            // 
            this.backbutton.Location = new System.Drawing.Point(12, 314);
            this.backbutton.Name = "backbutton";
            this.backbutton.Size = new System.Drawing.Size(75, 23);
            this.backbutton.TabIndex = 1;
            this.backbutton.Text = "Назад";
            this.backbutton.UseVisualStyleBackColor = true;
            this.backbutton.Click += new System.EventHandler(this.backbutton_Click);
            // 
            // ChangeButton
            // 
            this.ChangeButton.Location = new System.Drawing.Point(465, 314);
            this.ChangeButton.Name = "ChangeButton";
            this.ChangeButton.Size = new System.Drawing.Size(92, 23);
            this.ChangeButton.TabIndex = 2;
            this.ChangeButton.Text = "Изменить";
            this.ChangeButton.UseVisualStyleBackColor = true;
            this.ChangeButton.Click += new System.EventHandler(this.ChangeButton_Click);
            // 
            // change_pass
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(579, 365);
            this.Controls.Add(this.ChangeButton);
            this.Controls.Add(this.backbutton);
            this.Controls.Add(this.new_pass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "change_pass";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void new_pass_TextChanged(object sender, EventArgs e)
        {

        }

        private void new_pass_Enter(object sender, EventArgs e)
        {
            if (new_pass.Text == "Новый пароль")
            {
                new_pass.Text = "";
                new_pass.ForeColor = Color.Black;
            }
        }

        private void new_pass_Leave(object sender, EventArgs e)
        {
            if (new_pass.Text == "")
            {
                new_pass.Text = "Новый пароль";
                new_pass.ForeColor = Color.Gray;
            }
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(new_pass.Text) || new_pass.Text == "Новый пароль")
            {
                MessageBox.Show("Заполните новый пароль для изменения!");
                return;
            }
            DB db = new DB();
            int userId = 1; 

            MySqlCommand command = new MySqlCommand("UPDATE `user_registration` SET `password_hash` = @password_hash WHERE `id` = @id", db.getConnection());
            command.Parameters.AddWithValue("@password_hash", new_pass.Text);
            command.Parameters.AddWithValue("@id", userId);

            try
            {
                db.openConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Пароль успешно изменён");
                   
                    this.Close();
                }


                else
                    MessageBox.Show("Ошибка! Пароль не изменен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                db.closeConnection();
            }
        }

        private void backbutton_Click(object sender, EventArgs e)
        {
            
       
            var result = MessageBox.Show("Вы уверены, что хотите вернуться? Несохранённые данные будут потеряны.",
                                         "Подтверждение",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        
        }
    }
}
