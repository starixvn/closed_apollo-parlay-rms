// --------------------------------------------------------------------------
// <copyright file="MemberWinLossItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.16">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Models.MemberWinLoss
{
    using System;

    /// <summary>
    /// Win Loss model.
    /// </summary>
    [Serializable]
    public class MemberWinLossItem
    {
        public string Month { get; set; }

        public string Username { get; set; }

        public int TicketWon { get; set; }

        public int TicketLose { get; set; }

        public decimal StakeWon { get; set; }

        public decimal StakeLose { get; set; }

        public decimal MemberWon { get; set; }

        public decimal MemberLose { get; set; }

        public decimal OurPTWon { get; set; }

        public decimal OurPTLose { get; set; }

        public decimal OurPTNetWinLoss { get; set; }

        public bool IsSutotalRow { get; set; }

        public int NumOfSubrows { get; set; }

        /// <summary>
        /// Adds the specified object's data to the current object.
        /// </summary>
        /// <param name="obj">The object that needs to be added to the current object.</param>
        public void Add(MemberWinLossItem obj)
        {
            if (obj != null)
            {
                this.TicketWon += obj.TicketWon;
                this.TicketLose += obj.TicketLose;
                this.StakeWon += obj.StakeWon;
                this.StakeLose += obj.StakeLose;
                this.MemberWon += obj.MemberWon;
                this.MemberLose += obj.MemberLose;
                this.OurPTWon += obj.OurPTWon;
                this.OurPTLose += obj.OurPTLose;
                this.OurPTNetWinLoss += obj.OurPTNetWinLoss;
                this.NumOfSubrows++;
                ////this.ChildItems.Add(obj);
            }
        }
    }
}