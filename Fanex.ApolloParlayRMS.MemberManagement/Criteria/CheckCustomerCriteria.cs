// --------------------------------------------------------------------------
// <copyright file="CheckCustomerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.9">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.MemberManagement.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get customers criteria by user name or level.
    /// </summary>
    public class CheckCustomerCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool IsExists { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_CustomerCheckExisting";
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