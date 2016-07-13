using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace badpaybad.chickchatandroid
{
    [Activity(Label = "ChickChatBoxActivity")]
    public class ChickChatBoxActivity : Activity
    {
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ChickChatBox);

            
           

        }

        private void CreateTab(Type activityType, string tag, string label, int drawableId)
        {
           
        }
    }
}