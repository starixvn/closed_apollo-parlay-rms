// --------------------------------------------------------------------------
// <copyright file="TopWinnersRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Repositories.Implements.TopWinners
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Reports.Criteria.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Models.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Repositories.Interfaces.TopWinners;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Top Winners Repository.
    /// </summary>
    public class TopWinnersRepository : ITopWinnersRepository
    {
        /// <summary>
        /// Gets the top winners.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>List of TopWinners objects.</returns>
        public IEnumerable<TopWinner> GetTopWinners(GetTopWinnersCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return db.Query<TopWinner>(param);
            }
        }

        /// <summary>
        /// Gets a top winner's details.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>List of TopWinnerDetails objects.</returns>
        public IEnumerable<TopWinnerDetails> GetTopWinnerDetails(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return db.Query<TopWinnerDetails>(param);
            }
        }

        /// <summary>
        /// Gets a top winner's bet details.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>List of TopWinnerBetDetails objects.</returns>
        public IEnumerable<TopWinnerBetDetails> GetTopWinnerBetDetails(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return db.Query<TopWinnerBetDetails>(param);
            }
        }
    }
}