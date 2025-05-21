using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RegistrationForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Label lbl = new Label();
                lbl.Location = new System.Drawing.Point(16, 96);
                lbl.Size = new System.Drawing.Size(32, 23);
                lbl.Name = "labelPIN2";
                lbl.Text = "PIN2";
                groupBox1.Controls.Add(lbl);

                TextBox txt = new TextBox();
                txt.Location = new System.Drawing.Point(96, 96);
                txt.Size = new System.Drawing.Size(184, 20);
                txt.Name = "textBoxPIN2";
                groupBox1.Controls.Add(txt);
                txt.KeyPress += new
System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            }
            else
            {
                Control lblToRemove = groupBox1.Controls["labelPIN2"];
                Control txtToRemove = groupBox1.Controls["textBoxPIN2"];

                if (lblToRemove != null)
                    groupBox1.Controls.Remove(lblToRemove);

                if (txtToRemove != null)
                    groupBox1.Controls.Remove(txtToRemove);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Поле Name не может содержать цифры");
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Поле PIN не может содержать буквы");
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            {
                if (textBox2.Text == "")
                {
                    e.Cancel = false; // Если пусто – не блокируем ввод
                }
                else
                {
                    try
                    {
                        double.Parse(textBox2.Text); // Проверяем, что это число
                        e.Cancel = false; // Все ок
                    }
                    catch
                    {
                        e.Cancel = true; // Блокируем переключение на другое поле
                        MessageBox.Show("Поле PIN не может содержать буквы");
                    }
                }
            }
        }
    }
}