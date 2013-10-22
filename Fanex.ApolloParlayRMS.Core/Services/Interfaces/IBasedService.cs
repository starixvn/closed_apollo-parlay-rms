// --------------------------------------------------------------------------
// <copyright file="IBasedService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Services.Interfaces
{
    using System;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Service.Interface;

    public interface IBasedService
    {
        IServiceContext ServiceContext { get; set; }

        /// <summary>
        /// Handles Exception.
        /// </summary>
        /// <param name="ex">Occurred Exception.</param>
        void HandleException(Exception ex);

        /// <summary>
        /// Gets the criteria which are used to identify the user and the machine that update the data to the database.
        /// </summary>
        /// <returns>LogExecSPCriteria object.</returns>
        LogExecSpCriteria GetLogExecSpCriteria();
    }
}
