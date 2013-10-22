// --------------------------------------------------------------------------
// <copyright file="NumberExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Extensions
{
    using System;

    /// <summary>
    /// Number Extensions.
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Formats integer.
        /// </summary>
        /// <param name="value">Integer value needs to be formatted.</param>
        /// <returns>Integer value after rounding and formatting.</returns>
        public static string Format(this int value)
        {
            return DoFormat(value, 0);
        }

        /// <summary>
        /// Formats Object.
        /// </summary>
        /// <param name="value">Value needs to be formatted.</param>
        /// <returns>Value after rounding and formatting.</returns>
        public static string Format(this object value)
        {
            return DoFormat(value, 0);
        }

        /// <summary>
        /// Formats Decimal Value.
        /// </summary>
        /// <param name="value">Decimal value needs to be formatted.</param>
        /// <param name="decDigits">Number of digits after the point.</param>
        /// <returns>Decimal value after rounding and formatting.</returns>
        public static string Format(this decimal value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Formats Object value.
        /// </summary>
        /// <param name="value">Object value needs to be formatted.</param>
        /// <param name="decDigits">Number of digits after the point.</param>
        /// <returns>Value after rounding and formatting.</returns>
        public static string Format(this object value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Formats Double Value.
        /// </summary>
        /// <param name="value">Double value needs to be formatted.</param>
        /// <param name="decDigits">Number of digits after the point.</param>
        /// <returns>Double value after rounding and formatting.</returns>
        public static string Format(this double value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Formats Float Value.
        /// </summary>
        /// <param name="value">Float value needs to be formatted.</param>
        /// <param name="decDigits">Number of digits after the point.</param>
        /// <returns>Float value after rounding and formatting.</returns>
        public static string Format(this float value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Formats specified number value.
        /// </summary>
        /// <param name="value">Value needs to be formatted.</param>
        /// <param name="decDigits">Number of digits after the point.</param>
        /// <returns>Value after rounding and formatting.</returns>
        private static string DoFormat(object value, byte decDigits)
        {
            if (value != null)
            {
                string formatString = string.Format("{0:n{0}}", decDigits);
                string result = null;
                if (value != null)
                {
                    result = string.Format(formatString, value);
                }

                return result;
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}