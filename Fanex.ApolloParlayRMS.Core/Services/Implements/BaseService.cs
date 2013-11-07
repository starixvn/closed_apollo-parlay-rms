// --------------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Service Base.
    /// </summary>
    public class BaseService
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
    }
}
