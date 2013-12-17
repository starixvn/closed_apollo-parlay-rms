// --------------------------------------------------------------------------
// <copyright file="ManagementAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Management
{
    using System.Web.Mvc;

    /// <summary>
    /// Registers Member Management area.
    /// </summary>
    public class ManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Management";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Management_default",
                "Management/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Apollo.ParlayRMS.Management.Controllers" });
        }
    }
}