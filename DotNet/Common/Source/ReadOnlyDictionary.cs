//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ReadOnlyDictionary.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Read-Only dictionary
    /// </summary>
    /// <typeparam name="TKey">type of dictionary key</typeparam>
    /// <typeparam name="TValue">type of dictionary value</typeparam>
    /// <remarks>
    /// <para>There is no read-only dictionary class defined in .Net framework today. In most cases, Dictionary{T,V} is
    /// used "as" read-only because it supports IReadOnlyDictionary{T,V} interface. Unfortunately, people can "cast" it 
    /// back to "IDictionary" and change the data. This introduce a security problem. For example, the system defined
    /// a dictionary to manage white list. Hacker can cast the list and add themselves so they can bypass check.</para>
    /// <para>To avoid this, we need a "real" read-only dictionary.</para>
    /// </remarks>
    /// <example>
    /// <![CDATA[
    /// public IReadOnlyDictionary<TKey, TValue> LoadWhiteList()
    /// {
    ///     Dictionary<string, UserType> whitelist = GetWhiteListFromStorage();
    ///     return new ReadOnlyDictionary<string, UserType>(whitelist);
    /// }
    /// ]]>
    /// </example>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    public class ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        /// <summary>
        /// Original dictionary
        /// </summary>
        private IDictionary<TKey, TValue> reference;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyDictionary{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="reference">original dictionary</param>
        /// <remarks>
        /// <para>The reference should not be null.</para>
        /// <para>if want to use empty read-only dictionary, instead of creating them, you can use Empty.GetDictionary</para>
        /// </remarks>
        /// <seealso cref="IReadOnlyDictionary{TKey, TValue}"/>
        /// <seealso cref="Empty.GetDictionary{TKey, TValue}"/>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            this.reference = reference;
        }

        /// <summary>
        /// Gets number of items in the dictionary
        /// </summary>
        public int Count
        {
            get
            {
                return this.reference.Count;
            }
        }

        /// <summary>
        /// Gets keys of the dictionary
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                return this.reference.Keys;
            }
        }

        /// <summary>
        /// Gets dictionary values
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                return this.reference.Values;
            }
        }

        /// <summary>
        /// Gets the element that has the specified key in the read-only dictionary.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>The element that has the specified key in the read-only dictionary.</returns>
        public TValue this[TKey key]
        {
            get
            {
                return this.reference[key];
            }
        }

        /// <summary>
        /// Check whether the dictionary contains key
        /// </summary>
        /// <param name="key">key to check</param>
        /// <returns>true if dictionary contains key. false if not </returns>
        public bool ContainsKey(TKey key)
        {
            return this.reference.ContainsKey(key);
        }

        /// <summary>
        /// Get dictionary enumerator 
        /// </summary>
        /// <returns>dictionary enumerator instance</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.reference.GetEnumerator();
        }

        /// <summary>
        /// Try to get value from dictionary by given key
        /// </summary>
        /// <param name="key">key to locate the value</param>
        /// <param name="value">value in the dictionary</param>
        /// <returns>true if found, false it not</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.reference.TryGetValue(key, out value);
        }

        /// <summary>
        /// Get dictionary enumerator 
        /// </summary>
        /// <returns>dictionary enumerator instance</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            var enumerable = this.reference as IEnumerable;
            return enumerable.GetEnumerator();
        }
    }
}
