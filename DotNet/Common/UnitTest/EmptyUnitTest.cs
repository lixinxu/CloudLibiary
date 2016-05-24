//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EmptyUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest
{
    using System;
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
    }
}
