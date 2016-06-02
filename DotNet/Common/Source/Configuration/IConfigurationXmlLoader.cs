//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IConfigurationXmlLoader.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Configuration
{
    using System.Xml;

    /// <summary>
    /// XML loader for configuration
    /// </summary>
    /// <remarks>
    /// Object resolver factory will use it to load configuration. the configuration may from web.config/app.config or 
    /// local file. It is also possible to store the configuration in database so we can change the providers without 
    /// re-deployment. In unit test, we can generate configuration on fly as well.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// IConfigurationXmlLoader loader = ConfigurationXmlLoader.GetLoader();
    /// var configurationXml = loader.Load();
    /// if (configurationXml != null)
    /// {
    ///     foreach(var itemXml in configurationXml)
    ///     {
    ///         ...
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    public interface IConfigurationXmlLoader
    {
        /// <summary>
        /// Load configuration XML
        /// </summary>
        /// <param name="location">
        /// The location of the XML. It can be section name in web.config, file path on the disk, or URL
        /// </param>
        /// <returns>configuration XML. null if failed to load</returns>
        XmlElement Load(string location);
    }
}
