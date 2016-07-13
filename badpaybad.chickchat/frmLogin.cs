using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace badpaybad.chickchat
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("Name or Title is empty");
                return;
            }

            string channelKey;
            ServicesContext.ChickChatServices.RegisterGroup(txtName.Text, txtTitle.Text, out channelKey);

            ServicesContext.CurrentChannelKeyOwner = channelKey;

            DialogResult= DialogResult.OK;
        }
    }
}
