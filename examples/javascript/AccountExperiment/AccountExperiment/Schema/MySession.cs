﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace AccountExperiment.Schema
{
    class MySession : MySessionQueries
    {
        public readonly Action<Action<SQLiteConnection>> WithConnection;

        public SQLiteConnectionStringBuilder csb = new SQLiteConnectionStringBuilder
        {
            Version = 3,
            DataSource = "AccountExperiment.sqlite"
        };

        public MySession()
	    {
            this.WithConnection = csb.AsWithConnection();

             WithConnection(
                c =>
                {
                    new Create { }.ExecuteNonQuery(c);
                }
            );
	    }

        
        public long Insert(Insert value)
        {
            var id = -1L;

            WithConnection(
                c =>
                {
                    value.ExecuteNonQuery(c);

                    id = c.LastInsertRowId;
                }
            );

            return id;
        }

     
    }

}
