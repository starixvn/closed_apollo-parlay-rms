// --------------------------------------------------------------------------
// <copyright file="MemberManagementRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Management.Repositories.Implements
{
    using System.Collections;
    using Apollo.ParlayRMS.Core.Repositories;
    using Apollo.ParlayRMS.Management.Criteria;
    using Apollo.ParlayRMS.Management.Models;
    using Apollo.ParlayRMS.Management.Repositories.Interfaces;
    using Fanex.Data;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Member Management Repository.
    /// </summary>
    public class MemberManagementRepository : BaseRepository<Customer>, IMemberManagementRepository
    {
        /// <summary>
        /// Insert a new customer to the database.
        /// </summary>
        /// <param name="criteria">The criteria contains data of new customer.</param>
        /// <returns>CommandResult object indicates whether the insert process is successful or not.</returns>
        public ICommandResult Insert(InsertCustomerCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                ////criteria.CustId = Convert.ToInt32(db.GetParameterValue("CustId"));

                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Update the information of a customer.
        /// </summary>
        /// <param name="criteria">The criteria contains data of the customer.</param>
        /// <returns>CommandResult object indicates whether the update process is successful or not.</returns>
        public ICommandResult Update(ICriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Checks whether a customer is existing or not.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>Whether a customer is existing or not.</returns>
        public bool CheckCustomer(ICriteria criteria)
        {
            bool isExist = false;

            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();

                db.ExecuteNonQuery(param);
                isExist = (bool)param["isexists"];
            }

            return isExist;
        }
    }
}