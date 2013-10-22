// --------------------------------------------------------------------------
// <copyright file="GetCustomersCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get customers criteria by user name or level.
    /// </summary>
    public class GetCustomersCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public int? Level { get; set; }

        [SpParam(Ignored = true)]
        public bool IsLoadNew { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "RMS_CustomerSel";
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