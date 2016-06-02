//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EventItemAttribute.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    using System;

    /// <summary>
    /// Attribute for event enumerator item
    /// </summary>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    [AttributeUsage(AttributeTargets.Field)]
    public class EventItemAttribute : Attribute
    {
        /// <summary>
        /// Default event level
        /// </summary>
        public const EventLevel DefaultEventLevel = EventLevel.Error;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventItemAttribute" /> class.
        /// </summary>
        public EventItemAttribute() : this(DefaultEventLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventItemAttribute" /> class.
        /// </summary>
        /// <param name="level">event level</param>
        public EventItemAttribute(EventLevel level)
        {
            this.Level = level;
        }

        /// <summary>
        /// Gets event level
        /// </summary>
        public EventLevel Level { get; }
    }
}