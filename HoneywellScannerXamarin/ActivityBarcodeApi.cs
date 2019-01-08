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
    [Activity(Label = "ActivityBarcodeApi", MainLauncher = false)]
    public class ActivityBarcodeApi : Activity
    {
        BarcodeReaderApi BarcodeReader = null;
        Button btnScan;
        Button btnStop;
        TextView text;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.BarcoderReaderXM);
            btnScan = (Button)FindViewById(Resource.Id.btnStartAPI);
            btnScan.Click += BtnScan_Click;
            btnStop = (Button)FindViewById(Resource.Id.btnStopAPI);
            btnStop.Click += BtnStop_Click;
            text = (TextView)FindViewById(Resource.Id.textView1);

            startReader();
        }

        void startReader()
        {
            if (BarcodeReader == null)
            {
                BarcodeReader = new BarcodeReaderApi();
                BarcodeReader.onBarcodeReadEvent += BarcodeReader_onBarcodeReadEvent;
            }

        }

        private void BarcodeReader_onBarcodeReadEvent(BarcodeReaderApi.BarcodeReadEventArgs e)
        {
            myLog.doLog("OnBarcode: " + e.data);
            RunOnUiThread(() => text.Text = e.ToString()
            );
        }

        void stopReader()
        {
            if (BarcodeReader != null)
            {
                BarcodeReader.onBarcodeReadEvent -= BarcodeReader_onBarcodeReadEvent;
                BarcodeReader.Dispose();
            }
        }
        protected override void OnPause()
        {
            base.OnPause();
            stopReader();
        }

        protected override void OnResume()
        {
            base.OnResume();
            startReader();
        }
        private void BtnStop_Click(object sender, System.EventArgs e)
        {
            if (BarcodeReader != null)
            {
                BarcodeReader.stopScan();
            }
        }

        private void BtnScan_Click(object sender, System.EventArgs e)
        {
            myLog.doLog("BtnScan_Click");
            if (BarcodeReader != null)
            {
                BarcodeReader.startScan();
            }
        }

    }
}