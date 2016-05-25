//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TraceLoggerUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.DiagnosticsTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Common.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.CommonUnitTestHelper;
    using Shared.UnitTestHelper;

    /// <summary>
    /// Trace logger unit test
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TraceLoggerUnitTest
    {
        /// <summary>
        /// Test trace logger
        /// </summary>
        [TestMethod]
        public void TraceLogger_UnitTest()
        {
            var logger = new TraceLogger();
            var stackTrace = new StackTrace(true);
            var testCount = 3;
            var random = new Random();
            var runtimeInformation = new Dictionary<string, object>();
            for (var testId = 0; testId < testCount; testId++)
            {
                var eventId = Math.Abs(random.Next());
                var allEventLevels = EnumHelper.GetAllPossibleValues<EventLevel>();
                foreach (var eventLevel in allEventLevels)
                {
                    var eventSource = TestHelper.CreateUniqueName("SourceName{0}");
                    var message = TestHelper.CreateUniqueName("EventMessage{0}");
                    runtimeInformation.Clear();
                    var runtimeItemCount = random.Next(10) + 1;
                    for (var i = 0; i < runtimeItemCount; i++)
                    {
                        runtimeInformation.Add(
                            TestHelper.CreateUniqueName("RuntimeKey{0}"),
                            TestHelper.CreateUniqueName("RuntimeValue{0}"));
                    }

                    TraceListenerForTest.ClearMessage();
                    logger.Write(
                        eventSource,
                        eventId,
                        eventLevel,
                        message,
                        DateTime.UtcNow,
                        stackTrace,
                        runtimeInformation);
                    var messages = TraceListenerForTest.CopyMessges();
                    Assert.AreEqual(1, messages.Count);
                    var traceMessage = messages[0];

                    Assert.IsTrue(traceMessage.Contains(eventSource));
                    Assert.IsTrue(traceMessage.Contains(eventId.ToString()));
                    Assert.IsTrue(traceMessage.Contains(eventLevel.ToString()));
                    Assert.IsTrue(traceMessage.Contains(message));
                    foreach (var pair in runtimeInformation)
                    {
                        Assert.IsTrue(traceMessage.Contains(pair.Key));
                        Assert.IsTrue(traceMessage.Contains(pair.Value as string));
                    }
                }
            }
        }
    }
}