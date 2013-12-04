// --------------------------------------------------------------------------
// <copyright file="GetSimilarBetListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Apollo.ParlayRMS.Tools.Criteria.SimilarBets
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to get the similar bet list of a specific bet.
    /// </summary>
    public class GetSimilarBetListCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_Similar_Parlay_Betlist";
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