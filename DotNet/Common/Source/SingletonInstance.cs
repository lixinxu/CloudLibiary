//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SingletonInstance.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common
{
    using System;

    /// <summary>
    /// Provide a way to get singleton instance
    /// </summary>
    /// <typeparam name="T">type of singleton object</typeparam>
    /// <remarks>
    /// It uses "static constructor" to delay create singleton object. In another words, the singleton object 
    /// will not be created until caller try to access it first time.
    /// </remarks>
    /// <example>
    /// <![CDATA[
    /// public class MyClass
    /// {
    ///     ...
    /// }
    /// ...
    /// var instance = SingletonInstance<MyClass>.Instance;
    /// ...
    /// ]]>
    /// </example>
    public static class SingletonInstance<T> where T : class, new()
    {
        /// <summary>
        /// Gets singleton instance
        /// </summary>
        public static T Instance
        {
            get
            {
                var instance = SingletonStorage.Instance;
                if (instance == null)
                {
                }

                return instance;
            }
        }

        /// <summary>
        /// Singleton storage
        /// </summary>
        private static class SingletonStorage
        {
            /// <summary>
            /// Gets singleton instance
            /// </summary>
            internal static T Instance { get; }

            /// <summary>
            /// Type constructor
            /// </summary>
            static SingletonStorage()
            {
                try
                {
                    Instance = new T();
                }
                catch(Exception exception)
                {
                    throw new ApplicationException("failed to create singleton instance", exception);
                }
            }
        }
    }
}