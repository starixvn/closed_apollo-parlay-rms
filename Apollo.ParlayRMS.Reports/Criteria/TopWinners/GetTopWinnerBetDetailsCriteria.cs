// --------------------------------------------------------------------------
// <copyright file="GetTopWinnerBetDetailsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.18">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Criteria.TopWinners
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get a top winner's detail from data warehouse.
    /// </summary>
    public class GetTopWinnerBetDetailsCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        [SpParam(Ignored = true)]
        public long BetId { get; set; }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_Rpt_Parlay_TopWinners_Bet_Details";
        }

        /// <summary>
        /// Check validation of the criteria.
        /// </summary>
        /// <returns>Whether or not the criteria is valid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}