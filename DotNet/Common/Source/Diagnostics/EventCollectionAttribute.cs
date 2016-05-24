//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EventCollectionAttribute.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Enum)]
    public class EventCollectionAttribute : Attribute
    {
        public EventCollectionAttribute(string eventSource)
        {
        }

        public string EventString { get; }
    }
}
