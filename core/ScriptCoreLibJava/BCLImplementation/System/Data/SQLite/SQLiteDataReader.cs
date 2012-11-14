﻿using ScriptCoreLib;
using ScriptCoreLib.Shared.BCLImplementation.System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptCoreLibJava.BCLImplementation.System.Data.SQLite
{
    [Script(Implements = typeof(global::System.Data.SQLite.SQLiteDataReader))]
    internal class __SQLiteDataReader : __DbDataReader
    {
        public java.sql.ResultSet InternalResultSet;
        // https://sites.google.com/a/jsc-solutions.net/backlog/knowledge-base/2012/20121001-solutionbuilderv1/20121014-gae-data
        // http://msdn.microsoft.com/en-us/library/ms379039.aspx


        public override void Close()
        {

            try
            {
                this.InternalResultSet.close();
            }
            catch
            {
                throw;
            }

        }

        public override bool Read()
        {
            var value = default(bool);

            try
            {
                value = this.InternalResultSet.next();

            }
            catch
            {
                throw;
            }

            return value;
        }

        public override object this[string name]
        {
            get
            {

                var i = this.GetOrdinal(name);

                return this.GetString(i);
            }
        }

        public override int GetOrdinal(string name)
        {
            var value = default(int);
            try
            {
                value = this.InternalResultSet.findColumn(name) - 1;

            }
            catch
            {
                throw;
            }
            return value;
        }

        public override string GetString(int i)
        {
            var value = default(string);
            // the first column is 1
            try
            {
                value = this.InternalResultSet.getString(i + 1);


            }
            catch
            {
                throw;
            }
            return value;
        }

        public override int GetInt32(int i)
        {
            var value = default(int);
            try
            {
                value = this.InternalResultSet.getInt(i + 1);
            }
            catch
            {
                throw;
            }
            return value;
        }

        public override string GetName(int ordinal)
        {
            var r = default(string);

            try
            {
                r = this.InternalResultSet.getMetaData().getColumnName(ordinal + 1);
            }
            catch
            {
                throw;
            }

            return r;
        }

        public override long GetInt64(int ordinal)
        {
            var value = default(long);
            try
            {
                value = this.InternalResultSet.getLong(ordinal + 1);
            }
            catch
            {
                throw;
            }
            return value;
        }

        public override Type GetFieldType(int ordinal)
        {
            var f = default(int);

            try
            {
                f = this.InternalResultSet.getMetaData().getColumnType(ordinal + 1);
            }
            catch
            {
                throw;
            }

            if (f == 4)
                return typeof(int);

            // In MySQL 4.1.x, the four TEXT types (TINYTEXT, TEXT, MEDIUMTEXT, and LONGTEXT) return 'blob" as field types, not "string".
            // how to fix that?
            if (f == 2004)
                return typeof(string);

            if (f == 91)
                return typeof(string);

            // http://docs.oracle.com/javase/1.4.2/docs/api/constant-values.html#java.sql.Types.INTEGER
            throw new InvalidOperationException("GetFieldType unknown type: " + f);
        }

        public override int FieldCount
        {
            get
            {
                var r = default(int);
                try
                {

                    r = this.InternalResultSet.getMetaData().getColumnCount();

                }
                catch
                {
                    throw;
                }
                return r;
            }
        }
    }
}
