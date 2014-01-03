// --------------------------------------------------------------------------
// <copyright file="WinLossItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.14">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Models.WinLoss
{
    using System;

    /// <summary>
    /// Win Loss model.
    /// </summary>
    [Serializable]
    public class WinLossItem
    {
        public DateTime WinLossDate { get; set; }

        public int Combo { get; set; }

        public int TicketWon { get; set; }

        public int TicketLose { get; set; }

        public decimal StakeWon { get; set; }

        public decimal StakeLose { get; set; }

        public decimal MemberWon { get; set; }

        public decimal MemberLose { get; set; }

        public decimal OurPTWon { get; set; }

        public decimal OurPTLose { get; set; }

        public decimal OurPTNetWinLoss { get; set; }
    }
}