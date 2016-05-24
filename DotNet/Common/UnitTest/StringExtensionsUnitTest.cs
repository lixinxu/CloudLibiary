//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StringExtensionsUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// String extensions unit test
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class StringExtensionsUnitTest
    {
        #region SafeTrim()
        /// <summary>
        /// SafeTrim by given null
        /// </summary>
        [TestMethod]
        public void StringExtensions_SafeTrim_Null()
        {
            string value = null;
            Assert.IsNull(value.SafeTrim());
        }

        /// <summary>
        /// SafeTrim by given empty data
        /// </summary>
        [TestMethod]
        public void StringExtensions_SafeTrim_Empty()
        {
            var value = string.Empty;
            Assert.IsNull(value.SafeTrim());
        }

        /// <summary>
        /// SafeTrim by given blank data
        /// </summary>
        [TestMethod]
        public void StringExtensions_SafeTrim_Blank()
        {
            var value = "  \r\n  \t ";
            Assert.IsNull(value.SafeTrim());
        }

        /// <summary>
        /// SafeTrim a value which needs Trim
        /// </summary>
        [TestMethod]
        public void StringExtensions_SafeTrim_NeedTrim()
        {
            var value = "value";
            var test = "  " + value + " ";
            Assert.AreEqual(value, test.SafeTrim());
        }

        /// <summary>
        /// SafeTrim a value which does not need Trim
        /// </summary>
        [TestMethod]
        public void StringExtensions_SafeTrim_NoTrim()
        {
            var value = "value";
            Assert.AreEqual(value, value.SafeTrim());
        }
        #endregion SafeTrim()

        #region ToSecureString()
        #endregion ToSecureString()
    }
}
