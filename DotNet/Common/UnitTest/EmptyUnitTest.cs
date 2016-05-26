//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EmptyUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test Empty class
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EmptyUnitTest
    {
        /// <summary>
        /// Test GetArray()
        /// </summary>
        [TestMethod]
        public void Empty_GetArray_Test()
        {
            TestEmptyArray<int>();
            TestEmptyArray<string>();
        }

        /// <summary>
        /// Test GetDictionary()
        /// </summary>
        [TestMethod]
        public void Empty_Dictionary_Test()
        {
            TestEmptyDictionaty<int, string>();
            TestEmptyDictionaty<string, object>();
        }

        /// <summary>
        /// Helper for testing empty array
        /// </summary>
        /// <typeparam name="T">type of item in array</typeparam>
        private static void TestEmptyArray<T>()
        {
            var instance1 = Empty.GetArray<T>();
            Assert.IsNotNull(instance1);

            var instance2 = Empty.GetArray<T>();
            Assert.IsNotNull(instance2);

            Assert.IsTrue(object.ReferenceEquals(instance1, instance2));
        }

        /// <summary>
        /// Helper for testing empty dictionary
        /// </summary>
        /// <typeparam name="TKey">type of dictionary key</typeparam>
        /// <typeparam name="TValue">type of dictionary value</typeparam>
        private static void TestEmptyDictionaty<TKey, TValue>()
        {
            // Singleton test
            var instance1 = Empty.GetDictionary<TKey, TValue>();
            Assert.IsNotNull(instance1);
            var instance2 = Empty.GetDictionary<TKey, TValue>();
            Assert.IsNotNull(instance2);
            Assert.IsTrue(object.ReferenceEquals(instance1, instance2));

            // Count
            Assert.AreEqual(0, instance1.Count);

            // enumerator
            var hasValue = false;
            foreach (var pair in instance1)
            {
                hasValue = true;
            }

            Assert.IsFalse(hasValue);

            // cast test
            Assert.IsNull(instance1 as IDictionary<TKey, TValue>);
        }
    }
}
