﻿using ScriptCoreLib;
using ScriptCoreLib.Shared.BCLImplementation.System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ScriptCoreLibJava.BCLImplementation.System.Data.SQLite
{
    [Script(Implements = typeof(global::System.Data.SQLite.SQLiteConnectionStringBuilder))]
    public class __SQLiteConnectionStringBuilder : __DbConnectionStringBuilder
    {
        // https://sites.google.com/a/jsc-solutions.net/backlog/knowledge-base/2013/201311/20131128-nuget
        // X:\jsc.smokescreen.svn\core\javascript\com.abstractatech.analytics\com.abstractatech.analytics\ApplicationWebService.cs

        // https://sites.google.com/a/jsc-solutions.net/backlog/knowledge-base/2012/20121001-solutionbuilderv1/20121014-gae-data

        public string DataSource { get; set; }
        public int Version { get; set; }
        public bool ReadOnly { get; set; }
        public string Uri { get; set; }
        public string Password { get; set; }

        public string InternalUser { get; set; }
        public string InternalHost { get; set; }
        public string InternalInstanceName { get; set; }

        public static string InternalDefaultInternalInstanceName = "instance_name";

        public __SQLiteConnectionStringBuilder()
        {
            this.InternalUser = "root";
            this.InternalHost = "localhost";
            this.InternalInstanceName = InternalDefaultInternalInstanceName;
            this.Password = "";
        }

        public override void InternalAdd(string keyword, object value)
        {
            if (keyword == "InternalUser")
                this.InternalUser = (string)value;
            else if (keyword == "InternalHost")
                this.InternalHost = (string)value;
            else if (keyword == "InternalInstanceName")
                this.InternalInstanceName = (string)value;
        }



        public override string InternalGetConnectionString()
        {
            var key = new { DataSource, ReadOnly, InternalUser, InternalHost, InternalInstanceName }.ToString();

            InterlockedInternalGetConnectionString(key, this);

            return key;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public static __SQLiteConnectionStringBuilder InterlockedInternalGetConnectionString(
            string key,
            __SQLiteConnectionStringBuilder value = null
            )
        {
            if (value != null)
            {
                lookup[key] = value;
            }

            return lookup[key];
        }

        public static __SQLiteConnectionStringBuilder InternalGetConnectionString(string key)
        {
            return InterlockedInternalGetConnectionString(key);
        }

        public static Dictionary<string, __SQLiteConnectionStringBuilder> lookup = new Dictionary<string, __SQLiteConnectionStringBuilder>();
    }
}
