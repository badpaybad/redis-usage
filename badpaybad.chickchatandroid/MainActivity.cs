using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Text;
using Android.Webkit;

namespace badpaybad.chickchatandroid
{
    [Activity(Label = "Anonymous chickchat", MainLauncher = true, Icon = "@drawable/favicon")]
    public class MainActivity : Activity
    {
        private Button btnLogin;
        private EditText txtUid;
        private EditText txtGroup;

        private WebView wvChickChat;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.ChickChatWebView);

            //txtUid =FindViewById<EditText>(Resource.Id.txtUid);
            //txtGroup = FindViewById<EditText>(Resource.Id.txtGroup);
            //btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            
            //btnLogin.Click += BtnLogin_Click;\

            wvChickChat = FindViewById<WebView>(Resource.Id.webView1);
            wvChickChat.Settings.JavaScriptEnabled = true;
            wvChickChat.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            
            wvChickChat.LoadUrl("http://chickchat.badpaybad.info");
        }

      

        //private void BtnLogin_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtUid.Text) || string.IsNullOrEmpty(txtGroup.Text))
        //    {
        //        AlertDialog.Builder alert= new AlertDialog.Builder(this);
        //        alert.SetTitle("Plz check your input")
        //            .SetPositiveButton("Ok",(s,a)=> {});

        //        RunOnUiThread(() =>
        //        {
        //            alert.Show();
        //        });
        //        return;
        //    }

        //    string key;
        //    AppContext.ChickChatServices.RegisterGroup(txtUid.Text,txtGroup.Text,out key);

        //    AppContext._ownerKey = key;
        //    AppContext._groupOwner = AppContext.ChickChatServices.GetChickChat(key);
        //    AppContext._selectedKey = key;

        //    AppContext.TaskScheduler.Add("keep_me_alive", () =>
        //    {
        //        AppContext.ChickChatServices.PingKeepAlive(AppContext._ownerKey);
        //    },5,true);

        //    StartActivity(typeof(ChickChatBoxActivity));
        //}
    }
}

