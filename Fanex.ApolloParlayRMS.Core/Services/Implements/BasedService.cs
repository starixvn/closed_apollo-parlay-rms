// --------------------------------------------------------------------------
// <copyright file="BasedService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Services.Implements
{
    using System;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Core.Services.Interfaces;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Service Base.
    /// </summary>
    public class BasedService : IBasedService
    {
        public IServiceContext ServiceContext { get; set; }

        public CommandResult InternalError
        {
            get
            {
                return new CommandResult
                {
                    Code = 500,
                    Message = Messages.InternalError
                };
            }
        }

        /// <summary>
        /// Handles Exception.
        /// </summary>
        /// <param name="ex">Occurred Exception.</param>
        public void HandleException(Exception ex)
        {
            LoggingHandler.Log(ex);
        }

        /// <summary>
        /// Gets the criteria which are used to identify the user and the machine that update the data to the database.
        /// </summary>
        /// <returns>LogExecSPCriteria object.</returns>
        public LogExecSpCriteria GetLogExecSpCriteria()
        {
            Check.Argument.IsNotNull(this.ServiceContext, "ServiceContext");

            return new LogExecSpCriteria
            {
                CurrentUserId = this.ServiceContext.UserId,
                CurrentSiteId = this.ServiceContext.SiteId,
                LogDate = DateTime.Now,
                LoginIp = this.ServiceContext.Ip
            };
        }
    }
}
