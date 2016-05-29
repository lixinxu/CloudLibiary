//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ResourceCollectionAttributeUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.GlobalizationTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Common.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test resource collection attribute
    /// </summary>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ResourceCollectionAttributeUnitTest
    {
        /// <summary>
        /// Test passing null as storage name to constructor
        /// </summary>
        [TestMethod]
        public void ResourceCollectionAttribute_NullName()
        {
            var attribute = new ResourceCollectionAttribute(null);
            Assert.IsNull(attribute.StorageName);
        }

        /// <summary>
        /// Test passing empty as storage name to constructor
        /// </summary>
        [TestMethod]
        public void ResourceCollectionAttribute_EmptyName()
        {
            var attribute = new ResourceCollectionAttribute(string.Empty);
            Assert.IsNull(attribute.StorageName);
        }

        /// <summary>
        /// Test passing blank as storage name to constructor
        /// </summary>
        [TestMethod]
        public void ResourceCollectionAttribute_BlankName()
        {
            var attribute = new ResourceCollectionAttribute(" \r\n ");
            Assert.IsNull(attribute.StorageName);
        }

        /// <summary>
        /// Test passing storage name needs trim to constructor
        /// </summary>
        [TestMethod]
        public void ResourceCollectionAttribute_NameNeedsTrim()
        {
            var name = "Name" + Guid.NewGuid().ToString("N");
            var attribute = new ResourceCollectionAttribute($" \r\n {name}  ");
            Assert.AreEqual(name, attribute.StorageName);
        }

        /// <summary>
        /// Test passing storage name which does not need trim to constructor
        /// </summary>
        [TestMethod]
        public void ResourceCollectionAttribute_NameNoNeedTrim()
        {
            var name = "Name" + Guid.NewGuid().ToString("N");
            var attribute = new ResourceCollectionAttribute(name);
            Assert.AreEqual(name, attribute.StorageName);
        }
    }
}
