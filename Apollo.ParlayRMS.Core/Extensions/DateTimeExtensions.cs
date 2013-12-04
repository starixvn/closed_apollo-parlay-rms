// --------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Core.Extensions
{
    using System;
    using Apollo.ParlayRMS.Core.Constants;

    /// <summary>
    /// Contains date extension methods.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets first date of month.
        /// </summary>
        /// <param name="dateTime">DateTime type that the method needs to be attached to.</param>
        /// <returns>First date of month.</returns>
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Gets last date of month.
        /// </summary>
        /// <param name="dateTime">DateTime type that the method needs to be attached to.</param>
        /// <returns>Last date of month.</returns>
        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Format a DateTime value to short date format, separated by '-'.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as 2013-10-10.</returns>
        public static string ToShortFormatedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToShortFormatedDate();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format a DateTime value to short date format, separated by '-'.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as 2013-10-10.</returns>
        public static string ToShortFormatedDate(this DateTime date)
        {
            return date.ToString(CommonConstants.ShortDateFormat);
        }

        /// <summary>
        /// Format a DateTime value to long date format.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as Oct 10, 2013 10:10.</returns>
        public static string ToLongFormatedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToLongFormatedDate();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format a DateTime value to long date format.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as Oct 10, 2013 10:10.</returns>
        public static string ToLongFormatedDate(this DateTime date)
        {
            return date.ToString(CommonConstants.LongDateFormat);
        }

        /// <summary>
        /// Format a DateTime value to month and day, separated by '-'.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted string such as '10-Oct'.</returns>
        public static string ToMonthAndDay(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthAndDay();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format a DateTime value to month and day, separated by '-'.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted string such as '10-Oct'.</returns>
        public static string ToMonthAndDay(this DateTime date)
        {
            return date.ToString(CommonConstants.DayMonthFormat);
        }

        /// <summary>
        /// Format a DateTime value to month and year string.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date string such as 'Oct 2013'.</returns>
        public static string ToMonthYear(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthYear();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format a DateTime value to month and year string.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date string such as 'Oct 2013'.</returns>
        public static string ToMonthYear(this DateTime date)
        {
            return date.ToString(CommonConstants.MonthYearFormat);
        }

        /// <summary>
        /// Format a DateTime value to year/month/day format with slash.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date string such as '2013/10/10'.</returns>
        public static string ToYearMonthDateFormatWithSlash(this DateTime date)
        {
            return date.ToString(CommonConstants.YearMonthDateFormatWithSlash);
        }

        /// <summary>
        /// Format a DateTime value to month-day-year format.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date string such as '10-10-2013'.</returns>
        public static string ToMonthDayYearFormat(this DateTime date)
        {
            return date.ToString(CommonConstants.MonthDayYearFormat);
        }

        /// <summary>
        /// Format a DateTime value to short date format with slashes.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as 10/10/2013.</returns>
        public static string ToShortFormatedDateWithSlash(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToShortFormatedDateWithSlash();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format a DateTime value to long date format with slashes.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as 10/10/2013 06:10:55.</returns>
        public static string ToLongFormatedDateWithSlash(this DateTime date)
        {
            return date.ToString(CommonConstants.LongDateFormatWithSlash);
        }

        /// <summary>
        /// Format a DateTime value to short date format with slashes.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as 10/10/2013.</returns>
        public static string ToShortFormatedDateWithSlash(this DateTime date)
        {
            return date.ToString(CommonConstants.ShortDateFormatWithSlash);
        }

        /// <summary>
        /// Format date time value to month and year string.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted month year such as 10/2013.</returns>
        public static string ToMonthYearWithSlash(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthYearWithSlash();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format date time value to month and year string.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted month year such as 10/2013.</returns>
        public static string ToMonthYearWithSlash(this DateTime date)
        {
            return date.ToString(CommonConstants.MonthYearFormatWithSlash);
        }
    }
}