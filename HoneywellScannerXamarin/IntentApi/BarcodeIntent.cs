using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

using System;

// do not forget to add
// <uses-permission android:name="com.honeywell.decode.permission.DECODE" />
// to manifest.xml

namespace HoneywellScannerXamarin
{
    class BarcodeIntent:IDisposable
    {
        Context _context;
        #region Intent_Strings
        public const String ACTION_BARCODE_DATA = "com.honeywell.sample.action.BARCODE_DATA";
        /**
         * Honeywell DataCollection Intent API
         * Claim scanner
         * Package Permissions:
         * "com.honeywell.decode.permission.DECODE"
         */
        public const String ACTION_CLAIM_SCANNER = "com.honeywell.aidc.action.ACTION_CLAIM_SCANNER";
        /**
         * Honeywell DataCollection Intent API
         * Release scanner claim
         * Permissions:
         * "com.honeywell.decode.permission.DECODE"
         */
        public const String ACTION_RELEASE_SCANNER = "com.honeywell.aidc.action.ACTION_RELEASE_SCANNER";
        /**
         * Honeywell DataCollection Intent API
         * Optional. Sets the scanner to claim. If scanner is not available or if extra is not used,
         * DataCollection will choose an available scanner.
         * Values : String
         * "dcs.scanner.imager" : Uses the internal scanner
         * "dcs.scanner.ring" : Uses the external ring scanner
         */
        public const String EXTRA_SCANNER = "com.honeywell.aidc.extra.EXTRA_SCANNER";
        /**
         * Honeywell DataCollection Intent API
         * Optional. Sets the profile to use. If profile is not available or if extra is not used,
         * the scanner will use factory default properties (not "DEFAULT" profile properties).
         * Values : String
         */
        public const String EXTRA_PROFILE = "com.honeywell.aidc.extra.EXTRA_PROFILE";
        /**
         * Honeywell DataCollection Intent API
         * Optional. Overrides the profile properties (non-persistent) until the next scanner claim.
         * Values : Bundle
         */
        public const String EXTRA_PROPERTIES = "com.honeywell.aidc.extra.EXTRA_PROPERTIES";

        public const String EXTRA_CONTROL = "com.honeywell.aidc.action.ACTION_CONTROL_SCANNER";
        /*
            Extras
            "com.honeywell.aidc.extra.EXTRA_SCAN" (boolean): Set to true to start or continue scanning. Set to false to stop scanning. Most scenarios only need this extra, however the scanner can be put into other states by adding from the following extras.
            "com.honeywell.aidc.extra.EXTRA_AIM" (boolean): Specify whether to turn the scanner aimer on or off. This is optional; the default value is the value of EXTRA_SCAN.
            "com.honeywell.aidc.extra.EXTRA_LIGHT" (boolean): Specify whether to turn the scanner illumination on or off. This is optional; the default value is the value of EXTRA_SCAN.
            "com.honeywell.aidc.extra.EXTRA_DECODE" (boolean): Specify whether to turn the decoding operation on or off. This is optional; the default value is the value of EXTRA_SCAN
        */
        public const String EXTRA_SCAN = "com.honeywell.aidc.extra.EXTRA_SCAN";
        #endregion

        //public barcodeDataReceiver _barcodeDataReceiver;
        public BarcodeIntent(Context context)
        {
            _context = context;
            //_barcodeDataReceiver = new barcodeDataReceiver(this);
            claimScanner();
        }

        public void Dispose()
        {
            releaseScanner();
        }
        private void claimScanner()
        {
            myLog.doLog("IntentApiSample: ", "claimScanner");
            Bundle properties = new Bundle();
            properties.PutBoolean("DPR_DATA_INTENT", true);
            properties.PutString("DPR_DATA_INTENT_ACTION", ACTION_BARCODE_DATA);
            _context.SendBroadcast(new Intent(ACTION_CLAIM_SCANNER)
                    .PutExtra(EXTRA_SCANNER, "dcs.scanner.imager")
                    .PutExtra(EXTRA_PROFILE, "DEFAULT")// "MyProfile1")
                    .PutExtra(EXTRA_PROPERTIES, properties)
            );
        }

        public void setProperty(String key, bool bEnable)
        {
            Bundle props = new Bundle();
            props.PutBoolean(key, bEnable);
            _context.SendBroadcast(new Intent(EXTRA_PROFILE)
                .PutExtra(EXTRA_PROPERTIES, props)
                );
        }
        private void releaseScanner()
        {
            myLog.doLog("IntentApiSample: ", "releaseScanner");
            _context.SendBroadcast(new Intent(ACTION_RELEASE_SCANNER));
        }

        public void startScan()
        {
            myLog.doLog("IntentApiSample: ", "startScan");
            _context.SendBroadcast(new Intent(EXTRA_CONTROL)
                            .PutExtra(EXTRA_SCAN, true)
                            );
        }
        public void stopScan()
        {
            myLog.doLog("IntentApiSample: ", "stopScan");
            _context.SendBroadcast(new Intent(EXTRA_CONTROL)
                            .PutExtra(EXTRA_SCAN, false)
                            );
        }

        [BroadcastReceiver(Enabled = true)]
        [IntentFilter(new[] { ACTION_BARCODE_DATA })]
        public class barcodeDataReceiver : Android.Content.BroadcastReceiver
        {
            BarcodeIntent _scanner = null;
            //public barcodeDataReceiver(HoneywellScanner scanner)
            //{
            //    _scanner = scanner;
            //}
            public barcodeDataReceiver()
            {

            }
            public override void OnReceive(Context context, Intent intent)
            {
                myLog.doLog("IntentApiSample: ", "onReceive");
                if (ACTION_BARCODE_DATA.Equals(intent.Action))
                {
                    /*
                    These extras are available:
                    "version" (int) = Data Intent Api version
                    "aimId" (String) = The AIM Identifier
                    "charset" (String) = The charset used to convert "dataBytes" to "data" string
                    "codeId" (String) = The Honeywell Symbology Identifier
                    "data" (String) = The barcode data as a string
                    "dataBytes" (byte[]) = The barcode data as a byte array
                    "timestamp" (String) = The barcode timestamp
                    */
                    int version = intent.GetIntExtra("version", 0);
                    if (version >= 1)
                    {
                        String aimId = intent.GetStringExtra("aimId");
                        String charset = intent.GetStringExtra("charset");
                        String codeId = intent.GetStringExtra("codeId");
                        String data = intent.GetStringExtra("data");
                        byte[] dataBytes = intent.GetByteArrayExtra("dataBytes");
                        String dataBytesStr = "";
                        if (dataBytes != null && dataBytes.Length > 0)
                            dataBytesStr = bytesToHexString(dataBytes);
                        String timestamp = intent.GetStringExtra("timestamp");
                        String text = String.Format(
                                "Data:{0}\n" +
                                        "Charset:{1}\n" +
                                        "Bytes:{2}\n" +
                                        "AimId:{3}\n" +
                                        "CodeId:{4}\n" +
                                        "Timestamp:{5}\n",
                                data, charset, dataBytesStr, aimId, codeId, timestamp);

                        myLog.doLog(text);

                        //if(_scanner!=null)
                            BarcodeIntent.onBarcodeRead(new BarcodeReadEventArgs(aimId, charset, codeId, data, dataBytes, timestamp));
                    }
                }
            }
        }
        public delegate void BarcodeReadHandler(BarcodeReadEventArgs e);
        public static event BarcodeReadHandler onBarcodeReadEvent;
        static void onBarcodeRead(BarcodeReadEventArgs a)
        {
            if (onBarcodeReadEvent != null)
            {
                myLog.doLog("onBarcodeRead");
                BarcodeReadEventArgs args = a; //local copy
                onBarcodeReadEvent(a);
            }
        }
        public class BarcodeReadEventArgs : EventArgs
        {
            public string aimID = "";
            public string charSet = "";
            public string codeId = "";
            public string data = "";
            public byte[] dataBytes = new byte[] { };
            public string timestamp = "";
            public BarcodeReadEventArgs(string aim, string chrset, string codeid, string datastr, byte[] databytes, string timestmp)
            {
                aimID = aim;
                charSet = chrset;
                codeId = codeid;
                data = datastr;
                dataBytes = databytes;
                timestamp = timestmp;
            }
            public override string ToString()
            {
                string binstr = bytesToHexString(this.dataBytes);
                string s = data+"\n"+binstr+"\n"+timestamp+"\n"+ aimID + "\n" + charSet + "\n" + codeId;
                return s;
            }
        }
        private static String bytesToHexString(byte[] arr)
        {
            String s = "[]";
            if (arr != null)
            {
                s = "[";
                for (int i = 0; i < arr.Length; i++)
                {
                    s += "0x" + arr[i].ToString("x2") + ", ";// + Integer.toHexString(arr[i]) + ", ";
                }
                s = s.Substring(0, s.Length - 2) + "]";
            }
            return s;
        }
    }
}