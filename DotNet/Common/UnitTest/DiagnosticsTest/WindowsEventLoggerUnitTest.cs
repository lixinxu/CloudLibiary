//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WindowsEventLoggerUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.DiagnosticsTest
{
    using Common.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.UnitTestHelper;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

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
            var testCount = 1;
            var random = new Random();

            var logger = new WindowsEventLogger();
            var stackTrace = new StackTrace(true);

            // Keep track of all the data for verifying later
            var testInformationList = new Dictionary<string, Tuple<string, Dictionary<string, object>>>();

            // Run through all the tests
            for (var testId = 0; testId < testCount; testId++)
            {
                var eventId = Math.Abs(random.Next(256));
                var allEventLevels = EnumHelper.GetAllPossibleValues<EventLevel>();

                // Test all event levels
                foreach (var eventLevel in allEventLevels)
                {
                    var eventSource = TestHelper.CreateUniqueName("SourceName{0}");
                    var message = TestHelper.CreateUniqueName("EventMessage{0}");
                    var runtimeInformation = new Dictionary<string, object>();
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

                    // Add the information to the lists
                    testInformationList.Add(eventSource, new Tuple<string, Dictionary<string, object>>(message, runtimeInformation));
                }
            }

            // Add all the logs to a log stack
            var totalEventLogs = EventLog.GetEventLogs(Environment.MachineName);
            EventLogEntryCollection applicationEventLogs = null;
            foreach (var log in totalEventLogs)
            {
                if (log.Log == "Application")
                {
                    applicationEventLogs = log.Entries;
                }
            }

            Assert.IsNotNull(applicationEventLogs);

            // Go through all the logs and try to match the source
            for (int i = applicationEventLogs.Count - 1; i >= 0; i--)
            {
                var currentEntry = applicationEventLogs[i];
                var currentEntryEventSource = currentEntry.Source;
                Tuple<string, Dictionary<string, object>> resultTestInformation;
                if (!testInformationList.TryGetValue(currentEntryEventSource, out resultTestInformation))
                {
                    continue;
                }

                // Found the correct entry, verity the contents
                var logMessage = currentEntry.Message;

                // Get the messsage and runtime information
                var currentMessage = resultTestInformation.Item1;
                var currentRuntimeInformation = resultTestInformation.Item2;

                // Remove the entry from the dictionary
                testInformationList.Remove(currentEntryEventSource);

                // Verify the test information
                Assert.IsTrue(logMessage.Contains(currentMessage));
                foreach (var pair in currentRuntimeInformation)
                {
                    Assert.IsTrue(logMessage.Contains(pair.Key));
                    Assert.IsTrue(logMessage.Contains(pair.Value as string));
                }
            }

            // If there are still things left in the lists, that means there is a log that was NOT written to the windwos event logs
            Assert.IsTrue(testInformationList.IsNullOrEmpty());
        }
    }
}
