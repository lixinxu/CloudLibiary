//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ResourceItemAttribute.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Globalization
{
    using System;

    /// <summary>
    /// Resource item attribute
    /// </summary>
    /// <remarks>
    /// <para>Instead of using integer to represent a resource id, using enumerator is more easy to understand.</para>
    /// <para>In the meantime, the attribute can provide a default content in case the system failed to load data
    /// from resource storage.</para>
    /// </remarks>
    /// <example>
    /// <![CDATA[
    /// public enum MyResource
    /// {
    ///     [ResourceItem("The Email Title")]
    ///     TitleInformation,
    /// }
    /// ...
    /// var provider = SingletonInstance<ObjectResolverFactory>.Instance.Resolve<IResourceProvider>();
    /// var titleInformation = provider.GetResource<MyResource>(MyResource.TitleInformation);
    /// ...
    /// ]]>
    /// </example>
    [AttributeUsage(AttributeTargets.Enum)]
    public class ResourceItemAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceItemAttribute" /> class.
        /// </summary>
        /// <param name="content">default resource content</param>
        public ResourceItemAttribute(string content = null) : this(null, content)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceItemAttribute" /> class.
        /// </summary>
        /// <param name="name">resource name</param>
        /// <param name="content">default resource content</param>
        public ResourceItemAttribute(string name, string content)
        {
            this.Name = name.SafeTrim();
            this.Content = content;
        }

        /// <summary>
        /// Gets resource name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets resource content
        /// </summary>
        public string Content { get; }
    }
}
