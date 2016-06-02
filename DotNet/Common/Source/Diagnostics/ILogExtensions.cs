//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ILogExtensions.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Globalization;

    /// <summary>
    /// ILog extensions
    /// </summary>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    public static class ILogExtensions
    {
        public static void Write<T>(
            this ILog logger,
            DateTime eventTime,
            StackTrace stackTrace,
            IReadOnlyDictionary<string, object> runtimeInformation,
            IResourceProvider resourceProvider,
            params object[] parameters) where T: struct
        {
        }
    }
}
