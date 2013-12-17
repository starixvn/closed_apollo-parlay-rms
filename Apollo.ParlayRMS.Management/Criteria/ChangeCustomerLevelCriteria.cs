// --------------------------------------------------------------------------
// <copyright file="ChangeCustomerLevelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.9">Created</change>
// </history>

namespace Apollo.ParlayRMS.Management.Criteria
{
    /// <summary>
    /// Change Customer Level Criteria.
    /// </summary>
    public class ChangeCustomerLevelCriteria
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public int Level { get; set; }
    }
}