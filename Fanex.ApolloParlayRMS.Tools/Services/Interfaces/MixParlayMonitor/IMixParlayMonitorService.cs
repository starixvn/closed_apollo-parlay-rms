// --------------------------------------------------------------------------
// <copyright file="IMixParlayMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.21">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Services.Interfaces.MixParlayMonitor
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Tools.Criteria.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Models.MixParlayMonitor;

    public interface IMixParlayMonitorService
    {
        /// <summary>
        /// Gets list of bets of the unsettled parlay tickets.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of BetItem objects represent the bet list of tickets satisfied the passed query conditions.</returns>
        IEnumerable<ParlayBetItem> GeMixParlayData(GetMonitorParlayBetListCriteria criteria);
    }
}