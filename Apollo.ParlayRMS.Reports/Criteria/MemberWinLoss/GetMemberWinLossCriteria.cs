// --------------------------------------------------------------------------
// <copyright file="GetMemberWinLossCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.16">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Criteria.MemberWinLoss
{
    using System;
    using System.Linq;
    using Apollo.ParlayRMS.Core.Configurations;
    using Apollo.ParlayRMS.Core.Extensions;
    using Apollo.ParlayRMS.Core.Models;
    using Apollo.ParlayRMS.Core.Resources;
    using Apollo.ParlayRMS.Reports.Resources;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get member win loss data.
    /// </summary>
    public class GetMemberWinLossCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public byte? SportType { get; set; }

        [SpParam(Ignored = true)]
        public int BetType { get; set; }

        public string Selection { get; set; }

        public long CustId { get; set; }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        [SpParam(Ignored = true)]
        public bool IsMember { get; set; }

        [SpParam(Ignored = true)]
        public bool IsLoadNew { get; set; }

        /// <summary>
        /// Gets the minimum date that the supports to query data for Member Win Loss report.
        /// </summary>
        /// <returns>The minimum date of the date range.</returns>
        public static DateTime GetMinDate()
        {
            int limitMonths = AppSettings.MemberWinLossLimitMonths;
            DateTime limitDate = AppSettings.MemberWinLossLimitDate;
            DateTime minDate = DateTime.Now.AddMonths(limitMonths * -1);

            return (limitDate > minDate) ? limitDate : minDate;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var combosTitle = Lang.All;
            var combos = this.Selection.Split(',');
            if (combos.Length < 5)
            {
                combosTitle = string.Join(",", combos.Select(x => x == "6" ? Lang.SixUp : x));
            }

            string title = string.Format(
                                      ReportsLang.MemberWinLossTitle,
                                      this.Username.ToUpperInvariant(),
                                      this.FromDate.ToShortFormatedDate(),
                                      this.ToDate.ToShortFormatedDate(),
                                      FilterForm.GetBetType(this.BetType),
                                      combosTitle,
                                      FilterForm.GetSportType(this.SportType));
            return title;
        }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return this.IsMember ? "RMS_Rpt_Parlay_MemberWinLoss" : "RMS_Rpt_Parlay_AgentWinLoss";
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