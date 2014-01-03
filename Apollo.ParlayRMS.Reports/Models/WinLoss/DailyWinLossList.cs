// --------------------------------------------------------------------------
// <copyright file="DailyWinLossList.cs" company="Nexcel Solutions Vietnam">
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
    public class DailyWinLossList
    {
        public DailyWinLossList()
        {
            this.Total = new WinLossItem();
            this.ChildItems = new List<DailyWinLossItem>();
        }

        public WinLossItem Total { get; set; }

        public List<DailyWinLossItem> ChildItems { get; set; }

        /// <summary>
        /// Adds the specified object's data to the current object.
        /// </summary>
        /// <param name="obj">The object that needs to be added to the current object.</param>
        public void Add(DailyWinLossItem obj)
        {
            if (obj != null)
            {
                this.Total.TicketWon += obj.Total.TicketWon;
                this.Total.TicketLose += obj.Total.TicketLose;
                this.Total.StakeWon += obj.Total.StakeWon;
                this.Total.StakeLose += obj.Total.StakeLose;
                this.Total.MemberWon += obj.Total.MemberWon;
                this.Total.MemberLose += obj.Total.MemberLose;
                this.Total.OurPTWon += obj.Total.OurPTWon;
                this.Total.OurPTLose += obj.Total.OurPTLose;
                this.Total.OurPTNetWinLoss += obj.Total.OurPTNetWinLoss;
                this.ChildItems.Add(obj);
            }
        }
    }
}