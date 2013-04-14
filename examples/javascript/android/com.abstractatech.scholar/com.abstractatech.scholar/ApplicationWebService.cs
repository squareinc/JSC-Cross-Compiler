using ScriptCoreLib;
using ScriptCoreLib.Delegates;
using ScriptCoreLib.Extensions;
using ScriptCoreLib.Ultra.WebService;
using System;
using System.Linq;
using System.Xml.Linq;

namespace com.abstractatech.scholar
{
    /// <summary>
    /// Methods defined in this type can be used from JavaScript. The method calls will seamlessly be proxied to the server.
    /// </summary>
    public sealed class ApplicationWebService :
        // can we do explicit implementations too?
        Abstractatech.JavaScript.FileStorage.IApplicationWebService
    {
        // jsc does not yet look deep enough
        Type ref0 = typeof(System.Data.SQLite.SQLiteCommand);
        Type ref1 = typeof(ScriptCoreLib.Shared.Data.DynamicDataReader);

        // { Message = Could not load file or assembly 'ScriptCoreLib.Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies. The system cannot find the file specified., StackTrace =    at Abstractatech.JavaScript.FileStorage.Schema.XX.WithEach(SQLiteDataReader reader, Action`1 y)







        #region service
        public Abstractatech.JavaScript.FileStorage.ApplicationWebService service = new Abstractatech.JavaScript.FileStorage.ApplicationWebService();



        public void DeleteAsync(string Key, Action done = null)
        {
            service.DeleteAsync(Key, done);

        }

        public void EnumerateFilesAsync(Abstractatech.JavaScript.FileStorage.AtFile y, Action<string> done = null)
        {
            service.EnumerateFilesAsync(y, done);
        }

        public void GetTransactionKeyAsync(Action<string> done = null)
        {
            service.GetTransactionKeyAsync(done);
        }

        public void UpdateAsync(string Key, string Value, Action done = null)
        {
            service.UpdateAsync(Key, Value, done);
        }
        #endregion


        public void InternalHandler(WebServiceHandler h)
        {
            // HTTP routing? how to do this more elegantly?
            service.InternalHandler(h);
        }
    }
}
