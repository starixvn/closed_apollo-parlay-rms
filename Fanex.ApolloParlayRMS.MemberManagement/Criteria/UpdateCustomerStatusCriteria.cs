// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.9">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.MemberManagement.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Customer Criteria.
    /// </summary>
    public class UpdateCustomerStatusCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool Monitored { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_CustomerUpdStatus";
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