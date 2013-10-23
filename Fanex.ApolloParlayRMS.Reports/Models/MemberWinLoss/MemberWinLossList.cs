// --------------------------------------------------------------------------
// <copyright file="MemberWinLossList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.16">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Models.MemberWinLoss
{
    using System.Collections.Generic;

    /// <summary>
    /// List of MemberWinLoss objects.
    /// </summary>
    public class MemberWinLossList
    {
        public MemberWinLossList()
        {
            this.Records = new List<MemberWinLossItem>();
            this.Total = new MemberWinLossItem();
        }

        public List<MemberWinLossItem> Records { get; set; }

        public MemberWinLossItem Total { get; set; }
    }
}