// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.9">Created</change>
// </history>

namespace Apollo.ParlayRMS.MemberManagement.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update customer criteria.
    /// </summary>
    public class UpdateCustomerCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int Level { get; set; }

        public int CurrentUserId { get; set; }

        public string Ip { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_CustomerUpd";
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