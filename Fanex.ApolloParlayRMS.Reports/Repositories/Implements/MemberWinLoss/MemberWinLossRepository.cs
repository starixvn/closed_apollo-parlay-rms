// --------------------------------------------------------------------------
// <copyright file="MemberWinLossRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.16">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Repositories.Implements.MemberWinLoss
{
    using System;
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Reports.Criteria.MemberWinLoss;
    using Fanex.ApolloParlayRMS.Reports.Models.MemberWinLoss;
    using Fanex.ApolloParlayRMS.Reports.Repositories.Interfaces.MemberWinLoss;
    using Fanex.Data;

    /// <summary>
    /// Member win loss repository.
    /// </summary>
    public class MemberWinLossRepository : IMemberWinLossRepository
    {
        /// <summary>
        /// Gets the member win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of WinLossItem objects.</returns>
        public IEnumerable<MemberWinLossItem> GetMemberWinLossData(GetMemberWinLossCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                return db.Query<MemberWinLossItem>(param);
            }
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>Customer information.</returns>
        public CustomerInfo GetCustomerInfo(GetCustomerByUserNameCriteria criteria)
        {
            CustomerInfo custInfo = new CustomerInfo();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                custInfo.UserName = criteria.UserName;
                custInfo.CustId = Convert.ToInt64(db.GetParameterValue("custid"));
                custInfo.RoleId = Convert.ToInt32(db.GetParameterValue("roleid"));
            }

            return custInfo;
        }
    }
}