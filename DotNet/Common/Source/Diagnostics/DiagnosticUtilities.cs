//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DiagnosticUtilities.cs" company="CloudLibrary">
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
    /// Utilities for diagnostic
    /// </summary>
    public static class DiagnosticUtilities
    {
        /// <summary>
        /// Add stack trace information into message builder
        /// </summary>
        /// <param name="builder">string builder</param>
        /// <param name="stackTrace">stack trace</param>
        /// <param name="title">stack trace title</param>
        /// <param name="fileLabel">file label</param>
        /// <param name="lineLable">line label</param>
        /// <param name="columnLabel">column label</param>
        public static void AppendStackTraceInformation(
            StringBuilder builder, 
            StackTrace stackTrace, 
            string title = "Stack trace",
            string fileLabel = "File",
            string lineLable = "Line",
            string columnLabel = "Column")
        {
            if (stackTrace != null)
            {
                var frames = stackTrace.GetFrames();
                if (!frames.IsNullOrEmpty())
                {
                    builder.AppendLine($"{title}:");
                    foreach (var frame in frames)
                    {
                        builder.Append('\t');
                        DiagnosticUtilities.AppendMethodInformation(builder, frame, fileLabel, lineLable, columnLabel);
                        builder.AppendLine();
                    }
                }
            }
        }

        /// <summary>
        /// Add method information to message builder
        /// </summary>
        /// <param name="builder">string builder</param>
        /// <param name="frame">stack frame</param>
        /// <param name="fileLabel">file label</param>
        /// <param name="lineLable">line label</param>
        /// <param name="columnLabel">column label</param>
        public static void AppendMethodInformation(
            StringBuilder builder, 
            StackFrame frame,
            string fileLabel,
            string lineLable,
            string columnLabel)
        {
            try
            {
                var methodInformation = frame.GetMethod();
                builder.AppendFormat("{0}(", methodInformation.Name);
                var parameters = methodInformation.GetParameters();
                if (!parameters.IsNullOrEmpty())
                {
                    var first = false;
                    foreach (var parameter in parameters)
                    {
                        if (!first)
                        {
                            builder.Append(", ");
                            first = false;
                        }

                        builder.AppendFormat("{0} {1}", parameter.ParameterType.Name, parameter.Name);
                    }
                }

                builder.Append(")");
                var filename = frame.GetFileName();
                if (!string.IsNullOrEmpty(filename))
                {
                    builder.AppendFormat(" {0}:\"{1}\"", fileLabel, filename);

                    var lineNumber = frame.GetFileLineNumber();
                    if (lineNumber > 0)
                    {
                        builder.AppendFormat(" {0}:{1}", lineLable, lineNumber);
                    }

                    var columnNumber = frame.GetFileColumnNumber();
                    if (columnNumber > 0)
                    {
                        builder.AppendFormat(" {0}:{1}", columnLabel, columnNumber);
                    }
                }
            }
            catch (Exception exception)
            {
                builder.AppendLine(exception.ToString());
            }
        }

        /// <summary>
        /// Add runtime information
        /// </summary>
        /// <param name="builder">message builder</param>
        /// <param name="runtimeInformation">runtime information</param>
        /// <param name="title">title for runtime information</param>
        public static void AddRuntimeInformation(
            StringBuilder builder, 
            IReadOnlyDictionary<string, object> runtimeInformation,
            string title = "Runtime Information")
        {
            if (!runtimeInformation.IsReadOnlyNullOrEmpty())
            {
                builder.AppendFormat("{0}:", title);
                builder.AppendLine();
                foreach (var pair in runtimeInformation)
                {
                    string value;
                    try
                    {
                        value = pair.Value.ToString();
                    }
                    catch (Exception exception)
                    {
                        value = exception.ToString();
                    }

                    builder.AppendLine($"\t{pair.Key}: {value}");
                }
            }
        }
    }
}
