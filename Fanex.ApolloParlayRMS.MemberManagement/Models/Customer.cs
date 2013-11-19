// --------------------------------------------------------------------------
// <copyright file="Customer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.MemberManagement.Models
{
    using System;

    /// <summary>
    /// Customer Model.
    /// </summary>
    [Serializable]
    public class Customer
    {
        public int CustId { get; set; }

        public byte Level { get; set; }

        public string Username { get; set; }
    }
}