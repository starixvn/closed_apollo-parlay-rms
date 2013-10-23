// --------------------------------------------------------------------------
// <copyright file="IMemberWinLossRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.16">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Repositories.Interfaces.MemberWinLoss
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Reports.Criteria.MemberWinLoss;
    using Fanex.ApolloParlayRMS.Reports.Models.MemberWinLoss;

    public interface IMemberWinLossRepository
    {
        /// <summary>
        /// Gets the member win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of WinLossItem objects.</returns>
        IEnumerable<MemberWinLossItem> GetMemberWinLossData(GetMemberWinLossCriteria criteria);
    }
}