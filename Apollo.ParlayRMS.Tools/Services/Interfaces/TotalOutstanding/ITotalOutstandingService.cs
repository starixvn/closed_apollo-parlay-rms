// --------------------------------------------------------------------------
// <copyright file="ITotalOutstandingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Apollo.ParlayRMS.Tools.Services.Interfaces.TotalOutstanding
{
    using Apollo.ParlayRMS.Tools.Criteria.TotalOutstanding;
    using Apollo.ParlayRMS.Tools.Models.TotalOutstanding;

    public interface ITotalOutstandingService
    {
        /// <summary>
        /// Gets total outstanding of agent.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>
        /// AgentTotalOutstandingList object contains list of AgentTotalOutstanding objects and Total property which
        /// summarize the Stake After PT, Maximum Payout After PT and Total Bets of all items.
        /// </returns>
        AgentTotalOutstandingList GetTotalOutstandingOfAgent(GetTotalOutstandingOfAgentCriteria criteria);

        /// <summary>
        /// Gets total outstanding of member.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>
        /// MemberTotalOutstandingList object contains list of MemberTotalOutstanding objects and Total property which
        /// summarize the Stake After PT and Maximum Payout After PT of all items.
        /// </returns>
        MemberTotalOutstandingList GetTotalOutstandingOfMember(GetTotalOutstandingOfMemCriteria criteria);
    }
}