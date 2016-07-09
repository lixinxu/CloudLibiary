//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="XmlConfigurationSectionHandler.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Configuration
{
    using System.Configuration;
    using System.Xml;

    /// <summary>
    /// XML Configuration section handler
    /// </summary>
    /// <remarks>
    /// A simple wrapper to return XML element directly
    /// </remarks>
    public class XmlConfigurationSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Get configuration section raw data (in XML)
        /// </summary>
        /// <param name="parent">Parent object</param>
        /// <param name="configContext">Configuration context object</param>
        /// <param name="section">Section XML node</param>
        /// <returns>The raw section XML</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            return section as XmlElement;
        }
    }
}
