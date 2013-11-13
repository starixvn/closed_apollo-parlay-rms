// --------------------------------------------------------------------------
// <copyright file="MemberTotalOutstandingList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Models.TotalOutstanding
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// List of total outstanding items of member.
    /// </summary>
    [Serializable]
    public class MemberTotalOutstandingList
    {
        public MemberTotalOutstandingList()
        {
            this.Records = new List<MemberTotalOutstanding>();
            this.Total = new MemberTotalOutstanding();
        }

        public List<MemberTotalOutstanding> Records { get; set; }

        public MemberTotalOutstanding Total { get; set; }
    }
}