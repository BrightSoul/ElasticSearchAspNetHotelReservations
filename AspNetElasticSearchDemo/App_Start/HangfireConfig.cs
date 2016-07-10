using Hangfire;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo
{
    public static class HangfireConfig
    {
        public static void ConfigureJobs(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
    .UseSqlServerStorage(
    ConfigurationManager.ConnectionStrings["HotelReservationContext"].ConnectionString,
    new Hangfire.SqlServer.SqlServerStorageOptions { SchemaName = "jobs", PrepareSchemaIfNecessary = true });

            app.UseHangfireDashboard("/jobs");
            app.UseHangfireServer();
            RecurringJob.AddOrUpdate("ElasticSearchIndexingJob", () => ElasticSearchIndexingJob.Execute(), "*/1 * * * *");
            RecurringJob.Trigger("ElasticSearchIndexingJob");
        }
    }
}
