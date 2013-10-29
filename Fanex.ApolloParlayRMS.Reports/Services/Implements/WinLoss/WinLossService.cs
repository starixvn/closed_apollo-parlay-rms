// --------------------------------------------------------------------------
// <copyright file="WinLossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.15">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Services.Implements.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.ApolloParlayRMS.Core.Constants;
    using Fanex.ApolloParlayRMS.Core.ExcelUtility;
    using Fanex.ApolloParlayRMS.Core.Extensions;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Core.Services.Implements;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.Reports.Constants;
    using Fanex.ApolloParlayRMS.Reports.Criteria.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Models.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Repositories.Implements.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Repositories.Interfaces.WinLoss;
    using Fanex.ApolloParlayRMS.Reports.Resources;
    using Fanex.ApolloParlayRMS.Reports.Services.Interfaces.WinLoss;

    /// <summary>
    /// Win loss service.
    /// </summary>
    public class WinLossService : BasedService, IWinLossService
    {
        private readonly IWinLossRepository _winLossRepository;

        public WinLossService() :
            this(null)
        {
        }

        public WinLossService(IWinLossRepository winLossRepository)
        {
            this._winLossRepository = winLossRepository ?? new WinLossRepository();
        }

        /// <summary>
        /// Gets the win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>Queried and grouped win loss data.</returns>
        public WinLossItem GetWinLossData(GetWinLossCriteria criteria)
        {
            WinLossItem result = null;
            try
            {
                if (!criteria.IsLoadNew)
                {
                    result = SessionManager.Get<WinLossItem>(ReportConstants.WinLossSessionKey);
                }

                if (result == null)
                {
                    BalanceUp balanceUp = BalanceUpService.GetBalanceUp();
                    criteria.BalanceUpDay = balanceUp.BalanceUpDay;
                    criteria.MoveBetsDay = balanceUp.MoveBetsDay;

                    List<WinLossItem> rawData = this._winLossRepository.GetWinLossData(criteria).ToList();

                    if ((rawData != null) && (rawData.Count > 0))
                    {
                        result = GroupData(rawData);
                        SessionManager.Set<WinLossItem>(ReportConstants.WinLossSessionKey, result);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        /// <summary>
        /// Exports the win loss report to Excel file.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="templatePath">The template's path.</param>
        public void ExportWinLoss(GetWinLossCriteria criteria, string templatePath)
        {
            try
            {
                WinLossItem winLoss = GetWinLossData(criteria);
                string fileName = string.Format(
                                             ReportsLang.WinLossFileName,
                                             criteria.FromDate.ToShortFormatedDate().Replace("-", string.Empty),
                                             criteria.ToDate.ToShortFormatedDate().Replace("-", string.Empty));
                string reportTitle = criteria.ToString();
                var writer = new ExcelTemplateWriter(templatePath, fileName, Lang.WinLoss);

                // Copy title.
                writer.CopyRows(0, 2);
                writer.WriteRow(2, new { reportTitle });
                writer.CopyRows(3, 2);

                // Write content
                foreach (var dateData in winLoss.ChildItems)
                {
                    // For combos
                    int numOfItems = dateData.ChildItems.Count();
                    var combos = dateData.ChildItems.Select(obj => new
                    {
                        Date = obj.WinLossDate,
                        Combo = (obj.Combo == (int)CommonConstants.SixUp) ? Lang.SixUp : obj.Combo.ToString(),
                        TicketWon = obj.TicketWon,
                        TicketLose = obj.TicketLose,
                        StakeWon = obj.StakeWon,
                        StakeLose = obj.StakeLose,
                        MemberWon = obj.MemberWon,
                        MemberLose = obj.MemberLose,
                        OurPTWon = obj.OurPTWon,
                        OurPTLose = obj.OurPTLose,
                        OurPTNetWinLoss = obj.OurPTNetWinLoss
                    });
                    writer.WriteRows(5, combos);
                    writer.WriteCellRange(writer.RowId - numOfItems, numOfItems, 0, 1, winLoss.ChildItems[0].WinLossDate);

                    // For WinLossDate summary
                    var subtotal = new object[]
                    {
                        Lang.Subtotal,
                        string.Empty,
                        dateData.TicketWon,
                        dateData.TicketLose,
                        dateData.StakeWon,
                        dateData.StakeLose,
                        dateData.MemberWon,
                        dateData.MemberLose,
                        dateData.OurPTWon,
                        dateData.OurPTLose,
                        dateData.OurPTNetWinLoss
                    };
                    writer.WriteRow(6, subtotal);
                }

                // Total row
                var total = new object[]
                {
                    Lang.Total,
                    string.Empty,
                    winLoss.TicketWon,
                    winLoss.TicketLose,
                    winLoss.StakeWon,
                    winLoss.StakeLose,
                    winLoss.MemberWon,
                    winLoss.MemberLose,
                    winLoss.OurPTWon,
                    winLoss.OurPTLose,
                    winLoss.OurPTNetWinLoss
                };
                
                writer.WriteRow(7, total);
                writer.SyncColumnsWidth(11);
                writer.Render();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Groups the raw data into date.
        /// </summary>
        /// <param name="rawData">The raw data that needs to be grouped.</param>
        /// <returns>WinLossItem object represents the grouped data.</returns>
        private WinLossItem GroupData(IEnumerable<WinLossItem> rawData)
        {
            DateTime currentDate = new DateTime(1970, 1, 1);
            WinLossItem currentDateData = null;
            WinLossItem result = new WinLossItem();

            rawData.ToList().ForEach(obj =>
            {
                if (!currentDate.Equals(obj.WinLossDate))
                {
                    if (currentDateData != null)
                    {
                        result.Add(currentDateData);
                    }
                    
                    currentDateData = new WinLossItem();
                    currentDateData.WinLossDate = obj.WinLossDate;
                    currentDate = obj.WinLossDate;
                }

                obj.OurPTNetWinLoss = obj.OurPTWon + obj.OurPTLose;
                currentDateData.Add(obj);
            });
            result.Add(currentDateData);

            return result;
        }
    }
}