//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ResourceInformationCollection.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Resource information collection
    /// </summary>
    /// <typeparam name="T">type of enumerator</typeparam>
    /// <seealso cref="ResourceCollectionAttribute"/>
    /// <seealso cref="ResourceItemAttribute"/>
    public class ResourceInformationCollection<T>
    {
        /// <summary>
        /// Resource name to item mapping
        /// </summary>
        private IReadOnlyDictionary<string, ResourceItem> nameMapping;

        /// <summary>
        /// Resource id to item mapping
        /// </summary>
        private IReadOnlyDictionary<T, ResourceItem> idMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceInformationCollection{T}" /> class.
        /// </summary>
        public ResourceInformationCollection()
        {
            var nameMapping = new Dictionary<string, ResourceItem>();
            var idMapping = new Dictionary<T, ResourceItem>();

            var type = typeof(T);
            if (!type.IsEnum)
            {
                // TODO: log error
            }

            string storageName = null;
            var collectionAttribute = type.GetCustomAttribute<ResourceCollectionAttribute>();
            if (collectionAttribute != null)
            {
                storageName = collectionAttribute.StorageName;
            }

            this.StorageName = storageName ?? type.Name;

            // we should not use type.GetFields() because the system may have internal field if there is none enumerator value 
            // presents "0". the internal field will mislead our logic
            var enumNames = Enum.GetNames(type);
            foreach (var enumName in enumNames)
            {
                var fieldInformation = type.GetField(enumName);
                var value = (T)fieldInformation.GetValue(null);

                string resourceName = null;
                string content = null;

                var itemAttribute = fieldInformation.GetCustomAttribute<ResourceItemAttribute>();
                if (itemAttribute != null)
                {
                    resourceName = itemAttribute.Name;
                    content = itemAttribute.Content;
                }

                if (resourceName == null)
                {
                    resourceName = enumName;
                }

                var resourceItem = new ResourceItem(resourceName, content);

                ResourceItem previousResourceItem;
                if (idMapping.TryGetValue(value, out previousResourceItem))
                {
                    // TODO: log error
                }
                else
                {
                    idMapping.Add(value, resourceItem);
                }

                if (nameMapping.TryGetValue(resourceName, out previousResourceItem))
                {
                    // TODO: log error
                }
                else
                {
                    nameMapping.Add(resourceName, resourceItem);
                }
            }

            this.nameMapping = nameMapping;
            this.idMapping = idMapping;
        }

        /// <summary>
        /// Gets resource storage name
        /// </summary>
        public string StorageName { get; }

        /// <summary>
        /// Get resource default content by resource name
        /// </summary>
        /// <param name="name">resource name</param>
        /// <returns>resource default content</returns>
        public string GetContent(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            ResourceItem item;
            if (this.nameMapping.TryGetValue(name, out item))
            {
                return item.Content;
            }

            return null;
        }

        /// <summary>
        /// Try to get resource information by resource id
        /// </summary>
        /// <param name="resourceId">resource id. The enumerator value</param>
        /// <param name="information">resource information (resource name, default content pair)</param>
        /// <returns>true if information returned. false if resource id is invalid</returns>
        public bool TryGetResourceInformation(T resourceId, out KeyValuePair<string, string> information)
        {
            ResourceItem item;
            if (this.idMapping.TryGetValue(resourceId, out item))
            {
                information = new KeyValuePair<string, string>(item.Name, item.Content);
                return true;
            }

            information = default(KeyValuePair<string, string>);
            return false;
        }

        /// <summary>
        /// Private class to store resource information
        /// </summary>
        private class ResourceItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ResourceItem" /> class.
            /// </summary>
            /// <param name="name">resource name</param>
            /// <param name="content">resource default content</param>
            internal ResourceItem(string name, string content)
            {
                this.Name = name;
                this.Content = content;
            }

            /// <summary>
            /// Gets resource name
            /// </summary>
            internal string Name { get; }

            /// <summary>
            /// Gets resource default content
            /// </summary>
            internal string Content { get; }
        }
    }
}
