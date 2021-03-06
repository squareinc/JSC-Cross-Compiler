﻿using System;
using System.Collections.Generic;
using System.Text;
using ScriptCoreLib;
using java.io;
using javax.net.ssl;

namespace java.net
{
    // http://java.sun.com/j2se/1.4.2/docs/api/java/net/HttpURLConnection.html
    // http://developer.android.com/reference/java/net/HttpURLConnection.html
    [Script(IsNative = true)]
    public abstract class HttpURLConnection : URLConnection
    {
        //public void setHostnameVerifier(HostnameVerifier v) { }


        public int getResponseCode()
        {
            return default(int);
        }

        public void setChunkedStreamingMode(int chunkLength)
        {
        }

        /// <summary>
        /// Sets whether HTTP redirects (requests with response code 3xx) should be automatically followed by this HttpURLConnection instance.
        /// </summary>
        /// <param name="followRedirects"></param>
        public void setInstanceFollowRedirects(bool followRedirects)
        {
        }

        /// <summary>
        /// Set the method for the URL request, one of: GET POST HEAD OPTIONS PUT DELETE TRACE are legal, subject to protocol restrictions.
        /// </summary>
        /// <param name="method"></param>
        public void setRequestMethod(string method)
        {
        }


        public abstract void disconnect();



    }
}
