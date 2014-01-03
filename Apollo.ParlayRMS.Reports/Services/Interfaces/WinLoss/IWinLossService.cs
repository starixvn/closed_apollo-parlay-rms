// --------------------------------------------------------------------------
// <copyright file="IWinLossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.15">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Services.Interfaces.WinLoss
{
    using Apollo.ParlayRMS.Reports.Criteria.WinLoss;
    using Apollo.ParlayRMS.Reports.Models.WinLoss;

    public interface IWinLossService
    {
        /// <summary>
        /// Gets the win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>Queried and grouped win loss data.</returns>
        DailyWinLossList GetWinLossData(GetWinLossCriteria criteria);

        /// <summary>
        /// Exports the win loss report to Excel file.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="templatePath">The template's path.</param>
        void ExportWinLoss(GetWinLossCriteria criteria, string templatePath);
    }
}