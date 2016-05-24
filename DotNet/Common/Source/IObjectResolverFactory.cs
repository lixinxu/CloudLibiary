//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IObjectResolverFactory.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object resolver interface
    /// </summary>
    public interface IObjectResolverFactory
    {
        /// <summary>
        /// Get resolver by given type and name
        /// </summary>
        /// <param name="typeToResolve">type to resolve</param>
        /// <param name="resolverName">name of the resolver</param>
        /// <returns>object resolver instance</returns>
        IObjectResolver GetResolver(Type typeToResolve, string resolverName = null);

        /// <summary>
        /// Get all resolvers for given type
        /// </summary>
        /// <param name="typeToResolve">type to resolve</param>
        /// <returns>resolvers collection</returns>
        IReadOnlyDictionary<string, IObjectResolver> GetResolvers(Type typeToResolve);

        /// <summary>
        /// Get all resolvers for given type
        /// </summary>
        /// <typeparam name="T">type to resolve</typeparam>
        /// <returns>resolvers collection</returns>
        IReadOnlyDictionary<string, IObjectResolver<T>> GetResolvers<T>();
    }
}
