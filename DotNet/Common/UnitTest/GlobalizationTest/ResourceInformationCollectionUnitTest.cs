//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ResourceInformationCollectionUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.GlobalizationTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Reflection.Emit;

    using Common.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.UnitTestHelper;

    /// <summary>
    /// Unit test for ResourceInformationCollection
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ResourceInformationCollectionUnitTest
    {
        /// <summary>
        /// Normal test. All data are set
        /// </summary>
        [TestMethod]
        public void ResourceInformationCollection_Normal()
        {
            var storageName = TestHelper.CreateUniqueName("Storage{0}");
            var itemCount = 5;
            var testItemDataList = new Tuple<string, int, string, string>[itemCount];
            var type = CreateResourceEnumeratorType(testItemDataList, true, storageName, true, true, true);
            Test(type, storageName, testItemDataList);
        }

        /// <summary>
        /// Storage name is null in collection attribute
        /// </summary>
        [TestMethod]
        public void ResourceInformationCollection_NullStorageName()
        {
            string storageName = null;
            var itemCount = 5;
            var testItemDataList = new Tuple<string, int, string, string>[itemCount];
            var type = CreateResourceEnumeratorType(testItemDataList, true, storageName, true, true, true);
            Test(type, storageName, testItemDataList);
        }

        /// <summary>
        /// Resource name is null in item resource attribute
        /// </summary>
        [TestMethod]
        public void ResourceInformationCollection_NullResourceName()
        {
            var storageName = TestHelper.CreateUniqueName("Storage{0}");
            var itemCount = 5;
            var testItemDataList = new Tuple<string, int, string, string>[itemCount];
            var type = CreateResourceEnumeratorType(testItemDataList, true, storageName, true, false, true);
            Test(type, storageName, testItemDataList);
        }

        /// <summary>
        /// Content is null in item resource attribute
        /// </summary>
        [TestMethod]
        public void ResourceInformationCollection_NUllContent()
        {
            var storageName = TestHelper.CreateUniqueName("Storage{0}");
            var itemCount = 5;
            var testItemDataList = new Tuple<string, int, string, string>[itemCount];
            var type = CreateResourceEnumeratorType(testItemDataList, true, storageName, true, true, false);
            Test(type, storageName, testItemDataList);
        }

        /// <summary>
        /// No item resource attribute
        /// </summary>
        [TestMethod]
        public void ResourceInformationCollection_NoItemAttribute()
        {
            var storageName = TestHelper.CreateUniqueName("Storage{0}");
            var itemCount = 5;
            var testItemDataList = new Tuple<string, int, string, string>[itemCount];
            var type = CreateResourceEnumeratorType(testItemDataList, true, storageName, false, false, false);
            Test(type, storageName, testItemDataList);
        }

        /// <summary>
        /// Test pass null to get content
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResourceInformationCollection_GetContent_NullResourceName()
        {
            var informationCollection = new ResourceInformationCollection<TestResource>();
            var content = informationCollection.GetContent(null);
        }

        /// <summary>
        /// Test resource enumerator wrapper
        /// </summary>
        /// <param name="type">type of resource enumerator</param>
        /// <param name="storageName">storage name</param>
        /// <param name="testItemDataList">test item data list</param>
        private static void Test(Type type, string storageName, Tuple<string, int, string, string>[] testItemDataList)
        {
            var methodInformation = typeof(ResourceInformationCollectionUnitTest)
                .GetMethod(nameof(TestGeneric), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(type);
            methodInformation.Invoke(null, new object[] { storageName, testItemDataList });
        }

        /// <summary>
        /// Test resource enumerator
        /// </summary>
        /// <typeparam name="T">type of enumerator</typeparam>
        /// <param name="storageName">storage name</param>
        /// <param name="testItemDataList">test item data list</param>
        private static void TestGeneric<T>(string storageName, Tuple<string, int, string, string>[] testItemDataList)
        {
            var informationCollection = new ResourceInformationCollection<T>();
            string content;
            string resourceName;
            KeyValuePair<string, string> pair;
            T id;

            Assert.AreEqual(storageName ?? typeof(T).Name, informationCollection.StorageName);
            foreach (var testItemData in testItemDataList)
            {
                resourceName = testItemData.Item3 ?? testItemData.Item1;
                content = informationCollection.GetContent(resourceName);
                Assert.AreEqual(testItemData.Item4, content);

                id = (T)Enum.ToObject(typeof(T), testItemData.Item2);
                Assert.IsTrue(informationCollection.TryGetResourceInformation(id, out pair));
                Assert.AreEqual(resourceName, pair.Key);
                Assert.AreEqual(testItemData.Item4, pair.Value);
            }

            //Test bad resource name
            resourceName = TestHelper.CreateUniqueName("ResourceName{0}");
            content = informationCollection.GetContent(resourceName);
            Assert.IsNull(content);

            //Test bad id
            id = (T)(object)(testItemDataList.Length * 5);
            Assert.IsFalse(informationCollection.TryGetResourceInformation(id, out pair));
        }

        /// <summary>
        /// Create a test resource enumerator
        /// </summary>
        /// <param name="itemData">item data list</param>
        /// <param name="hasCollectionAttribute">need to add collection attribute</param>
        /// <param name="storageName">storage name for collection attribute. if no attribute, this value will be ignored</param>
        /// <param name="hasResourceName">create resource name</param>
        /// <param name="hasContent">create content</param>
        /// <returns>resource enumerator type</returns>
        private static Type CreateResourceEnumeratorType(
            Tuple<string, int, string, string>[] itemData,
            bool hasCollectionAttribute,
            string storageName,
            bool hasItemAttribute,
            bool hasResourceName,
            bool hasContent)
        {
            var random = new Random();
            var modelBuilder = TestHelper.CreateModuleBuilder();
            var enumBuilder = modelBuilder.DefineEnum(TestHelper.CreateUniqueName("Enumerator{0}"), TypeAttributes.Public, typeof(int));
            if (hasCollectionAttribute)
            {
                var collectionAttributeBuilder = new CustomAttributeBuilder(
                    typeof(ResourceCollectionAttribute).GetConstructor(new Type[] { typeof(string) }),
                    new object[] { storageName });
                enumBuilder.SetCustomAttribute(collectionAttributeBuilder);
            }

            var values = new HashSet<int>();
            var itemConstructorInformation = typeof(ResourceItemAttribute).GetConstructor(new Type[] { typeof(string), typeof(string) });
            for (var i = 0; i < itemData.Length; i++)
            {
                var enumName = TestHelper.CreateUniqueName("Item{0}");
                int value;
                do
                {
                    value = Math.Abs(random.Next(itemData.Length * 2));
                }
                while (values.Contains(value));
                values.Add(value);

                string resourceName = (hasItemAttribute && hasResourceName) ? TestHelper.CreateUniqueName("ResourceName{0}") : null;
                string resourceValue = (hasItemAttribute && hasContent) ? TestHelper.CreateUniqueName("ResourceValue{0}") : null;
                itemData[i] = new Tuple<string, int, string, string>(enumName, value, resourceName, resourceValue);

                var itemBuilder = enumBuilder.DefineLiteral(enumName, value);
                if (hasItemAttribute)
                {
                    var itemAttributeBuilder = new CustomAttributeBuilder(itemConstructorInformation, new object[] { resourceName, resourceValue });
                    itemBuilder.SetCustomAttribute(itemAttributeBuilder);
                }
            }

            return enumBuilder.CreateType();
        }

        /// <summary>
        /// Resource enumerator for test
        /// </summary>
        public enum TestResource
        {
        }
    }
}