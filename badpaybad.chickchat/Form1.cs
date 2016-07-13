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

namespace badpaybad.chickchat
{
    public partial class Form1 : Form
    {
        frmLogin _frmLogin = new frmLogin();

        public Form1()
        {
            InitializeComponent();
            grbBg.Visible = false;
            ServicesContext.ChickChatServices.RefreshListGroupChickChat((list) =>
            {
                if (lsvGroups.InvokeRequired)
                {
                    lsvGroups.Invoke(new MethodInvoker(() => { BindChatGroups(list); }));
                }
                else
                {
                    BindChatGroups(list);
                }
            });

           
        }

        private void BindChatGroups(List<ChickChatGroup> list)
        {
            lsvGroups.Items.Clear();

            if (list.Count > 0)
            {
                if (string.IsNullOrEmpty(ServicesContext.CurrentChannelKeyChatting))
                {
                    var xxx = ServicesContext.CurrentChannelKeyOwner + "";
                    ServicesContext.CurrentChannelKeyChatting = xxx;
                }

                foreach (var cc in list)
                {
                    if (!cc.IsOnline) continue;

                    var listViewItem = new ListViewItem();
                    listViewItem.Text = cc.AnonymousName + ":" + cc.AnonymousTitle + ":" + cc.ChannelKey.GetHashCode() +
                                        ":" + (cc.IsOnline == true ? "Online" : "Offline");
                    listViewItem.Name = cc.ChannelKey;

                    if (cc.ChannelKey.Equals(ServicesContext.CurrentChannelKeyChatting))
                    {
                        listViewItem.Selected = true;
                    }

                    lsvGroups.Items.Add(listViewItem);
                }

                JoinChat();
                BindListMessage();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_frmLogin.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            grbBg.Visible = true;

            var owner = ServicesContext.ChickChatServices.GetChickChat(
                ServicesContext.CurrentChannelKeyOwner);

            this.Text = "Anonymous chick chat as " + owner.AnonymousName + ":" + owner.AnonymousTitle + ":" +
                        owner.ChannelKey.GetHashCode();
        }

        private void lsvGroups_ItemActivate(object sender, EventArgs e)
        {
        }

        private void lsvGroups_DoubleClick(object sender, EventArgs e)
        {
            if (lsvGroups.SelectedItems.Count == 0)
            {
                return;
            }

            JoinChat();
            BindListMessage();
        }

        private void JoinChat()
        {
            if (lsvGroups.SelectedItems.Count <= 0) return;

            var selected = lsvGroups.SelectedItems[0];
            var channelKey = selected.Name;
            ServicesContext.CurrentChannelKeyChatting = channelKey;

            grbMessages.Text = selected.Text;

            ServicesContext.ChickChatServices.JoinChat(channelKey, (list) =>
            {
                ServicesContext.MessagesHanlder.Register(channelKey, list);

                BindListMessage(list);
            });
        }

        private void BindListMessage(List<ChatMessage> list = null)
        {
            if (list == null)
            {
                list = ServicesContext.ChickChatServices.GetMessageseChat(
                    ServicesContext.CurrentChannelKeyChatting);
            }
            list = list.OrderBy(i => i.Date).ToList();
            if (txtListMessage.InvokeRequired)
            {
                txtListMessage.Invoke(new MethodInvoker(() => { BindListMessageToTextBox(list); }));
            }
            else
            {
                BindListMessageToTextBox(list);
            }
        }

        private void BindListMessageToTextBox(List<ChatMessage> currentMsgs)
        {
            var temp = "";
            foreach (var cm in currentMsgs)
            {
                temp += BuildMessageDisplay(cm);
            }
            txtListMessage.Text = temp;

            txtListMessage.SelectionStart = txtListMessage.TextLength;
            txtListMessage.ScrollToCaret();
        }

        private static string BuildMessageDisplay(ChatMessage cm)
        {
            var indexOf = cm.Message.IndexOf(":");
            var idx = indexOf + 1;
            var temp = cm.Message.Substring(idx);
            var cnk = cm.Message.Substring(0, indexOf).GetHashCode();
            return "\r\n" + cm.Date + "-" + cnk + "\r\n" + temp;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void SendMessage()
        {
            if (string.IsNullOrEmpty(txtMessageToSend.Text)) return;

            ServicesContext.ChickChatServices.PublishMessage(
                ServicesContext.CurrentChannelKeyChatting
                , ServicesContext.CurrentChannelKeyOwner
                , txtMessageToSend.Text);

            txtMessageToSend.Text = "";
        }

        private void txtMessageToSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendMessage();
            }
        }

        private void trmPing_Tick(object sender, EventArgs e)
        {
            ServicesContext.ChickChatServices.PingKeepAlive(ServicesContext.CurrentChannelKeyOwner);
        }
    }
}