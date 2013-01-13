using ScriptCoreLib;
using ScriptCoreLib.Delegates;
using ScriptCoreLib.Extensions;
using ScriptCoreLib.Ultra.WebService;
using System;
using System.Linq;
using System.Xml.Linq;

namespace AccountExperiment
{
    /// <summary>
    /// Methods defined in this type can be used from JavaScript. The method calls will seamlessly be proxied to the server.
    /// </summary>
    public sealed class ApplicationWebService
    {
        public void Authenticate(
            string user,
            string password,

            Action<string> yield_session)
        {


            // create a new session audit object for us to use
            yield_session("foo");
        }

        // future versions will let client use this directly.
        public global::GravatarExperiment.ApplicationWebService gravatar = new GravatarExperiment.ApplicationWebService();


        public void gravatar_Gravatar(string e, Action<string> avatar, Action<string> profile)
        {
            gravatar.Gravatar(e, avatar, profile);
        }


        public void CreateAccount(

            string name,
            string web,
            string email,
            string password,
            string skype,

            Action<string> yield_session
            )
        {
            yield_session("foo");
        }


        public void Handler(WebServiceHandler h)
        {
            #region /view-source
            h.Applications.Where(k => h.Context.Request.Path == "/" + k.TypeName.ToLower() + "/view-source").WithEach(
                app =>
                {
                    h.Context.Response.ContentType = "text/javascript";

                    foreach (var item in app.References)
                    {
                        // asp.net needs absolute paths
                        h.Context.Response.WriteFile("/" + item.AssemblyFile + ".js");
                    }

                    h.CompleteRequest();
                    return;
                }
            );
            #endregion

            #region /register
            if (h.Context.Request.Path == "/register")
            {


                h.Applications.Single(k => k.TypeName == "Register").With(
                    app =>
                    {
                        var html = XElement.Parse(app.PageSource);

                        html.Add(
                            new XElement("script",
                                new XAttribute("src", "/register/view-source"),
                                " "
                            )
                        );

                        h.Context.Response.Write(html.ToString());
                        h.CompleteRequest();
                    }
                );
            }
            #endregion

            #region /login
            if (h.Context.Request.Path == "/login")
            {
                if (h.Context.Request.HttpMethod == "POST")
                {
                    // browser sending us the credentials.

                    var email = h.Context.Request.Form["email"];
                    var password = h.Context.Request.Form["password"];


                    Console.WriteLine(new { email, password, h.Context.Request.UrlReferrer });

                    h.Context.Response.SetCookie(new System.Web.HttpCookie("session", "foo"));

                    h.Context.Response.Redirect(h.Context.Request.UrlReferrer.ToString().TakeUntilLastOrNull("/login"));

                    h.CompleteRequest();
                    return;
                }

                h.Applications.Single(k => k.TypeName == "Login").With(
                    app =>
                    {
                        var html = XElement.Parse(app.PageSource);

                        html.Add(
                            new XElement("script",
                                new XAttribute("src", "/login/view-source"),
                                " "
                            )
                        );

                        h.Context.Response.Write(html.ToString());
                        h.CompleteRequest();
                    }
                );
            }
            #endregion



            #region /dashboard
            if (h.Context.Request.Cookies["session"] != null)
                if (h.IsDefaultPath)
                {
                    h.Applications.Single(k => k.TypeName == "Dashboard").With(
                        app =>
                        {
                            var html = XElement.Parse(app.PageSource);

                            html.Add(
                                new XElement("script",
                                    new XAttribute("src", "/dashboard/view-source"),
                                    " "
                                )
                            );

                            h.Context.Response.Write(html.ToString());
                            h.CompleteRequest();
                        }
                    );
                }
            #endregion
        }
    }
}
