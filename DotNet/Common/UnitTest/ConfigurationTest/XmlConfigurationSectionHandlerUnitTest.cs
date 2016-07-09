//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="XmlConfigurationSectionHandlerUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.ConfigurationTest
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.UnitTestHelper;

    /// <summary>
    /// XmlConfigurationSectionHandler Unit Test
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class XmlConfigurationSectionHandlerUnitTest : ConfigurationXmlLoaderUnitTestBase
    {
        /// <summary>
        /// Test section does not exist
        /// </summary>
        [TestMethod]
        public void XmlConfigurationSectionHandler_SectionDoesNotExist()
        {
            var actual = ConfigurationManager.GetSection(TestHelper.CreateUniqueName("NotExist{0}"));
            Assert.IsNull(actual);
        }

        /// <summary>
        /// Test empty section
        /// </summary>
        [TestMethod]
        public void XmlConfigurationSectionHandler_EmptySection()
        {
            var sectionName = "empty";
            var actual = ConfigurationManager.GetSection(sectionName) as XmlElement;
            var document = new XmlDocument();
            var expected = document.CreateElement(sectionName);
            document.AppendChild(expected);
            AssertHelper.AreXmlEqual(expected, actual);
        }

        /// <summary>
        /// Test empty section
        /// </summary>
        [TestMethod]
        public void XmlConfigurationSectionHandler_SectionHasChild()
        {
            var sectionName = "test";
            var actual = ConfigurationManager.GetSection(sectionName) as XmlElement;
            Assert.IsNotNull(actual);
            var document = new XmlDocument();
            document.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            var root = document.DocumentElement;
            var expected = document.SelectSingleNode($"//{root.Name}/{sectionName}") as XmlElement;
            Assert.IsNotNull(expected);
            AssertHelper.AreXmlEqual(expected, actual);
        }
    }
}
