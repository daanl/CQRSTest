using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Document;

namespace Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static DocumentStore DocumentStore;

        public static string Sessionkey = "RavenDb.Session";

        protected void Application_Start()
        {
            DocumentStore = new DocumentStore
            {
                Url = ConfigurationManager.AppSettings["RavenDbUr"],
                DefaultDatabase = ConfigurationManager.AppSettings["RavenDbDatabase"],
                ApiKey = ConfigurationManager.AppSettings["RavenDbApiKey"]
            };

            DocumentStore.Initialize();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_EndRequest(object sender, System.EventArgs e)
        {
            // Dispose NHibernate session if exists
            if (!Context.Items.Contains(Sessionkey)) return;

            var session = (IAsyncDocumentSession)Context.Items[Sessionkey];
            session.Dispose();
            Context.Items[Sessionkey] = null;

        }
    }
}