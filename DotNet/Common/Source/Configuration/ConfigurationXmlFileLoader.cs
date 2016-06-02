//------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationXmlFileLoader.cs" company="Ereadian">
//     Copyright (c) Ereadian.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Configuration
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Load configuration XML from file system
    /// </summary>
    /// <history>
    ///     <create date="2015-6-4" by="lixinxu" />
    /// </history>
    public class ConfigurationXmlFileLoader : ConfigurationXmlLoaderBase
    {
        /// <summary>
        /// Load XML by given path
        /// </summary>
        /// <param name="location">configuration file path</param>
        /// <returns>XML instance</returns>
        /// <remarks>
        /// It implementations the abstract method in base class.
        /// If file does not exist or had problem to load the file, null will be returned
        /// </remarks>
        protected override XmlElement LoadXml(string location)
        {
            XmlElement xml = null;
            if (!File.Exists(location))
            {
                // TODO: log error
            }
            else
            {
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(location);
                    xml = doc.DocumentElement;
                }
                catch
                {
                    // TODO: log error
                }
            }

            return xml;
        }

        /// <summary>
        /// Get new file location
        /// </summary>
        /// <param name="currentLocation">current file location</param>
        /// <param name="relativeLocation">the new location which related to the current location</param>
        /// <returns>the new file location</returns>
        /// <remarks>
        /// It implementations the abstract method in base class.
        /// </remarks>
        protected override string GetNewLocation(string currentLocation, string relativeLocation)
        {
            var folder = Path.GetDirectoryName(currentLocation);
            return Path.GetFullPath(Path.Combine(folder, relativeLocation));
        }
    }
}
