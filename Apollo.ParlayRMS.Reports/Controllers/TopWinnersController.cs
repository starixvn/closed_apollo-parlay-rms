// --------------------------------------------------------------------------
// <copyright file="TopWinnersController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.14">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.ParlayRMS.Core.Controllers;
    using Apollo.ParlayRMS.Core.Models;
    using Apollo.ParlayRMS.Reports.Criteria.TopWinners;
    using Apollo.ParlayRMS.Reports.Models.TopWinners;
    using Apollo.ParlayRMS.Reports.Services.Implements.TopWinners;
    using Apollo.ParlayRMS.Reports.Services.Interfaces.TopWinners;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Top Winners controller.
    /// </summary>
    public class TopWinnersController : BaseController
    {
        private const string TopWinnerTemplateFilePath = @"~\Areas\Reports\ExcelTemplates\TopWinners.xls";
        private readonly ITopWinnersService _topWinnersService;

        public TopWinnersController() :
            this(null)
        {
        }

        public TopWinnersController(ITopWinnersService topWinnersService)
        {
            this._topWinnersService = topWinnersService ?? new TopWinnersService { ServiceContext = this.ServiceContext };
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.TopWinnersReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.MinDate = GetTopWinnersCriteria.GetMinDate();
            ViewBag.SelectTops = FilterForm.SelectTops;
            ViewBag.DateRanges = FilterForm.DateRanges;
            ViewBag.SportTypes = FilterForm.SportTypes;
            ViewBag.Combos = FilterForm.Combos;
            ViewBag.Levels = FilterForm.Levels;
            ViewBag.HasEditRight = this.HasRight(ApolloParlayRMSFunctionKeys.MemberManagement, Constant.Policy.Host);

            return this.View();
        }

        /// <summary>
        /// Gets the top winners' data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>JSON object contains the report data and report title.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.TopWinnersReport, Policy = Constant.Policy.Read)]
        public JsonResult GetTopWinners(GetTopWinnersCriteria criteria)
        {
            TopWinnerList data = this._topWinnersService.GetTopWinners(criteria);
            string title = criteria.ToString();

            return Json(new
            {
                Data = data,
                Title = title
            });
        }

        /// <summary>
        /// Export top winners to Excel file.
        /// </summary>
        /// <param name="criteria">Criteria that are used to filter data.</param>
        /// <returns>Excel file contains top winner list.</returns>
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.TopWinnersReport, Policy = Constant.Policy.Read)]
        public ActionResult ExportTopWinners(GetTopWinnersCriteria criteria)
        {
            string templateUrl = System.Web.HttpContext.Current.Server.MapPath(TopWinnerTemplateFilePath);
            this._topWinnersService.ExportTopWinners(criteria, templateUrl);
            return new EmptyResult();
        }

        /// <summary>
        /// Gets the top winner details data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>JSON object contains the details of a customer.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.TopWinnersReport, Policy = Constant.Policy.Read)]
        public JsonResult GetTopWinnerDetails(GetTopWinnerDetailsCriteria criteria)
        {
            TopWinnerDetailList data = this._topWinnersService.GetTopWinnerDetails(criteria);

            return Json(data);
        }

        /// <summary>
        /// Gets detail of a bet.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>View represents the detail of a bet.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.TopWinnersReport, Policy = Constant.Policy.Read)]
        public ViewResult BetDetails(GetTopWinnerBetDetailsCriteria criteria)
        {
            ViewBag.BetId = criteria.BetId;
            ViewBag.Username = criteria.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            IEnumerable<TopWinnerBetDetails> data = this._topWinnersService.GetTopWinnerBetDetails(criteria);
            IEnumerable<ITicket> tickets = Map(criteria.RefNo, data);

            string openTags = "<div id='{0}'><div class='ticketDetail'>";
            string closeTags = "</div><div class='ticketStatus'><div class='divStatus'>[{0}]</div></div></div>{1}";
            string borderTag = string.Empty;

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

            ViewBag.HtmlContent = builder.GetBetList();
            return this.View();
        }

        /// <summary>
        /// Maps a list of TopWinnerBetDetails objects to list of ITicket objects.
        /// </summary>
        /// <param name="refNo">The reference number.</param>
        /// <param name="data">The top winner bet details data.</param>
        /// <returns>List of ITicket objects.</returns>
        private IEnumerable<ITicket> Map(long refNo, IEnumerable<TopWinnerBetDetails> data)
        {
            List<ITicket> result = new List<ITicket>();
            foreach (var item in data)
            {
                ITicket ticket = new Ticket
                {
                    BetId = item.TransId,
                    BetTypeId = item.BetType,
                    HomeId = item.HomeId,
                    AwayId = item.AwayId,
                    Handicap1 = item.Hdp1,
                    Handicap2 = item.Hdp2,
                    Odds = item.Odds,
                    BetTeam = item.BetTeam,
                    EventStatus = item.BetStatus,
                    LiveHomeScore = item.LiveHomeScore,
                    LiveAwayScore = item.LiveAwayScore,
                    IsLive = item.LiveIndicator,
                    LeagueId = item.LeagueId,
                    SportTypeId = item.SportType,
                    IsNeutral = item.IsNeutral,
                    MatchId = item.MatchId,
                    MatchCode = item.MatchCode.ToString(),
                    RefNo = refNo.ToString(),
                    Status = item.Status
                };

                result.Add(ticket);
            }

            return result.OrderBy(x => x.BetId).ToList();
        }
    }
}