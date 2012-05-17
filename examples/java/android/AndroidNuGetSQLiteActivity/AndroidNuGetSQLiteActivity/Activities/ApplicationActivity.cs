
using android.app;
using android.content;
using android.database;
using android.database.sqlite;
using android.provider;
using android.webkit;
using android.widget;
using AndroidNuGetSQLiteActivity.Library;
using ScriptCoreLib;
using ScriptCoreLib.Android;

namespace AndroidNuGetSQLiteActivity.Activities
{
    public class AndroidNuGetSQLiteActivity : Activity
    {
        // inspired by http://android-er.blogspot.com/2011/06/simple-example-using-androids-sqlite.html

        // C:\util\android-sdk-windows\tools\android.bat create project --package AndroidNuGetSQLiteActivity.Activities --activity AndroidNuGetSQLiteActivity  --target 2  --path y:\jsc.svn\examples\java\android\AndroidNuGetSQLiteActivity\AndroidNuGetSQLiteActivity\staging\


        // running it in emulator:
        // start C:\util\android-sdk-windows\tools\android.bat avd
        // "C:\util\android-sdk-windows\platform-tools\adb.exe" install -r  "y:\jsc.svn\examples\java\android\AndroidNuGetSQLiteActivity\AndroidNuGetSQLiteActivity\staging\bin\AndroidNuGetSQLiteActivity-debug.apk"

        // note: rebuild could auto reinstall

        // running it on device:
        // attach device to usb
        //Z:\jsc.svn\examples\java\android\HelloAndroid>C:\util\android-sdk-windows\platform-tools\adb.exe devices
        //List of devices attached
        //3330A17632C000EC        device 

        protected override void onCreate(global::android.os.Bundle savedInstanceState)
        {

            base.onCreate(savedInstanceState);

            setContentView(R.layout.main);

            TextView listContent = (TextView)findViewById(R.id.contentlist);



            #region   Create/Open a SQLite database and fill with dummy content and close it
            var a = new Adapter(this);
            a.openToWrite();
            a.deleteAll();
            a.insert("ABCDE");
            a.insert("FGHIJK");
            a.insert("1234567");
            a.insert("890");
            a.insert("Testing");
            a.close();
            #endregion

            #region  Open the same SQLite database             *  and read all it's content.
            a = new Adapter(this);
            a.openToRead();
            var contentRead = a.queueAll();
            a.close();
            #endregion


            listContent.setText(contentRead);


            this.ShowToast("http://jsc-solutions.net");
        }

        public class Adapter
        {

            string MYDATABASE_NAME;

            public const string MYDATABASE_TABLE = "MY_TABLE";
            public const int MYDATABASE_VERSION = 1;
            public const string KEY_CONTENT = "Content";

            //create table MY_DATABASE (ID integer primary key, Content text not null);
            private const string SCRIPT_CREATE_DATABASE =
             "create table " + MYDATABASE_TABLE + " ("
             + KEY_CONTENT + " text not null);";

            private AtCreate sqLiteHelper;
            private SQLiteDatabase sqLiteDatabase;

            private Context context;

            public Adapter(Context c, string MYDATABASE_NAME = "MY_DATABASE")
            {
                this.MYDATABASE_NAME = MYDATABASE_NAME;
                this.context = c;
            }

            public Adapter openToRead() /* throws android.database.SQLException */ {
                sqLiteHelper = new AtCreate(context, MYDATABASE_NAME, null, MYDATABASE_VERSION);
                sqLiteDatabase = sqLiteHelper.getReadableDatabase();
                return this;
            }

            public Adapter openToWrite() /* throws android.database.SQLException */ {
                sqLiteHelper = new AtCreate(context, MYDATABASE_NAME, null, MYDATABASE_VERSION);
                sqLiteDatabase = sqLiteHelper.getWritableDatabase();
                return this;
            }

            public void close()
            {
                sqLiteHelper.close();
            }

            public long insert(string content)
            {

                ContentValues contentValues = new ContentValues();
                contentValues.put(KEY_CONTENT, content);
                return sqLiteDatabase.insert(MYDATABASE_TABLE, null, contentValues);
            }

            public int deleteAll()
            {
                return sqLiteDatabase.delete(MYDATABASE_TABLE, null, null);
            }

            public string queueAll()
            {
                var columns = new[] { KEY_CONTENT };
                Cursor cursor = sqLiteDatabase.query(MYDATABASE_TABLE, columns,
                  null, null, null, null, null);

                var result = new java.lang.StringBuilder();

                int index_CONTENT = cursor.getColumnIndex(KEY_CONTENT);
                for (cursor.moveToFirst(); !(cursor.isAfterLast()); cursor.moveToNext())
                {
                    result.append(cursor.getString(index_CONTENT)).append("\n");
                }

                return result.ToAndroidString();
            }

            public class AtCreate : SQLiteOpenHelper
            {

                public AtCreate(Context context, string name, android.database.sqlite.SQLiteDatabase.CursorFactory factory, int version)
                    : base(context, name, factory, version)
                {

                }

                public override void onCreate(SQLiteDatabase db)
                {
                    // TODO Auto-generated method stub
                    db.execSQL(SCRIPT_CREATE_DATABASE);
                }

                public override void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
                {
                    // TODO Auto-generated method stub

                }

            }

        }
    }
}
