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
    }
}