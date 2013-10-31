// --------------------------------------------------------------------------
// <copyright file="SimilarBetItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Models.SimilarBets
{
    /// <summary>
    /// Similar bet item.
    /// </summary>
    public class SimilarBetItem
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public string UserName { get; set; }

        public int Level { get; set; }

        public decimal MaxPayout { get; set; }

        public int SimilarCombo { get; set; }
    }
}