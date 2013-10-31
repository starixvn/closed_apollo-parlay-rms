// --------------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Services.Interfaces.BetList
{
    using System.Collections.Generic;
    using Fanex.ApolloParlayRMS.Tools.Models.BetList;
    using Fanex.BetList.Core.Entities;

    public interface IBetListService
    {
        /// <summary>
        /// Creates the choices.
        /// </summary>
        /// <param name="refNo">The reference number of the transaction.</param>
        /// <returns>List of Ticket objects.</returns>
        IEnumerable<ITicket> CreateChoices(long refNo);

        /// <summary>
        /// Maps the list of BetItem objects queried from DB to Ticket objects which will be used to generate bet list.
        /// </summary>
        /// <param name="refNo">The reference number of the transaction.</param>
        /// <param name="data">The bet data that need to be mapped to Ticket objects.</param>
        /// <returns>List of Ticket objects.</returns>
        IEnumerable<ITicket> Map(long refNo, IEnumerable<BetItem> data);
    }
}