//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StringExtensions.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Common
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Trim a string
        /// </summary>
        /// <param name="value">a string to trim</param>
        /// <returns>string after trim</returns>
        /// <remarks>
        /// If string is null or blank, null will be returned. otherwise, return the string after trim
        /// </remarks>
        /// <example>
        /// " data " => "data"
        /// "   " => null
        /// "  \r\n  " => null
        /// null => null
        /// <![CDATA[
        /// public void Foo(string value,...)
        /// {
        ///     this.Name = value.SafeTrim();
        ///     ...
        /// }
        /// ]]>
        /// </example>
        public static string SafeTrim(this string value)
        {
            if (value != null)
            {
                value = value.Trim();
                if (value.Length > 0)
                {
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// Create Secure String
        /// </summary>
        /// <param name="value">string to make it secure</param>
        /// <param name="markReadOnly">make secure string read-only</param>
        /// <returns>secure string instance</returns>
        /// <remarks>
        /// if value is null, null will be returned.
        /// </remarks>
        /// <example>
        /// <![CDATA[
        /// var userName = configurationProvier.GetValue("SqlUserName");
        /// var userPassword = configurationProvier.GetValue("SqlUserPassword");
        /// var credential = new SqlCredential(userName, userPassword.ToSecureString());
        /// ...
        /// ]]>
        /// </example>
        public static SecureString ToSecureString(this string value, bool markReadOnly = true)
        {
            SecureString secureString = null;
            if (value != null)
            {
                secureString = new SecureString();
                foreach (var c in value)
                {
                    secureString.AppendChar(c);
                }

                if (markReadOnly)
                {
                    secureString.MakeReadOnly();
                }
            }

            return secureString;
        }

        /// <summary>
        /// Get plain text from secure string
        /// </summary>
        /// <param name="secureString">secure string instance</param>
        /// <returns>plain text</returns>
        public static string ToPlainText(this SecureString secureString)
        {
            string value = null;
            if (secureString != null)
            {
                IntPtr unmanagedString = IntPtr.Zero;
                try
                {
                    unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                    value = Marshal.PtrToStringUni(unmanagedString);
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
                }
            }
            return value;
        }
    }
}