// --------------------------------------------------------------------------
// <copyright file="TotalOutstandingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Services.Implements.TotalOutstanding
{
    using System;
    using System.Linq;
    using Fanex.ApolloParlayRMS.Core.Services.Implements;
    using Fanex.ApolloParlayRMS.Tools.Criteria.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Models.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Repositories.Implements.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Repositories.Interfaces.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.TotalOutstanding;

    /// <summary>
    /// Total outstanding service.
    /// </summary>
    public class TotalOutstandingService : BasedService, ITotalOutstandingService
    {
        private readonly ITotalOutstandingRepository _totalOutstandingRepository;

        public TotalOutstandingService() :
            this(null)
        {
        }

        public TotalOutstandingService(ITotalOutstandingRepository totalOutstandingRepository)
        {
            this._totalOutstandingRepository = totalOutstandingRepository ?? new TotalOutstandingRepository();
        }

        /// <summary>
        /// Gets total outstanding of agent.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>
        /// AgentTotalOutstandingList object contains list of AgentTotalOutstanding objects and Total property which
        /// summarize the Stake After PT, Maximum Payout After PT and Total Bets of all items.
        /// </returns>
        public AgentTotalOutstandingList GetTotalOutstandingOfAgent(GetTotalOutstandingOfAgentCriteria criteria)
        {
            var agentTotalOutstanding = new AgentTotalOutstandingList();
            try
            {
                var records = this._totalOutstandingRepository.GetTotalOutstandingOfAgent(criteria).ToList();
                var total = new AgentTotalOutstanding();

                records.ForEach(obj =>
                {
                    total.StakePT += obj.StakePT;
                    total.MaxPayoutPT += obj.MaxPayoutPT;
                    total.BetCount += obj.BetCount;
                });

                agentTotalOutstanding = new AgentTotalOutstandingList
                {
                    Records = records,
                    Total = total
                };
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return agentTotalOutstanding;
        }

        /// <summary>
        /// Gets total outstanding of member.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>
        /// MemberTotalOutstandingList object contains list of MemberTotalOutstanding objects and Total property which
        /// summarize the Stake After PT and Maximum Payout After PT of all items.
        /// </returns>
        public MemberTotalOutstandingList GetTotalOutstandingOfMember(GetTotalOutstandingOfMemCriteria criteria)
        {
            var memberTotalOutstanding = new MemberTotalOutstandingList();
            try
            {
                var records = this._totalOutstandingRepository.GetTotalOutstandingOfMember(criteria).ToList();
                var total = new MemberTotalOutstanding();

                records.ForEach(obj =>
                {
                    total.StakePT += obj.StakePT;
                    total.MaxPayoutPT += obj.MaxPayoutPT;
                });

                memberTotalOutstanding = new MemberTotalOutstandingList
                {
                    Records = records,
                    Total = total
                };
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return memberTotalOutstanding;
        }
    }
}