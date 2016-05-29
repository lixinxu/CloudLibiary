//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ResourceCollectionAttribute.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Globalization
{
    using System;

    /// <summary>
    /// Resource collection attribute
    /// </summary>
    /// <remarks>
    /// Define resource storage name on enumerator so we can use resource provider extension to simplify the code.
    /// </remarks>
    /// <example>
    /// <![CDATA[
    /// [ResourceCollection("MyResourceStorage")]
    /// public enum MyResource
    /// {
    ///     [ResourceItem("Title information")]
    ///     TitleInformation,
    /// }
    /// ]]>
    /// To get the resource:
    /// <![CDATA[
    /// var provider = Singleton<ObjectResolverFactory>.Instance.Resolve<IResourceProvider>();
    /// var titileInformation = provider.GetResource(MyResource.TitleInformation);
    /// ]]>
    /// The resource provider implementation will use the storage name to load resource for external storage.
    /// <![CDATA[
    /// public MyResourceProvier : IResourceProvider
    /// {
    ///     public string GetResource(string storageName, string key) //is this case, the storageName is "MyResourceStorage"
    ///     {
    ///         ...
    ///     }
    /// }
    /// ]]>
    /// </example>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    [AttributeUsage(AttributeTargets.Enum)]
    public class ResourceCollectionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceCollectionAttribute" /> class.
        /// </summary>
        /// <param name="resourceStorageName">resource storage name</param>
        public ResourceCollectionAttribute(string resourceStorageName)
        {
            this.StorageName = resourceStorageName.SafeTrim();
        }

        /// <summary>
        /// Gets resource storage name
        /// </summary>
        public string StorageName { get; }
    }
}
