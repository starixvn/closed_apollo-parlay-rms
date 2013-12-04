// --------------------------------------------------------------------------
// <copyright file="BalanceUpCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Core.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// BalanceUpCriteria: criteria for getting balance up day and move bet day.
    /// </summary>
    public class BalanceUpCriteria : CriteriaBase
    {
        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_SelBalanceUpDay";
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