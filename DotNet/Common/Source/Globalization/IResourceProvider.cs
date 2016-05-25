//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IResourceProvider.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Globalization
{
    /// <summary>
    /// Resource provider interface
    /// </summary>
    public interface IResourceProvider
    {
        /// <summary>
        /// Get resource
        /// </summary>
        /// <param name="storageName">resource storage name</param>
        /// <param name="key">resource key</param>
        /// <returns>resource value</returns>
        /// <remarks>
        /// <para>If resource was not found, null will be returned</para>
        /// <para>The interface is mainly for resource provider implementation. Actually the library provides an extension 
        /// which can be uses easily</para>
        /// </remarks>
        /// <example>
        /// <![CDATA[
        /// var provider = SingletonInstance<ObjectResolverFactory>.Instance.Resolve<IResourceProvider>();
        /// var resource = provider.GetResource("MyResourceStorage", "TitleInformation");
        /// ]]>
        /// To use extension:
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
        /// <seealso cref="IResourceProviderExtensions.GetResource{T}(IResourceProvider, string)"/>
        /// <seealso cref="IResourceProviderExtensions.GetResource{T}(IResourceProvider, T)"/>
        string GetResource(string storageName, string key);
    }
}
