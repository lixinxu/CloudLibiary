//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IResourceProviderExtensions.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Globalization
{
    using System.Collections.Generic;

    /// <summary>
    /// IResourceProvider extensions
    /// </summary>
    public static class IResourceProviderExtensions
    {
        /// <summary>
        /// Get resource by resource id
        /// </summary>
        /// <typeparam name="T">type of resource enumerator</typeparam>
        /// <param name="provider">resource provider</param>
        /// <param name="id">resource id in the enumerator</param>
        /// <returns>resource content</returns>
        /// <remarks>
        /// If resource was not found in the storage, the default value which defined in the enumerator will be returned.
        /// </remarks>
        /// <example>
        /// <![CDATA[
        /// <![CDATA[
        /// [ResourceCollection("MyResourceStorage")]
        /// public enum MyResource
        /// {
        ///     [ResourceItem("Title information")]
        ///     TitleInformation,
        /// }
        /// ...
        /// var provider = SingletonInstance<ObjectResolverFactory>.Instance.Resolve<IResourceProvider>();
        /// var resource = provider.GetResource(MyResource.TitleInformation);
        /// ]]>
        /// </example>
        /// <history>
        ///     <create time="2016/5/16" author="lixinxu" />
        /// </history>
        public static string GetResource<T>(this IResourceProvider provider, T id)
        {
            var resourceInformationCollection = SingletonInstance<ResourceInformationCollection<T>>.Instance;
            KeyValuePair<string, string> resourceItem;
            if (resourceInformationCollection.TryGetResourceInformation(id, out resourceItem))
            {
                var resource = provider.GetResource(resourceInformationCollection.StorageName, resourceItem.Key);
                return resource ?? resourceItem.Value;
            }

            return null;
        }

        /// <summary>
        /// Get resource by name
        /// </summary>
        /// <typeparam name="T">type of resource enumerator</typeparam>
        /// <param name="provider">resource provider</param>
        /// <param name="name">resource name</param>
        /// <returns>resource data</returns>
        /// <remarks>
        /// If resource was not found in the storage, the default value which defined in the enumerator will be returned.
        /// </remarks>
        /// <example>
        /// <![CDATA[
        /// <![CDATA[
        /// [ResourceCollection("MyResourceStorage")]
        /// public enum MyResource
        /// {
        ///     [ResourceItem("Title information line 1")]
        ///     TitleInformation1,
        ///     [ResourceItem("Title information line 2")]
        ///     TitleInformation2,
        /// }
        /// ...
        /// var provider = SingletonInstance<ObjectResolverFactory>.Instance.Resolve<IResourceProvider>();
        /// var titles = new List<string>();
        /// for(var i=0; i<2;i++)
        /// {
        ///     var resource = provider.GetResource<MyResource>($"TitleInformation{i}");
        ///     titles.Add(resource);
        /// }
        /// ]]>
        /// </example>
        public static string GetResource<T>(this IResourceProvider provider, string name)
        {
            var resourceInformationCollection = SingletonInstance<ResourceInformationCollection<T>>.Instance;
            var resource = provider.GetResource(resourceInformationCollection.StorageName, name);
            return resource ?? resourceInformationCollection.GetContent(name);
        }
    }
}
