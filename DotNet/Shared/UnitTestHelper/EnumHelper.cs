//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnumHelper.cs" company="CloudLibrary">
//    Copyright (c) CloudLibrary 2016.  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace CloudLibrary.Shared.UnitTestHelper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Enumerator helpers
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class EnumHelper
    {
        /// <summary>
        /// Get all enumerator possible values
        /// </summary>
        /// <typeparam name="T">type of enumerator</typeparam>
        /// <returns>possible value list</returns>
        public static IReadOnlyList<T> GetAllPossibleValues<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"type {typeof(T).FullName} is not enumerator");
            }

            return EnumValues<T>.AllPossibleValues;
        }

        /// <summary>
        /// Enumerator possible values storage
        /// </summary>
        /// <typeparam name="T">type of enumerator</typeparam>
        [ExcludeFromCodeCoverage]
        private static class EnumValues<T>
        {
            /// <summary>
            /// Initializes static members of the <see cref="EnumValues{T}" /> class.
            /// </summary>
            static EnumValues()
            {
                var type = typeof(T);
                IReadOnlyList<T> values = Enum.GetValues(type) as T[];

                var attribute = type.GetCustomAttributes<FlagsAttribute>();
                if (attribute == null)
                {
                    var combined = new HashSet<T>();
                    foreach (var flag in values)
                    {
                        combined.Add(flag);
                    }

                    int max = 1 << values.Count;
                    for (int mask = 0; mask < max; mask++)
                    {
                        long flag = 0L;
                        var detector = 1;
                        var index = 0;
                        while (detector < max)
                        {
                            if ((mask & detector) != 0)
                            {
                                flag += (long)(object)values[index];
                            }

                            index++;
                        }

                        combined.Add((T)Enum.ToObject(type, flag));
                    }

                    values = combined.ToArray();
                }

                AllPossibleValues = values;
            }

            /// <summary>
            /// Gets all possible values
            /// </summary>
            internal static IReadOnlyList<T> AllPossibleValues { get; }
        }
    }
}
