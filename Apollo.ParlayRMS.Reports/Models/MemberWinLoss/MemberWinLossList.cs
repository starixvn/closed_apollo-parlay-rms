// --------------------------------------------------------------------------
// <copyright file="MemberWinLossList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.01.02">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Models.MemberWinLoss
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Win Loss list model.
    /// </summary>
    [Serializable]
    public class MemberWinLossList
    {
        public MemberWinLossList()
        {
            this.Total = new MemberWinLossItem();
            this.ChildItems = new List<MemberWinLossItem>();
        }

        public MemberWinLossItem Total { get; set; }

        public List<MemberWinLossItem> ChildItems { get; set; }
    }
}