// --------------------------------------------------------------------------
// <copyright file="SimilarBetsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Apollo.ParlayRMS.Tools.Services.Implements.SimilarBets
{
    using Apollo.ParlayRMS.Tools.Criteria.SimilarBets;
    using Apollo.ParlayRMS.Tools.Models.SimilarBets;
    using Apollo.ParlayRMS.Tools.Repositories.Implements.SimilarBets;
    using Apollo.ParlayRMS.Tools.Repositories.Interfaces.SimilarBets;
    using Apollo.ParlayRMS.Tools.Services.Interfaces.SimilarBets;
    using Fanex.NAP.Core.Service.Impl;

    /// <summary>
    /// Similar bets service.
    /// </summary>
    public class SimilarBetsService : BaseService, ISimilarBetsService
    {
        private readonly ISimilarBetsRepository _similarBetsRepository;

        public SimilarBetsService() :
            this(null)
        {
        }

        public SimilarBetsService(ISimilarBetsRepository similarBetsRepository)
        {
            this._similarBetsRepository = similarBetsRepository ?? new SimilarBetsRepository();
        }

        /// <summary>
        /// Gets the similar bet list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>
        /// SimilarBetList object contains the found information about the bet of passed reference number
        /// and list of its similar bets.
        /// </returns>
        public SimilarBetList GetSimilarBetList(GetSimilarBetListCriteria criteria)
        {
            return this._similarBetsRepository.GetSimilarBetList(criteria);
        }
    }
}