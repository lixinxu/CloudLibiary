//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Empty.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common
{
    /// <summary>
    /// Empty storage
    /// </summary>
    /// <remarks>
    /// Sometimes we need to return empty collection. Instead of creating new empty object over and over, we can re-use
    /// pre-created object to avoid performance and memory issue.
    /// </remarks>
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
    }
}
