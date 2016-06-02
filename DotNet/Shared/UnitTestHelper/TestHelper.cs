//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TestHelper.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Shared.UnitTestHelper
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Unit Test helper
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class TestHelper
    {
        /// <summary>
        /// Create unique name
        /// </summary>
        /// <param name="template">name template</param>
        /// <returns>unique name</returns>
        /// <remarks>it inject GUID to the string template</remarks>
        /// <example>
        /// <![CDATA[
        /// var template = "name{0}";
        /// var data = CreateUniqueName(template);
        /// ]]>
        /// </example>
        /// <history>
        ///     <create time="2016/5/16" author="lixinxu" />
        /// </history>
        public static string CreateUniqueName(string template)
        {
            return string.Format(template, Guid.NewGuid().ToString("N"));
        }

        #region Emit
        /// <summary>
        /// Create dynamic module
        /// </summary>
        /// <returns>module builder</returns>
        public static ModuleBuilder CreateModuleBuilder()
        {
            var currentDomain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName(CreateUniqueName("DynamicAssembly{0}"));
            var assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            return assemblyBuilder.DefineDynamicModule(CreateUniqueName("DynamicModule{0}"));
        }
        #endregion Emit
    }
}
