// --------------------------------------------------------------------------
// <copyright file="GetWinLossCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.14">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Criteria.WinLoss
{
    using System;
    using System.Linq;
    using Fanex.ApolloParlayRMS.Core.Configurations;
    using Fanex.ApolloParlayRMS.Core.Extensions;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Reports.Resources;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get win loss data criteria.
    /// </summary>
    public class GetWinLossCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public byte? SportType { get; set; }

        public int BetType { get; set; }

        public string Selection { get; set; }

        [SpParam(Ignored = true)]
        public bool IsLoadNew { get; set; }

        /// <summary>
        /// Gets the minimum date that the supports to query data for Win Loss report.
        /// </summary>
        /// <returns>The minimum date of the date range.</returns>
        public static DateTime GetMinDate()
        {
            int dateRange = AppSettings.WinLossDateRange;
            DateTime limitDate = AppSettings.LimitDataFromDate;
            DateTime minDate = DateTime.Now.AddDays(dateRange * -1);

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
                                      ReportsLang.WinLossReportTitle,
                                      this.FromDate.ToMonthDayYearFormat(),
                                      this.ToDate.ToMonthDayYearFormat(),
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
            return "RMS_Rpt_Parlay_Winloss";
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