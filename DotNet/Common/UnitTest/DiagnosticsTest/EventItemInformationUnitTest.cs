//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EventItemInformationUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.DiagnosticsTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Common.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.UnitTestHelper;

    /// <summary>
    /// Unit test for EventItemInformation
    /// </summary>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EventItemInformationUnitTest
    {
        /// <summary>
        /// Test EventItemInformation constructor
        /// </summary>
        [TestMethod]
        public void EventItemInformation_Test()
        {
            var testCount = 3;
            var random = new Random();
            for (var i = 0; i < testCount; i++)
            {
                var id = random.Next();
                var name = TestHelper.CreateUniqueName("EventName{0}");
                var allLevels = EnumHelper.GetAllPossibleValues<EventLevel>();
                foreach (var level in allLevels)
                {
                    var itemInformation = new EventItemInformation(name, id, level);
                    Assert.AreEqual(name, itemInformation.Name);
                    Assert.AreEqual(id, itemInformation.Id);
                }
            }
        }
    }
}
