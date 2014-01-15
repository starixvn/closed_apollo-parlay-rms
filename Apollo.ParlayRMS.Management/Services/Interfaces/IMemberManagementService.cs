// --------------------------------------------------------------------------
// <copyright file="IMemberManagementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Management.Services.Interfaces
{
    using System.Collections.Generic;
    using Apollo.ParlayRMS.Management.Criteria;
    using Apollo.ParlayRMS.Management.Models;
    using Fanex.NAP.Framework.Command;

    public interface IMemberManagementService
    {
        /// <summary>
        /// Gets the list of customers.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of Customer objects.</returns>
        IEnumerable<Customer> GetCustomers(GetCustomersCriteria criteria);

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="criteria">The criteria that contain data of the new customer.</param>
        /// <returns>CommandResult indicates whether the new customer is inserted successfully or not.</returns>
        ICommandResult InsertCustomer(InsertCustomerCriteria criteria);

        /// <summary>
        /// Updates the level of an existing customer.
        /// </summary>
        /// <param name="criteria">The criteria that contain the new level and the ID of customer.</param>
        /// <returns>CommandResult indicates whether the level of customer is updated successfully or not.</returns>
        ICommandResult UpdateCustomerLevel(UpdateCustomerCriteria criteria);

        /// <summary>
        /// Checks the customer exists or not.
        /// </summary>
        /// <param name="criteria">The criteria contain the information using for querying data purpose.</param>
        /// <returns>Boolean value indicates whether the customer is existing or not.</returns>
        bool CheckCustomer(CheckCustomerCriteria criteria);

        /// <summary>
        /// Changes the level of an existing customer, or add a new one if the passed user name does not exist.
        /// </summary>
        /// <param name="criteria">The criteria contain the identification data and new level value of the customer.</param>
        /// <returns>CommandResult indicates whether the level of customer is updated successfully or not.</returns>
        ICommandResult ChangeCustomerLevel(ChangeCustomerLevelCriteria criteria);

        /// <summary>
        /// Exports the customer list to Excel file.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="templatePath">The template's path.</param>
        void ExportCustomerList(GetCustomersCriteria criteria, string templatePath);
    }
}