// --------------------------------------------------------------------------
// <copyright file="TopWinnersController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.14">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.Core.Web.Controllers;
    using Fanex.ApolloParlayRMS.Reports.Criteria.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Models.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Services.Implements.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Services.Interfaces.TopWinners;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Top Winners controller.
    /// </summary>
    public class TopWinnersController : BasedController
    {
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
            var data = this._topWinnersService.GetTopWinners(criteria);
            var title = criteria.ToString();

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
            string templateUrl = @"~\Areas\Reports\ExcelTemplates\TopWinners.xls";
            templateUrl = System.Web.HttpContext.Current.Server.MapPath(templateUrl);
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
            var data = this._topWinnersService.GetTopWinnerDetails(criteria);

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
            ViewBag.Username = criteria.UserName;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var data = this._topWinnersService.GetTopWinnerBetDetails(criteria);
            var tickets = Map(criteria.RefNo, data);

            foreach (var ticket in tickets)
            {
                if (tickets.Last() == ticket)
                {
                    builder.AddChoice(
                        ticket,
                        ticketHelper,
                        null,
                        string.Format("<div id='{0}'><div class='ticketDetail'>", ticket.BetId),
                        string.Format("</div><div class='ticketStatus'><div class='divStatus'>[{0}]</div></div></div>", ticket.Status),
                        false);
                }
                else
                {
                    builder.AddChoice(
                        ticket,
                        ticketHelper,
                        null,
                        string.Format("<div id='{0}'><div class='ticketDetail'>", ticket.BetId),
                        string.Format("</div><div class='ticketStatus'><div class='divStatus'>[{0}]</div></div></div><div class='line'></div>", ticket.Status),
                        false);
                }
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