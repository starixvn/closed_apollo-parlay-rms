// --------------------------------------------------------------------------
// <copyright file="HSSFCellRangeAddressExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.ExcelUtility.NPOIExtensions
{
    using NPOI.SS.Util;

    /// <summary>
    /// Cell Range Address Extensions.
    /// </summary>
    public static class HSSFCellRangeAddressExtensions
    {
        /// <summary>
        /// Clones cell range address.
        /// </summary>
        /// <param name="range">Current cell range address.</param>
        /// <returns>New cell range address instance.</returns>
        public static CellRangeAddress Clone(this CellRangeAddress range)
        {
            CellRangeAddress result = new CellRangeAddress(range.FirstRow, range.LastRow, range.FirstColumn, range.LastColumn);
            return result;
        }
    }
}