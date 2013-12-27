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

    /// <summary>
    /// Top winner's detail model.
    /// </summary>
    [Serializable]
    public class TopWinnerDetails
    {
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
    }
}