using Hangfire;
using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(AspNetElasticSearchDemo.Startup))]

namespace AspNetElasticSearchDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ElasticSearchConfig.Configure();
            DatabaseConfig.Configure();
            HangfireConfig.ConfigureJobs(app);
            
        }
    }
}