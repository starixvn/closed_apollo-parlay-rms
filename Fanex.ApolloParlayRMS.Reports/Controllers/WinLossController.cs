// --------------------------------------------------------------------------
// <copyright file="WinLossController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.14">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Controllers
{
    using System.Web.Mvc;
    using Fanex.ApolloParlayRMS.Core.Controllers;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.Reports.Criteria.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Services.Implements.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Services.Interfaces.WinLoss;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Win loss controller.
    /// </summary>
    public class WinLossController : BaseController
    {
        private readonly IWinLossService _winLossService;

        public WinLossController() :
            this(null)
        {
        }

        public WinLossController(IWinLossService winLossService)
        {
            this._winLossService = _winLossService ?? new WinLossService { ServiceContext = this.ServiceContext };
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.WinLossReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.MinDate = GetWinLossCriteria.GetMinDate();
            ViewBag.BetTypes = FilterForm.BetTypes;
            ViewBag.SportTypes = FilterForm.SportTypes;
            ViewBag.Combos = FilterForm.Combos;

            return this.View();
        }

        /// <summary>
        /// Gets the win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>JSON object contains the report data and report title.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.WinLossReport, Policy = Constant.Policy.Read)]
        public JsonResult GetWinLoss(GetWinLossCriteria criteria)
        {
            var data = this._winLossService.GetWinLossData(criteria);
            var title = criteria.ToString();

            return Json(new
            {
                Data = data,
                Title = title
            });
        }

        /// <summary>
        /// Export win loss report to Excel file.
        /// </summary>
        /// <param name="criteria">Criteria that are used to filter data.</param>
        /// <returns>Excel file contains win loss report.</returns>
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.WinLossReport, Policy = Constant.Policy.Read)]
        public ActionResult ExportWinLoss(GetWinLossCriteria criteria)
        {
            string templateUrl = @"~\Areas\Reports\ExcelTemplates\WinLoss.xls";
            templateUrl = System.Web.HttpContext.Current.Server.MapPath(templateUrl);
            this._winLossService.ExportWinLoss(criteria, templateUrl);
            return new EmptyResult();
        }
    }
}