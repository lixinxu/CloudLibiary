//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ConfigurationXmlLoaderBase.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Configuration
{
    using System.Xml;

    /// <summary>
    /// The base class to load configuration XML
    /// </summary>
    /// <remarks>
    /// The base XML loader provides the capability to load and merge XML content from multiple sources.This will give us a advantage that 
    /// we can create multiple configuration files for different environments. These configuration files can also share some files to avoid
    /// creating duplicate entries in different files. To switch different environments, we just change the master configuration which make
    /// the management very easy.
    /// </remarks>
    /// <history>
    ///     <create date="2015-6-4" by="lixinxu" />
    /// </history>
    public abstract class ConfigurationXmlLoaderBase : IConfigurationXmlLoader
    {
        #region System defined XML element names
        /// <summary>
        /// the default root XML element
        /// </summary>
        /// <remarks>
        /// If the "include" XML is the XML which we loaded, then we need a root element. Otherwise we will
        /// use the original XML element name as root element.
        /// </remarks>
        public const string DefaultRootElementName = "configuration";

        /// <summary>
        /// The default name of "include" element.
        /// </summary>
        public const string DefaultIncludeElementName = "include";

        /// <summary>
        /// The default attribute name for locating root element name
        /// </summary>
        public const string DefaultRootElementLocateAttributeName = "root";

        /// <summary>
        /// The default attribute name for locating include element name
        /// </summary>
        public const string DefaultIncludeElementLocateAttributeName = "include";

        #endregion System defined XML element names

        /// <summary>
        /// Load XML from give location
        /// </summary>
        /// <param name="location">the location of XML</param>
        /// <returns>The loaded XML</returns>
        /// <remarks>
        /// The "location" is very generic. It can be file path, URL, database column value or section name in configuration file.
        /// </remarks>
        /// <param name="rootElementLocateAttributeName">
        /// attribute name for locating root element name
        /// </param>
        /// <param name="includeElementLocateAttributeName">
        /// attribute name for locating include element name
        /// </param>
        public virtual XmlElement Load(
            string location,
            string rootElementLocateAttributeName = null,
            string includeElementLocateAttributeName = null)
        {
            var rawXml = this.LoadXml(location);
            if (rawXml == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(rootElementLocateAttributeName))
            {
                rootElementLocateAttributeName = DefaultRootElementLocateAttributeName;
            }

            if (string.IsNullOrEmpty(includeElementLocateAttributeName))
            {
                includeElementLocateAttributeName = DefaultIncludeElementLocateAttributeName;
            }

            var rootElementName = rawXml.GetAttribute(rootElementLocateAttributeName);
            if (string.IsNullOrEmpty(rootElementName))
            {
                rootElementName = DefaultRootElementName;
            }

            var includeElementName = rawXml.GetAttribute(includeElementLocateAttributeName);
            if (string.IsNullOrEmpty(includeElementName))
            {
                includeElementName = DefaultIncludeElementName;
            }

            var xmlDocument = new XmlDocument();
            this.ProcessRawXml(xmlDocument, rawXml, location, rootElementName, includeElementName);
            return xmlDocument.DocumentElement;
        }

        #region help for loading/copying XML
        /// <summary>
        /// Process raw XML
        /// </summary>
        /// <param name="targetNode">target XML node. It could be element or document</param>
        /// <param name="sourceXml">source XML to process</param>
        /// <param name="location">the location of the XML</param>
        /// <param name="rootElementName">root element name</param>
        /// <param name="includeElementName">name of element which for inserting external XML</param>
        protected void ProcessRawXml(
            XmlNode targetNode, 
            XmlElement sourceXml, 
            string location, 
            string rootElementName, 
            string includeElementName)
        {
            if (sourceXml != null)
            {
                var targetDocument = targetNode as XmlDocument;
                if (targetDocument == null)
                {
                    targetDocument = targetNode.OwnerDocument;
                }

                if (sourceXml.Name == includeElementName)
                {
                    var relativeLocation = sourceXml.Value;
                    var newLocation = this.GetNewLocation(location, relativeLocation);
                    var containerXml = this.LoadXml(newLocation);
                    if (containerXml != null)
                    {
                        if (containerXml.Name == rootElementName)
                        {
                            if (containerXml.HasChildNodes)
                            {
                                for (var i = 0; i < sourceXml.ChildNodes.Count; i++)
                                {
                                    var childNode = sourceXml.ChildNodes[i];
                                    if (childNode.NodeType == XmlNodeType.Element)
                                    {
                                        this.ProcessRawXml(
                                            targetNode, 
                                            childNode as XmlElement, 
                                            newLocation, 
                                            rootElementName, 
                                            includeElementName);
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.ProcessRawXml(targetNode, containerXml, newLocation, rootElementName, includeElementName);
                        }
                    }
                }
                else
                {
                    var configurationXml = this.DuplicateXmlElement(targetDocument, sourceXml, location, includeElementName);
                    targetNode.AppendChild(configurationXml);
                }
            }
        }

        /// <summary>
        /// Copy attribute from source XML to target XML
        /// </summary>
        /// <param name="sourceXml">source XML which contains attribute to copy</param>
        /// <param name="targetXml">target XML to receive attributes</param>
        protected void CopyAttributes(XmlElement sourceXml, XmlElement targetXml)
        {
            if (sourceXml.HasAttributes)
            {
                foreach (XmlAttribute attribute in sourceXml.Attributes)
                {
                    var newAttribute = targetXml.OwnerDocument.CreateAttribute(attribute.Name);
                    newAttribute.Value = attribute.Value;
                    targetXml.Attributes.Append(newAttribute);
                }
            }
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
        protected void CopyChildren(
            XmlElement sourceXml, 
            XmlElement targetXml, 
            string sourceXmlLocation,
            string includeElementName)
        {
            if (sourceXml.HasChildNodes)
            {
                for (var i = 0; i < sourceXml.ChildNodes.Count; i++)
                {
                    var childNode = sourceXml.ChildNodes[i];
                    if (childNode.NodeType == XmlNodeType.Element)
                    {
                        var childXml = childNode as XmlElement;
                        if (childXml.Name != includeElementName)
                        {
                            var newChildXml = this.DuplicateXmlElement(
                                targetXml.OwnerDocument, 
                                childXml, 
                                sourceXmlLocation,
                                includeElementName);
                            targetXml.AppendChild(newChildXml);
                        }
                        else
                        {
                            var relativeLocation = childXml.Value;
                            if (string.IsNullOrEmpty(relativeLocation))
                            {
                                // TODO: log error
                                continue;
                            }

                            var newLocation = this.GetNewLocation(sourceXmlLocation, relativeLocation);
                            var referenceXml = this.LoadXml(newLocation);
                            if (referenceXml != null)
                            {
                                if (referenceXml.Name == includeElementName)
                                {
                                    this.CopyChildren(referenceXml, targetXml, newLocation, includeElementName);
                                }
                                else
                                {
                                    var newChildXml = this.DuplicateXmlElement(
                                        targetXml.OwnerDocument, 
                                        referenceXml, 
                                        newLocation,
                                        includeElementName);
                                    targetXml.AppendChild(newChildXml);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Duplicate XML element
        /// </summary>
        /// <param name="targetDocument">target XML document</param>
        /// <param name="sourceXml">XML element to be duplicated</param>
        /// <param name="sourceXmlLocation">the location of the source XML which it loaded from</param>
        /// <param name="includeElementName">name of element which for inserting external XML</param>
        /// <returns>the duplicated XML element</returns>
        protected XmlElement DuplicateXmlElement(
            XmlDocument targetDocument, 
            XmlElement sourceXml, 
            string sourceXmlLocation,
            string includeElementName)
        {
            var newXml = targetDocument.CreateElement(sourceXml.Name);
            this.CopyAttributes(sourceXml, newXml);
            this.CopyChildren(sourceXml, newXml, sourceXmlLocation, includeElementName);
            return newXml;
        }
        #endregion help for loading/copying XML

        /// <summary>
        /// Load XML from specific location
        /// </summary>
        /// <param name="location">the location of XML to load</param>
        /// <returns>The XML which loaded from the specified location</returns>
        protected abstract XmlElement LoadXml(string location);

        /// <summary>
        /// Get the new location
        /// </summary>
        /// <param name="currentLocation">the current location</param>
        /// <param name="relativeLocation">the relative location of current location</param>
        /// <returns>the merged location</returns>
        protected abstract string GetNewLocation(string currentLocation, string relativeLocation);
    }
}
