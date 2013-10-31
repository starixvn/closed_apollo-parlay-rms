// --------------------------------------------------------------------------
// <copyright file="ISimilarBetsRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Repositories.Interfaces.SimilarBets
{
    using Fanex.ApolloParlayRMS.Tools.Models.SimilarBets;
    using Fanex.NAP.Framework.Data;

    public interface ISimilarBetsRepository
    {
        /// <summary>
        /// Gets the similar bet list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>
        /// SimilarBetList object contains the found information about the bet of passed reference number
        /// and list of its similar bets.
        /// </returns>
        SimilarBetList GetSimilarBetList(ICriteria criteria);
    }
}