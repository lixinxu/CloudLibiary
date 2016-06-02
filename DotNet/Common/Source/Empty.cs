//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Empty.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// Empty storage
    /// </summary>
    /// <remarks>
    /// Sometimes we need to return empty collection. Instead of creating new empty object over and over, we can re-use
    /// pre-created object to avoid performance and memory issue.
    /// </remarks>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    public static class Empty
    {
        /// <summary>
        /// Get empty array
        /// </summary>
        /// <typeparam name="T">type of item in array</typeparam>
        /// <returns>empty array</returns>
        public static T[] GetArray<T>()
        {
            return EmptyArray<T>.Instance;
        }

        /// <summary>
        /// Get empty dictionary
        /// </summary>
        /// <typeparam name="TKey">type of dictionary key</typeparam>
        /// <typeparam name="TValue">type of dictionary value</typeparam>
        /// <returns>Empty read-only dictionary</returns>
        public static IReadOnlyDictionary<TKey, TValue> GetDictionary<TKey, TValue>()
        {
            return EmptyDictionary<TKey, TValue>.Instance;
        }

        /// <summary>
        /// Empty array storage
        /// </summary>
        /// <typeparam name="T">type of item in array</typeparam>
        private static class EmptyArray<T>
        {
            /// <summary>
            /// Gets empty array instance
            /// </summary>
            internal static T[] Instance { get; } = new T[0];
        }

        /// <summary>
        /// Empty dictionary storage
        /// </summary>
        /// <typeparam name="TKey">type of dictionary key</typeparam>
        /// <typeparam name="TValue">type of dictionary value</typeparam>
        private static class EmptyDictionary<TKey, TValue>
        {
            /// <summary>
            /// Gets empty dictionary instance
            /// </summary>
            internal static IReadOnlyDictionary<TKey, TValue> Instance { get; } = new ReadOnlyDictionary<TKey, TValue>(new Dictionary<TKey, TValue>(0));
        }
    }
}
