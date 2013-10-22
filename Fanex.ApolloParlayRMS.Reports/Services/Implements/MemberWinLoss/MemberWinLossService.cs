// --------------------------------------------------------------------------
// <copyright file="MemberWinLossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.15">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Services.Implements.MemberWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.ApolloParlayRMS.Core.Constants;
    using Fanex.ApolloParlayRMS.Core.ExcelUtility;
    using Fanex.ApolloParlayRMS.Core.Extensions;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Core.Services.Implements;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.Reports.Constants;
    using Fanex.ApolloParlayRMS.Reports.Criteria.MemberWinLoss;
    using Fanex.ApolloParlayRMS.Reports.Models.MemberWinLoss;
    using Fanex.ApolloParlayRMS.Reports.Repositories.Implements.MemberWinLoss;
    using Fanex.ApolloParlayRMS.Reports.Repositories.Interfaces.MemberWinLoss;
    using Fanex.ApolloParlayRMS.Reports.Resources;
    using Fanex.ApolloParlayRMS.Reports.Services.Interfaces.MemberWinLoss;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Member win loss service.
    /// </summary>
    public class MemberWinLossService : BasedService, IMemberWinLossService
    {
        private readonly IMemberWinLossRepository _memberWinLossRepository;
        private readonly IDynamicRepository _userRepository;

        public MemberWinLossService() :
            this(null, null)
        {
        }

        public MemberWinLossService(IMemberWinLossRepository memberWinLossRepository, IDynamicRepository userRepository)
        {
            this._memberWinLossRepository = memberWinLossRepository ?? new MemberWinLossRepository();
            this._userRepository = userRepository ?? new DynamicRepository();
        }

        /// <summary>
        /// Gets the member win loss data.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>Queried and grouped member win loss data.</returns>
        public MemberWinLossItem GetMemberWinLossData(GetMemberWinLossCriteria criteria)
        {
            MemberWinLossItem result = null;
            try
            {
                var user = this._userRepository.Get<CustomerInfo>(new GetCustomerByUserNameCriteria
                {
                    UserName = criteria.UserName
                });
                criteria.RoleId = user.RoleId;

                if (!criteria.IsLoadNew)
                {
                    result = SessionManager.Get<MemberWinLossItem>(ReportConstants.WinLossSessionKey);
                }

                if (result == null)
                {
                    List<MemberWinLossItem> rawData = this._memberWinLossRepository.GetMemberWinLossData(criteria).ToList();
                    if ((rawData != null) && (rawData.Count > 0))
                    {
                        if (criteria.RoleId == (int)CommonEnums.RoleId.Member)
                        {
                            result = new MemberWinLossItem();
                            rawData.ToList().ForEach(obj =>
                            {
                                obj.OurPTNetWinLoss = obj.OurPTWon + obj.OurPTLose;
                                obj.Month = ReFormatMonthStr(obj.Month.Trim());
                                result.Add(obj);
                                result.ChildItems.Add(obj);
                            });
                        }
                        else
                        {
                            result = GroupAgentDataByMonth(rawData);
                        }

                        SessionManager.Set<MemberWinLossItem>(ReportConstants.WinLossSessionKey, result);
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
        /// <param name="agentTemplatePath">The path of agent win loss template.</param>
        /// <param name="memberTemplatePath">The path of member win loss template.</param>
        public void ExportMemberWinLoss(GetMemberWinLossCriteria criteria, string agentTemplatePath, string memberTemplatePath)
        {
            try
            {
                MemberWinLossItem winLoss = GetMemberWinLossData(criteria);
                bool isMember = criteria.RoleId == (int)CommonEnums.RoleId.Member;
                string fileName = string.Format(
                                             ReportsLang.MemberWinLossFileName,
                                             criteria.UserName,
                                             criteria.FromDate.ToMonthDayYearFormat().Replace("-", string.Empty),
                                             criteria.ToDate.ToMonthDayYearFormat().Replace("-", string.Empty));
                string reportTitle = criteria.ToString();
                string templatePath = isMember ? memberTemplatePath : agentTemplatePath;
                var writer = new ExcelTemplateWriter(templatePath, fileName, ReportsLang.MemberWinLossSheetName);

                // Copy title.
                writer.CopyRows(0, 2);
                writer.WriteRow(2, new { reportTitle });
                writer.CopyRows(3, 2);

                if ((winLoss != null) && (winLoss.ChildItems.Count > 0))
                {
                    if (isMember)
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
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Renders the agent win loss report to Excel file.
        /// </summary>
        /// <param name="data">The raw data of agent win loss report.</param>
        /// <param name="writer">The writer that is used to write Excel content.</param>
        private void RenderAgentWinLossExcel(MemberWinLossItem data, ExcelTemplateWriter writer)
        {
            // Write content
            foreach (var item in data.ChildItems)
            {
                if (item.IsSutotalRow)
                {
                    writer.WriteCellRange(writer.RowId - item.NumOfSubrows, item.NumOfSubrows, 1, 1, item.Month);

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
                        item.UserName,
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
                data.TicketWon,
                data.TicketLose,
                data.StakeWon,
                data.StakeLose,
                data.MemberWon,
                data.MemberLose,
                data.OurPTWon,
                data.OurPTLose,
                data.OurPTNetWinLoss
            };

            writer.WriteRow(7, total);
            writer.SyncColumnsWidth(11);
        }

        /// <summary>
        /// Renders the member win loss report to Excel file.
        /// </summary>
        /// <param name="data">The raw data of member win loss report.</param>
        /// <param name="writer">The writer that is used to write Excel content.</param>
        private void RenderMemberWinLossExcel(MemberWinLossItem data, ExcelTemplateWriter writer)
        {
            // Write content
            foreach (var item in data.ChildItems)
            {
                var row = new object[]
                {
                    item.Month,
                    item.UserName,
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
                writer.WriteRows(5, row);
            }

            // Total row
            var total = new object[]
            {
                Lang.Total,
                string.Empty,
                data.TicketWon,
                data.TicketLose,
                data.StakeWon,
                data.StakeLose,
                data.MemberWon,
                data.MemberLose,
                data.OurPTWon,
                data.OurPTLose,
                data.OurPTNetWinLoss
            };
            writer.WriteRow(6, total);
            writer.SyncColumnsWidth(10);
        }

        /// <summary>
        /// Group raw data of agent win loss report by month.
        /// </summary>
        /// <param name="rawData">Raw data queried from DB.</param>
        /// <returns>Data for agent win loss report that are grouped by month.</returns>
        private MemberWinLossItem GroupAgentDataByMonth(List<MemberWinLossItem> rawData)
        {
            MemberWinLossItem records = new MemberWinLossItem();
            MemberWinLossItem subtotal = new MemberWinLossItem();
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
                            records.ChildItems[firstRecordIndex].NumOfSubrows = subtotal.NumOfSubrows;
                            records.ChildItems.Add(subtotal);
                            records.Add(subtotal);
                            firstRecordIndex = records.ChildItems.Count;
                        }

                        subtotal = new MemberWinLossItem
                        {
                            Month = currentRecord.Month,
                            IsSutotalRow = true
                        };
                    }

                    subtotal.Add(currentRecord);
                    records.ChildItems.Add(currentRecord);
                });

                if (!string.IsNullOrEmpty(subtotal.Month))
                {
                    records.ChildItems[firstRecordIndex].NumOfSubrows = subtotal.NumOfSubrows;
                    records.ChildItems.Add(subtotal);
                    records.Add(subtotal);
                }
            }

            return records;
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
