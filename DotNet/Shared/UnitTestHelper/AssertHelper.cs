//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AssertHelper.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace CloudLibrary.Shared.UnitTestHelper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Assert helper
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AssertHelper
    {
        /// <summary>
        /// Check whether two read-only dictionaries are equal;
        /// </summary>
        /// <typeparam name="TKey">type of key</typeparam>
        /// <typeparam name="TValue">type of value</typeparam>
        /// <param name="expected">expected dictionary</param>
        /// <param name="actual">actual dictionary</param>
        /// <param name="compareValueCallback">call back to compare two values</param>
        public static void AreEqual<TKey, TValue>(
            IReadOnlyDictionary<TKey, TValue> expected,
            IReadOnlyDictionary<TKey, TValue> actual,
            Action<TValue, TValue> compareValueCallback = null)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
                return;
            }

            Assert.AreEqual(expected.Count, actual.Count);

            foreach (var pair in expected)
            {
                TValue value;
                Assert.IsTrue(actual.TryGetValue(pair.Key, out value));
                if (compareValueCallback != null)
                {
                    compareValueCallback(pair.Value, value);
                }
                else
                {
                    Assert.AreEqual<TValue>(pair.Value, value);
                }
            }
        }
    }
}
