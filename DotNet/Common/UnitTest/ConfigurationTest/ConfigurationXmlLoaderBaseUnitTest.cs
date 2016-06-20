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
        /// <summary>
        /// name of folder which stores configuration XML files for test
        /// </summary>
        private const string TestXmlFilesFolderName = "ConfigurationTestXmlFiles";

        /// <summary>
        /// directory separator character for embedded resources
        /// </summary>
        private const char DirectorySeparatorChar = '.';

        /// <summary>
        /// current folder indicator
        /// </summary>
        private const string CurrentFolder = ".";

        /// <summary>
        /// up level folder indicator
        /// </summary>
        private const string UpFolder = "..";

        /// <summary>
        /// All possible directory separator characters
        /// </summary>
        private static readonly char[] ExternalDirectorySeparatorChars = new char[] { '/', '\\' };

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
                GenerateRandomXml(source, random, 0, maxDepth, maxElements, maxAttributes);

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
        /// Create random XML
        /// </summary>
        /// <param name="elementRoot">element root</param>
        /// <param name="random">random number generator</param>
        /// <param name="currentDepth">current depth</param>
        /// <param name="maxDepth">max depth</param>
        /// <param name="maxElements">max element count</param>
        /// <param name="maxAttrubutes">max attribute count</param>
        private static void GenerateRandomXml(
            XmlElement elementRoot, 
            Random random, 
            int currentDepth,
            int maxDepth, 
            int maxElements, 
            int maxAttrubutes)
        {
            var elementCount = random.Next(maxElements + 1);
            var document = elementRoot.OwnerDocument;
            currentDepth++;
            for (var elementId = 0; elementId < elementCount; elementId++)
            {
                var element = document.CreateElement(TestHelper.CreateUniqueName("Element{0}"));
                elementRoot.AppendChild(element);
                var attributeCount = random.Next(maxAttrubutes + 1);
                for (var attributeId = 0; attributeId < attributeCount; attributeId++)
                {
                    var attribute = document.CreateAttribute(TestHelper.CreateUniqueName("Attribute{0}"));
                    attribute.Value = TestHelper.CreateUniqueName("Value{0}");
                    element.Attributes.Append(attribute);
                }

                if (currentDepth < maxDepth - 1)
                {
                    var depth = random.Next(maxDepth - currentDepth);
                    if (depth > 0)
                    {
                        GenerateRandomXml(
                            element, 
                            random,
                            currentDepth,
                            currentDepth + depth, 
                            maxElements, 
                            maxAttrubutes);
                    }
                }
            }
        }

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

        /// <summary>
        /// Get XML location
        /// </summary>
        /// <param name="path">test XML relative path</param>
        /// <returns>full path</returns>
        private static string GetXmlLocation(string path)
        {
            return $"{typeof(ConfigurationXmlLoaderBaseUnitTest).Namespace}.{TestXmlFilesFolderName}.{NormalizePath(path)}";
        }

        /// <summary>
        /// Normalize path
        /// </summary>
        /// <param name="path">path to normalize</param>
        /// <returns>normalized path</returns>
        private static string NormalizePath(string path)
        {
            foreach (var separatorChar in ExternalDirectorySeparatorChars)
            {
                path = path.Replace(separatorChar, DirectorySeparatorChar);
            }

            return path;
        }

        /// <summary>
        /// Get new location
        /// </summary>
        /// <param name="currentLocation">current XML location</param>
        /// <param name="relativeLocation">new XML relative location</param>
        /// <returns>new location</returns>
        private static string GetNewLocation(string currentLocation, string relativeLocation)
        {
            if (string.IsNullOrEmpty(currentLocation))
            {
                return GetXmlLocation(relativeLocation);
            }

            var index = 0;
            var position = currentLocation.LastIndexOf('.', currentLocation.Length - 1, 2);
            if (position < 0)
            {
                currentLocation = string.Empty;
            }

            position = index;
            while (position < relativeLocation.Length)
            {
                foreach (var separatorChar in ExternalDirectorySeparatorChars)
                {
                    if (relativeLocation[position] == separatorChar)
                    {
                        break;
                    }
                }

                if (position >= relativeLocation.Length)
                {
                    break;
                }

                var folderName = relativeLocation.Substring(index, position - index);
                position++;
                if (folderName == CurrentFolder)
                {
                    continue;
                }
                else if (folderName != UpFolder)
                {
                    index = position;
                    break;
                }

                var lastFolderPosition = currentLocation.LastIndexOf(DirectorySeparatorChar);
                Assert.IsTrue(lastFolderPosition >= 0);
                currentLocation = currentLocation.Substring(0, lastFolderPosition);
            }

            return $"{currentLocation}.{relativeLocation.Substring(index)}";
        }

        /// <summary>
        /// Load XML from assembly
        /// </summary>
        /// <param name="location">XML location</param>
        /// <returns>loaded XML</returns>
        private static XmlElement LoadXml(string location)
        {
            XmlDocument document = new XmlDocument();
            using (var stream = typeof(ConfigurationXmlLoaderBaseUnitTest).Assembly.GetManifestResourceStream(location))
            {
                document.Load(stream);
            }

            return document.DocumentElement;
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
