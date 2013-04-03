using android.content;
using android.os;
using ScriptCoreLib;
using ScriptCoreLib.Delegates;
using ScriptCoreLib.Extensions;
using System;
using System.Linq;
using System.Xml.Linq;

namespace com.abstractatech.battery
{
    /// <summary>
    /// Methods defined in this type can be used from JavaScript. The method calls will seamlessly be proxied to the server.
    /// </summary>
    public sealed class ApplicationWebService
    {
        Context context = global::ScriptCoreLib.Android.ThreadLocalContextReference.CurrentContext;


        // http://developer.android.com/training/monitoring-device-state/battery-monitoring.html
        // http://davidwalsh.name/battery-api
        // https://developer.mozilla.org/en-US/docs/DOM/window.navigator.battery

        // Because it's a sticky intent, you don't need to 
        // register a BroadcastReceiveróby simply 
        // calling registerReceiver passing in null as the receiver as 
        // shown in the next snippet, the current battery status 
        // intent is returned.


        public void batteryStatus(Action<string> y)
        {
            var ifilter = new IntentFilter(Intent.ACTION_BATTERY_CHANGED);
            var batteryStatus = context.registerReceiver(null, ifilter);

            int level = batteryStatus.getIntExtra(BatteryManager.EXTRA_LEVEL, -1);
            int scale = batteryStatus.getIntExtra(BatteryManager.EXTRA_SCALE, -1);

            float batteryPct = level / (float)scale;

            y("" + batteryPct);
        }

        // jsc please implement other datatypes :)
    }
}
