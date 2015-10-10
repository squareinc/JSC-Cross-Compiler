using java.io;
using java.security.cert;
using java.util;
using ScriptCoreLib;

namespace java.security
{
    // http://docs.oracle.com/javase/1.5.0/docs/api/java/security/KeyStore.html
    // http://developer.android.com/reference/java/security/KeyStore.html
    [Script(IsNative = true)]
    public class KeyStore
    {
        // Z:\jsc.svn\examples\java\hybrid\Test\TestKeyStoreWindowsROOT\TestKeyStoreWindowsROOT\Program.cs

        // TPM, NFC ?


        // https://sites.google.com/a/jsc-solutions.net/backlog/knowledge-base/2014/201408/20140829
        // tested by?

        public Certificate getCertificate(string alias) { throw null; }
        public Enumeration aliases()
        {
            throw null;
        }

        public void load(InputStream stream, char[] password)
        { }

        public static KeyStore getInstance(string type) { throw null; }
    }
}
