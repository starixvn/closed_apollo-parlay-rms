// --------------------------------------------------------------------------
// <copyright file="DailyWinLossItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.01.02">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Models.WinLoss
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Daily win loss model.
    /// </summary>
    [Serializable]
    public class DailyWinLossItem
    {
        public DailyWinLossItem()
        {
            this.Total = new WinLossItem();
            this.ChildItems = new List<WinLossItem>();
        }

        public WinLossItem Total { get; set; }

        public List<WinLossItem> ChildItems { get; set; }

        /// <summary>
        /// Adds the specified object's data to the current object.
        /// </summary>
        /// <param name="obj">The object that needs to be added to the current object.</param>
        public void Add(WinLossItem obj)
        {
            if (obj != null)
            {
                this.Total.TicketWon += obj.TicketWon;
                this.Total.TicketLose += obj.TicketLose;
                this.Total.StakeWon += obj.StakeWon;
                this.Total.StakeLose += obj.StakeLose;
                this.Total.MemberWon += obj.MemberWon;
                this.Total.MemberLose += obj.MemberLose;
                this.Total.OurPTWon += obj.OurPTWon;
                this.Total.OurPTLose += obj.OurPTLose;
                this.Total.OurPTNetWinLoss += obj.OurPTNetWinLoss;
                this.ChildItems.Add(obj);
            }
        }
    }
}