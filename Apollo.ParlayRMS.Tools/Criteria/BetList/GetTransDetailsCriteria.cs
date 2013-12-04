// --------------------------------------------------------------------------
// <copyright file="GetTransDetailsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Apollo.ParlayRMS.Tools.Criteria.BetList
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to get the transaction details of a bet.
    /// </summary>
    public class GetTransDetailsCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_Parlay_TransDetails";
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