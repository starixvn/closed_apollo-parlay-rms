// --------------------------------------------------------------------------
// <copyright file="GetTotalOutstandingOfAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Apollo.ParlayRMS.Tools.Criteria.TotalOutstanding
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to get total outstanding of an agent.
    /// </summary>
    public class GetTotalOutstandingOfAgentCriteria : CriteriaBase
    {
        public long CustId { get; set; }

        public string Selection { get; set; }

        public string SportType { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_Rpt_Parlay_Outstanding_Ag";
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