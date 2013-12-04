// --------------------------------------------------------------------------
// <copyright file="IMemberWinLossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.15">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Services.Interfaces.MemberWinLoss
{
    using Apollo.ParlayRMS.Reports.Criteria.MemberWinLoss;
    using Apollo.ParlayRMS.Reports.Models.MemberWinLoss;

    public interface IMemberWinLossService
    {
        /// <summary>
        /// Gets the member win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>Queried and grouped member win loss data.</returns>
        MemberWinLossItem GetMemberWinLossData(GetMemberWinLossCriteria criteria);

        /// <summary>
        /// Exports the win loss report to Excel file.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="agentTemplatePath">The path of agent win loss template.</param>
        /// <param name="memberTemplatePath">The path of member win loss template.</param>
        void ExportMemberWinLoss(GetMemberWinLossCriteria criteria, string agentTemplatePath, string memberTemplatePath);
    }
}