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
    public partial class frmDashBoard : Form
    {
        public frmDashBoard()
        {
            InitializeComponent();
        }

        private void frmDashBoard_DoubleClick(object sender, EventArgs e)
        {
          

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = User.ActiveUser.Username;
            toolStripStatusLabel2.Text = DateTime.Now.ToString();

            if (MyConnection.PING == 0)
            {
                toolStripStatusLabel3.Text = "> 1 ms";
            }
            else
            {
                toolStripStatusLabel3.Text = MyConnection.PING.ToString() + " ms";
            }

            Event ee = new Event("", DateTime.Now);

            while(Event.Read(ref ee))
            {
                ListViewItem lvi = new ListViewItem(ee.GetMessage);
                lvi.SubItems.Add(ee.GetTime.ToString());
                listView1.Items.Add(lvi);
                listView1.EnsureVisible(listView1.Items.Count - 1);
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TreeNode tv = treeView1.SelectedNode; ;

            if(tv != null)
            {
                if (tv.Tag != null)
                {
                    int num = 0;
                    if(int.TryParse(tv.Tag.ToString(),out num))
                    {
                        switch(num)
                        {
                            case 0:
                                {
                                    break;
                                }
                            case 15:
                                {
                                    frmOGT ogt = new frmOGT();
                                    ogt.MdiParent = this;
                                    ogt.Show();
                                    break;
                                }
                            case 16:
                                {
                                    break;
                                }
                        }
                    }


                }
            }
        }
    }
}
