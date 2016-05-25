//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EventItemAttributeUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.DiagnosticsTest
{
    using System.Diagnostics.CodeAnalysis;
    using Common.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.UnitTestHelper;

    /// <summary>
    /// Event item attribute unit test
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EventItemAttributeUnitTest
    {
        /// <summary>
        /// Test default constructor
        /// </summary>
        [TestMethod]
        public void EventItemAttribute_DefaultConstructor()
        {
            var attribute = new EventItemAttribute();
            Assert.AreEqual(EventItemAttribute.DefaultEventLevel, attribute.Level);
        }

        /// <summary>
        /// Test constructor which accepts event level
        /// </summary>
        [TestMethod]
        public void EventItemAttribute_Constructor_PassLevel()
        {
            var allLevels = EnumHelper.GetAllPossibleValues<EventLevel>();
            foreach (var level in allLevels)
            {
                var attribute = new EventItemAttribute(level);
                Assert.AreEqual(level, attribute.Level);
            }
        }
    }
}
