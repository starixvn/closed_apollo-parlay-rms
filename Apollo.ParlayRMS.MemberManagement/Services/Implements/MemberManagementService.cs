// --------------------------------------------------------------------------
// <copyright file="MemberManagementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.MemberManagement.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.ParlayRMS.Core.ExcelUtility;
    using Apollo.ParlayRMS.Core.Resources;
    using Apollo.ParlayRMS.Core.Services;
    using Apollo.ParlayRMS.Core.Web;
    using Apollo.ParlayRMS.MemberManagement.Criteria;
    using Apollo.ParlayRMS.MemberManagement.Models;
    using Apollo.ParlayRMS.MemberManagement.Repositories.Implements;
    using Apollo.ParlayRMS.MemberManagement.Repositories.Interfaces;
    using Apollo.ParlayRMS.MemberManagement.Services.Interfaces;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Member Management Repository.
    /// </summary>
    public class MemberManagementService : BaseService, IMemberManagementService
    {
        private static readonly string MemberManagementSessionKey = "Apollo.ParlayRMS. MemberManagement. MemberManagement";
        private readonly IMemberManagementRepository _memberManagementRepository;

        public MemberManagementService() :
            this(null)
        {
        }

        public MemberManagementService(IMemberManagementRepository memberManagementRepository)
        {
            this._memberManagementRepository = memberManagementRepository ?? new MemberManagementRepository();
        }

        /// <summary>
        /// Gets the list of customers.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of Customer objects.</returns>
        public IEnumerable<Customer> GetCustomers(GetCustomersCriteria criteria)
        {
            IEnumerable<Customer> records = null;

            if (!criteria.IsLoadNew)
            {
                records = SessionManager.Get<IEnumerable<Customer>>(MemberManagementSessionKey);
            }

            if (records == null)
            {
                records = this._memberManagementRepository.Fetch(criteria);
                SessionManager.Set<IEnumerable<Customer>>(MemberManagementSessionKey, records);
            }

            return records;
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="criteria">The criteria that contain data of the new customer.</param>
        /// <returns>CommandResult indicates whether the new customer is inserted successfully or not.</returns>
        public ICommandResult InsertCustomer(InsertCustomerCriteria criteria)
        {
            criteria.CurrentUserId = this.ServiceContext.UserId;
            criteria.Ip = this.ServiceContext.Ip;

            return this._memberManagementRepository.Insert(criteria);
        }

        /// <summary>
        /// Updates the level of an existing customer.
        /// </summary>
        /// <param name="criteria">The criteria that contain the new level and the ID of customer.</param>
        /// <returns>CommandResult indicates whether the level of customer is updated successfully or not.</returns>
        public ICommandResult UpdateCustomerLevel(UpdateCustomerCriteria criteria)
        {
            criteria.CurrentUserId = this.ServiceContext.UserId;
            criteria.Ip = this.ServiceContext.Ip;

            return this._memberManagementRepository.Update(criteria);
        }

        /// <summary>
        /// Checks the customer exists or not.
        /// </summary>
        /// <param name="criteria">The criteria contain the information using for querying data purpose.</param>
        /// <returns>Boolean value indicates whether the customer is existing or not.</returns>
        public bool CheckCustomer(CheckCustomerCriteria criteria)
        {
            return this._memberManagementRepository.CheckCustomer(criteria);
        }

        /// <summary>
        /// Changes the level of an existing customer, or add a new one if the passed user name does not exist.
        /// </summary>
        /// <param name="criteria">The criteria contain the identification data and new level value of the customer.</param>
        /// <returns>CommandResult indicates whether the level of customer is updated successfully or not.</returns>
        public ICommandResult ChangeCustomerLevel(ChangeCustomerLevelCriteria criteria)
        {
            CheckCustomerCriteria checkCustomerCriteria = new CheckCustomerCriteria
            {
                CustId = criteria.CustId,
                IsExists = false
            };

            if (this.CheckCustomer(checkCustomerCriteria))
            {
                UpdateCustomerCriteria updateCustomerCriteria = new UpdateCustomerCriteria
                {
                    CustId = criteria.CustId,
                    Level = criteria.Level
                };

                return this.UpdateCustomerLevel(updateCustomerCriteria);
            }
            else
            {
                InsertCustomerCriteria insertCustomerCriteria = new InsertCustomerCriteria
                {
                    Username = criteria.Username,
                    Level = criteria.Level
                };

                return this.InsertCustomer(insertCustomerCriteria);
            }
        }

        /// <summary>
        /// Exports the customer list to Excel file.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="templatePath">The template's path.</param>
        public void ExportCustomerList(GetCustomersCriteria criteria, string templatePath)
        {
            string fileName = Lang.SpecialCustomersFileName;
            var customerList = this.GetCustomers(criteria);
            var writer = new ExcelTemplateWriter(templatePath, fileName, Lang.SpecialCustomers);
            int counter = 0;

            // Copy title and header.
            writer.CopyRows(0, 4);

            // Prepare content
            var records = customerList.Select(obj => new
            {
                No = ++counter,
                Username = obj.Username,
                Level = (obj.Level == 0) ? Lang.None : string.Format("{0} {1}", Lang.Level, obj.Level)
            });

            writer.WriteRows(4, records);
            writer.SyncColumnsWidth(3);
            writer.Render();
        }
    }
}