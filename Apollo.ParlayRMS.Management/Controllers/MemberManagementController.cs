// --------------------------------------------------------------------------
// <copyright file="MemberManagementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Management.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Apollo.ParlayRMS.Core.Controllers;
    using Apollo.ParlayRMS.Core.Models;
    using Apollo.ParlayRMS.Management.Criteria;
    using Apollo.ParlayRMS.Management.Services.Implements;
    using Apollo.ParlayRMS.Management.Services.Interfaces;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Member Management Controller.
    /// </summary>
    public class MemberManagementController : BaseController
    {
        private readonly IMemberManagementService _memberManagementService;

        public MemberManagementController() :
            this(null)
        {
        }

        public MemberManagementController(IMemberManagementService memberManagementService)
        {
            this._memberManagementService = memberManagementService ?? new MemberManagementService { ServiceContext = this.ServiceContext };
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MemberManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.Levels = FilterForm.Levels;
            ViewBag.LevelList = new List<SelectListItem>();
            ViewBag.HasEditRight = this.HasRight(ApolloParlayRMSFunctionKeys.MemberManagement, Constant.Policy.Host);
            return this.View();
        }

        /// <summary>
        /// Gets the special member list.
        /// </summary>
        /// <param name="criteria">Criteria that used to query special customers.</param>
        /// <returns>JSON result of special customers.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MemberManagement, Policy = Constant.Policy.Read)]
        public JsonResult GetSpecialCustomers(GetCustomersCriteria criteria)
        {
            var specialMembers = this._memberManagementService.GetCustomers(criteria);
            return Json(specialMembers);
        }

        /// <summary>
        /// Adds the special customer.
        /// </summary>
        /// <param name="criteria">The criteria contain the new customer's information.</param>
        /// <returns>JSON result indicates whether the customer is added successfully or not.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MemberManagement, Policy = Constant.Policy.Host)]
        public JsonResult AddSpecialCustomer(InsertCustomerCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                return Json(this._memberManagementService.InsertCustomer(criteria));
            }
            else
            {
                return this.GetInvalidModelErrors();
            }
        }

        /// <summary>
        /// Changes the customer's level.
        /// </summary>
        /// <param name="criteria">The criteria that contains the customer ID and new level value.</param>
        /// <returns>Whether the customer's level is updated successfully or not.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MemberManagement, Policy = Constant.Policy.Host)]
        public JsonResult ChangeCustomerLevel(ChangeCustomerLevelCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                return Json(this._memberManagementService.ChangeCustomerLevel(criteria));
            }
            else
            {
                return this.GetInvalidModelErrors();
            }
        }

        /// <summary>
        /// Export customer list to Excel file.
        /// </summary>
        /// <param name="criteria">Criteria that are used to filter data.</param>
        /// <returns>Excel file contains customer list.</returns>
        public ActionResult ExportCustomerList(GetCustomersCriteria criteria)
        {
            string templateUrl = @"~\Areas\Management\ExcelTemplates\SpecialMembers.xls";
            templateUrl = System.Web.HttpContext.Current.Server.MapPath(templateUrl);
            this._memberManagementService.ExportCustomerList(criteria, templateUrl);
            return new EmptyResult();
        }
    }
}