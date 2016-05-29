//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TraceLogger.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// ILog trance implementation
    /// </summary>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    public class TraceLogger : ILog
    {
        /// <summary>
        /// Trace prefix
        /// </summary>
        public const string TracePrefix = "CloudLibrary:";

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
        public void Write(
            string eventSource,
            int eventId,
            EventLevel eventLevel,
            string message,
            DateTime eventTime,
            StackTrace stackTrace,
            IReadOnlyDictionary<string, object> runtimeInformation)
        {
            var builder = new StringBuilder(TracePrefix);
            builder.AppendLine($"[Source:{eventSource}][id:{eventId}][Level:{eventLevel}][Time:{eventTime.ToString()}]");
            if (!string.IsNullOrEmpty(message))
            {
                builder.AppendLine("Message:");
                builder.AppendLine($"\t{message}");
            }

            DiagnosticUtilities.AppendStackTraceInformation(builder, stackTrace);
            DiagnosticUtilities.AddRuntimeInformation(builder, runtimeInformation);

            Trace.WriteLine(builder.ToString());
        }
    }
}
