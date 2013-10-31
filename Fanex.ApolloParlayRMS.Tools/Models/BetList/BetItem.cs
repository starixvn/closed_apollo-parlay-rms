// <copyright file="BetItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Models.BetList
{
    using System;

    /// <summary>
    /// Bet item model.
    /// </summary>
    [Serializable]
    public class BetItem
    {
        public long TransId { get; set; }

        public int BetType { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }
        
        public decimal Hdp1 { get; set; }

        public decimal Hdp2 { get; set; }

        public string BetTeam { get; set; }

        public decimal Odds { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }

        public bool LiveIndicator { get; set; }

        public int LeagueId { get; set; }

        public int SportType { get; set; }

        public bool IsNeutral { get; set; }
        
        public int MatchId { get; set; }

        public string MatchCode { get; set; }

        public string BetStatus { get; set; }
    }
}