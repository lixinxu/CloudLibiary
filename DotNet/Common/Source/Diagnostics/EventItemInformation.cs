//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EventItemInformation.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    /// <summary>
    /// Event item information
    /// </summary>
    /// <remarks>
    /// This class is used internally for processing event enumerator.
    /// </remarks>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    public class EventItemInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventItemInformation" /> class.
        /// </summary>
        /// <param name="name">event name</param>
        /// <param name="id">event id</param>
        /// <param name="level">event level</param>
        public EventItemInformation(
            string name, 
            int id,
            EventLevel level)
        {
            this.Name = name;
            this.Id = id;
            this.Level = level;
        }

        /// <summary>
        /// Gets event name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets event id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets event level
        /// </summary>
        public EventLevel Level { get; }
    }
}
