// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Run style cop.</change>
// </history>

namespace Apollo.ParlayRMS.Websites
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.Data.Configuration;
    using Fanex.Logging;

    /// <summary>
    /// MVC application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Sets all needed configuration when the application starts.
        /// </summary>
        protected void Application_Start()
        {
            DbSettingProviderManager.Current.Start();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Handles the Error event of the Application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();

            if (!Request.IsLocal)
            {
                string message = string.Format(
                                            "{0}\r\nSource: {1}\r\nDomain: {2}\r\nForm: {3}\r\nQueryString: {4}\r\nTargetSite: {5}\r\nStackTrace: {6}",
                                            ex.Message,
                                            ex.Source,
                                            HttpContext.Current.Request.Headers["Host"],
                                            Request.Form.ToString(),
                                            Request.QueryString.ToString(),
                                            ex.TargetSite,
                                            ex.StackTrace);

                LogService.Error(message);
            }
        }
    }
}