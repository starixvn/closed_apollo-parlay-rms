// --------------------------------------------------------------------------
// <copyright file="TotalOutstandingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Repositories.Implements.TotalOutstanding
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Tools.Criteria.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Models.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Repositories.Interfaces.TotalOutstanding;
    using Fanex.Data;

    /// <summary>
    /// Total outstanding repository.
    /// </summary>
    public class TotalOutstandingRepository : ITotalOutstandingRepository
    {
        /// <summary>
        /// Gets total oustanding of agent.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of AgentTotalOutstanding objects.</returns>
        public IEnumerable<AgentTotalOutstanding> GetTotalOutstandingOfAgent(GetTotalOutstandingOfAgentCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return db.Query<AgentTotalOutstanding>(param);
            }
        }

        /// <summary>
        /// Gets total oustanding of member.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of MemberTotalOutstanding objects.</returns>
        public IEnumerable<MemberTotalOutstanding> GetTotalOutstandingOfMember(GetTotalOutstandingOfMemCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return db.Query<MemberTotalOutstanding>(param);
            }
        }
    }
}