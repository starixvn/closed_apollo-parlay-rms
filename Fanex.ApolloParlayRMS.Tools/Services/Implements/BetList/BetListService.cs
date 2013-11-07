// --------------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Services.Implements.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.ApolloParlayRMS.Core.Services.Implements;
    using Fanex.ApolloParlayRMS.Tools.Criteria.BetList;
    using Fanex.ApolloParlayRMS.Tools.Models.BetList;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.BetList;
    using Fanex.BetList.Core.Entities;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Bet list service.
    /// </summary>
    public class BetListService : BaseService, IBetListService
    {
        private readonly IDynamicRepository _repository;

        public BetListService() :
            this(null)
        {
        }

        public BetListService(IDynamicRepository repository)
        {
            this._repository = repository ?? new DynamicRepository();
        }

        /// <summary>
        /// Creates the choices.
        /// </summary>
        /// <param name="refNo">The reference number of the transaction.</param>
        /// <returns>List of Ticket objects.</returns>
        public IEnumerable<ITicket> CreateChoices(long refNo)
        {
            var criteria = new GetTransDetailsCriteria
            {
                RefNo = refNo
            };
            IEnumerable<BetItem> data = this._repository.Fetch<BetItem>(criteria);

            return Map(refNo, data);
        }

        /// <summary>
        /// Maps the list of BetItem objects queried from DB to Ticket objects which will be used to generate bet list.
        /// </summary>
        /// <param name="refNo">The reference number of the transaction.</param>
        /// <param name="data">The bet data that need to be mapped to Ticket objects.</param>
        /// <returns>List of Ticket objects.</returns>
        public IEnumerable<ITicket> Map(long refNo, IEnumerable<BetItem> data)
        {
            List<ITicket> result = new List<ITicket>();
            if ((data != null) && (data.Count() > 0))
            {
                foreach (var item in data)
                {
                    ITicket ticket = new Ticket
                    {
                        BetId = item.TransId,
                        BetTypeId = item.BetType,
                        HomeId = item.HomeId,
                        AwayId = item.AwayId,
                        Handicap1 = item.Hdp1,
                        Handicap2 = item.Hdp2,
                        Odds = item.Odds,
                        BetTeam = item.BetTeam,
                        EventStatus = item.BetStatus,
                        LiveHomeScore = item.LiveHomeScore,
                        LiveAwayScore = item.LiveAwayScore,
                        IsLive = item.LiveIndicator,
                        LeagueId = item.LeagueId,
                        SportTypeId = item.SportType,
                        IsNeutral = item.IsNeutral,
                        MatchId = item.MatchId,
                        MatchCode = item.MatchCode.ToString(),
                        RefNo = refNo.ToString(),
                        Status = item.Status
                    };

                    result.Add(ticket);
                }
            }

            return result.OrderBy(x => x.BetId).ToList();
        }
    }
}