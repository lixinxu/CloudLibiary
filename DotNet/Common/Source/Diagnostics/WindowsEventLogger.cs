//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WindowsEventLogger.cs" company="CloudLibrary">
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
    /// ILog Windows Event implementation
    /// </summary>
    public class WindowsEventLogger : ILog
    {
        /// <summary>
        /// Write the Windows Event Log
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
            // Choose the corresponding Windows EventLogEntryType with the given EventLevel
            var resultEventLogEntryType = EventLogEntryType.Error;
            switch (eventLevel)
            {
                case EventLevel.Debug:
                case EventLevel.Information:
                    resultEventLogEntryType = EventLogEntryType.Information;
                    break;
                case EventLevel.Warning:
                    resultEventLogEntryType = EventLogEntryType.Warning;
                    break;
                default:
                    break;
            }

            // Build the message
            var builder = new StringBuilder();
            builder.AppendLine($"Time:{ eventTime.ToString()}");
            if (!string.IsNullOrEmpty(message))
            {
                builder.AppendLine("Message:");
                builder.AppendLine($"\t{message}");
            }

            // Append the stack trace and additional information to the builder
            DiagnosticUtilities.AppendStackTraceInformation(builder, stackTrace);
            DiagnosticUtilities.AddRuntimeInformation(builder, runtimeInformation);

            // Try to write the log if the program has the access to do so
            try
            {
                EventLog.WriteEntry(eventSource, builder.ToString(), resultEventLogEntryType, eventId);
            }
            catch (Exception e)
            {
                //TODO: Report error when the access is denied, or when the source doesnot exist
            }
        }
    }
}
