//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TraceListenerForTest.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Shared.CommonUnitTestHelper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Common.Diagnostics;

    /// <summary>
    /// Trace listener for test
    /// </summary>
    /// <remarks>
    /// To use the listener, need to add following in app.config in test project
    /// <![CDATA[
    /// <configuration>
    ///     ...
    ///     <system.diagnostics>
    ///         <trace autoflush = "false" indentsize="4">
    ///             <listeners>
    ///                 <add name="TraceForTest" 
    ///                     type="CloudLibrary.Shared.CommonUnitTestHelper.TraceListenerForTest, CloudLibrary.Shared.CommonUnitTestHelper" 
    ///                     initializeData="" />
    ///             </listeners>
    ///             ...
    ///         </trace>
    ///     </system.diagnostics>
    ///     ...
    /// </configuration>    
    /// /// ]]>
    /// </remarks>
    /// <history>
    ///     <create time="2016/5/16" author="lixinxu" />
    /// </history>
    [ExcludeFromCodeCoverage]
    public class TraceListenerForTest : TraceListener
    {
        /// <summary>
        /// Trace message storage
        /// </summary>
        /// <remarks>
        /// This class is used for unit test so we can get the trace information.
        /// </remarks>
        private static readonly IList<string> MessageStore = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceListenerForTest" /> class.
        /// </summary>
        public TraceListenerForTest() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceListenerForTest" /> class.
        /// </summary>
        /// <param name="name">listener name</param>
        public TraceListenerForTest(string name) : base(name)
        {
        }

        /// <summary>
        /// Clear message
        /// </summary>
        public static void ClearMessage()
        {
            lock (MessageStore)
            {
                MessageStore.Clear();
            }
        }

        /// <summary>
        /// Copy messages for validation
        /// </summary>
        /// <returns>messages from message store</returns>
        public static IReadOnlyList<string> CopyMessges()
        {
            List<string> copy;
            lock (MessageStore)
            {
                copy = new List<string>(MessageStore);
            }

            return copy;
        }

        /// <summary>
        /// Write trace message
        /// </summary>
        /// <param name="message">message to write</param>
        /// <remarks>
        /// If the message does not contain trace prefix, the message will be ignore. The reason is that we want to trace the 
        /// message from our system only.
        /// </remarks>
        public override void Write(string message)
        {
            if ((message != null) && message.StartsWith(TraceLogger.TracePrefix, StringComparison.Ordinal))
            {
                lock (MessageStore)
                {
                    MessageStore.Add(message);
                }
            }
        }

        /// <summary>
        /// Write message with new line
        /// </summary>
        /// <param name="message">message to write</param>
        public override void WriteLine(string message)
        {
            this.Write(message + Environment.NewLine);
        }
    }
}
