// --------------------------------------------------------------------------
// <copyright file="GetTopWinnersCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.14">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Criteria.TopWinners
{
    using System;
    using System.Linq;
    using Apollo.ParlayRMS.Core.Configurations;
    using Apollo.ParlayRMS.Core.Models;
    using Apollo.ParlayRMS.Core.Resources;
    using Apollo.ParlayRMS.Reports.Resources;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get top winners criteria.
    /// </summary>
    public class GetTopWinnersCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public int LimitedRow { get; set; }

        public byte? SportType { get; set; }

        public string Selection { get; set; }

        [SpParam(Ignored = true)]
        public bool IsLoadNew { get; set; }

        /// <summary>
        /// Gets the minimum date that the supports to query data for Top Winners report.
        /// </summary>
        /// <returns>The minimum date of the date range.</returns>
        public static DateTime GetMinDate()
        {
            int dateRange = AppSettings.TopWinnersDateRange;
            DateTime limitDate = AppSettings.LimitDataFromDate;
            DateTime minDate = DateTime.Now.AddDays(dateRange * -1);

            return (limitDate > minDate) ? limitDate : minDate;
        }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_Rpt_Parlay_TopWinners";
        }

        /// <summary>
        /// Check validation of the criteria.
        /// </summary>
        /// <returns>Whether or not the criteria is valid.</returns>
        public override bool IsValid()
        {
            return true;
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
                                      ReportsLang.TopWinnersReportTitle,
                                      this.LimitedRow,
                                      this.FromDate.ToShortFormatedDate(),
                                      this.ToDate.ToShortFormatedDate(),
                                      combosTitle,
                                      FilterForm.GetSportType(this.SportType));
            return title;
        }
    }
}