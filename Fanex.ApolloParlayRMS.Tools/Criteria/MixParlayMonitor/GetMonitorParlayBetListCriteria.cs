// --------------------------------------------------------------------------
// <copyright file="GetMonitorParlayBetListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.21">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Criteria.MixParlayMonitor
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class GetMonitorParlayBetListCriteria : CriteriaBase
    {
        public string Selection { get; set; }

        public string Sporttype { get; set; }

        public string Level { get; set; }

        public decimal MaxPayout { get; set; }

        public long Max_TransId { get; set; }

        public int LimitRowCount { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_Monitor_Parlay_Betlist";
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