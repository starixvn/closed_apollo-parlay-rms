// --------------------------------------------------------------------------
// <copyright file="MixParlayMonitorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.21">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.ApolloParlayRMS.Core.Controllers;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.Tools.Criteria.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Services.Implements.BetList;
    using Fanex.ApolloParlayRMS.Tools.Services.Implements.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.BetList;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.MixParlayMonitor;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Mix parlay monitor controller.
    /// </summary>
    public class MixParlayMonitorController : BaseController
    {
        private readonly IMixParlayMonitorService _mixParlayMonitorService;
        private readonly IBetListService _betListService;

        public MixParlayMonitorController() :
            this(null, null)
        {
        }

        public MixParlayMonitorController(IMixParlayMonitorService mixParlayMonitorService, IBetListService betListService)
        {
            this._mixParlayMonitorService = mixParlayMonitorService ?? new MixParlayMonitorService { ServiceContext = this.ServiceContext };
            this._betListService = betListService ?? new BetListService();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MixParlayMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.SportTypes = FilterForm.SportTypes.GetRange(1, 3);
            ViewBag.Combos = FilterForm.Combos;
            ViewBag.Levels = FilterForm.LevelList;
            ViewBag.SelectTop = FilterForm.SelectTops;
            ViewBag.RefreshTimes = FilterForm.RefreshTimes.GetRange(1, 3);

            // Check permissions of current user for Similar Bets and Total Outstanding functions.
            ViewBag.HasSimilarBetRight = this.HasRight(ApolloParlayRMSFunctionKeys.SimilarBets, Constant.Policy.Read);
            ViewBag.HasTotalOutstandingRight = this.HasRight(ApolloParlayRMSFunctionKeys.TotalOutstanding, Constant.Policy.Read);

            return this.View();
        }

        /// <summary>
        /// Gets the mix parlay data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>JSON object contains the maximum queried transaction ID and the bet list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MixParlayMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetMixParlayData(GetMonitorParlayBetListCriteria criteria)
        {
            var records = this._mixParlayMonitorService.GeMixParlayData(criteria);
            var maxTransId = criteria.Max_TransId;

            if ((records != null) && (records.Count() > 0))
            {
                maxTransId = Math.Max(criteria.Max_TransId, records.First().TransId);
            }

            return Json(new
            {
                MaxTransId = maxTransId,
                Records = records
            });
        }

        /// <summary>
        /// Gets the bet details of a transaction.
        /// </summary>
        /// <param name="betId">The bet identifier.</param>
        /// <param name="refNo">The reference number.</param>
        /// <param name="userName">The user name of a member.</param>
        /// <returns>JSON object represents the bet list of the passed reference number.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MixParlayMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetBetDetails(long betId, long refNo, string userName)
        {
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };

            string openTags = "<div id='{0}'><div class='ticketDetail'>";
            string closeTags = "</div><div class='ticketStatus'><div class='divStatus'>[{0}]</div></div></div>{1}";
            string borderTag = string.Empty;

            var tickets = this._betListService.CreateChoices(refNo);
            foreach (var ticket in tickets)
            {
                borderTag = (tickets.Last() == ticket) ? string.Empty : "<div class='line'></div>";
                builder.AddChoice(
                            ticket,
                            ticketHelper,
                            null,
                            string.Format(openTags, ticket.BetId),
                            string.Format(closeTags, ticket.Status, borderTag),
                            false);
            }

            return Json(new
            {
                BetList = builder.GetBetList(),
                Title = string.Format("{0} - {1} - {2}", Lang.TicketDetails, userName, betId)
            });
        }
    }
}