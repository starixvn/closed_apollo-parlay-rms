// --------------------------------------------------------------------------
// <copyright file="SimilarBetList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Models.SimilarBets
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Similar bet list.
    /// </summary>
    public class SimilarBetList
    {
        public int NumOfBets { get; set; }

        public int Combo { get; set; }

        public int Level { get; set; }

        public decimal MaxPayout { get; set; }

        public decimal Odds { get; set; }

        public long RefNo { get; set; }

        public decimal Stake { get; set; }

        public DateTime TransDate { get; set; }

        public long TransId { get; set; }

        public string UserName { get; set; }

        public IEnumerable<SimilarBetItem> Details { get; set; }
    }
}