// --------------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.17">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Services
{
    using Fanex.ApolloParlayRMS.Core.Criteria;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Repositories;

    /// <summary>
    /// Customer service.
    /// </summary>
    public static class CustomerService
    {
        private static readonly CustomerRepository CustomerRepositoryInst;

        static CustomerService()
        {
            CustomerRepositoryInst = new CustomerRepository();
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>CustomerInfo contains customer user name and customer ID.</returns>
        public static CustomerInfo GetCustomerInfo(GetCustomerByUsernameCriteria criteria)
        {
            return CustomerRepositoryInst.GetCustomerInfo(criteria);
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="username">The user name of the member.</param>
        /// <returns>The information of the customer.</returns>
        public static CustomerInfo GetCustomerInfo(string username)
        {
            var criteria = new GetCustomerByUsernameCriteria
            {
                Username = username
            };

            return CustomerRepositoryInst.GetCustomerInfo(criteria);
        }
    }
}
