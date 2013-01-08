using android.app;
using ScriptCoreLib;
using ScriptCoreLib.Delegates;
using ScriptCoreLib.Extensions;
using ScriptCoreLib.Android.Extensions;
using ScriptCoreLib.Ultra.WebService;
using System;
using System.Linq;
using System.Xml.Linq;
using android.widget;
using ScriptCoreLib.Android;
using WebGLSpiral.Shaders;

namespace com.abstractatech.spiral
{
    /// <summary>
    /// Methods defined in this type can be used from JavaScript. The method calls will seamlessly be proxied to the server.
    /// </summary>
    public sealed class ApplicationWebService
    {
        /// <summary>
        /// This Method is a javascript callable method.
        /// </summary>
        /// <param name="e">A parameter from javascript.</param>
        /// <param name="y">A callback to javascript.</param>
        public void WebMethod2(string e, Action<string> y)
        {
            // Send it back to the caller.
            y(e);
        }


#if Android
        static __InitializeAndroidActivity __crazy_workaround;

        public void Hander(WebServiceHandler h)
        {
            if (__crazy_workaround == null)
            {
                Console.WriteLine("__crazy_workaround");
                __crazy_workaround = new __InitializeAndroidActivity();
            }
        }
#endif
    }


    class __InitializeAndroidActivity
    {
        static __InitializeAndroidActivity()
        {
            Console.WriteLine("StaticInvoke");

            //  Exception Ljava/lang/RuntimeException; thrown while initializing LTryHideActionbarExperiment/StaticInvoke;
            try
            {


                // https://groups.google.com/forum/?fromgroups=#!topic/android-developers/suLMCWiG0D8

                (ScriptCoreLib.Android.ThreadLocalContextReference.CurrentContext as Activity).runOnUiThread(
                    a =>
                    {
                        // http://stackoverflow.com/questions/4451641/change-android-layout-programatically
                        var v = new RenderingContextView(a);
                        var s = new SpiralSurface(v);

                        v.onsurface +=
                            gl =>
                            {

                                #region onaccelerometer
                                v.onaccelerometer +=
                                    (x, y, z) =>
                                    {
                                        s.speed = 10 + 200 * x / 10f + 200 * y / 10f;

                                        var ay = y;
                                        if (y < 0)
                                            ay = -y;

                                        s.ucolor_1 = (10f - ay) / 10f;

                                        if (s.ucolor_1 < 0)
                                            s.ucolor_1 = 0;

                                        if (s.ucolor_1 > 10)
                                            s.ucolor_1 = 10;

                                        var ax = x;
                                        if (x < 0)
                                            ax = -x;

                                        s.ucolor_2 = (10f - ax) / 10f;

                                        if (s.ucolor_2 < 0)
                                            s.ucolor_2 = 0;

                                        if (s.ucolor_2 > 10)
                                            s.ucolor_2 = 10;
                                    };
                                #endregion



                                //Log.wtf("com.abstractatech.spiral", "onsurface done");

                            };


                        a.setContentView(v);
                    }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + new { ex.Message, ex.StackTrace });
            }
        }
    }

}
