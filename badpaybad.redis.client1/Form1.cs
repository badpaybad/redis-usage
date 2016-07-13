using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using badpaybad.redis.repository;

namespace badpaybad.redis.client1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        RedisPubSubSample _pubSubSample = new RedisPubSubSample();

        private void Form1_Load(object sender, EventArgs e)
        {
            _pubSubSample.SubcribeMessage((m) =>
            {
                if (txtMsg.InvokeRequired)
                {
                    txtMsg.Invoke(new MethodInvoker(() => { txtMsg.Text = m; }));
                }
                else
                {
                    txtMsg.Text = m;
                }
            });
        }
    }
}