// --------------------------------------------------------------------------
// <copyright file="GetCustomerByUserNameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.17">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get customer by user name.
    /// </summary>
    public class GetCustomerByUserNameCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public int CustId { get; set; }

        public int RoleId { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_CustomerSelInfoByUsername";
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