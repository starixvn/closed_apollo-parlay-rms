// --------------------------------------------------------------------------
// <copyright file="IMemberManagementRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Management.Repositories.Interfaces
{
    using Apollo.ParlayRMS.Management.Criteria;
    using Apollo.ParlayRMS.Management.Models;
    using Fanex.NAP.Framework.Data;

    public interface IMemberManagementRepository : IReadonlyRepository<Customer>
    {
        /// <summary>
        /// Insert a new customer to the database.
        /// </summary>
        /// <param name="criteria">The criteria contains data of new customer.</param>
        /// <returns>CommandResult object indicates whether the insert process is successful or not.</returns>
        ICommandResult Insert(InsertCustomerCriteria criteria);

        /// <summary>
        /// Update the information of a customer.
        /// </summary>
        /// <param name="criteria">The criteria contains data of the customer.</param>
        /// <returns>CommandResult object indicates whether the update process is successful or not.</returns>
        ICommandResult Update(ICriteria criteria);

        /// <summary>
        /// Checks whether a customer is existing or not.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>Whether a customer is existing or not.</returns>
        bool CheckCustomer(ICriteria criteria);
    }
}