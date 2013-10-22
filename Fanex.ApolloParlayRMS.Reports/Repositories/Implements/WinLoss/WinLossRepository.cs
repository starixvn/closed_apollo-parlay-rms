// --------------------------------------------------------------------------
// <copyright file="WinLossRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Repositories.Implements.WinLoss
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Reports.Criteria.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Models.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Repositories.Interfaces.WinLoss;
    using Fanex.Data;

    /// <summary>
    /// Win loss repository.
    /// </summary>
    public class WinLossRepository : IWinLossRepository
    {
        /// <summary>
        /// Gets the win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of WinLossItem objects.</returns>
        public IEnumerable<WinLossItem> GetWinLossData(GetWinLossCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return db.Query<WinLossItem>(param);
            }
        }
    }
}