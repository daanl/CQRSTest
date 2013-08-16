using System;
using System.Collections;
using System.Web;
using Infrastructure.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Raven.Client;
using Web;

[assembly: WebActivator.PreApplicationStartMethod(typeof(UI.Web.App_Start.NinjectSetup), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(UI.Web.App_Start.NinjectSetup), "Stop")]

namespace UI.Web.App_Start
{
    public static class NinjectSetup
    {
        public static IKernel Kernel { get; private set; }
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);

            var hubKernel = new StandardKernel();
            hubKernel.Bind<IAsyncDocumentSession>().ToMethod(x => MvcApplication.DocumentStore.OpenAsyncSession());

            GlobalHost.DependencyResolver = new NinjectSignalRDependencyResolver(hubKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            Kernel = kernel;

            return kernel;
        }

     
        private static void RegisterServices(IKernel kernel)
        {
        }
      
        private static IAsyncDocumentSession GetRequestSession()
        {
            IDictionary httpContextItems = HttpContext.Current.Items;

            IAsyncDocumentSession session;
            if (HttpContext.Current == null)
            {
                session = MvcApplication.DocumentStore.OpenAsyncSession();
            }
            else if (!httpContextItems.Contains(MvcApplication.Sessionkey))
            {
                session = MvcApplication.DocumentStore.OpenAsyncSession();
                httpContextItems.Add(MvcApplication.Sessionkey, session);
            }
            else
            {
                session = (IAsyncDocumentSession)httpContextItems[MvcApplication.Sessionkey];
            }
            return session;
        }
    }
}
