// --------------------------------------------------------------------------
// <copyright file="CustomerRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.17">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Repositories
{
    using System;
    using Fanex.ApolloParlayRMS.Core.Criteria;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.Data;

    /// <summary>
    /// Customer information repository.
    /// </summary>
    public class CustomerRepository
    {
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