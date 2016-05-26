//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CollectionExtensionsUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Collection extensions unit test
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CollectionExtensionsUnitTest
    {
        /// <summary>
        /// Test when collection is null
        /// </summary>
        [TestMethod]
        public void CollectionExtensions_IsNullOrEmpty_Null()
        {
            int[] data = null;
            Assert.IsTrue(data.IsNullOrEmpty());
            Assert.IsTrue(data.IsReadOnlyNullOrEmpty());
            Assert.IsTrue(((ICollection)data).IsNullOrEmpty());
        }

        /// <summary>
        /// Test when collection is empty
        /// </summary>
        [TestMethod]
        public void CollectionExtensions_IsNullOrEmpty_Empty()
        {
            var data = new int[0];
            Assert.IsTrue(data.IsNullOrEmpty());
            Assert.IsTrue(data.IsReadOnlyNullOrEmpty());
            Assert.IsTrue(((ICollection)data).IsNullOrEmpty());
        }

        /// <summary>
        /// Test when collection contains value
        /// </summary>
        [TestMethod]
        public void CollectionExtensions_IsNullOrEmpty_HasItem()
        {
            var countList = new int[] { 1, 2, 10 };
            foreach (var count in countList)
            {
                var data = new int[count];
                Assert.IsFalse(data.IsNullOrEmpty());
                Assert.IsFalse(data.IsReadOnlyNullOrEmpty());
                Assert.IsFalse(((ICollection)data).IsNullOrEmpty());
            }
        }
    }
}
