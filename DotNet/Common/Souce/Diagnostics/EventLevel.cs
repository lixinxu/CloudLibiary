//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EventLevel.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    /// <summary>
    /// Event level for logging
    /// </summary>
    /// <remarks>
    /// Not all application supports full set of event level. For example, Windows event does not support "Fault". In
    /// this case, the logging provide can merge the "fault" to "error".
    /// </remarks>
    public enum EventLevel
    {
        /// <summary>
        /// Debugging information only.
        /// </summary>
        /// <remarks>
        /// For example, enter method, exit method and so on.
        /// </remarks>
        Debug,

        /// <summary>
        /// Information just for reference
        /// </summary>
        /// <remarks>
        /// For example, new user added, service connected.
        /// </remarks>
        Information,

        /// <summary>
        /// For issue at runtime but can be recovered or fall-back.
        /// </summary>
        /// <remarks>
        /// For example, a configuration was not found. However, the application still can continue by using default 
        /// or pre-defined value.
        /// </remarks>
        Warning,

        /// <summary>
        /// Found error during application runtime. There is not fall-back or other way to continue the work. feature
        /// is broken. However, it does not impact to the entire application. Other features may be still working.
        /// </summary>
        /// <remarks>
        /// For example, in social application, translation service is not working. However, use still can add new content.
        /// </remarks>
        Error,

        /// <summary>
        /// Serious system problem was found. The entire application cannot work.
        /// </summary>
        /// <remarks>
        /// For example, the main data storage is outage. The entire application stop working
        /// </remarks>
        Fault,
    }
}
