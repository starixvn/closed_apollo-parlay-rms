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
    using Fanex.ApolloParlayRMS.Tools.Repositories.Implements.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Repositories.Interfaces.TotalOutstanding;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.TotalOutstanding;

    /// <summary>
    /// Total outstanding service.
    /// </summary>
    public class TotalOutstandingService : ITotalOutstandingService
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

        //private dynamic GetAgentTotalOutstanding(
    }
}