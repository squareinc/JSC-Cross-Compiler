using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using android.app;
using android.content;
using android.provider;
using android.view;
using android.webkit;
using android.widget;
using AndroidBootServiceNotificationActivity.Activities;
//using java.lang;
using ScriptCoreLib;
using ScriptCoreLib.Android;
using ScriptCoreLib.Android.Extensions;
using ScriptCoreLibJava.Extensions;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace AndroidBootServiceNotificationActivity.Activities
{
    [ScriptCoreLib.Android.Manifest.ApplicationMetaData(name = "android:minSdkVersion", value = "8")]
    [ScriptCoreLib.Android.Manifest.ApplicationMetaData(name = "android:targetSdkVersion", value = "22")]

    public class LocalApplication :
        // can we change our base depeinding on the process we are in?
       Application
    {
        public override void onCreate()
        {
            base.onCreate();

            // yes we are loaded for both processes.
            {
                var myPid = android.os.Process.myPid();

                Console.WriteLine("enter AndroidBootServiceNotificationActivity onCreate, first time? " + new { myPid });

                // https://stackoverflow.com/questions/7686482/when-does-applications-oncreate-method-is-called-on-android
                Toast.makeText(this, "AndroidBootServiceNotificationActivity " + new { myPid }, Toast.LENGTH_LONG).show();
            }
        }
    }




    // http://android-er.blogspot.com/2011/04/start-service-to-send-notification.html

    [ScriptCoreLib.Android.Manifest.ApplicationMetaData(name = "android:theme", value = "@android:style/Theme.Holo.Dialog")]
    public class ApplicationActivity : Activity
    {
        // https://github.com/opersys/raidl


        // http://stackoverflow.com/questions/6274141/trigger-background-service-at-a-specific-time-in-android
        // http://stackoverflow.com/questions/7144908/how-is-an-intent-service-declared-in-the-android-manifest
        // http://developer.android.com/guide/topics/manifest/service-element.html

        // https://github.com/android/platform_frameworks_base/blob/master/services/accessibility/java/com/android/server/accessibility/AccessibilityManagerService.java

        //AtBootCompleted hack1;

        protected override void onCreate(global::android.os.Bundle savedInstanceState)
        {
            // http://developer.android.com/guide/topics/ui/notifiers/notifications.html

            base.onCreate(savedInstanceState);

            var sv = new ScrollView(this);
            var ll = new LinearLayout(this);

            ll.setOrientation(LinearLayout.VERTICAL);

            sv.addView(ll);

            #region startservice
            var startservice = new Button(this);
            startservice.setText("Start Service to send Notification");
            startservice.AtClick(
                delegate
                {
                    startservice.setEnabled(false);
                    //this.ShowToast("startservice_onclick");

                    //var intent = new Intent(this, NotifyService.Class);
                    var intent = new Intent(this, typeof(NotifyService).ToClass());
                    this.startService(intent);

                    // http://developer.android.com/reference/android/app/Activity.html#recreate%28%29
                    this.recreate();
                }
            );
            ll.addView(startservice);
            #endregion

            #region stopservice
            var stopservice = new Button(this);
            stopservice.setText("Stop Service");
            stopservice.AtClick(
                delegate
                {
                    this.ShowToast("stopservice_onclick");

                    var intent = new Intent();
                    intent.setAction(NotifyService.ACTION);
                    intent.putExtra("RQS", NotifyService.RQS_STOP_SERVICE);
                    this.sendBroadcast(intent);

                    // seems stop takes a while

                    //Task.Delay(100);

                    Thread.Sleep(30);

                    this.recreate();
                }
            );
            ll.addView(stopservice);
            #endregion

            stopservice.setEnabled(false);

            // http://stackoverflow.com/questions/12891903/android-check-if-my-service-is-running-in-the-background
            var m = (ActivityManager)this.getSystemService(Context.ACTIVITY_SERVICE);


            Console.WriteLine("getRunningServices");

            var s = m.getRunningServices(1000);

            Console.WriteLine("getRunningServices " + s.size());

            var se =
                // http://stackoverflow.com/questions/7170730/how-to-set-a-control-panel-for-my-service-in-android

                from i in Enumerable.Range(0, s.size())
                let rsi = (android.app.ActivityManager.RunningServiceInfo)s.get(i)
                let cn = rsi.service.getClassName()
                let cp = m.getRunningServiceControlPanel(rsi.service)

                //orderby cn
                orderby cp != null

                select new { i, rsi, cn, cp };
            //java.lang.JavaSystem.ex
            //     I/System.Console( 1617): { i = 45, cn = android.hardware.location.GeofenceHardwareService, cp =  }
            //I/System.Console( 1617): { i = 17, cn = ccc71.at.services.at_service, cp =  }
            //I/System.Console( 1617): { i = 34, cn = com.android.bluetooth.a2dp.A2dpService, cp =  }
            //I/System.Console( 1617): { i = 13, cn = com.android.bluetooth.btservice.AdapterService, cp =  }
            //I/System.Console( 1617): { i = 23, cn = com.android.bluetooth.gatt.GattService, cp =  }
            //I/System.Console( 1617): { i = 68, cn = com.android.bluetooth.hfp.HeadsetService, cp =  }
            //I/System.Console( 1617): { i = 0, cn = com.android.bluetooth.hid.HidService, cp =  }
            //I/System.Console( 1617): { i = 84, cn = com.android.bluetooth.pan.PanService, cp =  }
            //I/System.Console( 1617): { i = 80, cn = com.android.defcontainer.DefaultContainerService, cp =  }
            //I/System.Console( 1617): { i = 37, cn = com.android.incallui.InCallServiceImpl, cp =  }
            //I/System.Console( 1617): { i = 71, cn = com.android.incallui.MCIDService, cp =  }
            //I/System.Console( 1617): { i = 55, cn = com.android.incallui.SecInCallService, cp =  }
            //I/System.Console( 1617): { i = 73, cn = com.android.internal.backup.LocalTransportService, cp =  }
            //I/System.Console( 1617): { i = 81, cn = com.android.phone.TelephonyDebugService, cp =  }
            //I/System.Console( 1617): { i = 66, cn = com.android.providers.media.MtpService, cp =  }
            //I/System.Console( 1617): { i = 65, cn = com.android.server.DrmEventService, cp =  }
            //I/System.Console( 1617): { i = 57, cn = com.android.server.telecom.BluetoothPhoneService, cp =  }
            //I/System.Console( 1617): { i = 50, cn = com.android.server.telecom.BluetoothVoIPService, cp =  }
            //I/System.Console( 1617): { i = 62, cn = com.android.stk.StkAppService, cp =  }
            //I/System.Console( 1617): { i = 15, cn = com.android.systemui.ImageWallpaper, cp = PendingIntent{2759cef2: android.os.BinderProxy@181ef173} }
            //I/System.Console( 1617): { i = 44, cn = com.android.systemui.SystemUIService, cp =  }
            //I/System.Console( 1617): { i = 12, cn = com.android.systemui.keyguard.KeyguardService, cp =  }
            //I/System.Console( 1617): { i = 21, cn = com.dsi.ant.server.AntService, cp =  }
            //I/System.Console( 1617): { i = 48, cn = com.fmm.dm.XDMService, cp =  }
            //I/System.Console( 1617): { i = 22, cn = com.google.android.gms.analytics.service.AnalyticsService, cp =  }
            //I/System.Console( 1617): { i = 51, cn = com.google.android.gms.auth.trustagent.GoogleTrustAgent, cp =  }
            //I/System.Console( 1617): { i = 86, cn = com.google.android.gms.backup.BackupTransportService, cp =  }
            //I/System.Console( 1617): { i = 4, cn = com.google.android.gms.car.CarService, cp =  }
            //I/System.Console( 1617): { i = 76, cn = com.google.android.gms.clearcut.service.ClearcutLoggerService, cp =  }
            //I/System.Console( 1617): { i = 75, cn = com.google.android.gms.common.stats.GmsCoreStatsService, cp =  }
            //I/System.Console( 1617): { i = 67, cn = com.google.android.gms.deviceconnection.service.DeviceConnectionServiceBroker, cp =  }
            //I/System.Console( 1617): { i = 19, cn = com.google.android.gms.gcm.GcmService, cp =  }
            //I/System.Console( 1617): { i = 38, cn = com.google.android.gms.gcm.http.GoogleHttpService, cp =  }
            //I/System.Console( 1617): { i = 74, cn = com.google.android.gms.playlog.service.PlayLogBrokerService, cp =  }
            //I/System.Console( 1617): { i = 18, cn = com.google.android.gms.trustagent.api.trustagent.GoogleTrustAgentService, cp =  }
            //I/System.Console( 1617): { i = 25, cn = com.google.android.gms.usagereporting.service.UsageReportingService, cp =  }
            //I/System.Console( 1617): { i = 82, cn = com.google.android.gms.wearable.service.WearableService, cp =  }
            //I/System.Console( 1617): { i = 40, cn = com.google.android.hotword.service.HotwordService, cp =  }
            //I/System.Console( 1617): { i = 60, cn = com.google.android.libraries.hangouts.video.VideoChatService, cp =  }
            //I/System.Console( 1617): { i = 30, cn = com.google.android.location.fused.FusedLocationService, cp =  }
            //I/System.Console( 1617): { i = 16, cn = com.google.android.location.geocode.GeocodeService, cp =  }
            //I/System.Console( 1617): { i = 39, cn = com.google.android.location.geofencer.service.GeofenceProviderService, cp =  }
            //I/System.Console( 1617): { i = 63, cn = com.google.android.location.internal.GoogleLocationManagerService, cp =  }
            //I/System.Console( 1617): { i = 54, cn = com.google.android.location.internal.PendingIntentCallbackService, cp =  }
            //I/System.Console( 1617): { i = 58, cn = com.google.android.location.internal.server.GoogleLocationService, cp =  }
            //I/System.Console( 1617): { i = 61, cn = com.google.android.location.network.NetworkLocationService, cp =  }
            //I/System.Console( 1617): { i = 3, cn = com.google.android.music.dial.DialMediaRouteProviderService, cp =  }
            //I/System.Console( 1617): { i = 6, cn = com.google.android.search.core.service.BroadcastListenerService, cp =  }
            //I/System.Console( 1617): { i = 35, cn = com.google.android.search.core.service.SearchService, cp =  }
            //I/System.Console( 1617): { i = 69, cn = com.google.android.voiceinteraction.GsaVoiceInteractionService, cp =  }
            //I/System.Console( 1617): { i = 85, cn = com.ime.framework.spellcheckservice.SamsungIMESpellCheckerService, cp =  }
            //I/System.Console( 1617): { i = 49, cn = com.samsung.android.MtpApplication.MtpService, cp =  }
            //I/System.Console( 1617): { i = 78, cn = com.samsung.android.app.catchfavorites.catchnotifications.CatchNotificationsService, cp = PendingIntent{1f770943: android.os.BinderProxy@6924ca9} }
            //I/System.Console( 1617): { i = 56, cn = com.samsung.android.app.edge.nightclock.NightClockService, cp =  }
            //I/System.Console( 1617): { i = 59, cn = com.samsung.android.app.galaxyfinder.recommended.RecommendedService, cp =  }
            //I/System.Console( 1617): { i = 53, cn = com.samsung.android.app.galaxyfinder.tag.TagReadyService, cp =  }
            //I/System.Console( 1617): { i = 72, cn = com.samsung.android.app.shealth.tracker.pedometer.service.PedometerService, cp =  }
            //I/System.Console( 1617): { i = 32, cn = com.samsung.android.app.shealth.tracker.sport.livetracker.LiveTrackerService, cp =  }
            //I/System.Console( 1617): { i = 79, cn = com.samsung.android.beaconmanager.BeaconService, cp =  }
            //I/System.Console( 1617): { i = 42, cn = com.samsung.android.health.wearable.service.WearableService, cp =  }
            //I/System.Console( 1617): { i = 70, cn = com.samsung.android.providers.context.ContextService, cp =  }
            //I/System.Console( 1617): { i = 24, cn = com.samsung.android.scloud.auth.RelayService, cp =  }
            //I/System.Console( 1617): { i = 26, cn = com.samsung.android.sconnect.periph.PeriphService, cp =  }
            //I/System.Console( 1617): { i = 29, cn = com.samsung.android.sensor.framework.SensorService, cp =  }
            //I/System.Console( 1617): { i = 64, cn = com.samsung.android.service.health.HealthService, cp =  }
            //I/System.Console( 1617): { i = 14, cn = com.samsung.android.service.peoplestripe.PeopleNotiListenerService, cp = PendingIntent{17538bc0: android.os.BinderProxy@6924ca9} }
            //I/System.Console( 1617): { i = 77, cn = com.samsung.android.service.peoplestripe.PeopleStripeService, cp =  }
            //I/System.Console( 1617): { i = 28, cn = com.samsung.android.sm.widgetapp.SMWidgetService, cp =  }
            //I/System.Console( 1617): { i = 47, cn = com.samsung.android.thememanager.ThemeManagerService, cp =  }
            //I/System.Console( 1617): { i = 52, cn = com.samsung.appcessory.server.SAPService, cp =  }
            //I/System.Console( 1617): { i = 5, cn = com.samsung.hs20settings.WifiHs20UtilityService, cp =  }
            //I/System.Console( 1617): { i = 41, cn = com.samsung.sec.android.application.csc.CscUpdateService, cp =  }
            //I/System.Console( 1617): { i = 2, cn = com.sec.android.app.bluetoothtest.BluetoothBDTestService, cp =  }
            //I/System.Console( 1617): { i = 31, cn = com.sec.android.app.launcher.services.LauncherService, cp =  }
            //I/System.Console( 1617): { i = 10, cn = com.sec.android.daemonapp.ap.accuweather.WeatherClockService, cp =  }
            //I/System.Console( 1617): { i = 8, cn = com.sec.android.inputmethod.SamsungKeypad, cp = PendingIntent{39a730f9: android.os.BinderProxy@2b45775c} }
            //I/System.Console( 1617): { i = 46, cn = com.sec.android.pagebuddynotisvc.PageBuddyNotiSvc, cp =  }
            //I/System.Console( 1617): { i = 1, cn = com.sec.android.sensor.framework.SensorService, cp =  }
            //I/System.Console( 1617): { i = 83, cn = com.sec.android.service.sm.service.SecurityManagerService, cp =  }
            //I/System.Console( 1617): { i = 7, cn = com.sec.android.widgetapp.ap.weather.common.appservice.WeatherScreenService, cp =  }
            //I/System.Console( 1617): { i = 11, cn = com.sec.android.widgetapp.ap.weather.common.appservice.WeatherService, cp =  }
            //I/System.Console( 1617): { i = 36, cn = com.sec.android.widgetapp.ap.weather.widget.surfacewidget.WeatherSurfaceWidget, cp =  }
            //I/System.Console( 1617): { i = 33, cn = com.sec.android.widgetapp.digitalclockeasy.DigitalClockEasyService, cp =  }
            //I/System.Console( 1617): { i = 9, cn = com.sec.bcservice.BroadcastService, cp =  }
            //I/System.Console( 1617): { i = 20, cn = com.sec.enterprise.mdm.services.simpin.EnterpriseSimPin, cp =  }
            //I/System.Console( 1617): { i = 87, cn = com.sec.phone.SecPhoneService, cp =  }
            //I/System.Console( 1617): { i = 43, cn = com.sec.spp.push.PushClientService, cp =  }
            //I/System.Console( 1617): { i = 27, cn = org.simalliance.openmobileapi.service.SmartcardService, cp =  }

            //I/System.Console( 5883): { i = 85, cn = com.google.android.gms.backup.BackupTransportService, process = com.google.android.gms.persistent }
            //I/System.Console( 5883): { i = 86, cn = com.sec.phone.SecPhoneService, process = com.sec.phone }
            //I/System.Console( 5883): { i = 7, cn = com.sec.android.inputmethod.SamsungKeypad, process = com.sec.android.inputmethod, cp = PendingIntent{e6c79e2: android.os.BinderProxy@181ef173}, describeContents = 0 }
            //I/System.Console( 5883): { i = 13, cn = com.samsung.android.service.peoplestripe.PeopleNotiListenerService, process = com.samsung.android.service.peoplestripe, cp = PendingIntent{24b00830: android.os.BinderProxy@6924ca9}, describeContents = 0 }
            //I/System.Console( 5883): { i = 14, cn = com.android.systemui.ImageWallpaper, process = com.android.systemui.imagewallpaper, cp = PendingIntent{135c522e: android.os.BinderProxy@1ced31cf}, describeContents = 0 }
            //I/System.Console( 5883): { i = 77, cn = com.samsung.android.app.catchfavorites.catchnotifications.CatchNotificationsService, process = com.samsung.android.app.catchfavorites, cp = PendingIntent{2b45775c: android.os.BinderProxy@6924ca9}, describeContents = 0 }

            // http://stackoverflow.com/questions/7170730/how-to-set-a-control-panel-for-my-service-in-android
            // The service's description and configuration intent can be set during a service binding
            foreach (var ss in se)
            {


                var cn = ss.cn;

                PendingIntent cp = ss.cp;

                // whats a ControlPanel ?

                //  Caused by: java.lang.NullPointerException: Attempt to invoke virtual method 'int android.app.PendingIntent.describeContents()' on a null object reference

                if (cp == null)
                    Console.WriteLine(new { ss.i, cn, ss.rsi.process });
                else
                    Console.WriteLine(new { ss.i, cn, ss.rsi.process, cp, describeContents = cp.describeContents() });

                // I/System.Console(17713): { cn = AndroidBootServiceNotificationActivity.Activities.NotifyService }
                if (cn == typeof(NotifyService).FullName)
                {
                    // cannot find ourself? unless its running

                    startservice.setEnabled(false);
                    stopservice.setEnabled(true);

                    // its running

                    // http://stackoverflow.com/questions/7170730/how-to-set-a-control-panel-for-my-service-in-android
                    // http://www.techques.com/question/1-7170730/How-to-set-a-control-panel-for-my-Service-in-Android
                    // http://alvinalexander.com/java/jwarehouse/android/core/java/android/app/ActivityManagerNative.java.shtml
                }


                #region cp
                if (cp != null)
                {
                    // could we not infer activity from code from application?

                    new Button(this).WithText(
                        cn
                    ).AtClick(
                        delegate
                        {
                            // http://codetheory.in/android-pending-intents/
                            try
                            {
                                cp.send();
                            }
                            catch
                            {
                            }

                            //this.startActivity(
                            //    cp
                            //);
                        }
                    ).AttachTo(ll);
                }
                #endregion


            }

            this.setContentView(sv);

            //this.ShowToast("http://jsc-solutions.net");


            new Button(this).WithText("exit").AttachTo(ll).AtClick(
                delegate
                {

                    // will it be logged?
                    System.Environment.Exit(13);

                    // application still visible in tasks?
                }
            );

            new Button(this).WithText("finish").AttachTo(ll).AtClick(
               delegate
               {
                   //this.finishAndRemoveTask();
                   this.finish();

                   // will it be logged?
                   //System.Environment.Exit(13);

                   // application still visible in tasks?
               }
           );

            new Button(this).WithText("finishAndRemoveTask").AttachTo(ll).AtClick(
             delegate
             {
                 this.finishAndRemoveTask();
                 //this.finish();

                 // will it be logged?
                 //System.Environment.Exit(13);

                 // application still visible in tasks?
             }
         );
        }


    }

    [ScriptCoreLib.Android.Manifest.ApplicationMetaData(name = "android:process", value = ":foo5")]
    public sealed class NotifyService : Service
    {
        public const string ACTION = "NotifyServiceAction";

        public const int RQS_STOP_SERVICE = 1;

        NotifyServiceReceiver notifyServiceReceiver;

        public override void onCreate()
        {
            notifyServiceReceiver = new NotifyServiceReceiver { that = this };

            base.onCreate();

        }

        public override int onStartCommand(Intent value0, int value1, int value2)
        {
            // http://stackoverflow.com/questions/14182014/android-oncreate-or-onstartcommand-for-starting-service


            new Thread(
                delegate()
                {
                    var sw = Stopwatch.StartNew();

                    while (true)
                    {
                        var xmyPid = android.os.Process.myPid();


                        // cpu 
                        Console.WriteLine(new { xmyPid, Thread.CurrentThread.ManagedThreadId, sw });

                        Thread.Sleep(1000);
                    }
                }
            ) { Name = "namedthread1" }.Start();

            var myPid = android.os.Process.myPid();

            var intentFilter = new IntentFilter();
            intentFilter.addAction(ACTION);
            registerReceiver(notifyServiceReceiver, intentFilter);


            #region Send Notification
            var notificationManager = (NotificationManager)getSystemService(Context.NOTIFICATION_SERVICE);

            var myNotification = new Notification(
                android.R.drawable.star_on,
                //(CharSequence)(object)"Boot!!",
                "Boot!!" + new { myPid },

                when: 0

            //java.lang.System.currentTimeMillis()
            );

            var context = getApplicationContext();

            var myIntent = new Intent(Intent.ACTION_VIEW, android.net.Uri.parse("http://youtube.com"));

            var pendingIntent
              = PendingIntent.getActivity(getBaseContext(),
                0, myIntent,
                Intent.FLAG_ACTIVITY_NEW_TASK);
            myNotification.defaults |= Notification.DEFAULT_SOUND;
            myNotification.flags |= Notification.FLAG_AUTO_CANCEL;
            myNotification.setLatestEventInfo(context,
                "Boot!!" + new { myPid },
                    "Proud to be a jsc developer :)",
               pendingIntent);
            notificationManager.notify(1, myNotification);
            #endregion



            return base.onStartCommand(value0, value1, value2);
        }

        public override void onDestroy()
        {
            this.unregisterReceiver(notifyServiceReceiver);
            base.onDestroy();

            //            I/System.Console( 8080): onDestroy { xmyPid = 8080 }
            //I/art     ( 8080): System.exit called, status: 42

            var xmyPid = android.os.Process.myPid();
            Console.WriteLine("onDestroy " + new { xmyPid });

            System.Environment.Exit(42);

        }


        // http://apiwave.com/java/api/android.app.PendingIntent

        public override android.os.IBinder onBind(Intent value)
        {
            return null;
        }


        public class NotifyServiceReceiver : BroadcastReceiver
        {
            public NotifyService that;

            public override void onReceive(Context c, Intent i)
            {
                //android.content.IntentFilter
                //android.content.Intent.ACTION_BOOT_COMPLETED
                int rqs = i.getIntExtra("RQS", 0);

                if (rqs == RQS_STOP_SERVICE)
                    that.stopSelf();
            }
        }
    }


}

namespace foo
{


    // android.intent.action.BOOT_COMPLETED
    [ScriptCoreLib.Android.Manifest.ApplicationIntentFilterAttribute(Action = Intent.ACTION_BOOT_COMPLETED)]
    //[IntentFilter(Action = "android.intent.action.BOOT_COMPLETED")]
    public class AtBootCompleted : BroadcastReceiver
    {
        public override void onReceive(Context c, Intent i)
        {
            var that = c;

            //that.ShowToast("AtBootCompleted");

            var intent = new Intent(that, typeof(NotifyService).ToClass());
            that.startService(intent);
        }
    }

    //[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    //sealed class IntentFilterAttribute : Attribute
    //{
    //    // jsc does not support properties yet? are they even allowed in java?

    //    public string Action;
    //}

}

//0001 020001c0 ScriptCoreLibAndroid::ScriptCoreLibJava.BCLImplementation.System.Threading.__AutoResetEvent
//internal compiler error at method
// assembly: C:\util\jsc\bin\ScriptCoreLibAndroid.dll at
// type: ScriptCoreLibJava.BCLImplementation.System.Threading.__EventWaitHandle, ScriptCoreLibAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// method: Set
// Object reference not set to an instance of an object.
//    at jsc.Languages.Java.JavaCompiler.EmitTryBlock(Prestatement p) in X:\jsc.internal.git\compiler\jsc\Languages\Java\JavaCompiler.EmitTryBlock.cs:line 129


//    0001 020001c1 ScriptCoreLibAndroid::ScriptCoreLibJava.BCLImplementation.System.Threading.__AutoResetEvent
//internal compiler error at method
// assembly: C:\util\jsc\bin\ScriptCoreLibAndroid.dll at
// type: ScriptCoreLibJava.BCLImplementation.System.Threading.__EventWaitHandle, ScriptCoreLibAndroid, Version=
// method: Set
// Object reference not set to an instance of an object.