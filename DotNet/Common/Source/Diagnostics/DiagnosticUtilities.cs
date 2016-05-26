//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DiagnosticUtilities.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// Utilities for diagnostic
    /// </summary>
    public static class DiagnosticUtilities
    {
        /// <summary>
        /// GEt method information
        /// </summary>
        /// <param name="frame">stack frame</param>
        /// <returns>method information</returns>
        public static string GetMethodInformation(StackFrame frame)
        {
            var builder = new StringBuilder();
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

                builder.Append(") ");
                builder.AppendFormat("File: \"{0}\" ", frame.GetFileName());
                builder.AppendFormat("Line: \"{0}\" ", frame.GetFileLineNumber());
                builder.AppendFormat("Column: \"{0}\" ", frame.GetFileColumnNumber());
            }
            catch (Exception exception)
            {
                builder.AppendLine(exception.ToString());
            }

            return builder.ToString();
        }
    }
}
