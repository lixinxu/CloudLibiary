//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EventInformationCollectionUnitTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.UnitTest.DiagnosticsTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Reflection.Emit;
    using Common.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shared.UnitTestHelper;

    /// <summary>
    /// Unit tests for EventInformationCollection
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EventInformationCollectionUnitTest
    {
        /// <summary>
        /// Empty object array
        /// </summary>
        private static readonly object[] EmpyObjectArray = new object[0];

        /// <summary>
        /// Test normal. All data are set
        /// </summary>
        [TestMethod]
        public void EventInformationCollection_Test()
        {
            var itemCount = 5;
            var itemData = new Tuple<string, int, EventLevel>[itemCount];
            var sourceName = TestHelper.CreateUniqueName("EventSourceName{0}");
            var type = CreateEventEnumeratorType(itemData, true, sourceName, true, true);
            Test(type, sourceName, itemData);
        }
        
        /// <summary>
        /// Test normal event source is null
        /// </summary>
        [TestMethod]
        public void EventInformationCollection_SourceNameNull()
        {
            var itemCount = 5;
            var itemData = new Tuple<string, int, EventLevel>[itemCount];
            string sourceName = null;
            var type = CreateEventEnumeratorType(itemData, true, sourceName, true, true);
            Test(type, sourceName, itemData);
        }

        /// <summary>
        /// Test or collection attribute
        /// </summary>
        [TestMethod]
        public void EventInformationCollection_NoCollecionAttribute()
        {
            var itemCount = 5;
            var itemData = new Tuple<string, int, EventLevel>[itemCount];
            string sourceName = null;
            var type = CreateEventEnumeratorType(itemData, false, sourceName, true, true);
            Test(type, sourceName, itemData);
        }

        /// <summary>
        /// Test on item attribute
        /// </summary>
        [TestMethod]
        public void EventInformationCollection_NoItemAttribute()
        {
            var itemCount = 5;
            var itemData = new Tuple<string, int, EventLevel>[itemCount];
            var sourceName = TestHelper.CreateUniqueName("EventSourceName{0}");
            var type = CreateEventEnumeratorType(itemData, true, sourceName, false, false);
            Test(type, sourceName, itemData);
        }

        /// <summary>
        /// Test on item attribute
        /// </summary>
        [TestMethod]
        public void EventInformationCollection_DefaultLevel()
        {
            var itemCount = 5;
            var itemData = new Tuple<string, int, EventLevel>[itemCount];
            var sourceName = TestHelper.CreateUniqueName("EventSourceName{0}");
            var type = CreateEventEnumeratorType(itemData, true, sourceName, true, false);
            Test(type, sourceName, itemData);
        }

        /// <summary>
        /// Test event item information collection
        /// </summary>
        /// <param name="type">type of event enumerator</param>
        /// <param name="sourceName">event source name</param>
        /// <param name="itemDataList">test item list</param>
        private static void Test(Type type, string sourceName, Tuple<string, int, EventLevel>[] itemDataList)
        {
            var methodInformation = typeof(EventInformationCollectionUnitTest)
                .GetMethod(nameof(TestGeneric), BindingFlags.Static | BindingFlags.NonPublic)
                .MakeGenericMethod(type);
            methodInformation.Invoke(null, new object[] { sourceName, itemDataList });
        }

        /// <summary>
        /// Test event item information collection
        /// </summary>
        /// <typeparam name="T">type of event enumerator</typeparam>
        /// <param name="sourceName">event source name</param>
        /// <param name="itemDataList">test data list</param>
        private static void TestGeneric<T>(string sourceName, Tuple<string, int, EventLevel>[] itemDataList)
        {
            var informationCollection = new EventInformationCollection<T>();
            Assert.AreEqual(sourceName ?? typeof(T).Name, informationCollection.SourceName);

            EventItemInformation itemInformaion;
            foreach (var testItemData in itemDataList)
            {
                var id = (T)Enum.Parse(typeof(T), testItemData.Item1);
                itemInformaion = informationCollection[id];
                Assert.IsNotNull(itemInformaion);
                Assert.AreEqual(testItemData.Item1, itemInformaion.Name);
                Assert.AreEqual(testItemData.Item2, itemInformaion.Id);
                Assert.AreEqual(testItemData.Item3, itemInformaion.Level);
            }

            var invalidId = (T)(object)(itemDataList.Length * 2);
            Assert.IsNull(informationCollection[invalidId]);
        }

        /// <summary>
        /// Create a test event enumerator
        /// </summary>
        /// <param name="itemData">item data list</param>
        /// <param name="hasCollectionAttribute">need to add collection attribute</param>
        /// <param name="sourceName">storage name for collection attribute. if no attribute, this value will be ignored</param>
        /// <param name="hasItemAttribute">create item attribute</param>
        /// <param name="setEventLevel">give event level when calling constructor </param>
        /// <returns>resource enumerator type</returns>
        private static Type CreateEventEnumeratorType(
            Tuple<string, int, EventLevel>[] itemData,
            bool hasCollectionAttribute,
            string sourceName,
            bool hasItemAttribute,
            bool setEventLevel)
        {
            var random = new Random();
            var modelBuilder = TestHelper.CreateModuleBuilder();
            var enumBuilder = modelBuilder.DefineEnum(TestHelper.CreateUniqueName("Enumerator{0}"), TypeAttributes.Public, typeof(int));
            if (hasCollectionAttribute)
            {
                var collectionAttributeBuilder = new CustomAttributeBuilder(
                    typeof(EventCollectionAttribute).GetConstructor(new Type[] { typeof(string) }),
                    new object[] { sourceName });
                enumBuilder.SetCustomAttribute(collectionAttributeBuilder);
            }

            var values = new HashSet<int>();
            var itemConstructorInformation = typeof(EventItemAttribute).GetConstructor(new Type[] { typeof(EventLevel) });
            var itemDefaultConstructorInformation = typeof(EventItemAttribute).GetConstructor(Type.EmptyTypes);
            var allEventLevels = EnumHelper.GetAllPossibleValues<EventLevel>();
            for (var i = 0; i < itemData.Length; i++)
            {
                var enumName = TestHelper.CreateUniqueName("Item{0}");
                int value;
                do
                {
                    value = Math.Abs(random.Next(itemData.Length * 2));
                }
                while (values.Contains(value));
                values.Add(value);

                var level = (hasItemAttribute && setEventLevel) ? allEventLevels[random.Next(allEventLevels.Count)] : EventItemAttribute.DefaultEventLevel;
                itemData[i] = new Tuple<string, int, EventLevel>(enumName, value, level);

                var itemBuilder = enumBuilder.DefineLiteral(enumName, value);
                if (hasItemAttribute)
                {
                    CustomAttributeBuilder itemAttributeBuilder;
                    if (setEventLevel)
                    {
                        itemAttributeBuilder = new CustomAttributeBuilder(itemConstructorInformation, new object[] { level });
                    }
                    else
                    {
                        itemAttributeBuilder = new CustomAttributeBuilder(itemDefaultConstructorInformation, EmpyObjectArray);
                    }

                    itemBuilder.SetCustomAttribute(itemAttributeBuilder);
                }
            }

            return enumBuilder.CreateType();
        }
    }
}
