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
    public abstract class ConfigurationXmlLoaderBase :  IConfigurationXmlLoader
    {
        #region System defined XML element names
        /// <summary>
        /// the default root XML element
        /// </summary>
        /// <remarks>
        /// If the "include" XML is the XML which we loaded, then we need a root element. Otherwise we will
        /// use the original XML element name as root element.
        /// </remarks>
        private static readonly string SystemDefaultRootElementName = "configuration";

        /// <summary>
        /// The default name of "include" element.
        /// </summary>
        private static readonly string SystemDefaultIncludeElementName = "include";
        #endregion System defined XML element names

        #region XML element names
        /// <summary>
        /// Gets the default root element name if the first XML is "include" element.
        /// </summary>
        protected virtual string DefaultRootElementName => SystemDefaultRootElementName;

        /// <summary>
        /// Gets the name of "include" element 
        /// </summary>
        protected virtual string IncludeElementName => SystemDefaultIncludeElementName;
        #endregion XML element names

        /// <summary>
        /// Load XML from give location
        /// </summary>
        /// <param name="location">the location of XML</param>
        /// <returns>The loaded XML</returns>
        /// <remarks>
        /// The "location" is very generic. It can be file path, URL, database column name or section name in configuration file.
        /// </remarks>
        public virtual XmlElement Load(string location)
        {
            var rawXml = this.LoadXml(location);
            var xmlDocument = new XmlDocument();
            return this.ProcessRawXml(xmlDocument, rawXml, location);
        }

        #region help for loading/copying XML
        /// <summary>
        /// Process raw XML
        /// </summary>
        /// <param name="targetXmlDocument">target XML document</param>
        /// <param name="rawXml">raw XML to process</param>
        /// <param name="location">the location of the XML</param>
        /// <returns>processed XML</returns>
        protected XmlElement ProcessRawXml(XmlDocument targetXmlDocument, XmlElement rawXml, string location)
        {
            XmlElement configurationXml = null;
            if (rawXml != null)
            {
                if (rawXml.Name == this.IncludeElementName)
                {
                    configurationXml = targetXmlDocument.CreateElement(this.DefaultRootElementName);
                    this.CopyAttributes(rawXml, configurationXml);
                    this.CopyChildren(rawXml, configurationXml, location);
                }
                else
                {
                    configurationXml = this.DuplicateXmlElement(targetXmlDocument, rawXml, location);
                }
            }

            return configurationXml;
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
        /// <remarks>
        /// During copying, it will process "include elements" and merging those external XML if necessary
        /// </remarks>
        protected void CopyChildren(XmlElement sourceXml, XmlElement targetXml, string sourceXmlLocation)
        {
            if (sourceXml.HasChildNodes)
            {
                for (var i = 0; i < sourceXml.ChildNodes.Count; i++)
                {
                    var childNode = sourceXml.ChildNodes[i];
                    if (childNode.NodeType == XmlNodeType.Element)
                    {
                        var childXml = childNode as XmlElement;
                        if (childXml.Name != this.IncludeElementName)
                        {
                            targetXml.AppendChild(this.DuplicateXmlElement(targetXml.OwnerDocument, childXml, sourceXmlLocation));
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
                                if (referenceXml.Name == this.IncludeElementName)
                                {
                                    this.CopyChildren(referenceXml, targetXml, newLocation);
                                }
                                else
                                {
                                    targetXml.AppendChild(this.DuplicateXmlElement(targetXml.OwnerDocument, referenceXml, newLocation));
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
        /// <returns>the duplicated XML element</returns>
        protected XmlElement DuplicateXmlElement(XmlDocument targetDocument, XmlElement sourceXml, string sourceXmlLocation)
        {
            var newXml = targetDocument.CreateElement(sourceXml.Name);
            this.CopyAttributes(sourceXml, newXml);
            this.CopyChildren(sourceXml, newXml, sourceXmlLocation);
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
