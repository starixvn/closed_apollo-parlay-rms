// --------------------------------------------------------------------------
// <copyright file="AgentTotalOutstanding.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Apollo.ParlayRMS.Tools.Models.TotalOutstanding
{
    using System;

    /// <summary>
    /// Agent total outstanding model.
    /// </summary>
    [Serializable]
    public class AgentTotalOutstanding
    {
        public long CustId { get; set; }

        public string Username { get; set; }

        public decimal StakePT { get; set; }

        public decimal MaxPayoutPT { get; set; }

        public int BetCount { get; set; }
    }
}