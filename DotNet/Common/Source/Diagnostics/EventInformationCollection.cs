//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EventInformationCollection.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Common.Globalization;

    /// <summary>
    /// Event information collection
    /// </summary>
    /// <typeparam name="T">type of event enumerator</typeparam>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    public class EventInformationCollection<T>
    {
        /// <summary>
        /// event information collection storage
        /// </summary>
        private IReadOnlyDictionary<T, EventItemInformation> eventInformationCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventInformationCollection{T}" /> class.
        /// </summary>
        public EventInformationCollection()
        {
            var eventInformationCollection = new Dictionary<T, EventItemInformation>();
            var type = typeof(T);
            if (!type.IsEnum)
            {
                // TODO: log error
            }
            else
            {
                string sourceName = null;
                var collectionAttribute = type.GetCustomAttribute<EventCollectionAttribute>();
                if (collectionAttribute != null)
                {
                    sourceName = collectionAttribute.SourceName;
                }

                this.SourceName = sourceName ?? type.Name;

                var resourceInformationCollection = SingletonInstance<ResourceInformationCollection<T>>.Instance;

                // we should not use type.GetFields() because the system may have internal field if there is none enumerator value 
                // presents "0". the internal field will mislead our logic
                var enumNames = Enum.GetNames(type);
                foreach (var enumName in enumNames)
                {
                    var fieldInformation = type.GetField(enumName);
                    var rawValue = fieldInformation.GetValue(null);
                    var value = (T)rawValue;

                    var itemAttribute = fieldInformation.GetCustomAttribute<EventItemAttribute>();
                    var id = fieldInformation.GetValue(null);
                    EventLevel level;
                    if (itemAttribute == null)
                    {
                        level = EventItemAttribute.DefaultEventLevel;
                        // TODO: Log error
                    }
                    else
                    {
                        level = itemAttribute.Level;
                    }
                    var eventItem = new EventItemInformation(
                        fieldInformation.Name,
                        (int)rawValue,
                        level);

                    EventItemInformation previousEventItem;
                    if (eventInformationCollection.TryGetValue(value, out previousEventItem))
                    {
                        // TODO: log error
                    }
                    else
                    {
                        eventInformationCollection.Add(value, eventItem);
                    }
                }

                this.eventInformationCollection = eventInformationCollection;
            }
        }

        /// <summary>
        /// Gets event source name
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// Gets event item information by event id
        /// </summary>
        /// <param name="id">event id</param>
        /// <returns>event item information</returns>
        public EventItemInformation this[T id]
        {
            get
            {
                EventItemInformation eventItemInformation;
                return this.eventInformationCollection.TryGetValue(id, out eventItemInformation) ? eventItemInformation : null;
            }
        }
    }
}
