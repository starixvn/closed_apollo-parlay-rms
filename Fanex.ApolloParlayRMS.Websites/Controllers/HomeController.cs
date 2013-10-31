// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.9">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Websites.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Core.Web.Controllers;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Home controller.
    /// </summary>
    [NapSuperAuth]
    public class HomeController : BasedController
    {
        public HomeController()
        {
        }

        /// <summary>
        /// Gets Index page.
        /// </summary>
        /// <returns>Return Index View.</returns>
        public ActionResult Index()
        {
            Response.Cookies.Add(new HttpCookie("menu-id") { Expires = DateTime.Now.AddDays(-1d) });

            return View();
        }

        /// <summary>
        /// Get Menu Partial view.
        /// </summary>
        /// <returns>Partial menu view.</returns>
        [ChildActionOnly]
        public PartialViewResult _Menu()
        {
            ViewBag.MenuItemList = CurrentSession.Menu;
            return PartialView("~/Views/Shared/_Menu.cshtml");
        }

        /// <summary>
        /// Gets the validation error messages.
        /// </summary>
        /// <returns>JSON object contains all validation error messages.</returns>
        public JsonResult GetValidationErrorMessages()
        {
            dynamic errorMessages = new
            {
                RequiredField = Messages.RequiredFieldError,
                InvalidNumberField = Messages.InvalidNumberFieldError,
                PositiveField = Messages.PositiveFieldError,
                InvalidDateField = Messages.InvalidDateFieldError,
                DateRangeField = Messages.DateRangeFieldError
            };
            return Json(errorMessages);
        }
    }
}