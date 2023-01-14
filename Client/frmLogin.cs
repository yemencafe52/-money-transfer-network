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
    public partial class frmLogin : Form
    {
        private bool sucess = false;

        internal bool Sucess
        {
            get
            {
                return this.sucess;
            }
        }

        public frmLogin()
        {
            InitializeComponent();
            Preparing();
            
        }

        private bool Preparing()
        {
            comboBox1.SelectedIndex = 0;
            comboBox1.Enabled = false;
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!(MyConnection.IsConnected))
            {
                MessageBox.Show("الشبكة غير متوفرة حالياً");
                return;
            }

            if(string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                return;
            }

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Focus();
                return;
            }


            User user = new User(1, textBox1.Text, textBox2.Text);
            if(User.Login(user))
            {
                this.sucess = true;
            }

            this.Close();
        }
    }
}
