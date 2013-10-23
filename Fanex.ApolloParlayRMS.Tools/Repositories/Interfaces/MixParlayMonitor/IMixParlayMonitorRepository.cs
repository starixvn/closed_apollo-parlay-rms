// --------------------------------------------------------------------------
// <copyright file="IMixParlayMonitorRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Repositories.Interfaces.MixParlayMonitor
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Tools.Criteria.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Models.MixParlayMonitor;

    public interface IMixParlayMonitorRepository
    {
        /// <summary>
        /// Gets the member win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of WinLossItem objects.</returns>
        IEnumerable<BetItem> GetMonitorParlayBetListData(GetMonitorParlayBetListCriteria criteria);
    }
}
