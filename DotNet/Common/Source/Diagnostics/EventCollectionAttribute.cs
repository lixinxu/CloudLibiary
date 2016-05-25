//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EventCollectionAttribute.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Attribute for event enumerator definition
    /// </summary>
    /// <remarks>
    /// <para>If an enumerator does not have event source specified, then the attribute is not required.</para>
    /// <para>When using ILog.Write with generic extension, the event source of the attribute will be used.</para>
    /// <para>The constructor will trim the event source name before saving it</para>
    /// </remarks>
    /// <example>
    /// <![CDATA[
    /// [EventCollection("Common")]
    /// public enum CommenEvents
    /// {
    ///     [EventItem(...)]
    ///     FileNotFound,
    ///     ...
    /// }
    /// var logger = SingletonInstance<ObjectResolverFactory>.Instance.Resolve<ILog>();
    /// logger.Write(CommenEvents.FileNotFound, ...);
    /// ]]>
    /// <seealso cref="ILogExtensions.Write{T}(ILog, DateTime, StackTrace, IReadOnlyDictionary{string, object})"/>
    /// </example>
    [AttributeUsage(AttributeTargets.Enum)]
    public class EventCollectionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventCollectionAttribute" /> class.
        /// </summary>
        /// <param name="eventSource">event source name</param>
        public EventCollectionAttribute(string eventSource)
        {
            this.EventSource = eventSource.SafeTrim();
        }

        /// <summary>
        /// Gets event source
        /// </summary>
        public string EventSource { get; }
    }
}