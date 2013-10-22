// --------------------------------------------------------------------------
// <copyright file="ITopWinnersRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Repositories.Interfaces.TopWinners
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Reports.Criteria.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Models.TopWinners;
    using Fanex.NAP.Framework.Data;

    public interface ITopWinnersRepository
    {
        /// <summary>
        /// Gets the top winners.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>List of TopWinners objects.</returns>
        IEnumerable<TopWinner> GetTopWinners(GetTopWinnersCriteria criteria);

        /// <summary>
        /// Gets a top winner's details.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>List of TopWinners objects.</returns>
        IEnumerable<TopWinnerDetails> GetTopWinnerDetails(ICriteria criteria);

        /// <summary>
        /// Gets a top winner's bet details.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>List of TopWinnerBetDetails objects.</returns>
        IEnumerable<TopWinnerBetDetails> GetTopWinnerBetDetails(ICriteria criteria);
    }
}