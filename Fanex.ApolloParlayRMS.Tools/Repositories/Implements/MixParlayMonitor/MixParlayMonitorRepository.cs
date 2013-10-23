// --------------------------------------------------------------------------
// <copyright file="MixParlayMonitorRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.21">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Repositories.Implements.MixParlayMonitor
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Tools.Criteria.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Models.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Repositories.Interfaces.MixParlayMonitor;
    using Fanex.Data;

    /// <summary>
    /// Member win loss repository.
    /// </summary>
    public class MixParlayMonitorRepository : IMixParlayMonitorRepository
    {
        /// <summary>
        /// Gets the member win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of WinLossItem objects.</returns>
        public IEnumerable<BetItem> GetMonitorParlayBetListData(GetMonitorParlayBetListCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return db.Query<BetItem>(param);
            }
        }
    }
}