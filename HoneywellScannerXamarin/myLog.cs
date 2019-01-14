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
    public class myLog
    {
        public static void doLog(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
            Android.Util.Log.Debug("IntentApiClass: ", s);
        }
        public static void doLog(string t, string s)
        {
            System.Diagnostics.Debug.WriteLine(t+"\t"+s);
            Android.Util.Log.Debug(t, s);
        }

        public static String strToHexString(string sin)
        {
            byte[] arr = Encoding.GetEncoding(1252).GetBytes(sin);
            String s = "[]";
            if (arr != null)
            {
                s = "[";
                foreach (char c in sin)
                {
                    if (c < 32)
                    {
                        s += "0x" + Convert.ToByte(c).ToString("x2") + ", ";// + Integer.toHexString(arr[i]) + ", ";
                    }
                    else
                    {
                        s += "" + c.ToString() + ", ";
                    }
                }
                s = s.Substring(0, s.Length - 2) + "]";
            }
            return s;
        }

        public static String bytesToHexString(byte[] arr)
        {
            String s = "[]";
            if (arr != null)
            {
                s = "[";
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] < 32)
                        s += "0x" + arr[i].ToString("x2") + ", ";// + Integer.toHexString(arr[i]) + ", ";
                    else
                        s += "" + arr[i].ToString() + ", ";
                }
                s = s.Substring(0, s.Length - 2) + "]";
            }
            return s;
        }

    }
}