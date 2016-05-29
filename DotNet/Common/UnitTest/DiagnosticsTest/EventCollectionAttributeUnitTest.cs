//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EventCollectionAttributeUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.DiagnosticsTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Common.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for event collection attribute
    /// </summary>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EventCollectionAttributeUnitTest
    {
        /// <summary>
        /// Test constructor by passing null
        /// </summary>
        [TestMethod]
        public void EventCollectionAttribute_NullSourceName()
        {
            var attribute = new EventCollectionAttribute(null);
            Assert.IsNull(attribute.SourceName);
        }

        /// <summary>
        /// Test constructor by passing empty string
        /// </summary>
        [TestMethod]
        public void EventCollectionAttribute_EmptySourceName()
        {
            var attribute = new EventCollectionAttribute(string.Empty);
            Assert.IsNull(attribute.SourceName);
        }

        /// <summary>
        /// Test constructor by passing blank string
        /// </summary>
        [TestMethod]
        public void EventCollectionAttribute_BlankSourceName()
        {
            var attribute = new EventCollectionAttribute("  \r\n \t  ");
            Assert.IsNull(attribute.SourceName);
        }

        /// <summary>
        /// Test constructor by passing name needs trim
        /// </summary>
        [TestMethod]
        public void EventCollectionAttribute_SourceNameNeedTrim()
        {
            var name = "name" + Guid.NewGuid().ToString("N");
            var attribute = new EventCollectionAttribute($"  \r\n \t{name}  ");
            Assert.AreEqual(name, attribute.SourceName);
        }

        /// <summary>
        /// Test constructor by passing name does not need trim
        /// </summary>
        [TestMethod]
        public void EventCollectionAttribute_SourceNameNoTrim()
        {
            var name = "name" + Guid.NewGuid().ToString("N");
            var attribute = new EventCollectionAttribute(name);
            Assert.AreEqual(name, attribute.SourceName);
        }
    }
}
