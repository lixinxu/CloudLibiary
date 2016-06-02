//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IConfigurationProvider.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Configuration
{
    /// <summary>
    /// Configuration provider interface
    /// </summary>
    /// <remarks>
    /// <para>Abstract configuration is important because developer may use different configuration provider in different
    /// environment. For example, in unit test project, the code loads the configuration from app.config but in production
    /// it may load the value from cloud role configuration (in Azure) or from their own configuration store like SQL or
    /// storage table.</para>
    /// <para>The method in the interface is used for implementation. In most cases, code uses extension to retrieve 
    /// configuration which is more easier</para>
    /// </remarks>
    /// <example>
    /// <![CDATA[
    /// var provider = SingletonInstance<ObjectResolverFactory>.Instance.Resolve<IConfigurationProvider>();
    /// object value;
    /// if (provider.TryGetValue("Timeout", out value))
    /// {
    ///     var timeout = Convert.ChangeType(value, typeof(int));
    ///     ...
    /// }
    /// With extension:
    /// var provider = SingletonInstance<ObjectResolverFactory>.Instance.Resolve<IConfigurationProvider>();
    /// int value;
    /// if (provider.TryGetValue<int>("Timeout", out value))
    /// {
    ///     ...
    /// }
    /// ]]>
    /// </example>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Try get configuration value
        /// </summary>
        /// <param name="key">configuration key</param>
        /// <param name="value">configuration value</param>
        /// <returns>true if configuration value returned</returns>
        bool TryGetValue(string key, out object value);
    }
}
