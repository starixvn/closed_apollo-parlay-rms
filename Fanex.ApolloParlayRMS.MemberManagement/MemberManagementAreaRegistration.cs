// --------------------------------------------------------------------------
// <copyright file="MemberManagementAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.MemberManagement
{
    using System.Web.Mvc;

    /// <summary>
    /// Registers Member Management area.
    /// </summary>
    public class MemberManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MemberManagement";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MemberManagement_default",
                "MemberManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.ApolloParlayRMS.MemberManagement.Controllers" });
        }
    }
}