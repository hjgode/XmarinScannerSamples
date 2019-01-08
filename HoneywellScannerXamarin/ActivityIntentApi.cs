using Android.App;
using Android.Widget;
using Android.OS;

namespace HoneywellScannerXamarin
{
    [Activity(Label = "HoneywellScannerIntentApi", MainLauncher = false)]
    public class ActivityIntentApi : Activity
    {
        BarcodeIntent BarcodeReader=null;
        Button btnScan;
        Button btnStop;
        TextView text;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            myLog.doLog("OnCreate");
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.BarcodeReaderIN);
            btnScan = (Button)FindViewById(Resource.Id.btnStartIntent);
            btnScan.Click += BtnScan_Click;
            btnStop = (Button)FindViewById(Resource.Id.btnStopIntent);
            btnStop.Click += BtnStop_Click;
            text = (TextView)FindViewById(Resource.Id.textView1);

            if (BarcodeReader == null)
            {
                BarcodeReader = new BarcodeIntent(ApplicationContext);
                BarcodeIntent.onBarcodeReadEvent += new BarcodeIntent.BarcodeReadHandler(onBarcode);

                //example of changing properties
                BarcodeReader.setProperty(IntentScannerProperties.PROPERTY_PDF_417_ENABLED, true);
            }

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

        protected override void OnPause()
        {
            myLog.doLog("OnPause");
            base.OnPause();
            if (BarcodeReader != null)
            {
                BarcodeIntent.onBarcodeReadEvent -= onBarcode;
                BarcodeReader.Dispose();
                BarcodeReader = null;
            }
        }
        protected override void OnResume()
        {
            myLog.doLog("OnCreate");
            base.OnResume();
            if (BarcodeReader == null)
            {
                BarcodeReader = new BarcodeIntent(ApplicationContext);
                BarcodeIntent.onBarcodeReadEvent += new BarcodeIntent.BarcodeReadHandler(onBarcode);
            }
        }

        void onBarcode(BarcodeIntent.BarcodeReadEventArgs e)
        {
            myLog.doLog("OnBarcode: "+e.data);
            RunOnUiThread(() => text.Text = e.ToString()
            );
        }
    }
}

