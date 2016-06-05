//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ConfigurationXmlLoaderBaseUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.ConfigurationTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml;

    using Common.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.UnitTestHelper;

    /// <summary>
    /// ConfigurationXmlLoaderBase Unit Test
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ConfigurationXmlLoaderBaseUnitTest
    {
        #region CopyAttributes()
        /// <summary>
        /// Test copy attributes when element has no attribute
        /// </summary>
        [TestMethod]
        public void CopyAttributes_NoAttribute()
        {
            var elementName = TestHelper.CreateUniqueName("Root{0}");
            var sourceDoc = new XmlDocument();
            var sourceElement = sourceDoc.CreateElement(elementName);
            Assert.IsFalse(sourceElement.HasAttributes);
            var targetDoc = new XmlDocument();
            var targetlement = sourceDoc.CreateElement(elementName);
            Assert.IsFalse(targetlement.HasAttributes);

            var loader = new ConfigurationXmlLoaderBaseForTest();
            loader.CopyAttributesForTest(sourceElement, targetlement);
            Assert.IsFalse(sourceElement.HasAttributes);
            Assert.IsFalse(targetlement.HasAttributes);
        }

        /// <summary>
        /// Test copy attributes when element has attributes
        /// </summary>
        [TestMethod]
        public void CopyAttributes_HasAttributes()
        {
            var random = new Random();
            var attributeTestCountList = new int[]
            {
                1,
                2,
                random.Next(5) + 1
            };

            foreach (var attributeCount in attributeTestCountList)
            {
                var elementName = TestHelper.CreateUniqueName("Root{0}");
                var sourceDoc = new XmlDocument();
                var sourceElement = sourceDoc.CreateElement(elementName);
                var targetDoc = new XmlDocument();
                var targetlement = sourceDoc.CreateElement(elementName);

                var attributes = new Dictionary<string, string>(attributeCount);
                for (var i = 0; i < attributeCount; i++)
                {
                    var name = TestHelper.CreateUniqueName("name{0}");
                    var value = TestHelper.CreateUniqueName("value{0}");
                    attributes.Add(name, value);
                    var attribute = sourceDoc.CreateAttribute(name);
                    attribute.Value = value;
                    sourceElement.Attributes.Append(attribute);
                }

                var loader = new ConfigurationXmlLoaderBaseForTest();
                loader.CopyAttributesForTest(sourceElement, targetlement);

                // Make source was not changed
                var actual = TestHelper.GetAttributes(sourceElement);
                AssertHelper.AreEqual(attributes, actual);

                // Make attributes are copied
                actual = TestHelper.GetAttributes(targetlement);
                AssertHelper.AreEqual(attributes, actual);
            }
        }
        #endregion CopyAttributes()

        /// <summary>
        /// Dummy ConfigurationXmlLoaderBase for unit test
        /// </summary>
        private class ConfigurationXmlLoaderBaseForTest : ConfigurationXmlLoaderBase
        {
            /// <summary>
            /// Copy attribute from source XML to target XML
            /// </summary>
            /// <param name="sourceXml">source XML which contains attribute to copy</param>
            /// <param name="targetXml">target XML to receive attributes</param>
            public void CopyAttributesForTest(XmlElement sourceXml, XmlElement targetXml)
            {
                this.CopyAttributes(sourceXml, targetXml);
            }

            /// <summary>
            /// Get the new location
            /// </summary>
            /// <param name="currentLocation">the current location</param>
            /// <param name="relativeLocation">the relative location of current location</param>
            /// <returns>the merged location</returns>
            protected override string GetNewLocation(string currentLocation, string relativeLocation)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Load XML from specific location
            /// </summary>
            /// <param name="location">the location of XML to load</param>
            /// <returns>The XML which loaded from the specified location</returns>
            protected override XmlElement LoadXml(string location)
            {
                throw new NotImplementedException();
            }
        }
    }
}
