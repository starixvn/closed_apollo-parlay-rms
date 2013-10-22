// --------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.15">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Extensions
{
    using System;

    /// <summary>
    /// String extensions.
    /// </summary>
    public class StringExtensions
    {
        public enum TruncateOptions
        {
            /// <summary>
            /// Normal truncation.
            /// </summary>
            None = 0x0,

            /// <summary>
            /// Truncates to the last available word.
            /// </summary>
            FinishWord = 0x1,

            /// <summary>
            /// Allows last word to go over maximum length.
            /// </summary>
            AllowLastWordToGoOverMaxLength = 0x2,

            /// <summary>
            /// Includes '...' at the end of truncated string.
            /// </summary>
            IncludeEllipsis = 0x4
        }

        /// <summary>
        /// Truncates a string to a certain length.
        /// </summary>
        /// <param name="valueToTruncate">Value to truncate.</param>
        /// <param name="maxLength">Maximum length that the string should allow.</param>
        /// <param name="options">Truncate options.</param>
        /// <returns>Truncated string.</returns>
        public static string TruncateString(string valueToTruncate, int maxLength, TruncateOptions options)
        {
            if (valueToTruncate == null || maxLength <= 0)
            {
                return string.Empty;
            }

            if (valueToTruncate.Length <= maxLength)
            {
                return valueToTruncate;
            }

            bool includeEllipsis = (options & TruncateOptions.IncludeEllipsis) == TruncateOptions.IncludeEllipsis;
            bool finishWord = (options & TruncateOptions.FinishWord) == TruncateOptions.FinishWord;
            bool allowLastWordOverflow = (options & TruncateOptions.AllowLastWordToGoOverMaxLength) == TruncateOptions.AllowLastWordToGoOverMaxLength;

            string retValue = valueToTruncate;

            if (includeEllipsis)
            {
                maxLength -= 1;
            }

            int lastSpaceIndex = retValue.LastIndexOf(" ", maxLength, StringComparison.CurrentCultureIgnoreCase);

            if (!finishWord)
            {
                retValue = retValue.Remove(maxLength);
            }
            else if (allowLastWordOverflow)
            {
                int spaceIndex = retValue.IndexOf(" ", maxLength, StringComparison.CurrentCultureIgnoreCase);
                if (spaceIndex != -1)
                {
                    retValue = retValue.Remove(spaceIndex);
                }
            }
            else if (lastSpaceIndex > -1)
            {
                retValue = retValue.Remove(lastSpaceIndex);
            }

            if (includeEllipsis && retValue.Length < valueToTruncate.Length)
            {
                retValue += "...";
            }

            return retValue;
        }
    }
}