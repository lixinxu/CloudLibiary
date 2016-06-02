//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExceptionBaseUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit test for ExceptionBase
    /// </summary>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ExceptionBaseUnitTest
    {
        /// <summary>
        /// Serialization test
        /// </summary>
        [TestMethod]
        public void ExceptionBase_Test_Serialization()
        {
            var random = new Random();
            var testTimes = 3;
            for (var i = 0; i < testTimes; i++)
            {
                var source = CreateUniqueName("source");
                var eventId = Math.Abs(random.Next());
                var message = CreateUniqueName("message");
                Dictionary<string, object> runtimeInformation = null;
                if (i > 0)
                {
                    var itemCount = 1 + random.Next(3);
                    runtimeInformation = new Dictionary<string, object>(itemCount);
                    for (var j = 0; j < itemCount; j++)
                    {
                        var name = CreateUniqueName($"key_{j}_");
                        var value = CreateUniqueName($"value_{j}_");
                        runtimeInformation.Add(name, value);
                    }
                }

                var exception = new ExceptionBase(source, eventId, message, runtimeInformation);
                ExceptionBase actual;
                var formatter = new BinaryFormatter();
                using (var stream = new MemoryStream())
                {
                    formatter.Serialize(stream, exception);
                    stream.Position = 0;
                    actual = formatter.Deserialize(stream) as ExceptionBase;
                }

                Assert.IsNotNull(actual);
                Assert.AreEqual(source, actual.EventSource);
                Assert.AreEqual(eventId, actual.EventId);

                if (runtimeInformation != null)
                {
                    Assert.IsNotNull(actual.RuntimeInformation);
                    Assert.AreEqual(runtimeInformation.Count, actual.RuntimeInformation.Count);
                    object value;
                    foreach (var pair in runtimeInformation)
                    {
                        Assert.IsTrue(actual.RuntimeInformation.TryGetValue(pair.Key, out value));
                        Assert.AreEqual(pair.Value, value);
                    }
                }
            }
        }

        /// <summary>
        /// Create unique name
        /// </summary>
        /// <param name="prefix">name prefix</param>
        /// <returns>unique name</returns>
        private static string CreateUniqueName(string prefix)
        {
            return $"{prefix}{Guid.NewGuid().ToString("N")}";
        }
    }
}
