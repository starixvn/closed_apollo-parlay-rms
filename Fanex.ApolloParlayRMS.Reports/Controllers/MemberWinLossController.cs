// --------------------------------------------------------------------------
// <copyright file="MemberWinLossController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.14">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Controllers
{
    using System.Web.Mvc;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.Core.Web.Controllers;
    using Fanex.ApolloParlayRMS.Reports.Criteria.MemberWinLoss;
    using Fanex.ApolloParlayRMS.Reports.Criteria.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Services.Implements.MemberWinLoss;
    using Fanex.ApolloParlayRMS.Reports.Services.Interfaces.MemberWinLoss;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.ApolloParlayRMS.Core.Constants;

    /// <summary>
    /// Member win loss controller.
    /// </summary>
    public class MemberWinLossController : BasedController
    {
        private readonly IMemberWinLossService _memberWinLossService;

        public MemberWinLossController() :
            this(null)
        {
        }

        public MemberWinLossController(IMemberWinLossService memberWinLossService)
        {
            this._memberWinLossService = memberWinLossService ?? new MemberWinLossService { ServiceContext = this.ServiceContext };
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MemberWinLossReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.MinDate = GetMemberWinLossCriteria.GetMinDate();
            ViewBag.BetTypes = FilterForm.BetTypes;
            ViewBag.SportTypes = FilterForm.SportTypes;
            ViewBag.Combos = FilterForm.Combos;

            return this.View();
        }

        /// <summary>
        /// Gets the member win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>JSON object contains the report data and report title.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MemberWinLossReport, Policy = Constant.Policy.Read)]
        public JsonResult GetMemberWinLoss(GetMemberWinLossCriteria criteria)
        {
            var data = this._memberWinLossService.GetMemberWinLossData(criteria);
            var title = criteria.ToString();

            return Json(new
            {
                IsMember = (criteria.RoleId == (int)CommonEnums.RoleId.Member),
                Data = data,
                Title = title
            });
        }

        /// <summary>
        /// Export win loss report to Excel file.
        /// </summary>
        /// <param name="criteria">Criteria that are used to filter data.</param>
        /// <returns>Excel file contains win loss report.</returns>
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MemberWinLossReport, Policy = Constant.Policy.Read)]
        public ActionResult ExportMemberWinLoss(GetMemberWinLossCriteria criteria)
        {
            string agentTemplateUrl = @"~\Areas\Reports\ExcelTemplates\AgentWinLoss.xls";
            string memberTemplateUrl = @"~\Areas\Reports\ExcelTemplates\MemberWinLoss.xls";

            agentTemplateUrl = System.Web.HttpContext.Current.Server.MapPath(agentTemplateUrl);
            memberTemplateUrl = System.Web.HttpContext.Current.Server.MapPath(memberTemplateUrl);
            this._memberWinLossService.ExportMemberWinLoss(criteria, agentTemplateUrl, memberTemplateUrl);
            return new EmptyResult();
        }
    }
}