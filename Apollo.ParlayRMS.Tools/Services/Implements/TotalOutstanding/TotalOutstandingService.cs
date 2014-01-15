// --------------------------------------------------------------------------
// <copyright file="TotalOutstandingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Apollo.ParlayRMS.Tools.Services.Implements.TotalOutstanding
{
    using System.Linq;
    using Apollo.ParlayRMS.Tools.Criteria.TotalOutstanding;
    using Apollo.ParlayRMS.Tools.Models.TotalOutstanding;
    using Apollo.ParlayRMS.Tools.Services.Interfaces.TotalOutstanding;
    using Fanex.NAP.Core.Service.Impl;

    /// <summary>
    /// Total outstanding service.
    /// </summary>
    public class TotalOutstandingService : BaseService, ITotalOutstandingService
    {
        /// <summary>
        /// Gets total outstanding of agent.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>
        /// AgentTotalOutstandingList object contains list of AgentTotalOutstanding objects and Total property which
        /// summarize the Stake After PT, Maximum Payout After PT and Total Bets of all items.
        /// </returns>
        public AgentTotalOutstandingList GetTotalOutstandingOfAgent(GetTotalOutstandingOfAgentCriteria criteria)
        {
            var agentTotalOutstanding = new AgentTotalOutstandingList();
            var total = new AgentTotalOutstanding();
            var records = this.GetData<AgentTotalOutstanding>(criteria).ToList();

            records.ForEach(obj =>
            {
                total.StakePT += obj.StakePT;
                total.MaxPayoutPT += obj.MaxPayoutPT;
                total.BetCount += obj.BetCount;
            });

            agentTotalOutstanding = new AgentTotalOutstandingList
            {
                Records = records,
                Total = total
            };

            return agentTotalOutstanding;
        }

        /// <summary>
        /// Gets total outstanding of member.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>
        /// MemberTotalOutstandingList object contains list of MemberTotalOutstanding objects and Total property which
        /// summarize the Stake After PT and Maximum Payout After PT of all items.
        /// </returns>
        public MemberTotalOutstandingList GetTotalOutstandingOfMember(GetTotalOutstandingOfMemCriteria criteria)
        {
            var memberTotalOutstanding = new MemberTotalOutstandingList();
            var total = new MemberTotalOutstanding();
            var records = this.GetData<MemberTotalOutstanding>(criteria).ToList();

            records.ForEach(obj =>
            {
                total.StakePT += obj.StakePT;
                total.MaxPayoutPT += obj.MaxPayoutPT;
            });

            memberTotalOutstanding = new MemberTotalOutstandingList
            {
                Records = records,
                Total = total
            };

            return memberTotalOutstanding;
        }
    }
}