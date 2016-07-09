//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ConfigurationXmlLoaderUnitTestBase.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.ConfigurationTest
{
    using System;
    using System.Xml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.UnitTestHelper;

    /// <summary>
    /// Base class for configuration XML loader unit test
    /// </summary>
    public class ConfigurationXmlLoaderUnitTestBase
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

        /// <summary>
        /// Create random XML
        /// </summary>
        /// <param name="elementRoot">element root</param>
        /// <param name="random">random number generator</param>
        /// <param name="currentDepth">current depth</param>
        /// <param name="maxDepth">max depth</param>
        /// <param name="maxElements">max element count</param>
        /// <param name="maxAttrubutes">max attribute count</param>
        protected static void GenerateRandomXml(
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
        /// Get XML location
        /// </summary>
        /// <param name="path">test XML relative path</param>
        /// <returns>full path</returns>
        protected static string GetXmlLocation(string path)
        {
            return $"{typeof(ConfigurationXmlLoaderBaseUnitTest).Namespace}.{TestXmlFilesFolderName}.{NormalizePath(path)}";
        }

        /// <summary>
        /// Get new location
        /// </summary>
        /// <param name="currentLocation">current XML location</param>
        /// <param name="relativeLocation">new XML relative location</param>
        /// <returns>new location</returns>
        protected static string GetNewLocation(string currentLocation, string relativeLocation)
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
        protected static XmlElement LoadXml(string location)
        {
            XmlDocument document = new XmlDocument();
            using (var stream = typeof(ConfigurationXmlLoaderBaseUnitTest).Assembly.GetManifestResourceStream(location))
            {
                document.Load(stream);
            }

            return document.DocumentElement;
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
    }
}
