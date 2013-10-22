// --------------------------------------------------------------------------
// <copyright file="BalanceUpService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Services.Implements
{
    using System;
    using Fanex.ApolloParlayRMS.Core.Criteria;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Balance Up Service.
    /// </summary>
    public static class BalanceUpService
    {
        #region Static fields

        public const string CachedBalanceUp = "Fanex.ApolloParlayRMS.Core.BalanceUpService.BalanceUp";
        public const string BetTran14SPSuffix = "_14";
        public const string WareHouseSPSuffix = "_WH";
        private static readonly IDynamicRepository BalanceUpRepository;

        #endregion Static fields

        #region Constructor

        static BalanceUpService()
        {
            BalanceUpRepository = new DynamicRepository();
        }

        #endregion Constructor

        #region Static methods

        /// <summary>
        /// Gets the balance up.
        /// </summary>
        /// <returns>BalanceUp object contains balance up day and move bet day.</returns>
        public static BalanceUp GetBalanceUp()
        {
            var balanceUp = CacheManager.Get<BalanceUp>(CachedBalanceUp);
            if (balanceUp == null)
            {
                var criteria = new BalanceUpCriteria();
                balanceUp = BalanceUpRepository.Get<BalanceUp>(criteria);
                CacheManager.Set<BalanceUp>(CachedBalanceUp, balanceUp, new TimeSpan(0, 0, 5, 0));
            }

            return balanceUp;
        }

        /// <summary>
        /// Gets the store mapping suffix.
        /// </summary>
        /// <param name="reportDate">The report date.</param>
        /// <returns>The suffix of stored procedure.</returns>
        public static string GetStoreMappingSuffix(DateTime reportDate)
        {
            BalanceUp balanceUpDate = GetBalanceUp();

            if (reportDate > balanceUpDate.MoveBetsDay)
            {
                return string.Empty;
            }
            else if (reportDate >= balanceUpDate.BalanceUpDay)
            {
                return BetTran14SPSuffix;
            }
            else
            {
                return WareHouseSPSuffix;
            }
        }

        #endregion Static methods
    }
}
