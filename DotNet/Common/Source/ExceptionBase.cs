//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExceptionBase.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    /// Exception base
    /// </summary>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    [Serializable]
    public class ExceptionBase : Exception
    {
        /// <summary>
        /// event source key for exception data collection
        /// </summary>
        public const string EventSourceDataKey = "EventSource";

        /// <summary>
        /// event id key for exception data collection
        /// </summary>
        public const string EventIdDataKey = "EventId";

        /// <summary>
        /// runtime information item key prefix for exception data collection
        /// </summary>
        public const string RuntineInformationNamesKey = "RuntimeInformationNames";

        /// <summary>
        /// runtime information item key prefix for exception data collection
        /// </summary>
        public const string RuntineInformationValuesKey = "RuntimeInformationValuess";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase" /> class.
        /// </summary>
        /// <param name="eventSource">event source name</param>
        /// <param name="eventId">event id</param>
        /// <param name="message">exception message</param>
        /// <param name="runtimeInformation">runtime information</param>
        /// <param name="innerException">inner exception</param>
        public ExceptionBase(
            string eventSource,
            int eventId,
            string message,
            IReadOnlyDictionary<string, object> runtimeInformation = null,
            Exception innerException = null) : base(message, innerException)
        {
            this.EventSource = eventSource;
            this.EventId = eventId;
            this.RuntimeInformation = runtimeInformation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase" /> class.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual information
        /// about the source or destination.
        /// </param>
        [SecuritySafeCritical]
        protected ExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.EventSource = info.GetString(EventSourceDataKey);
            this.EventId = info.GetInt32(EventIdDataKey);
            var names = info.GetValue(RuntineInformationNamesKey, typeof(string[])) as string[];
            Dictionary<string, object> runtimeInformation = null;
            if (!names.IsNullOrEmpty())
            {
                runtimeInformation = new Dictionary<string, object>(names.Length);
                var values = info.GetValue(RuntineInformationValuesKey, typeof(object[])) as object[];
                for (var i = 0; i < names.Length; i++)
                {
                    runtimeInformation.Add(names[i], values[i]);
                }
            }

            this.RuntimeInformation = runtimeInformation;
        }

        /// <summary>
        /// Gets event source
        /// </summary>
        public string EventSource { get; }
        
        /// <summary>
        /// Gets event id
        /// </summary>
        public int EventId { get; }

        /// <summary>
        /// Gets runtime information
        /// </summary>
        public IReadOnlyDictionary<string, object> RuntimeInformation { get; }

        /// <summary>
        /// Get object data for serialization
        /// </summary>
        /// <param name="info">serialization information</param>
        /// <param name="context">streaming context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(EventSourceDataKey, this.EventSource);
            info.AddValue(EventIdDataKey, this.EventId);
            int count = 0;
            if (this.RuntimeInformation.IsReadOnlyNullOrEmpty())
            {
                info.AddValue(RuntineInformationNamesKey, Empty.GetArray<string>());
            }
            else
            {
                count = this.RuntimeInformation.Count;
                var names = new string[count];
                var values = new object[count];
                var index = 0;
                foreach (var pair in this.RuntimeInformation)
                {
                    names[index] = pair.Key;
                    values[index] = pair.Value;
                    index++;
                }

                info.AddValue(RuntineInformationNamesKey, names);
                info.AddValue(RuntineInformationValuesKey, values);
            }

            base.GetObjectData(info, context);
        }
    }
}