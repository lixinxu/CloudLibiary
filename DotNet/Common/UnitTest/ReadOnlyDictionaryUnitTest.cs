//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ReadOnlyDictionaryUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.UnitTestHelper;

    /// <summary>
    /// ReadOnlyDictionary Unit Test
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ReadOnlyDictionaryUnitTest
    {
        /// <summary>
        /// Test dictionary constructor by passing null
        /// </summary>
        [TestMethod]
        public void ReadOnlyDictionary_NullReference()
        {
            TestNullReferenceInConstructor<string, object>();
            TestNullReferenceInConstructor<int, Guid>();
        }

        /// <summary>
        /// Test dictionary count
        /// </summary>
        [TestMethod]
        public void ReadOnlyDictionary_TestCount()
        {
            var random = new Random();
            var testCount = 5;
            for (var i = 0; i < testCount; i++)
            {
                var count = (i == 0) ? 0 : random.Next(10) + 1;
                var original = new Dictionary<Guid, int>(count);
                for (var j = 0; j < count; j++)
                {
                    original.Add(Guid.NewGuid(), random.Next());
                }

                var dictionary = new ReadOnlyDictionary<Guid, int>(original);
                Assert.AreEqual(count, dictionary.Count);
            }
        }

        /// <summary>
        /// Test dictionary keys
        /// </summary>
        [TestMethod]
        public void ReadOnlyDictionary_TestKeys()
        {
            var random = new Random();
            var testCount = 5;
            for (var i = 0; i < testCount; i++)
            {
                var count = (i == 0) ? 0 : random.Next(10) + 1;
                var original = new Dictionary<Guid, int>(count);
                var keys = new HashSet<Guid>();
                for (var j = 0; j < count; j++)
                {
                    var key = Guid.NewGuid();
                    original.Add(key, random.Next());
                    keys.Add(key);
                }

                Assert.AreEqual(count, keys.Count);
                var dictionary = new ReadOnlyDictionary<Guid, int>(original);
                var keyCount = 0;
                foreach (var key in dictionary.Keys)
                {
                    keyCount++;
                    Assert.IsTrue(keys.Contains(key));
                }

                Assert.AreEqual(count, keyCount);
            }
        }

        /// <summary>
        /// Test dictionary values
        /// </summary>
        [TestMethod]
        public void ReadOnlyDictionary_TestValues()
        {
            var random = new Random();
            var testCount = 5;
            for (var i = 0; i < testCount; i++)
            {
                var count = (i == 0) ? 0 : random.Next(10) + 1;
                var original = new Dictionary<Guid, string>(count);
                var values = new HashSet<string>();
                for (var j = 0; j < count; j++)
                {
                    var value = TestHelper.CreateUniqueName("value{0}");
                    original.Add(Guid.NewGuid(), value);
                    values.Add(value);
                }

                Assert.AreEqual(count, values.Count);
                var dictionary = new ReadOnlyDictionary<Guid, string>(original);
                var valueCount = 0;
                foreach (var value in dictionary.Values)
                {
                    valueCount++;
                    Assert.IsTrue(values.Contains(value));
                }

                Assert.AreEqual(count, valueCount);
            }
        }

        /// <summary>
        /// Test dictionary find by key
        /// </summary>
        [TestMethod]
        public void ReadOnlyDictionary_FindByKey()
        {
            var random = new Random();
            var testCount = 5;
            for (var i = 0; i < testCount; i++)
            {
                var count = (i == 0) ? 0 : random.Next(10) + 1;
                var original = new Dictionary<Guid, int>(count);
                for (var j = 0; j < count; j++)
                {
                    original.Add(Guid.NewGuid(), random.Next());
                }

                // create a copy to make sure we can detect if read-only dictionary changed the original data.
                var copy = new Dictionary<Guid, int>(original); 
                var dictionary = new ReadOnlyDictionary<Guid, int>(copy);
                foreach (var pair in original)
                {
                    var value = dictionary[pair.Key];
                    Assert.AreEqual(pair.Value, value);
                }

                // try key not exist
                bool hasException = false;
                try
                {
                    var value = dictionary[Guid.NewGuid()];
                }
                catch (KeyNotFoundException)
                {
                    hasException = true;
                }

                Assert.IsTrue(hasException);
            }
        }

        /// <summary>
        /// Test dictionary contains key
        /// </summary>
        [TestMethod]
        public void ReadOnlyDictionary_ContainsKey()
        {
            var random = new Random();
            var testCount = 5;
            for (var i = 0; i < testCount; i++)
            {
                var count = (i == 0) ? 0 : random.Next(10) + 1;
                var original = new Dictionary<Guid, int>(count);
                var keys = new HashSet<Guid>();
                for (var j = 0; j < count; j++)
                {
                    var key = Guid.NewGuid();
                    original.Add(key, random.Next());
                    keys.Add(key);
                }

                Assert.AreEqual(count, keys.Count);
                var dictionary = new ReadOnlyDictionary<Guid, int>(original);
                foreach (var key in keys)
                {
                    Assert.IsTrue(dictionary.ContainsKey(key));
                }

                Assert.IsFalse(dictionary.ContainsKey(Guid.NewGuid()));
            }
        }

        /// <summary>
        /// Test dictionary count
        /// </summary>
        [TestMethod]
        public void ReadOnlyDictionary_GetEnumerator_Generic()
        {
            var random = new Random();
            var testCount = 5;
            for (var i = 0; i < testCount; i++)
            {
                var count = (i == 0) ? 0 : random.Next(10) + 1;
                var original = new Dictionary<Guid, int>(count);
                for (var j = 0; j < count; j++)
                {
                    original.Add(Guid.NewGuid(), random.Next());
                }

                // Create a copy to make sure we can detect if read-only dictionary changed the original data.
                var copy = new Dictionary<Guid, int>(original);
                var dictionary = new ReadOnlyDictionary<Guid, int>(copy);
                var itemCount = 0;
                foreach (var pair in dictionary)
                {
                    Assert.AreEqual(original[pair.Key], pair.Value);
                    itemCount++;
                }

                Assert.AreEqual(count, itemCount);
            }
        }

        /// <summary>
        /// Test dictionary try get value
        /// </summary>
        [TestMethod]
        public void ReadOnlyDictionary_TryGetValue()
        {
            var random = new Random();
            var testCount = 5;
            for (var i = 0; i < testCount; i++)
            {
                var count = (i == 0) ? 0 : random.Next(10) + 1;
                var original = new Dictionary<Guid, int>(count);
                for (var j = 0; j < count; j++)
                {
                    original.Add(Guid.NewGuid(), random.Next());
                }

                // Create a copy to make sure we can detect if read-only dictionary changed the original data.
                var copy = new Dictionary<Guid, int>(original);
                var dictionary = new ReadOnlyDictionary<Guid, int>(copy);
                foreach (var pair in original)
                {
                    int value;
                    Assert.IsTrue(dictionary.TryGetValue(pair.Key, out value));
                    Assert.AreEqual(pair.Value, value);
                }

                int notExist;
                Assert.IsFalse(dictionary.TryGetValue(Guid.NewGuid(), out notExist));
            }
        }

        /// <summary>
        /// Test dictionary count
        /// </summary>
        [TestMethod]
        public void ReadOnlyDictionary_GetEnumerator()
        {
            var random = new Random();
            var testCount = 5;
            for (var i = 0; i < testCount; i++)
            {
                var count = (i == 0) ? 0 : random.Next(10) + 1;
                var original = new Dictionary<Guid, int>(count);
                for (var j = 0; j < count; j++)
                {
                    original.Add(Guid.NewGuid(), random.Next());
                }

                // Create a copy to make sure we can detect if read-only dictionary changed the original data.
                var copy = new Dictionary<Guid, int>(original);
                var dictionary = new ReadOnlyDictionary<Guid, int>(copy);
                var itemCount = 0;

                var enumerable = dictionary as IEnumerable;
                Assert.IsNotNull(enumerable);
                foreach (KeyValuePair<Guid, int> pair in enumerable)
                {
                    Assert.AreEqual(original[pair.Key], pair.Value);
                    itemCount++;
                }

                Assert.AreEqual(count, itemCount);
            }
        }

        /// <summary>
        /// Test constructor when passing null
        /// </summary>
        /// <typeparam name="TKey">type of key</typeparam>
        /// <typeparam name="TValue">type of value</typeparam>
        private static void TestNullReferenceInConstructor<TKey, TValue>()
        {
            ArgumentNullException exception = null;
            try
            {
                var dictionary = new ReadOnlyDictionary<TKey, TValue>(null);
            }
            catch (ArgumentNullException ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
            var parameterName = GetConstructorParameterName<TKey, TValue>();
            Assert.IsTrue(exception.Message.Contains(parameterName));
        }

        /// <summary>
        /// Get read-only dictionary constructor parameter name
        /// </summary>
        /// <typeparam name="TKey">type of key</typeparam>
        /// <typeparam name="TValue">type of value</typeparam>
        /// <returns>name of constructor parameter</returns>
        private static string GetConstructorParameterName<TKey, TValue>()
        {
            var type = typeof(ReadOnlyDictionary<TKey, TValue>);
            var constructorInformation = type.GetConstructor(new Type[] { typeof(IDictionary<TKey, TValue>) });
            Assert.IsNotNull(constructorInformation);
            var parameters = constructorInformation.GetParameters();
            Assert.IsNotNull(parameters);
            Assert.AreEqual(1, parameters.Length);
            return parameters[0].Name;
        }
    }
}
