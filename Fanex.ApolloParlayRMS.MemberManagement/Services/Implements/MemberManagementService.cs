// --------------------------------------------------------------------------
// <copyright file="MemberManagementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.MemberManagement.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.ApolloParlayRMS.Core.ExcelUtility;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Core.Services.Implements;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.MemberManagement.Criteria;
    using Fanex.ApolloParlayRMS.MemberManagement.Models;
    using Fanex.ApolloParlayRMS.MemberManagement.Repositories.Implements;
    using Fanex.ApolloParlayRMS.MemberManagement.Repositories.Interfaces;
    using Fanex.ApolloParlayRMS.MemberManagement.Services.Interfaces;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Member Management Repository.
    /// </summary>
    public class MemberManagementService : BasedService, IMemberManagementService
    {
        private static readonly string MemberManagementSessionKey = "Fanex.ApolloParlayRMS. MemberManagement. MemberManagement";
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
            try
            {
                if (!criteria.IsLoadNew)
                {
                    records = SessionManager.Get<IEnumerable<Customer>>(MemberManagementSessionKey);
                }

                if (records == null)
                {
                    records = this._memberManagementRepository.Fetch(criteria);
                    SessionManager.Set<IEnumerable<Customer>>(MemberManagementSessionKey, records);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
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
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._memberManagementRepository.Insert(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return this.InternalError;
        }

        /// <summary>
        /// Updates the level of an existing customer.
        /// </summary>
        /// <param name="criteria">The criteria that contain the new level and the ID of customer.</param>
        /// <returns>CommandResult indicates whether the level of customer is updated successfully or not.</returns>
        public ICommandResult UpdateCustomerLevel(UpdateCustomerCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._memberManagementRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return this.InternalError;
        }

        /// <summary>
        /// Updates the status of an existing customer.
        /// </summary>
        /// <param name="criteria">The criteria that contain the status and ID of customer.</param>
        /// <returns>CommandResult indicates whether the status of customer is updated successfully or not.</returns>
        public ICommandResult UpdateCustomerStatus(UpdateCustomerStatusCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._memberManagementRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return this.InternalError;
        }

        /// <summary>
        /// Checks the customer exists or not.
        /// </summary>
        /// <param name="criteria">The criteria contain the information using for querying data purpose.</param>
        /// <returns>Boolean value indicates whether the customer is existing or not.</returns>
        public bool CheckCustomer(CheckCustomerCriteria criteria)
        {
            bool isExisting = false;
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                if (criteria.IsValid())
                {
                    isExisting = this._memberManagementRepository.CheckCustomer(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return isExisting;
        }

        /// <summary>
        /// Changes the level of an existing customer, or add a new one if the passed user name does not exist.
        /// </summary>
        /// <param name="criteria">The criteria contain the identification data and new level value of the customer.</param>
        /// <returns>CommandResult indicates whether the level of customer is updated successfully or not.</returns>
        public ICommandResult ChangeCustomerLevel(ChangeCustomerLevelCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();
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

                    if (updateCustomerCriteria.IsValid() && logCriteria.IsValid())
                    {
                        return this.UpdateCustomerLevel(updateCustomerCriteria);
                    }
                }
                else
                {
                    InsertCustomerCriteria insertCustomerCriteria = new InsertCustomerCriteria
                    {
                        UserName = criteria.UserName,
                        Level = criteria.Level
                    };

                    if (insertCustomerCriteria.IsValid() && logCriteria.IsValid())
                    {
                        return this.InsertCustomer(insertCustomerCriteria);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return this.InternalError;
        }

        /// <summary>
        /// Exports the customer list to Excel file.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="templatePath">The template's path.</param>
        public void ExportCustomerList(GetCustomersCriteria criteria, string templatePath)
        {
            try
            {
                string fileName = string.Format("{0}.xls", Lang.SpecialCustomersFileName);
                var customerList = this.GetCustomers(criteria);
                var writer = new ExcelTemplateWriter(templatePath, fileName, Lang.SpecialCustomers);
                int counter = 0;

                // Copy title and header.
                writer.CopyRows(0, 4);

                // Prepare content
                var records = customerList.Select(obj => new
                {
                    No = ++counter,
                    UserName = obj.UserName,
                    Level = (obj.Level == 0) ? Lang.None : string.Format("{0} {1}", Lang.Level, obj.Level)
                });

                writer.WriteRows(4, records);
                writer.SyncColumnsWidth(3);
                writer.Render();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}