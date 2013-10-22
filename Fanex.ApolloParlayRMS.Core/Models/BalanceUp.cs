// --------------------------------------------------------------------------
// <copyright file="BalanceUp.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Models
{
    using System;

    /// <summary>
    /// BalanceUp: getting balance up day and move bet day.
    /// </summary>
    public class BalanceUp
    {
        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}