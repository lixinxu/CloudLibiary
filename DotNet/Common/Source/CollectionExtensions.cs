//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CollectionExtensions.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Collection extensions
    /// </summary>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Check whether a collection is null or empty
        /// </summary>
        /// <typeparam name="T">type of item in collection</typeparam>
        /// <param name="collection">collection to check</param>
        /// <returns>true if collection is either null or empty. False if collection is not null and contains item</returns>
        /// <remarks>
        /// String has a method called IsNullOrEmpty. We need similar method for collection because it is used wildly.
        /// </remarks>
        /// <example>
        /// Following is example of how to use it:
        /// <![CDATA[
        /// void Foo(ICollection<int> dataToProcess)
        /// {
        ///     if (dataToProcess.IsNullOrEmpty())
        ///     {
        ///         throw new ArgumentException("data should not be null or empty");
        ///     }
        ///     ...
        /// }
        /// ]]>
        /// </example>
        public static bool IsReadWriteNullOrEmpty<T>(this ICollection<T> collection)
        {
            return (collection == null) || (collection.Count < 1);
        }

        /// <summary>
        /// Check whether a read-only collection is null or empty
        /// </summary>
        /// <typeparam name="T">type of item in collection</typeparam>
        /// <param name="collection">collection to check</param>
        /// <returns>true if collection is either null or empty. False if collection is not null and contains item</returns>
        /// <remarks>
        /// String has a method called IsNullOrEmpty. We need similar method for collection because it is used wildly.
        /// </remarks>
        /// <example>
        /// Following is example of how to use it:
        /// <![CDATA[
        /// void Foo(IReadOnlyCollection<int> dataToProcess)
        /// {
        ///     if (dataToProcess.IsNullOrEmpty())
        ///     {
        ///         throw new ArgumentException("data should not be null or empty");
        ///     }
        ///     ...
        /// }
        /// ]]>
        /// </example>
        public static bool IsReadOnlyNullOrEmpty<T>(this IReadOnlyCollection<T> collection)
        {
            return (collection == null) || (collection.Count < 1);
        }

        /// <summary>
        /// Check whether a collection is null or empty
        /// </summary>
        /// <param name="collection">collection to check</param>
        /// <returns>true if collection is either null or empty. False if collection is not null and contains item</returns>
        /// <remarks>
        /// String has a method called IsNullOrEmpty. We need similar method for collection because it is used wildly.
        /// </remarks>
        /// <example>
        /// Following is example of how to use it:
        /// <![CDATA[
        /// void Foo(ICollection dataToProcess)
        /// {
        ///     if (dataToProcess.IsNullOrEmpty())
        ///     {
        ///         throw new ArgumentException("data should not be null or empty");
        ///     }
        ///     ...
        /// }
        /// ]]>
        /// </example>
        public static bool IsNullOrEmpty(this ICollection collection)
        {
            return (collection == null) || (collection.Count < 1);
        }
    }
}
