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
        /// <param name="eventSource">event source name</param>
        /// <param name="eventId">event id</param>
        /// <param name="eventLevel">event level</param>
        /// <param name="message">detail event information</param>
        /// <param name="eventTime">the time the event happened</param>
        /// <param name="stackTrace">stack trace</param>
        /// <param name="runtimeInformation">additional runtime information</param>
        /// <remarks>
        /// This is high level design which covers most scenarios. The implementation can customize the actual behavior.
        /// For example, add more details information like "cloud service name", "role name" and so on. The logging 
        /// provider will add information in different repository based on the additional runtime information. On
        /// the other hand, the caller can add more diagnostics information like correlation id as well.
        /// </remarks>
        /// <example>
        /// <![CDATA[
        /// var logger = SingletonInstance<ObjectResolverFactory>.Instance.GetResolver<ILog>();
        /// logger.Write(
        ///     "Common",
        ///     EventIds.FileNotFound,
        ///     EventLevel.Error,
        ///     $"ConfigurationFile {path} was not found",
        ///     Datetime.UtcNow,
        ///     GetStackTrace(),
        ///     GetRuntimeInformation());
        /// ]]>
        /// Using logging extension may simplify the code
        /// <![CDATA[
        /// var logger = Singleton<ObjectResolverFactory>.Instance.GetResolver<ILog>();
        /// logger.Write<EventIds>(EventIds.FileNotFound, GetRuntimeInformation(), path);
        /// ]]>
        /// </example>
        void Write(
            string eventSource,
            ulong eventId,
            EventLevel eventLevel,
            string message,
            DateTime eventTime,
            StackTrace stackTrace,
            IReadOnlyDictionary<string, object> runtimeInformation);
    }
}
