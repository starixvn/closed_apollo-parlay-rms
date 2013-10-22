// --------------------------------------------------------------------------
// <copyright file="IWinLossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.15">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Services.Interfaces.WinLoss
{
    using Fanex.ApolloParlayRMS.Reports.Criteria.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Models.WinLoss;

    public interface IWinLossService
    {
        /// <summary>
        /// Gets the win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>Queried and grouped win loss data.</returns>
        WinLossItem GetWinLossData(GetWinLossCriteria criteria);

        /// <summary>
        /// Exports the win loss report to Excel file.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="templatePath">The template's path.</param>
        void ExportWinLoss(GetWinLossCriteria criteria, string templatePath);
    }
}