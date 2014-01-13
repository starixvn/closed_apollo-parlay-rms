// --------------------------------------------------------------------------
// <copyright file="MemberWinLossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.15">Created</change>
// </history>

namespace Apollo.ParlayRMS.Reports.Services.Implements.MemberWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Apollo.ParlayRMS.Core.Constants;
    using Apollo.ParlayRMS.Core.Criteria;
    using Apollo.ParlayRMS.Core.ExcelUtility;
    using Apollo.ParlayRMS.Core.Models;
    using Apollo.ParlayRMS.Core.Resources;
    using Apollo.ParlayRMS.Core.Services;
    using Apollo.ParlayRMS.Core.Web;
    using Apollo.ParlayRMS.Reports.Constants;
    using Apollo.ParlayRMS.Reports.Criteria.MemberWinLoss;
    using Apollo.ParlayRMS.Reports.Models.MemberWinLoss;
    using Apollo.ParlayRMS.Reports.Resources;
    using Apollo.ParlayRMS.Reports.Services.Interfaces.MemberWinLoss;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Web;

    /// <summary>
    /// Member win loss service.
    /// </summary>
    public class MemberWinLossService : BaseService, IMemberWinLossService
    {
        /// <summary>
        /// Gets the member win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>Queried and grouped member win loss data.</returns>
        public MemberWinLossList GetMemberWinLossData(GetMemberWinLossCriteria criteria)
        {
            MemberWinLossList result = null;

            if (!criteria.IsLoadNew)
            {
                result = SessionManager.Get<MemberWinLossList>(ReportConstants.AgentMemberWinLossSessionKey);
            }

            if (result == null)
            {
                BalanceUp balanceUp = BalanceUpService.GetBalanceUp();
                CustomerInfo user = CustomerService.GetCustomerInfo(new GetCustomerByUsernameCriteria
                {
                    Username = criteria.Username
                });

                criteria.IsMember = user.RoleId == (int)CommonEnums.RoleId.Member;
                criteria.CustId = user.CustId;
                criteria.BalanceUpDay = balanceUp.BalanceUpDay;
                criteria.MoveBetsDay = balanceUp.MoveBetsDay;

                List<MemberWinLossItem> rawData = this.GetData<MemberWinLossItem>(criteria).ToList();
                if ((rawData != null) && (rawData.Count > 0))
                {
                    if (criteria.IsMember)
                    {
                        result = new MemberWinLossList();
                        rawData.ToList().ForEach(obj =>
                        {
                            obj.OurPTNetWinLoss = obj.OurPTWon + obj.OurPTLose;
                            obj.Month = ReFormatMonthStr(obj.Month.Trim());
                            result.Total.Add(obj);
                            result.ChildItems.Add(obj);
                        });
                    }
                    else
                    {
                        result = GroupAgentDataByMonth(rawData);
                    }

                    SessionManager.Set<MemberWinLossList>(ReportConstants.AgentMemberWinLossSessionKey, result);
                }
            }

            return result;
        }

        /// <summary>
        /// Exports the win loss report to Excel file.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="agentTemplatePath">The path of agent win loss template.</param>
        /// <param name="memberTemplatePath">The path of member win loss template.</param>
        public void ExportMemberWinLoss(GetMemberWinLossCriteria criteria, string agentTemplatePath, string memberTemplatePath)
        {
            MemberWinLossList winLoss = GetMemberWinLossData(criteria);
            string fileName = string.Format(
                                         ReportsLang.MemberWinLossFileName,
                                         criteria.Username,
                                         criteria.FromDate.ToShortFormatedDate().Replace("/", string.Empty),
                                         criteria.ToDate.ToShortFormatedDate().Replace("/", string.Empty));
            string reportTitle = criteria.ToString();
            string templatePath = criteria.IsMember ? memberTemplatePath : agentTemplatePath;
            var writer = new ExcelTemplateWriter(templatePath, fileName, ReportsLang.MemberWinLossSheetName);

            // Copy title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new { reportTitle });
            writer.CopyRows(3, 2);

            if ((winLoss != null) && (winLoss.ChildItems.Count > 0))
            {
                if (criteria.IsMember)
                {
                    RenderMemberWinLossExcel(winLoss, writer);
                }
                else
                {
                    RenderAgentWinLossExcel(winLoss, writer);
                }
            }

            writer.Render();
        }

        /// <summary>
        /// Renders the agent win loss report to Excel file.
        /// </summary>
        /// <param name="data">The raw data of agent win loss report.</param>
        /// <param name="writer">The writer that is used to write Excel content.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        private void RenderAgentWinLossExcel(MemberWinLossList data, ExcelTemplateWriter writer)
        {
            // Write content
            foreach (var item in data.ChildItems)
            {
                if (item.IsSutotalRow)
                {
                    writer.WriteCellRange(writer.RowId - item.NumOfSubrows, item.NumOfSubrows, 0, 1, item.Month);

                    // For WinLossDate summary
                    var subtotal = new object[]
                    {
                        Lang.Subtotal,
                        string.Empty,
                        item.TicketWon,
                        item.TicketLose,
                        item.StakeWon,
                        item.StakeLose,
                        item.MemberWon,
                        item.MemberLose,
                        item.OurPTWon,
                        item.OurPTLose,
                        item.OurPTNetWinLoss
                    };
                    writer.WriteRow(6, subtotal);
                }
                else
                {
                    var row = new object[]
                    {
                        item.Month,
                        item.Username,
                        item.TicketWon,
                        item.TicketLose,
                        item.StakeWon,
                        item.StakeLose,
                        item.MemberWon,
                        item.MemberLose,
                        item.OurPTWon,
                        item.OurPTLose,
                        item.OurPTNetWinLoss
                    };
                    writer.WriteRow(5, row);
                }
            }

            // Total row
            var total = new object[]
            {
                Lang.Total,
                string.Empty,
                data.Total.TicketWon,
                data.Total.TicketLose,
                data.Total.StakeWon,
                data.Total.StakeLose,
                data.Total.MemberWon,
                data.Total.MemberLose,
                data.Total.OurPTWon,
                data.Total.OurPTLose,
                data.Total.OurPTNetWinLoss
            };

            writer.WriteRow(7, total);
            writer.SyncColumnsWidth(11);
        }

        /// <summary>
        /// Renders the member win loss report to Excel file.
        /// </summary>
        /// <param name="data">The raw data of member win loss report.</param>
        /// <param name="writer">The writer that is used to write Excel content.</param>
        private void RenderMemberWinLossExcel(MemberWinLossList data, ExcelTemplateWriter writer)
        {
            // Write content
            var records = data.ChildItems.Select(item => new
            {
                Month = item.Month,
                TicketWon = item.TicketWon,
                TicketLose = item.TicketLose,
                StakeWon = item.StakeWon,
                StakeLose = item.StakeLose,
                MemberWon = item.MemberWon,
                MemberLose = item.MemberLose,
                OurPTWon = item.OurPTWon,
                OurPTLose = item.OurPTLose,
                OurPTNetWinLoss = item.OurPTNetWinLoss
            });
            writer.WriteRows(5, records);

            // Total row
            var total = new object[]
            {
                Lang.Total,
                data.Total.TicketWon,
                data.Total.TicketLose,
                data.Total.StakeWon,
                data.Total.StakeLose,
                data.Total.MemberWon,
                data.Total.MemberLose,
                data.Total.OurPTWon,
                data.Total.OurPTLose,
                data.Total.OurPTNetWinLoss
            };
            writer.WriteRow(6, total);
            writer.SyncColumnsWidth(10);
        }

        /// <summary>
        /// Group raw data of agent win loss report by month.
        /// </summary>
        /// <param name="rawData">Raw data queried from DB.</param>
        /// <returns>Data for agent win loss report that are grouped by month.</returns>
        private MemberWinLossList GroupAgentDataByMonth(List<MemberWinLossItem> rawData)
        {
            var result = new MemberWinLossList();
            var subtotal = new MemberWinLossItem();
            int firstRecordIndex = 0;
            int numOfRecords = rawData.Count;

            if (numOfRecords > 0)
            {
                rawData.ForEach(currentRecord =>
                {
                    currentRecord.OurPTNetWinLoss = currentRecord.OurPTWon + currentRecord.OurPTLose;
                    currentRecord.Month = ReFormatMonthStr(currentRecord.Month.Trim());

                    if (!currentRecord.Month.Equals(subtotal.Month, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(subtotal.Month))
                        {
                            result.ChildItems[firstRecordIndex].NumOfSubrows = subtotal.NumOfSubrows;
                            result.ChildItems.Add(subtotal);
                            result.Total.Add(subtotal);
                            firstRecordIndex = result.ChildItems.Count;
                        }

                        subtotal = new MemberWinLossItem
                        {
                            Month = currentRecord.Month,
                            IsSutotalRow = true
                        };
                    }

                    subtotal.Add(currentRecord);
                    result.ChildItems.Add(currentRecord);
                });

                if (!string.IsNullOrEmpty(subtotal.Month))
                {
                    result.ChildItems[firstRecordIndex].NumOfSubrows = subtotal.NumOfSubrows;
                    result.ChildItems.Add(subtotal);
                    result.Total.Add(subtotal);
                }
            }

            return result;
        }

        /// <summary>
        /// Format month string from '2013-01' to '01-2013'.
        /// </summary>
        /// <param name="date">The date that needs to be converted.</param>
        /// <returns>Converted value.</returns>
        private string ReFormatMonthStr(string date)
        {
            int separatorIndex = date.IndexOf('-');
            string year = date.Substring(0, separatorIndex);
            string month = date.Substring(separatorIndex + 1, 2);

            return string.Format("{0}-{1}", month, year);
        }
    }
}