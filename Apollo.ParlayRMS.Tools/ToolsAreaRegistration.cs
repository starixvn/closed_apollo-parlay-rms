// --------------------------------------------------------------------------
// <copyright file="ToolsAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.21">Created</change>
// </history>

namespace Apollo.ParlayRMS.Tools
{
    using System.Web.Mvc;

    /// <summary>
    /// Register Tools area to the web.
    /// </summary>
    public class ToolsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tools";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Tools_default",
                "Tools/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Apollo.ParlayRMS.Tools.Controllers" });
        }
    }
}