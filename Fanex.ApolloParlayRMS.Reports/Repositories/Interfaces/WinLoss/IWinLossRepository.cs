// --------------------------------------------------------------------------
// <copyright file="IWinLossRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.15">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Repositories.Interfaces.WinLoss
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Reports.Criteria.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Models.WinLoss;

    public interface IWinLossRepository
    {
        /// <summary>
        /// Gets the win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of WinLossItem objects.</returns>
        IEnumerable<WinLossItem> GetWinLossData(GetWinLossCriteria criteria);
    }
}