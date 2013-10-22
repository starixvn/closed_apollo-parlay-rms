// --------------------------------------------------------------------------
// <copyright file="LoggingHandler.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Services.Implements
{
    using System;
    using Fanex.Logging;

    public enum LogType : int
    {
        /// <summary>
        /// Information log type.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Error log type.
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// Logging handler.
    /// </summary>
    public class LoggingHandler
    {
        /// <summary>
        /// Log exception.
        /// </summary>
        /// <param name="ex">Exception that needs to be logged.</param>
        public static void Log(Exception ex)
        {
            LogService.Error(ex.Message, ex);
        }

        /// <summary>
        /// Log exception.
        /// </summary>
        /// <param name="message">Message that needs to be logged.</param>
        /// <param name="logType">Exception type.</param>
        public static void Log(string message = "", LogType logType = LogType.Error)
        {
            switch (logType)
            {
                case LogType.Info:
                    LogService.Info(message);
                    break;
                case LogType.Error:
                    LogService.Error(message);
                    break;
            }
        }
    }
}