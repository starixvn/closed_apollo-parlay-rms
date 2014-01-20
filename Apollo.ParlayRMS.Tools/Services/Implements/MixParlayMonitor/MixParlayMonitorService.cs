// --------------------------------------------------------------------------
// <copyright file="MixParlayMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.21">Created</change>
// </history>

namespace Apollo.ParlayRMS.Tools.Services.Implements.MixParlayMonitor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.ParlayRMS.Tools.Criteria.MixParlayMonitor;
    using Apollo.ParlayRMS.Tools.Models.MixParlayMonitor;
    using Apollo.ParlayRMS.Tools.Services.Interfaces.MixParlayMonitor;
    using Fanex.NAP.Core.Service.Impl;

    /// <summary>
    /// Mix parlay monitor service.
    /// </summary>
    public class MixParlayMonitorService : BaseService, IMixParlayMonitorService
    {
        /// <summary>
        /// Gets list of bets of the unsettled parlay tickets.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of BetItem objects represent the bet list of tickets satisfied the passed query conditions.</returns>
        public IEnumerable<ParlayBetItem> GeMixParlayData(GetMonitorParlayBetListCriteria criteria)
        {
            IEnumerable<ParlayBetItem> betList = this.GetData<ParlayBetItem>(criteria);

            // Use for testing purpose.
            ////if (!betList.Any())
            ////{
            ////    betList = betList.Union(CreateTestData(criteria));
            ////}

            return betList;
        }

        /// <summary>
        /// Creates the test data.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of BetItem objects.</returns>
        private IEnumerable<ParlayBetItem> CreateTestData(GetMonitorParlayBetListCriteria criteria)
        {
            Random rnd = new Random();
            var count = rnd.Next(0, 5);
            var list = new List<ParlayBetItem>();
            for (int i = count; i > 0; i--)
            {
                var combo = rnd.Next(2, 6);
                var level = rnd.Next(0, 10);
                var stake = Convert.ToDecimal(rnd.NextDouble() * 100);
                var odds = Convert.ToDecimal(rnd.NextDouble() * 100);
                var refno = rnd.Next(1000000, 9999999);

                if (criteria.Selection.Contains(combo.ToString())
                    && criteria.Level.Contains(level.ToString())
                    && (stake * odds > criteria.MaxPayout))
                {
                    list.Add(new ParlayBetItem
                    {
                        Combo = combo,
                        Level = level,
                        Username = "test" + i,
                        Stake = stake,
                        Odds = odds,
                        MaxPayout = stake * odds,
                        RefNo = refno,
                        TransDate = DateTime.Now,
                        TransId = criteria.Max_TransId + i
                    });
                }
            }

            return list;
        }
    }
}