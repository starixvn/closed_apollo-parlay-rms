// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Core.Configurations
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Application settings.
    /// </summary>
    public static class AppSettings
    {
        public static bool IsDebug
        {
            get
            {
                return Convert.ToBoolean(GetValue("IsDebug"));
            }
        }

        public static DateTime LimitDataFromDate
        {
            get
            {
                return Convert.ToDateTime(GetValue("LimitDataFromDate"));
            }
        }

        public static int TopWinnersDateRange
        {
            get
            {
                return Convert.ToInt32(GetValue("TopWinnersDateRange"));
            }
        }

        public static int WinLossDateRange
        {
            get
            {
                return Convert.ToInt32(GetValue("WinLossDateRange"));
            }
        }

        public static int MemberWinLossLimitMonths
        {
            get
            {
                return Convert.ToInt32(GetValue("MemberWinLossLimitMonths"));
            }
        }

        public static DateTime MemberWinLossLimitDate
        {
            get
            {
                return Convert.ToDateTime(GetValue("MemberWinLossLimitDate"));
            }
        }

        /// <summary>
        /// Gets the value from web configuration.
        /// </summary>
        /// <param name="key">The key which is used to identify the value.</param>
        /// <returns>Associated value of passed key.</returns>
        private static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}