// --------------------------------------------------------------------------
// <copyright file="GetTopWinnerDetailsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.14">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Criteria.TopWinners
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get a top winner's detail.
    /// </summary>
    public class GetTopWinnerDetailsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte? SportType { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public string Selection { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_Rpt_Parlay_TopWinners_Details";
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