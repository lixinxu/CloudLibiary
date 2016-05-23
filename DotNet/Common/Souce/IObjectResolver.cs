//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IObjectResolver.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common
{
    /// <summary>
    /// Object Resolver interface
    /// </summary>
    /// <remarks>
    /// <para>Some IoC providers like Unity supports resolve object from factory/container. However, they requires the
    /// caller provides type and name every time. That cause the system needs to find the resolver every time.</para>
    /// <para>CloudLibrary introduces a new IoC container which caller can retrieve a resolver so we don't need to search
    /// the resolver in container/factory every time. This improves the performance a lot</para>
    /// <para>For classic IoC providers like Unity, developer can create a wrapper/adapter so it can support new 
    /// dependency model.</para>
    /// </remarks>CloudLibrary
    public interface IObjectResolver
    {
        /// <summary>
        /// Resolve an object
        /// </summary>
        /// <returns>resolved object</returns>
        /// <remarks>
        /// <para>If object failed to be resolved, exception will be thrown. "null" does not mean object was not resolved 
        /// because it is expected value. </para>
        /// <para>Resolving object is not recommended in object constructor if the class is part of dependency injection
        /// which will be "resolved" by factory/container. The reason is that the container may not be ready when creating
        /// the instance of the class</para>
        /// </remarks>
        /// <example>
        /// <![CDATA[
        /// var factory = Singleton<ObjectResolverFactory>.Instance;
        /// var resolver = factory.GetResolver(typeof(ILogger));
        /// if (resolver != null)
        /// {
        ///     var logger = resolver.Resolve() as ILogger;
        ///     logger.Write(...);
        /// }
        /// ]]>
        /// Using IObjectResolver and IObjectResolverFactory extensions my simplify the code:
        /// <![CDATA[
        /// var factory = Singleton<ObjectResolverFactory>.Instance;
        /// var resolver = factory.GetResolver<ILogger, TraceLogger>();
        /// var logger = resolver.Resolve();
        /// logger.Write(...);
        /// ]]>
        /// </example>
        object Resove();
    }

    /// <summary>
    /// Interface of object resolver supports generic type
    /// </summary>
    /// <typeparam name="T">Type to resolve</typeparam>
    /// <seealso cref="IObjectResolver"/>
    public interface IObjectResolver<T> : IObjectResolver
    {
        /// <summary>
        /// Resolve object
        /// </summary>
        /// <returns>resolved object</returns>
        /// <seealso cref="IObjectResolver.Resove"/>
        T Resolve();
    }
}
