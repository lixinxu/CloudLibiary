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
    public class ConfigurationXmlLoaderBaseUnitTest : ConfigurationXmlLoaderUnitTestBase
    {
        #region CopyAttributes()
        /// <summary>
        /// Test copy attributes when element has no attribute
        /// </summary>
        [TestMethod]
        public void ConfigurationXmlLoaderBase_CopyAttributes_NoAttribute()
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
        public void ConfigurationXmlLoaderBase_CopyAttributes_HasAttributes()
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
                AssertHelper.AreReadOnlyDictionaryEqual(attributes, actual);

                // Make attributes are copied
                actual = TestHelper.GetAttributes(targetlement);
                AssertHelper.AreReadOnlyDictionaryEqual(attributes, actual);
            }
        }
        #endregion CopyAttributes()

        #region CopyChildren()
        /// <summary>
        /// Test copy children without child
        /// </summary>
        [TestMethod]
        public void ConfigurationXmlLoaderBase_CopyChildren_NoChild()
        {
            var document = new XmlDocument();
            var location = TestHelper.CreateUniqueName("Location{0}");
            var source = document.CreateElement(TestHelper.CreateUniqueName("Root{0}"));
            document.AppendChild(source);

            document = new XmlDocument();
            var target = document.CreateElement(TestHelper.CreateUniqueName("Host{0}"));
            document.AppendChild(target);

            var loader = new ConfigurationXmlLoaderBaseForTest();
            loader.CopyChildrenForTest(source, target, location, ConfigurationXmlLoaderBase.DefaultIncludeElementName);
            Assert.IsFalse(target.HasChildNodes);
        }

        /// <summary>
        /// Test copy children with simple element
        /// </summary>
        [TestMethod]
        public void ConfigurationXmlLoaderBase_CopyChildren_SimpleChildNoAttribute()
        {
            var document = new XmlDocument();
            var location = TestHelper.CreateUniqueName("Location{0}");
            var source = document.CreateElement(TestHelper.CreateUniqueName("Root{0}"));
            document.AppendChild(source);
            var name = TestHelper.CreateUniqueName("Child{0}");
            source.AppendChild(document.CreateElement(name));

            document = new XmlDocument();
            var target = document.CreateElement(TestHelper.CreateUniqueName("Host{0}"));
            document.AppendChild(target);

            var loader = new ConfigurationXmlLoaderBaseForTest();
            loader.CopyChildrenForTest(source, target, location, ConfigurationXmlLoaderBase.DefaultIncludeElementName);
            Assert.IsTrue(target.HasChildNodes);
            IReadOnlyList<XmlElement> children = TestHelper.GetElements(target);
            Assert.AreEqual(1, children.Count);
            var child = children[0];
            Assert.IsFalse(child.HasAttributes);
            Assert.IsFalse(child.HasChildNodes);
            Assert.AreEqual(name, child.Name);
        }

        /// <summary>
        /// Test copy children with nested element
        /// </summary>
        [TestMethod]
        public void ConfigurationXmlLoaderBase_CopyChildren_NestedElements()
        {
            var testCount = 3;
            for (var testId = 0; testId < testCount; testId++)
            {
                var document = new XmlDocument();
                var location = TestHelper.CreateUniqueName("Location{0}");
                var source = document.CreateElement(TestHelper.CreateUniqueName("Root{0}"));
                document.AppendChild(source);

                var random = new Random();
                var maxDepth = random.Next(10) + 1;
                var maxElements = random.Next(5) + 1;
                var maxAttributes = random.Next(3) + 1;
                ConfigurationXmlLoaderBaseUnitTest.GenerateRandomXml(source, random, 0, maxDepth, maxElements, maxAttributes);

                document = new XmlDocument();
                var target = document.CreateElement(TestHelper.CreateUniqueName("Host{0}"));
                document.AppendChild(target);

                var loader = new ConfigurationXmlLoaderBaseForTest();
                loader.CopyChildrenForTest(source, target, location, ConfigurationXmlLoaderBase.DefaultIncludeElementName);

                AssertHelper.AreReadOnlyListEqual<XmlElement>(
                    TestHelper.GetElements(source), 
                    TestHelper.GetElements(target),
                    (expected, actual) => AssertHelper.AreXmlEqual(expected, actual));
            }
        }
        #endregion CopyChildren()

        #region LoadXml()
        /// <summary>
        /// Load simple XML test
        /// </summary>
        [TestMethod]
        public void ConfigurationXmlLoaderBase_LoadXml_Simple()
        {
            var location = GetXmlLocation("Simple.xml");
            TestLoadXml(location, location);
        }
        #endregion LoadXml()

        #region helper

        /// <summary>
        /// Load XML test helper
        /// </summary>
        /// <param name="xmlFileNameForTest">location of test XML</param>
        /// <param name="xmlFileNameForResult">location of result XML</param>
        private static void TestLoadXml(string xmlFileNameForTest, string xmlFileNameForResult)
        {
            var loader = new ConfigurationXmlLoaderBaseForTest();
            var actual = loader.Load(xmlFileNameForTest);
            var expected = LoadXml(xmlFileNameForResult);
            AssertHelper.AreXmlEqual(expected, actual);
        }
        #endregion helper

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
            /// Copy child XML elements
            /// </summary>
            /// <param name="sourceXml">Source XML which contains child elements to copy</param>
            /// <param name="targetXml">Target XML which receive elements</param>
            /// <param name="sourceXmlLocation">the location which the source XML loaded from</param>
            /// <param name="includeElementName">name of element which for inserting external XML</param>
            /// <remarks>
            /// During copying, it will process "include elements" and merging those external XML if necessary
            /// </remarks>
            public void CopyChildrenForTest(
                XmlElement sourceXml,
                XmlElement targetXml,
                string sourceXmlLocation,
                string includeElementName)
            {
                this.CopyChildren(sourceXml, targetXml, sourceXmlLocation, includeElementName);
            }

            /// <summary>
            /// Get the new location
            /// </summary>
            /// <param name="currentLocation">the current location</param>
            /// <param name="relativeLocation">the relative location of current location</param>
            /// <returns>the merged location</returns>
            protected override string GetNewLocation(string currentLocation, string relativeLocation)
            {
                return ConfigurationXmlLoaderBaseUnitTest.GetNewLocation(currentLocation, relativeLocation);
            }

            /// <summary>
            /// Load XML from specific location
            /// </summary>
            /// <param name="location">the location of XML to load</param>
            /// <returns>The XML which loaded from the specified location</returns>
            protected override XmlElement LoadXml(string location)
            {
                return ConfigurationXmlLoaderBaseUnitTest.LoadXml(location);
            }
        }
    }
}
