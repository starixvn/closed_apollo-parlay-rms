// --------------------------------------------------------------------------
// <copyright file="ParlayBetItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.21">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Models.MixParlayMonitor
{
    using System;

    /// <summary>
    /// Bet item model.
    /// </summary>
    public class ParlayBetItem
    {
        public DateTime TransDate { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public string Username { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal MaxPayout { get; set; }

        public int Combo { get; set; }

        public int Level { get; set; }
    }
}