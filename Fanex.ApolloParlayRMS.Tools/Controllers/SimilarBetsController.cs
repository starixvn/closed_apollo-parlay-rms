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
    using System.Web.Mvc;
    using Fanex.ApolloParlayRMS.Core.Controllers;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Core.Web;
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
    public class SimilarBetsController : BaseController
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
        /// <param name="transId">The reference number of the ticket which needs to get bet details.</param>
        /// <returns>JSON object contains bet list of the transaction of passed reference number.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.SimilarBets, Policy = Constant.Policy.Read)]
        public JsonResult GetSimilarBets(long transId)
        {
            var data = this._similarBetsService.GetSimilarBetList(new GetSimilarBetListCriteria
            {
                TransId = transId
            });

            return Json(data);
        }

        /// <summary>
        /// Gets the bet details of a transaction.
        /// </summary>
        /// <param name="transId">The bet identifier.</param>
        /// <param name="refNo">The reference number.</param>
        /// <param name="originalRefNo">The original reference number.</param>
        /// <param name="username">The user name of a member.</param>
        /// <returns>JSON object represents the bet list of the passed reference number.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.SimilarBets, Policy = Constant.Policy.Read)]
        public JsonResult GetBetDetails(long transId, long refNo, long originalRefNo, string username)
        {
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };

            string openTags = "<div id='{0}'><div class='ticketDetail{1}'>";
            string closeTags = "</div><div class='ticketStatus'><div class='divStatus'>[{0}]</div></div></div>{1}";
            string diffentCssClass = string.Empty;
            string borderTag = string.Empty;
            bool isDifferent;

            var tickets = this._betListService.CreateChoices(refNo);
            var originalTickets = this._betListService.CreateChoices(originalRefNo);

            foreach (var ticket in tickets)
            {
                isDifferent = true;
                foreach (var originalTicket in originalTickets)
                {
                    if (CompareTickets(originalTicket, ticket))
                    {
                        isDifferent = false;
                        break;
                    }
                }

                diffentCssClass = isDifferent ? " different-bet" : string.Empty;
                borderTag = (tickets.Last() == ticket) ? string.Empty : "<div class='line'></div>";
                builder.AddChoice(
                            ticket,
                            ticketHelper,
                            null,
                            string.Format(openTags, ticket.BetId, diffentCssClass),
                            string.Format(closeTags, ticket.Status, borderTag),
                            false);
            }

            return Json(new
            {
                TicketDetail = builder.GetBetList(),
                Title = string.Format("{0} - {1} - {2}", Lang.TicketDetails, username, transId)
            });
        }

        /// <summary>
        /// Compares two tickets and identifies whether these two tickets are the same or not.
        /// </summary>
        /// <param name="originalTicket">The original ticket.</param>
        /// <param name="ticket">The ticket that needs to compare with the original one.</param>
        /// <returns>Whether these two tickets are the same or not.</returns>
        private bool CompareTickets(ITicket originalTicket, ITicket ticket)
        {
            return (originalTicket.LeagueId == ticket.LeagueId)
                   && (originalTicket.SportTypeId == ticket.SportTypeId)
                   && (originalTicket.HomeId == ticket.HomeId)
                   && (originalTicket.AwayId == ticket.AwayId)
                   && (originalTicket.BetTypeId == ticket.BetTypeId)
                   && (originalTicket.BetTeam == ticket.BetTeam);
        }
    }
}