// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Core.Models
{
    /// <summary>
    /// Contains customer's role and Id.
    /// </summary>
    public class CustomerInfo
    {
        public long CustId { get; set; }

        public int RoleId { get; set; }

        public string Username { get; set; }
    }
}