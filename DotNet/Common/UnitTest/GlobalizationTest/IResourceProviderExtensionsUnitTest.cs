//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IResourceProviderExtensionsUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.GlobalizationTest
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Common.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.UnitTestHelper;

    /// <summary>
    /// Unit test for resource provider extension
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class IResourceProviderExtensionsUnitTest
    {
        /// <summary>
        /// default resource storage name
        /// </summary>
        private const string StorageName = "ResourceStore";

        /// <summary>
        /// default resource name of first item
        /// </summary>
        private const string ResourceName1 = "NameOne";

        /// <summary>
        /// default resource value of first item
        /// </summary>
        private const string ResourceData1 = "DataOne";

        /// <summary>
        /// default resource name of second item
        /// </summary>
        private const string ResourceName2 = "NameTwo";

        /// <summary>
        /// default resource value of second item
        /// </summary>
        private const string ResourceData2 = "DataTwo";

        /// <summary>
        /// Dummy resource enumerator for test
        /// </summary>
        [ResourceCollection(StorageName)]
        public enum ResourceForTest
        {
            /// <summary>
            /// first item
            /// </summary>
            [ResourceItem(ResourceName1, ResourceData1)]
            Item1,

            /// <summary>
            /// second item
            /// </summary>
            [ResourceItem(ResourceName2, ResourceData2)]
            Item2,
        }

        /// <summary>
        /// Test get resource by id
        /// </summary>
        [TestMethod]
        public void IResourceProviderExtensions_GetResourceById()
        {
            var data1 = TestHelper.CreateUniqueName("Data1{0}");
            var data2 = TestHelper.CreateUniqueName("Data2{0}");
            var provider = new ResourceProviderForTest(StorageName, data1, data2);
            Assert.AreEqual(data1, provider.GetResource(ResourceForTest.Item1));
            Assert.AreEqual(data2, provider.GetResource(ResourceForTest.Item2));

            provider = new ResourceProviderForTest(TestHelper.CreateUniqueName("Storage{0}"), data1, data2);
            Assert.AreEqual(ResourceData1, provider.GetResource(ResourceForTest.Item1));
            Assert.AreEqual(ResourceData2, provider.GetResource(ResourceForTest.Item2));
        }

        /// <summary>
        /// Test get resource by name
        /// </summary>
        [TestMethod]
        public void IResourceProviderExtensions_GetResourceByName()
        {
            var data1 = TestHelper.CreateUniqueName("Data1{0}");
            var data2 = TestHelper.CreateUniqueName("Data2{0}");
            var provider = new ResourceProviderForTest(StorageName, data1, data2);
            Assert.AreEqual(data1, provider.GetResource(ResourceForTest.Item1));
            Assert.AreEqual(data2, provider.GetResource(ResourceForTest.Item2));

            provider = new ResourceProviderForTest(TestHelper.CreateUniqueName("Storage{0}"), data1, data2);
            Assert.AreEqual(ResourceData1, provider.GetResource<ResourceForTest>(ResourceName1));
            Assert.AreEqual(ResourceData2, provider.GetResource<ResourceForTest>(ResourceName2));
        }

        /// <summary>
        /// Dummy resource provider for test
        /// </summary>
        private class ResourceProviderForTest : IResourceProvider
        {
            /// <summary>
            /// resource storage
            /// </summary>
            private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> resources;

            /// <summary>
            /// Initializes a new instance of the <see cref="ResourceProviderForTest" /> class.
            /// </summary>
            /// <param name="storageName">storage name</param>
            /// <param name="data1">first data</param>
            /// <param name="data2">second data</param>
            public ResourceProviderForTest(string storageName, string data1, string data2)
            {
                this.resources = new Dictionary<string, IReadOnlyDictionary<string, string>>()
                {
                    {
                        storageName,
                        new Dictionary<string, string>()
                        {
                            { ResourceName1, data1 },
                            { ResourceName2, data2 },
                        }
                    },
                };
            }

            /// <summary>
            /// Get resource by name
            /// </summary>
            /// <param name="storageName">storage name</param>
            /// <param name="key">resource name/key</param>
            /// <returns>resource if found</returns>
            public string GetResource(string storageName, string key)
            {
                IReadOnlyDictionary<string, string> storage;
                if (this.resources.TryGetValue(storageName, out storage))
                {
                    string value;
                    if (storage.TryGetValue(key, out value))
                    {
                        return value;
                    }
                }

                return null;
            }
        }
    }
}
