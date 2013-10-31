// --------------------------------------------------------------------------
// <copyright file="AgentTotalOutstandingList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Models.TotalOutstanding
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// List of total outstanding items of agent.
    /// </summary>
    [Serializable]
    public class AgentTotalOutstandingList
    {
        public AgentTotalOutstandingList()
        {
            this.Records = new List<AgentTotalOutstanding>();
        }

        public List<AgentTotalOutstanding> Records { get; set; }

        public AgentTotalOutstanding Total { get; set; }
    }
}