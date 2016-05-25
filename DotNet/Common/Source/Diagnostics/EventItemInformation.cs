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
    public class EventItemInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventItemInformation" /> class.
        /// </summary>
        /// <param name="name">event name</param>
        /// <param name="id">event id</param>
        /// <param name="level">event level</param>
        /// <param name="resourceName">resource name for template</param>
        /// <param name="defaultMessageTemplate">default template content</param>
        public EventItemInformation(
            string name, 
            int id,
            EventLevel level, 
            string resourceName, 
            string defaultMessageTemplate)
        {
            this.Name = name;
            this.Id = id;
            this.Level = level;
            this.ResourceName = resourceName;
            this.DefaultMessageTemplate = defaultMessageTemplate;
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

        /// <summary>
        /// Gets resource name for retrieving message template
        /// </summary>
        public string ResourceName { get; }

        /// <summary>
        /// Gets default message template
        /// </summary>
        public string DefaultMessageTemplate { get; }
    }
}
