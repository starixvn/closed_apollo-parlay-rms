// --------------------------------------------------------------------------
// <copyright file="MemberTotalOutstanding.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Models.TotalOutstanding
{
    using System;

    /// <summary>
    /// Member total outstanding model.
    /// </summary>
    [Serializable]
    public class MemberTotalOutstanding
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public DateTime TransDate { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal StakePT { get; set; }

        public decimal MaxPayoutPT { get; set; }

        public string Combo { get; set; }
    }
}