// --------------------------------------------------------------------------
// <copyright file="TopWinner.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.14">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Models.TopWinners
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Top winners model.
    /// </summary>
    [Serializable]
    public class TopWinner
    {
        public TopWinner()
        {
            this.ChildItems = new List<TopWinner>();
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Currency { get; set; }

        public decimal Stake { get; set; }

        public decimal WinLoss { get; set; }

        public decimal CompanyWinLoss { get; set; }

        public decimal StakeBaseRM { get; set; }

        public decimal WinLossBaseRM { get; set; }

        public decimal CompanyWinLossBaseRM { get; set; }

        public int BetCount { get; set; }

        public decimal AvgOdds { get; set; }

        public byte Level { get; set; }

        public List<TopWinner> ChildItems { get; set; }

        /// <summary>
        /// Adds the specified object's data to current object.
        /// </summary>
        /// <param name="obj">The object which its data will be added to the current object.</param>
        public void Add(TopWinner obj)
        {
            this.Stake += obj.Stake;
            this.WinLoss += obj.WinLoss;
            this.CompanyWinLoss += obj.CompanyWinLoss;
            this.StakeBaseRM += obj.StakeBaseRM;
            this.WinLossBaseRM += obj.WinLossBaseRM;
            this.CompanyWinLossBaseRM += obj.CompanyWinLossBaseRM;
            this.BetCount += obj.BetCount;
            this.ChildItems.Add(obj);
        }
    }
}