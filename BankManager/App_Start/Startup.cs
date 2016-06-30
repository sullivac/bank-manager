using System.IO;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using System.Data.SQLite;

[assembly: OwinStartup(typeof(BankManager.Startup))]

namespace BankManager
{
    internal class Startup
    {
        public static string ApplicationConnectionString
        {
            get { return string.Format(WebConfigurationManager.ConnectionStrings["Application"].ConnectionString, HostingEnvironment.MapPath("~")); }
        }

        public void Configuration(IAppBuilder app)
        {
            if (!File.Exists(HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["sqliteFileName"])))
            {
                SQLiteConnection.CreateFile(HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["sqliteFileName"]));

                DatabaseInitializer.Initialize();
            }

            var httpConfiguration = new HttpConfiguration();

            httpConfiguration.MapHttpAttributeRoutes();

            httpConfiguration.Routes.MapHttpRoute(
                name: "default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            app.UseWebApi(httpConfiguration);
        }
    }
}