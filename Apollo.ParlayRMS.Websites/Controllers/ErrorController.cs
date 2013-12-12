// --------------------------------------------------------------------------
// <copyright file="ErrorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.11.04">Created</change>
// </history>

namespace Apollo.ParlayRMS.Websites.Controllers
{
    using System.Web.Mvc;
    using Apollo.ParlayRMS.Core.Controllers;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Error controller.
    /// </summary>
    [NapSuperAuth]
    public class ErrorController : BaseController
    {
        /// <summary>
        /// Redirect to default error page.
        /// </summary>
        /// <returns>Access Denied view.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Redirect to access denied page.
        /// </summary>
        /// <returns>Access Denied view.</returns>
        public ActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Redirect to not found page.
        /// </summary>
        /// <returns>Access Denied view.</returns>
        public ActionResult NotFound()
        {
            return View();
        }
    }
}