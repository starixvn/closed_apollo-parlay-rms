// --------------------------------------------------------------------------
// <copyright file="TopWinnerDetails.cs" company="Nexcel Solutions Vietnam">
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
    /// Top winner's detail model.
    /// </summary>
    [Serializable]
    public class TopWinnerDetails
    {
        public TopWinnerDetails()
        {
            this.ChildItems = new List<TopWinnerDetails>();
        }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public byte SportType { get; set; }

        public decimal Stake { get; set; }

        public decimal WinLoss { get; set; }

        public decimal CompanyWinLoss { get; set; }

        public decimal StakeBaseRM { get; set; }

        public decimal WinLossBaseRM { get; set; }

        public decimal CompanyWinLossBaseRM { get; set; }

        public byte Combo { get; set; }

        public decimal Odds { get; set; }

        public List<TopWinnerDetails> ChildItems { get; set; }

        /// <summary>
        /// Adds the specified object's data to current object.
        /// </summary>
        /// <param name="obj">The object which its data will be added to the current object.</param>
        public void Add(TopWinnerDetails obj)
        {
            this.Stake += obj.Stake;
            this.WinLoss += obj.WinLoss;
            this.CompanyWinLoss += obj.CompanyWinLoss;
            this.StakeBaseRM += obj.StakeBaseRM;
            this.WinLossBaseRM += obj.WinLossBaseRM;
            this.CompanyWinLossBaseRM += obj.CompanyWinLossBaseRM;
            this.ChildItems.Add(obj);
        }
    }
}