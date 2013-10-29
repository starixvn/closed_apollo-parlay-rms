// --------------------------------------------------------------------------
// <copyright file="ITotalOutstandingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Repositories.Interfaces.TotalOutstanding
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Tools.Criteria.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Models.TotalOutstanding;

    public interface ITotalOutstandingRepository
    {
        /// <summary>
        /// Gets total outstanding of agent.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of AgentTotalOutstanding objects.</returns>
        IEnumerable<AgentTotalOutstanding> GetTotalOutstandingOfAgent(GetTotalOutstandingOfAgentCriteria criteria);

        /// <summary>
        /// Gets total outstanding of member.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of MemberTotalOutstanding objects.</returns>
        IEnumerable<MemberTotalOutstanding> GetTotalOutstandingOfMember(GetTotalOutstandingOfMemCriteria criteria);
    }
}