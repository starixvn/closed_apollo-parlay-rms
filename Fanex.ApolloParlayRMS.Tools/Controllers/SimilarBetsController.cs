// --------------------------------------------------------------------------
// <copyright file="SimilarBetsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.Core.Web.Controllers;
    using Fanex.ApolloParlayRMS.Tools.Criteria.SimilarBets;
    using Fanex.ApolloParlayRMS.Tools.Services.Implements.BetList;
    using Fanex.ApolloParlayRMS.Tools.Services.Implements.SimilarBets;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.BetList;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.SimilarBets;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Similar bet controller.
    /// </summary>
    public class SimilarBetsController : BasedController
    {
        private readonly ISimilarBetsService _similarBetsService;
        private readonly IBetListService _betListService;

        public SimilarBetsController() :
            this(null, null)
        {
        }

        public SimilarBetsController(ISimilarBetsService similarBetsService, IBetListService betListService)
        {
            this._similarBetsService = similarBetsService ?? new SimilarBetsService { ServiceContext = this.ServiceContext };
            this._betListService = betListService ?? new BetListService();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <param name="betId">The ID of the bet that needs to filter on.</param>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.SimilarBets, Policy = Constant.Policy.Read)]
        public ActionResult Index(long? betId = null)
        {
            ViewBag.BetId = betId;

            return this.View();
        }

        /// <summary>
        /// Gets the similar bets.
        /// </summary>
        /// <param name="refNo">The reference number of the ticket which needs to get bet details.</param>
        /// <returns>JSON object contains bet list of the transaction of passed reference number.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.SimilarBets, Policy = Constant.Policy.Read)]
        public JsonResult GetSimilarBets(long refNo)
        {
            var data = this._similarBetsService.GetSimilarBetList(new GetSimilarBetListCriteria
            {
                TransId = refNo
            });

            return Json(data);
        }

        /// <summary>
        /// Gets the bet details of a transaction.
        /// </summary>
        /// <param name="betId">The bet identifier.</param>
        /// <param name="refNo">The reference number.</param>
        /// <param name="originalRefNo">The original reference number.</param>
        /// <param name="userName">The user name of a member.</param>
        /// <returns>JSON object represents the bet list of the passed reference number.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.TotalOutstanding, Policy = Constant.Policy.Read)]
        public JsonResult GetBetDetails(long betId, long refNo, long originalRefNo, string userName)
        {
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            StringBuilder ticketsCompare = new StringBuilder();
            string ticketCompareStr = string.Empty;
            var tickets = this._betListService.CreateChoices(refNo);
            var originalTickets = this._betListService.CreateChoices(originalRefNo);

            foreach (var ticket in tickets)
            {
                bool flag = false;
                foreach (var originalTicket in originalTickets)
                {
                    if ((originalTicket.LeagueId == ticket.LeagueId)
                        && (originalTicket.SportTypeId == ticket.SportTypeId)
                        && (originalTicket.HomeId == ticket.HomeId)
                        && (originalTicket.AwayId == ticket.AwayId)
                        && (originalTicket.BetTypeId == ticket.BetTypeId)
                        && (originalTicket.BetTeam == ticket.BetTeam))
                    {
                        flag = true;
                    }
                }

                if (!flag)
                {
                    ticketsCompare.Append(ticket.BetId);
                    ticketsCompare.Append("-");
                }

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

            ticketCompareStr = ticketsCompare.ToString();
            return Json(new
            {
                TicketDetail = builder.GetBetList(),
                TicketCompare = string.IsNullOrEmpty(ticketCompareStr) ? "null" : ticketCompareStr,
                Title = string.Format("{0} {1} - {2} - {3}", Lang.TicketDetails, userName, betId, refNo)
            });
        }
    }
}