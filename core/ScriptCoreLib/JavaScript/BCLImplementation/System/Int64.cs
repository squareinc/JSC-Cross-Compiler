using System;
using System.Collections.Generic;
using System.Text;
using ScriptCoreLib.JavaScript.Runtime;

namespace ScriptCoreLib.JavaScript.BCLImplementation.System
{
    // http://referencesource.microsoft.com/#mscorlib/system/int64.cs
    // https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Int64.cs
    // https://github.com/Reactive-Extensions/IL2JS/blob/master/mscorlib/System/Int64.cs

    [Script(Implements = typeof(global::System.Int64))]
    internal class __Int64
    {
		// http://www.viva64.com/en/d/0306/

		// time to start marking it as a struct for
		// ServiceWorker contexts?

		static public long Parse(string e)
        {
            var value = parseInt(e);

            var x = "" + value;

            if (x == e)
                return value;

            // tested by X:\jsc.svn\examples\javascript\Test\TestLongParse\TestLongParse\Application.cs
            // Z:\jsc.svn\examples\javascript\data\GoogleMapsTracker\Application.cs


            // Uncaught Error: InvalidOperationException: parseInt failed for 84727352424136816

            throw new InvalidOperationException("Int64 parseInt failed for " + e);
        }

        [Script(OptimizedCode = "return parseInt(e);")]
        static public long parseInt(string e)
        {
            return default(long);
        }

        [Script(DefineAsStatic = true)]
        public int CompareTo(long e)
        {
            return Expando.Compare(this, e);

        }

        [Script(DefineAsStatic = true)]
        public string ToString(string format)
        {
            var value = (long)(object)this;


            return InternalToString(format, value);
        }

        internal static string InternalToString(string format, long value)
        {
            //E:\jsc.svn\examples\javascript\Test\TestToStringCurrencyFormat\TestToStringCurrencyFormat\Application.cs
            if (format == "0.00")
            {
                if (value < 0)
                {
                    value = Math.Abs(value);
                    var cent = value % 100;
                    var t = value - cent;
                    var to = t / 100;
                    var total = "-" + to + "." + cent.ToString().PadLeft(2, '0');
                    return total;
                }
                else
                {
                    var cent = value % 100;
                    var t = value - cent;
                    var to = t / 100;
                    var total = to + "." + cent.ToString().PadLeft(2, '0');
                    return total;
                }
            }

            if (format != "x8")
                throw new NotImplementedException("format");


            var s = new StringBuilder();

            s.Append(ToHexString((byte)(value >> 0x18)));
            s.Append(ToHexString((byte)(value >> 0x10)));
            s.Append(ToHexString((byte)(value >> 8)));
            s.Append(ToHexString((byte)(value)));

            return s.ToString();
        }

        public static string ToHexString(byte e)
        {
            const string u = "0123456789abcdef";

            return u.Substring((e >> 4) & 0xF, 1) + u.Substring((e >> 0) & 0xF, 1);
        }
    }
}
