//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DiagnosticUtilities.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// Utilities for diagnostic
    /// </summary>
    public static class DiagnosticUtilities
    {
        private const string DefaultStackTraceTitle = "Stack Trace";
        private const string DefaultFileLabel = "File";
        private const string DefaultLineLabel = "Line";
        private const string DefaultColumnLabel = "Column";
        private const string DefaultRuntimeInformationTitle = "Runtime Information";
        private const string DefaultEventSourceTitle = "Event Source";
        private const string DefaultEventIdTitle = "Event Id";
        private const string DefaultExceptionDataTitle = "Exception Data";
        private const string DefaultInnerExceptionTitle = "Inner Exception";

        private const string KeyStringPairTemplate = "{0}:\"{1}\"";
        private const string KeyValuePairTemplate = "{0}:{1}";

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
            string title = DefaultStackTraceTitle,
            string fileLabel = DefaultFileLabel,
            string lineLable = DefaultLineLabel,
            string columnLabel = DefaultColumnLabel)
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
                    builder.AppendFormat(" " + KeyStringPairTemplate, fileLabel, filename);

                    var lineNumber = frame.GetFileLineNumber();
                    if (lineNumber > 0)
                    {
                        builder.AppendFormat(" " + KeyValuePairTemplate, lineLable, lineNumber);
                    }

                    var columnNumber = frame.GetFileColumnNumber();
                    if (columnNumber > 0)
                    {
                        builder.AppendFormat(" " + KeyValuePairTemplate, columnLabel, columnNumber);
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
            string title = DefaultRuntimeInformationTitle)
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

        public static void FormatMessage(StringBuilder builder, string template, params object[] parameters)
        {
            if (parameters.IsNullOrEmpty())
            {
                builder.Append(template);
                return;
            }

            var items = new string[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                if ((parameter == null) || (parameter is string))
                {
                    items[i] = parameter as string;
                }
                else if (parameter is Exception)
                {
                    var exceptionBuilder = new StringBuilder();
                    BuildExceptionMessage(exceptionBuilder, parameter as Exception);
                    items[i] = exceptionBuilder.ToString();
                }
                else
                {
                    string value;
                    try
                    {
                        value = parameter.ToString();
                    }
                    catch(Exception exception)
                    {
                        // TODO: log error
                        value = exception.ToString();
                    }
                    items[i] = value;
                }
            }

            string message;
            try
            {
                message = string.Format(template, items);
                builder.Append(message);
            }
            catch (Exception exception)
            {
                builder.AppendFormat("Template:{0}", template);
                builder.AppendLine();
                builder.AppendLine("Parameters:");
                for (var i = 0; i < items.Length; i++)
                {
                    builder.AppendFormat(KeyStringPairTemplate, i, items[i]);
                    builder.AppendLine();
                }
                builder.AppendLine("Format exception:");
                BuildExceptionMessage(builder, exception);
            }
        }

        public static void BuildExceptionMessage(
            StringBuilder builder, 
            Exception exception,
            string eventSourceTitle = DefaultEventSourceTitle,
            string eventIdTitle = DefaultEventIdTitle)
        {
            var isInnerException = false;

            while (exception != null)
            {
                if (isInnerException)
                {
                    builder.AppendLine(" ------------------- " + DefaultInnerExceptionTitle + " ------------------- ");
                }

                var predefinedException = exception as ExceptionBase;
                if (predefinedException != null)
                {
                    builder.AppendFormat(KeyStringPairTemplate, eventSourceTitle, predefinedException.EventSource);
                    builder.AppendLine();
                    builder.AppendFormat(KeyValuePairTemplate, eventIdTitle, predefinedException.EventId);
                    builder.AppendLine();
                    AddRuntimeInformation(builder, predefinedException.RuntimeInformation);
                }

                if (!exception.Data.IsNullOrEmpty())
                {
                    builder.AppendFormat("{0}:", DefaultExceptionDataTitle);
                    foreach (DictionaryEntry pair in exception.Data)
                    {
                        builder.AppendFormat("\t" + KeyStringPairTemplate, pair.Key, pair.Value);
                    }
                }

                var stackTrace = new StackTrace(exception, true);
                AppendStackTraceInformation(builder, stackTrace);

                exception = exception.InnerException;
                isInnerException = true;
            }
        }
    }
}
