// --------------------------------------------------------------------------
// <copyright file="ISimilarBetsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Services.Interfaces.SimilarBets
{
    using Fanex.ApolloParlayRMS.Tools.Criteria.SimilarBets;
    using Fanex.ApolloParlayRMS.Tools.Models.SimilarBets;

    public interface ISimilarBetsService
    {
        /// <summary>
        /// Gets the similar bet list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>
        /// SimilarBetList object contains the found information about the bet of passed reference number
        /// and list of its similar bets.
        /// </returns>
        SimilarBetList GetSimilarBetList(GetSimilarBetListCriteria criteria);
    }
}