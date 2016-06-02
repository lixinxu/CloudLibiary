//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StringExtensionsUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest
{
    using System;
    using System.Security;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// String extensions unit test
    /// </summary>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
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

        #region SecureString
        /// <summary>
        /// null string to secure string test
        /// </summary>
        [TestMethod]
        public void StringExtensions_ToSecureString_Null()
        {
            string value = null;
            Assert.IsNull(value.ToSecureString());
        }

        /// <summary>
        /// null secure string to string test
        /// </summary>
        [TestMethod]
        public void StringExtensions_ToPlainText_Null()
        {
            SecureString value = null;
            Assert.IsNull(value.ToPlainText());
        }

        /// <summary>
        /// null secure string to string test
        /// </summary>
        [TestMethod]
        public void StringExtensions_ToSecureString_ToPlainText_Test()
        {
            var value = "data" + Guid.NewGuid().ToString();
            SecureString secure = value.ToSecureString();
            Assert.IsNotNull(secure);

            var actual = secure.ToPlainText();
            Assert.IsNotNull(actual);
            Assert.AreEqual(value, actual);
        }

        #endregion SecureString
    }
}
