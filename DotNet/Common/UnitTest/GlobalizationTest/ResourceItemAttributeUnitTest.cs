//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ResourceItemAttributeUnitTest.cs" company="CloudLibrary">
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
    /// Resource item attribute unit test
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ResourceItemAttributeUnitTest
    {
        /// <summary>
        /// Test constructor when passing null
        /// </summary>
        [TestMethod]
        public void ResourceItemAttribute_Constructor_Null()
        {
            var attribute = new ResourceItemAttribute();
            Assert.IsNull(attribute.Name);
            Assert.IsNull(attribute.Content);
        }

        /// <summary>
        /// Test constructor when passing content
        /// </summary>
        [TestMethod]
        public void ResourceItemAttribute_Constructor_HasContent()
        {
            var content = "Content" + Guid.NewGuid().ToString("N");
            var attribute = new ResourceItemAttribute(content);
            Assert.IsNull(attribute.Name);
            Assert.AreEqual(content, attribute.Content);
        }

        /// <summary>
        /// Test constructor when passing name and content
        /// </summary>
        [TestMethod]
        public void ResourceItemAttribute_Constructor_HasNameContent()
        {
            var name = "Name" + Guid.NewGuid().ToString("N");
            var content = "Content" + Guid.NewGuid().ToString("N");
            var attribute = new ResourceItemAttribute(name, content);
            Assert.AreEqual(name, attribute.Name);
            Assert.AreEqual(content, attribute.Content);
        }
    }
}
