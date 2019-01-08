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

namespace HoneywellScannerXamarin
{
    [Activity(Label = "ActivityMain", MainLauncher = true)]
    public class ActivityMain : Activity
    {
        Button btnBarcodeXamarin;
        Button btnBarcodeIntent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.MainActivity);
            btnBarcodeIntent = FindViewById<Button>(Resource.Id.btnScannerIntent);

            btnBarcodeXamarin = FindViewById<Button>(Resource.Id.btnScannerXamarin);

            btnBarcodeIntent.Click += BtnBarcodeIntent_Click;
            btnBarcodeXamarin.Click += BtnBarcodeXamarin_Click;
        }

        private void BtnBarcodeXamarin_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ActivityBarcodeApi));
        }

        private void BtnBarcodeIntent_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ActivityIntentApi));
        }
    }
}