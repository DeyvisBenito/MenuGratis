using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MenuGratis
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SetConnectionString();
        }

        private void SetConnectionString()
        {
            var connectionString = Environment.GetEnvironmentVariable("CLEARDB_DATABASE_URL");

            if (!string.IsNullOrEmpty(connectionString))
            {
                var uri = new Uri(connectionString);
                var userInfo = uri.UserInfo.Split(':');

                var builder = new MySqlConnectionStringBuilder
                {
                    Server = uri.Host,
                    Port = (uint)uri.Port,
                    Database = uri.AbsolutePath.Trim('/'),
                    UserID = userInfo[0],
                    Password = userInfo[1],
                    SslMode = MySqlSslMode.Required,
                };

                var settings = ConfigurationManager.ConnectionStrings["conex"];
                var fi = typeof(ConfigurationElement).GetField("_bReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                fi.SetValue(settings, false);

                settings.ConnectionString = builder.ConnectionString;
                fi.SetValue(settings, true);
            }
        }
    }
}

