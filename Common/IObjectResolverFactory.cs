//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IObjectResolverFactory.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common
{
    using System;

    /// <summary>
    /// Object resolver interface
    /// </summary>
    public interface IObjectResolverFactory
    {
        IObjectResolver GetResolver(Type typeToResolve, string resolverName = null);
    }
}
