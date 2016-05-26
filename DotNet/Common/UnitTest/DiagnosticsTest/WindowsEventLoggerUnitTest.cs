//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WindowsEventLoggerUnitTest.cs" company="CloudLibrary">
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
    /// Windows Event Logger unit test
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class WindowsEventLoggerUnitTest
    {
        /// <summary>
        /// Test Windows Event Logger
        /// </summary>
        [TestMethod]
        public void WindowsEventLogger_UnitTest()
        {
            var testCount = 3;
            var random = new Random();

            var logger = new WindowsEventLogger();
            var stackTrace = new StackTrace(true);
            var runtimeInformation = new Dictionary<string, object>();

            // Run through the test
            for (var testId = 0; testId < testCount; testId++)
            {
                var eventId = Math.Abs(random.Next()) % 256;
                var allEventLevels = EnumHelper.GetAllPossibleValues<EventLevel>();

                // Test all event levels
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

                    // Write the log
                    logger.Write(
                        eventSource,
                        eventId,
                        eventLevel,
                        message,
                        DateTime.UtcNow,
                        stackTrace,
                        runtimeInformation);

                    // Get all the logs from this machine
                    var eventLogs = EventLog.GetEventLogs();

                    // Find the right log
                    var eventLogName = EventLog.LogNameFromSourceName(eventSource, Environment.MachineName);
                    if (string.IsNullOrEmpty(eventLogName))
                    {
                        Assert.Fail();
                    }
                    var eventLog = new EventLog(eventLogName, Environment.MachineName, eventSource);
                    // Get the current entry from newest to oldest
                    for (int i = eventLog.Entries.Count - 1; i > 0; i--)
                    {
                        // Get the current entry from newest to oldest
                        var currentEntry = eventLog.Entries[i];
                        if (currentEntry.Source != eventSource)
                        {
                            continue;
                        }
                        // Found the correct entry, verity the contents
                        var logMessage = currentEntry.Message;
                        Assert.IsTrue(logMessage.Contains(message));
                        foreach (var pair in runtimeInformation)
                        {
                            Assert.IsTrue(logMessage.Contains(pair.Key));
                            Assert.IsTrue(logMessage.Contains(pair.Value as string));
                        }
                        return;
                    }
                    Assert.Fail();
                }
            }
        }
    }
}
