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
    using Fanex.Data.DNAide;

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

            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey())) 
            {
                var param = criteria.GetSpParams();

                db.ExecuteNonQuery(param);

                custInfo.UserName = criteria.UserName;
                custInfo.CustId = (int)param["custid"];
                custInfo.RoleId = (int)param["roleid"];
            }

            return custInfo;
        }
    }
}