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
    using System.Xml;

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
        public static void AreReadOnlyDictionaryEqual<TKey, TValue>(
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

            if (compareValueCallback == null)
            {
                compareValueCallback = (expectedItem, actualItem) => Assert.AreEqual(expectedItem, actualItem);
            }

            foreach (var pair in expected)
            {
                TValue value;
                Assert.IsTrue(actual.TryGetValue(pair.Key, out value));
                compareValueCallback(pair.Value, value);
            }
        }

        /// <summary>
        /// check whether two read-only list are equal
        /// </summary>
        /// <typeparam name="T">type of list item</typeparam>
        /// <param name="expected">expected list</param>
        /// <param name="actual">actual list</param>
        /// <param name="compareValueCallback">compare list item callback</param>
        public static void AreReadOnlyListEqual<T>(
            IReadOnlyList<T> expected,
            IReadOnlyList<T> actual,
            Action<T, T> compareValueCallback = null)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
                return;
            }

            Assert.AreEqual(expected.Count, actual.Count);
            if (compareValueCallback == null)
            {
                compareValueCallback = (expectedItem, actualItem) => Assert.AreEqual(expectedItem, actualItem);
            }

            for (var i = 0; i < expected.Count; i++)
            {
                compareValueCallback(expected[i], actual[i]);
            }
        }

        /// <summary>
        /// Are XML element equal
        /// </summary>
        /// <param name="expected">expected XML element</param>
        /// <param name="actual">actual XML element</param>
        public static void AreXmlEqual(XmlElement expected, XmlElement actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
                return;
            }

            Assert.AreEqual(expected.HasAttributes, actual.HasAttributes);
            if (expected.HasAttributes)
            {
                Assert.AreEqual(expected.Attributes.Count, actual.Attributes.Count);
                for (var i = 0; i < expected.Attributes.Count; i++)
                {
                    var expectedAttribute = expected.Attributes[i];
                    var actualAttribute = actual.Attributes[expectedAttribute.Name];
                    Assert.IsNotNull(actualAttribute);
                    Assert.AreEqual(expectedAttribute.Value, actualAttribute.Value);
                }
            }

            AreReadOnlyListEqual(TestHelper.GetElements(expected), TestHelper.GetElements(actual), AreXmlEqual);
        }
    }
}
