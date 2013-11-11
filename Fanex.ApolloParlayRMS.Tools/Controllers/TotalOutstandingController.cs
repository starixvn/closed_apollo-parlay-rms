// --------------------------------------------------------------------------
// <copyright file="TotalOutstandingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.ApolloParlayRMS.Core.Constants;
    using Fanex.ApolloParlayRMS.Core.Controllers;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Core.Services.Implements;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.Tools.Criteria.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Services.Implements.BetList;
    using Fanex.ApolloParlayRMS.Tools.Services.Implements.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.BetList;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.TotalOutstanding;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Total outstanding controller.
    /// </summary>
    public class TotalOutstandingController : BaseController
    {
        private readonly ITotalOutstandingService _totalOutstandingService;
        private readonly IBetListService _betListService;

        public TotalOutstandingController() :
            this(null, null)
        {
        }

        public TotalOutstandingController(ITotalOutstandingService totalOutstandingService, IBetListService betListService)
        {
            this._totalOutstandingService = totalOutstandingService ?? new TotalOutstandingService { ServiceContext = this.ServiceContext };
            this._betListService = betListService ?? new BetListService();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <param name="userName">The user name of the member.</param>
        /// <param name="combos">The selected combos which the ticket will be filtered on.</param>
        /// <param name="sportTypes">The sport types of needed tickets.</param>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.TotalOutstanding, Policy = Constant.Policy.Read)]
        public ActionResult Index(string userName = null, string combos = null, string sportTypes = null)
        {
            var comboList = FilterForm.Combos;
            var sportTypeList = new List<SelectListItem>();

            FilterForm.SportTypes.GetRange(1, 3).ForEach(obj => sportTypeList.Add(new SelectListItem
            {
                Text = obj.Text,
                Value = obj.Value
            }));

            if (!string.IsNullOrEmpty(combos))
            {
                comboList.ForEach(obj => obj.Selected = combos.Contains(obj.Value));
            }
            else
            {
                comboList.ForEach(obj => obj.Selected = true);
            }

            if (!string.IsNullOrEmpty(sportTypes))
            {
                sportTypeList.ForEach(obj => obj.Selected = sportTypes.Contains(obj.Value));
            }
            else
            {
                sportTypeList.ForEach(obj => obj.Selected = true);
            }

            ViewBag.HasSimilarBetRight = this.HasRight(ApolloParlayRMSFunctionKeys.SimilarBets, Constant.Policy.Read);
            ViewBag.UserName = userName ?? string.Empty;
            ViewBag.Combos = comboList;
            ViewBag.SportTypes = sportTypeList;

            return this.View();
        }

        /// <summary>
        /// Gets the total outstanding.
        /// </summary>
        /// <param name="userName">The user name of the customer.</param>
        /// <param name="combos">The combos of tickets which will be filtered on.</param>
        /// <param name="sportTypes">The sport types which will be filtered on.</param>
        /// <returns>JSON object contains the total outstanding data of the customer.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.TotalOutstanding, Policy = Constant.Policy.Read)]
        public JsonResult GetTotalOutstanding(string userName, string combos, string sportTypes)
        {
            var customerInfo = CustomerService.GetCustomerInfo(userName);

            if (customerInfo.RoleId == (int)CommonEnums.RoleId.Member)
            {
                var criteria = new GetTotalOutstandingOfMemCriteria
                {
                    CustId = customerInfo.CustId,
                    Selection = combos,
                    SportType = sportTypes
                };
                var memberTotalOutstanding = this._totalOutstandingService.GetTotalOutstandingOfMember(criteria);

                return Json(new
                {
                    Records = memberTotalOutstanding.Records,
                    Total = memberTotalOutstanding.Total,
                    IsMember = true
                });
            }
            else
            {
                var criteria = new GetTotalOutstandingOfAgentCriteria
                {
                    CustId = customerInfo.CustId,
                    Selection = combos,
                    SportType = sportTypes
                };
                var agentTotalOutstanding = this._totalOutstandingService.GetTotalOutstandingOfAgent(criteria);

                return Json(new
                {
                    Records = agentTotalOutstanding.Records,
                    Total = agentTotalOutstanding.Total,
                    IsMember = false
                });
            }
        }

        /// <summary>
        /// Gets the bet details of a transaction.
        /// </summary>
        /// <param name="betId">The bet identifier.</param>
        /// <param name="refNo">The reference number.</param>
        /// <param name="userName">The user name of a member.</param>
        /// <returns>JSON object represents the bet list of the passed reference number.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.TotalOutstanding, Policy = Constant.Policy.Read)]
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