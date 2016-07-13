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

namespace badpaybad.redis.sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        RedisPubSubSample _pubSubSample = new RedisPubSubSample();

        private void btnPush_Click(object sender, EventArgs e)
        {
            var msg = string.Format("Test msg at {0}\r\n{1}",DateTime.Now,txtMsg.Text);
            _pubSubSample.PushMessage(msg);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _pubSubSample.SubcribeMessage((m) =>
            {
                if (txtMsgPushed.InvokeRequired)
                {
                    txtMsgPushed.Invoke(new MethodInvoker(() => { txtMsgPushed.Text = m; }));
                }
                else
                {
                    txtMsgPushed.Text = m;
                }
            });
        }
    }
}