// --------------------------------------------------------------------------
// <copyright file="TopWinnerList.cs" company="Nexcel Solutions Vietnam">
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
    /// Top winner list model.
    /// </summary>
    [Serializable]
    public class TopWinnerList
    {
        public TopWinnerList()
        {
            this.Total = new TopWinner();
            this.TopWinners = new List<TopWinner>();
        }

        public TopWinner Total { get; set; }

        public List<TopWinner> TopWinners { get; set; }

        /// <summary>
        /// Adds the specified object's data to current object.
        /// </summary>
        /// <param name="obj">The object which its data will be added to the current object.</param>
        public void Add(TopWinner obj)
        {
            this.Total.Stake += obj.Stake;
            this.Total.WinLoss += obj.WinLoss;
            this.Total.CompanyWinLoss += obj.CompanyWinLoss;
            this.Total.StakeBaseRM += obj.StakeBaseRM;
            this.Total.WinLossBaseRM += obj.WinLossBaseRM;
            this.Total.CompanyWinLossBaseRM += obj.CompanyWinLossBaseRM;
            this.Total.BetCount += obj.BetCount;
            this.TopWinners.Add(obj);
        }
    }
}