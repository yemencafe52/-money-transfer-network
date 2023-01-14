using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class frmOGT : Form
    {
        public frmOGT()
        {
            InitializeComponent();
            Preparing();
        }

        private bool Preparing()
        {
            DisableALL();
            ClearALL();
            comboBox1.SelectedIndex = 0;
            button1.Enabled = true;
            button4.Enabled = true;
            return true;
        }

        private void DisableALL()
        {
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;

            comboBox1.Enabled = false;
            dateTimePicker1.Enabled = false;

            textBox1.Enabled = false;
            textBox2.Enabled = false;

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;


        }

        private void ClearALL()
        {
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;

            textBox1.Text = "";
            textBox2.Text = "";

         
        }

        private void Add()
        {
            if(!(MyConnection.IsConnected))
            {
                MessageBox.Show("الشبكة غير متوفرة حالياً");
                return;
            }

            //numericUpDown1.Value = OutGoingTransfer.GenerateNewOGTNumber();
            numericUpDown2.Enabled = true;
            comboBox1.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;

            button1.Enabled = false;
            button4.Enabled = false;

            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void Save()
        {
            if (!(MyConnection.IsConnected))
            {
                MessageBox.Show("الشبكة غير متوفرة حالياً");
                return;
            }

            if (!(numericUpDown2.Value>0))
            {
                numericUpDown2.Focus();
                return;
            }

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                return;
            }

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Focus();
                return;
            }


            OutGoingTransfer ogt = new OutGoingTransfer((long)numericUpDown1.Value,1, byte.Parse(comboBox1.Text), User.ActiveUser.Number, (double)numericUpDown2.Value, textBox1.Text, textBox2.Text, dateTimePicker1.Value);

            if(!OutGoingTransfer.Add(ref ogt))
            {

                MessageBox.Show("تعذر تنفيذ العملية");
                return;
            }

            Display(ogt);

            //numericUpDown2.Enabled = false;
            //textBox1.Enabled = false;
            //textBox2.Enabled = false;

            //button1.Enabled = true;
            //button2.Enabled = false;
            //button3.Enabled = false;
            //button4.Enabled = true;


        }

        private void Search()
        {
            DisableALL();
            ClearALL();
            button3.Enabled = true;
            numericUpDown1.Enabled = true;
            numericUpDown1.Focus();
        }

        private void Cancel()
        {
            Preparing();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(numericUpDown1.Value > 0)
                {
                    OutGoingTransfer ogt = new OutGoingTransfer(0, 0, 0, 0, 0, "", "", new DateTime());
                    
                    if (OutGoingTransfer.Query((long)numericUpDown1.Value, ref ogt))
                    {
                        Display(ogt);
                    }
                }
            }
        }

        private void Display(OutGoingTransfer ogt)
        {
            Preparing();
            ClearALL();

            numericUpDown1.Value = ogt.Number;
            numericUpDown2.Value = (decimal)ogt.Amount;
            dateTimePicker1.Value = ogt.Date;
            numericUpDown3.Value = 0;
            textBox1.Text = ogt.SenderName;
            textBox2.Text = ogt.ReciverName;
            button1.Enabled = true;
            button4.Enabled = true;
        }

        private void frmOGT_Shown(object sender, EventArgs e)
        {
         //   this.WindowState = FormWindowState.Maximized;
        }
    }
}
