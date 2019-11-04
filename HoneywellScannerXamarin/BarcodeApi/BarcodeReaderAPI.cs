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

using Honeywell.AIDC.CrossPlatform;

namespace HoneywellScannerXamarin
{
    class BarcodeReaderApi:IDisposable
    {
        const string DCS_Imager = "dcs.scanner.imager"; //you may need to adjust the name
        int ScanCount = 0;
        public BarcodeReader barcodeReader{
            get {
                if (mBarcodeReader == null)
                {
                    
                    OpenBarcodeReader();
                }
                return mBarcodeReader;
            }
        }

        private static BarcodeReader mBarcodeReader = null;
    
        public BarcodeReaderApi()
        {
            if (mBarcodeReader == null)
            {
                OpenBarcodeReader();
            }
        }

        public void Dispose()
        {
            if (mBarcodeReader != null)
            {
                CloseBarcodeScanner();
            }
        }

        public void startScan()
        {
            if (mBarcodeReader != null)
            {
                int res = mBarcodeReader.SoftwareTriggerAsync(true).Result.Code;
                if (res == BarcodeReader.Result.Codes.SUCCESS)
                    myLog.doLog("SoftwareTrigger on OK");
                else
                    myLog.doLog("SoftwareTrigger on Failed");
            }
        }

        public void stopScan()
        {
            if (mBarcodeReader != null)
            {
                int res = mBarcodeReader.SoftwareTriggerAsync(false).Result.Code;
                if (res == BarcodeReader.Result.Codes.SUCCESS)
                    myLog.doLog("SoftwareTrigger off OK");
                else
                    myLog.doLog("SoftwareTrigger off Failed");
            }

        }

        /// <summary>
        /// Opens the selected scanner device.
        /// </summary>
        async void OpenBarcodeReader()
        {
            myLog.doLog("OpenBarcodeReader...");
            if (mBarcodeReader==null)
            {
                myLog.doLog("mOpenReader...");
                mBarcodeReader = new BarcodeReader(DCS_Imager);
                if (!mBarcodeReader.IsReaderOpened)
                {
                    myLog.doLog("IsReaderOpened...");
                    BarcodeReader.Result result = await mBarcodeReader.OpenAsync();

                    mBarcodeReader.BarcodeDataReady += new EventHandler<BarcodeDataArgs>(MBarcodeReader_BarcodeDataReady);

                    // Check to see is reader opened or is already open
                    if (result.Code == BarcodeReader.Result.Codes.SUCCESS ||
                        result.Code == BarcodeReader.Result.Codes.READER_ALREADY_OPENED)
                    {
                        ScanCount = 0;
                    }
                    else
                    {
                        myLog.doLog("Error", "OpenAsync failed, Code:" + result.Code + " Message:" + result.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Closes the selected scanner device.
        /// </summary>
        async void CloseBarcodeScanner()
        {
            myLog.doLog("CloseBarcodeScanner...");
            if (mBarcodeReader != null && mBarcodeReader.IsReaderOpened)
            {
                //settings.Clear();
                BarcodeReader.Result result = await mBarcodeReader.CloseAsync();
                if (result.Code == BarcodeReader.Result.Codes.SUCCESS)
                {
                    mBarcodeReader.BarcodeDataReady -= MBarcodeReader_BarcodeDataReady;
                    mBarcodeReader.Dispose();
                    mBarcodeReader = null;
                }
                else
                {
                    myLog.doLog("Error", "CloseAsync failed, Code:" + result.Code + " Message:" + result.Message);
                }
            }
        }

        /// <summary>
        /// Event that receives the data read by the scanner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MBarcodeReader_BarcodeDataReady(object sender, BarcodeDataArgs e)
        {
            myLog.doLog("MBarcodeReader_BarcodeDataReady: >" + e.Data + "<");
            myLog.doLog("MBarcodeReader_BarcodeDataReady: " + myLog.strToHexString(e.Data));
            ScanCount++;
            //Changed Check there is a registered scan event
            myLog.doLog("MBarcodeReader_BarcodeDataReady: calling Scan_Result_Event...");
            onBarcodeRead(new BarcodeReadEventArgs(e.Data, e.TimeStamp, e.SymbologyType, e.SymbologyName));
        }

        public delegate void BarcodeReadHandler(BarcodeReadEventArgs e);
        public event BarcodeReadHandler onBarcodeReadEvent;
        void onBarcodeRead(BarcodeReadEventArgs a)
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
            public string data = "";
            public DateTime timestamp = DateTime.Now;
            public uint symbologyType = 0;
            public string symbologyName = "";
            public BarcodeReadEventArgs(string d, DateTime t, uint ty, string name)
            {
                data = d;
                timestamp = t;
                symbologyType = ty;
                symbologyName = name;
            }
            public override string ToString()
            {
                
                string s = data + "\n" +myLog.strToHexString(data)+"\n" + timestamp.ToLongTimeString() + "\n" + symbologyType.ToString() + "\n" + symbologyName +"\n";
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
