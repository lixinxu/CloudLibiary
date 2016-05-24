//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SingletonInstanceUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Singleton instance unit tests
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SingletonInstanceUnitTest
    {
        /// <summary>
        /// flag of whether instance get created
        /// </summary>
        private static bool dataCreated;

        /// <summary>
        /// <para>Test singleton instance which it can be created successfully</para>
        /// <para>1. Instance should not be created even the host class (SingletonInstance{T} was toughed/></para>
        /// <para>2. Instance can be created successfully</para>
        /// <para>3. No matter how many times you called to get instance, it always return same instance it created</para>
        /// </summary>
        [TestMethod]
        public void SingletonInstance_Success_Test()
        {
            dataCreated = false;
            var type = typeof(SingletonInstance<Test>);
            Assert.IsFalse(dataCreated);

            var instance = SingletonInstance<Test>.Instance;
            Assert.IsTrue(dataCreated);
            Assert.IsNotNull(instance);

            var testTimes = 3;
            dataCreated = false;
            for (var i = 0; i < testTimes; i++)
            {
                var another = SingletonInstance<Test>.Instance;
                Assert.IsFalse(dataCreated);
                Assert.IsTrue(object.ReferenceEquals(instance, another));
            }
        }

        /// <summary>
        /// Class used for singleton test
        /// </summary>
        private class Test
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Test" /> class.
            /// </summary>
            public Test()
            {
                dataCreated = true;
            }
        }
    }
}
