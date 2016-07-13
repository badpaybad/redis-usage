namespace badpaybad.chickchat
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.grbBg = new System.Windows.Forms.GroupBox();
            this.splGroup = new System.Windows.Forms.SplitContainer();
            this.grbGroupChannel = new System.Windows.Forms.GroupBox();
            this.lsvGroups = new System.Windows.Forms.ListView();
            this.grbMessages = new System.Windows.Forms.GroupBox();
            this.splMessages = new System.Windows.Forms.SplitContainer();
            this.txtListMessage = new System.Windows.Forms.TextBox();
            this.splSendMessage = new System.Windows.Forms.SplitContainer();
            this.txtMessageToSend = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.trmPing = new System.Windows.Forms.Timer(this.components);
            this.grbBg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splGroup)).BeginInit();
            this.splGroup.Panel1.SuspendLayout();
            this.splGroup.Panel2.SuspendLayout();
            this.splGroup.SuspendLayout();
            this.grbGroupChannel.SuspendLayout();
            this.grbMessages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splMessages)).BeginInit();
            this.splMessages.Panel1.SuspendLayout();
            this.splMessages.Panel2.SuspendLayout();
            this.splMessages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splSendMessage)).BeginInit();
            this.splSendMessage.Panel1.SuspendLayout();
            this.splSendMessage.Panel2.SuspendLayout();
            this.splSendMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Location = new System.Drawing.Point(0, 796);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1168, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1168, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // statusStrip2
            // 
            this.statusStrip2.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip2.Location = new System.Drawing.Point(0, 24);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(1168, 22);
            this.statusStrip2.TabIndex = 2;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // grbBg
            // 
            this.grbBg.Controls.Add(this.splGroup);
            this.grbBg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbBg.Location = new System.Drawing.Point(0, 46);
            this.grbBg.Name = "grbBg";
            this.grbBg.Size = new System.Drawing.Size(1168, 750);
            this.grbBg.TabIndex = 3;
            this.grbBg.TabStop = false;
            // 
            // splGroup
            // 
            this.splGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splGroup.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splGroup.Location = new System.Drawing.Point(3, 22);
            this.splGroup.Name = "splGroup";
            // 
            // splGroup.Panel1
            // 
            this.splGroup.Panel1.Controls.Add(this.grbGroupChannel);
            // 
            // splGroup.Panel2
            // 
            this.splGroup.Panel2.Controls.Add(this.grbMessages);
            this.splGroup.Size = new System.Drawing.Size(1162, 725);
            this.splGroup.SplitterDistance = 370;
            this.splGroup.TabIndex = 0;
            // 
            // grbGroupChannel
            // 
            this.grbGroupChannel.Controls.Add(this.lsvGroups);
            this.grbGroupChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbGroupChannel.Location = new System.Drawing.Point(0, 0);
            this.grbGroupChannel.Name = "grbGroupChannel";
            this.grbGroupChannel.Size = new System.Drawing.Size(370, 725);
            this.grbGroupChannel.TabIndex = 0;
            this.grbGroupChannel.TabStop = false;
            this.grbGroupChannel.Text = "Groups (double click to join)";
            // 
            // lsvGroups
            // 
            this.lsvGroups.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lsvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvGroups.FullRowSelect = true;
            this.lsvGroups.Location = new System.Drawing.Point(3, 22);
            this.lsvGroups.MultiSelect = false;
            this.lsvGroups.Name = "lsvGroups";
            this.lsvGroups.Size = new System.Drawing.Size(364, 700);
            this.lsvGroups.TabIndex = 0;
            this.lsvGroups.UseCompatibleStateImageBehavior = false;
            this.lsvGroups.View = System.Windows.Forms.View.List;
            this.lsvGroups.ItemActivate += new System.EventHandler(this.lsvGroups_ItemActivate);
            this.lsvGroups.DoubleClick += new System.EventHandler(this.lsvGroups_DoubleClick);
            // 
            // grbMessages
            // 
            this.grbMessages.Controls.Add(this.splMessages);
            this.grbMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbMessages.Location = new System.Drawing.Point(0, 0);
            this.grbMessages.Name = "grbMessages";
            this.grbMessages.Size = new System.Drawing.Size(788, 725);
            this.grbMessages.TabIndex = 1;
            this.grbMessages.TabStop = false;
            this.grbMessages.Text = "Messages";
            // 
            // splMessages
            // 
            this.splMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splMessages.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splMessages.Location = new System.Drawing.Point(3, 22);
            this.splMessages.Name = "splMessages";
            this.splMessages.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splMessages.Panel1
            // 
            this.splMessages.Panel1.Controls.Add(this.txtListMessage);
            // 
            // splMessages.Panel2
            // 
            this.splMessages.Panel2.Controls.Add(this.splSendMessage);
            this.splMessages.Size = new System.Drawing.Size(782, 700);
            this.splMessages.SplitterDistance = 540;
            this.splMessages.TabIndex = 0;
            // 
            // txtListMessage
            // 
            this.txtListMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtListMessage.Location = new System.Drawing.Point(0, 0);
            this.txtListMessage.Multiline = true;
            this.txtListMessage.Name = "txtListMessage";
            this.txtListMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtListMessage.Size = new System.Drawing.Size(782, 540);
            this.txtListMessage.TabIndex = 1;
            // 
            // splSendMessage
            // 
            this.splSendMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splSendMessage.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splSendMessage.Location = new System.Drawing.Point(0, 0);
            this.splSendMessage.Name = "splSendMessage";
            // 
            // splSendMessage.Panel1
            // 
            this.splSendMessage.Panel1.Controls.Add(this.txtMessageToSend);
            // 
            // splSendMessage.Panel2
            // 
            this.splSendMessage.Panel2.Controls.Add(this.btnSend);
            this.splSendMessage.Size = new System.Drawing.Size(782, 156);
            this.splSendMessage.SplitterDistance = 611;
            this.splSendMessage.TabIndex = 0;
            // 
            // txtMessageToSend
            // 
            this.txtMessageToSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessageToSend.Location = new System.Drawing.Point(0, 0);
            this.txtMessageToSend.Multiline = true;
            this.txtMessageToSend.Name = "txtMessageToSend";
            this.txtMessageToSend.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessageToSend.Size = new System.Drawing.Size(611, 156);
            this.txtMessageToSend.TabIndex = 0;
            this.txtMessageToSend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessageToSend_KeyPress);
            // 
            // btnSend
            // 
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSend.Location = new System.Drawing.Point(0, 0);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(167, 156);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // trmPing
            // 
            this.trmPing.Interval = 1000;
            this.trmPing.Tick += new System.EventHandler(this.trmPing_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 818);
            this.Controls.Add(this.grbBg);
            this.Controls.Add(this.statusStrip2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Anonymous - ChickChat";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grbBg.ResumeLayout(false);
            this.splGroup.Panel1.ResumeLayout(false);
            this.splGroup.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splGroup)).EndInit();
            this.splGroup.ResumeLayout(false);
            this.grbGroupChannel.ResumeLayout(false);
            this.grbMessages.ResumeLayout(false);
            this.splMessages.Panel1.ResumeLayout(false);
            this.splMessages.Panel1.PerformLayout();
            this.splMessages.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splMessages)).EndInit();
            this.splMessages.ResumeLayout(false);
            this.splSendMessage.Panel1.ResumeLayout(false);
            this.splSendMessage.Panel1.PerformLayout();
            this.splSendMessage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splSendMessage)).EndInit();
            this.splSendMessage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.GroupBox grbBg;
        private System.Windows.Forms.SplitContainer splGroup;
        private System.Windows.Forms.GroupBox grbGroupChannel;
        private System.Windows.Forms.SplitContainer splMessages;
        private System.Windows.Forms.ListView lsvGroups;
        private System.Windows.Forms.GroupBox grbMessages;
        private System.Windows.Forms.SplitContainer splSendMessage;
        private System.Windows.Forms.TextBox txtMessageToSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtListMessage;
        private System.Windows.Forms.Timer trmPing;
    }
}

