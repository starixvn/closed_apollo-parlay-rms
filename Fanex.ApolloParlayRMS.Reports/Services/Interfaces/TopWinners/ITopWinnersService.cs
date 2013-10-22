// --------------------------------------------------------------------------
// <copyright file="ITopWinnersService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Services.Interfaces.TopWinners
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Core.Services.Interfaces;
    using Fanex.ApolloParlayRMS.Reports.Criteria.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Models.TopWinners;

    public interface ITopWinnersService : IBasedService
    {
        /// <summary>
        /// Gets the top winner list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>TopWinnerList object contains the top winner list and total data of all top winners.</returns>
        TopWinner GetTopWinners(GetTopWinnersCriteria criteria);

        /// <summary>
        /// Gets a top winner's details.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>TopWinnerDetails object represents the detailed data of a top winner.</returns>
        TopWinnerDetails GetTopWinnerDetails(GetTopWinnerDetailsCriteria criteria);

        /// <summary>
        /// Exports the top winners report to Excel file.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="templatePath">The template's path.</param>
        void ExportTopWinners(GetTopWinnersCriteria criteria, string templatePath);

        /// <summary>
        /// Gets the top winner bet details.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>TopWinner object contains the top winner list and total data of all top winners.</returns>
        IEnumerable<TopWinnerBetDetails> GetTopWinnerBetDetails(GetTopWinnerBetDetailsCriteria criteria);
    }
}