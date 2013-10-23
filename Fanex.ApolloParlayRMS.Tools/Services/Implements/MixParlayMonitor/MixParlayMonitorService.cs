// --------------------------------------------------------------------------
// <copyright file="MixParlayMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.21">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Services.Implements.MixParlayMonitor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Core.Services.Implements;
    using Fanex.ApolloParlayRMS.Tools.Criteria.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Models.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Repositories.Implements.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Repositories.Interfaces.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.MixParlayMonitor;

    /// <summary>
    /// Mix parlay monitor service.
    /// </summary>
    public class MixParlayMonitorService : BasedService, IMixParlayMonitorService
    {
        private readonly IMixParlayMonitorRepository _mixParlayMonitorRepository;

        public MixParlayMonitorService() :
            this(null)
        {
        }

        public MixParlayMonitorService(IMixParlayMonitorRepository mixParlayMonitorRepository)
        {
            this._mixParlayMonitorRepository = mixParlayMonitorRepository ?? new MixParlayMonitorRepository();
        }

        /// <summary>
        /// Gets list of bets of the unsettled parlay tickets.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of BetItem objects represent the bet list of tickets satisfied the passed query conditions.</returns>
        public IEnumerable<BetItem> GetMemberWinLossData(GetMonitorParlayBetListCriteria criteria)
        {
            IEnumerable<BetItem> betList = null;
            try
            {
                betList = this._mixParlayMonitorRepository.GetMonitorParlayBetListData(criteria);

                // Use for testing purpose.
                betList = CreateTestData(criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return betList;
        }

        /// <summary>
        /// Creates the test data.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of BetItem objects.</returns>
        private IEnumerable<BetItem> CreateTestData(GetMonitorParlayBetListCriteria criteria)
        {
            Random rnd = new Random();
            var count = rnd.Next(0, 5);
            var list = new List<BetItem>();
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
                    list.Add(new BetItem
                    {
                        Combo = combo,
                        Level = level,
                        UserName = "test",
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