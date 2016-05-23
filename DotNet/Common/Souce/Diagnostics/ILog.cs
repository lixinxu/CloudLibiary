//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ILog.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Logging interface
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Write event log
        /// </summary>
        /// <param name="eventId">event id</param>
        /// <param name="eventLevel">event level</param>
        /// <param name="message">detail event information</param>
        /// <param name="eventTime">the time the event happened</param>
        /// <param name="stackTrace">stack trace</param>
        /// <param name="runtimeInformation">additional runtime information</param>
        void Write(
            ulong eventId,
            EventLevel eventLevel,
            string message,
            DateTime eventTime,
            StackTrace stackTrace,
            IReadOnlyDictionary<string, object> runtimeInformation);
    }
}
