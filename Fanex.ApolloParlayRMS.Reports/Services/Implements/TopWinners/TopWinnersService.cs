// --------------------------------------------------------------------------
// <copyright file="TopWinnersService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Reports.Services.Implements.TopWinners
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.ApolloParlayRMS.Core.ExcelUtility;
    using Fanex.ApolloParlayRMS.Core.Extensions;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Resources;
    using Fanex.ApolloParlayRMS.Core.Services.Implements;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.Reports.Constants;
    using Fanex.ApolloParlayRMS.Reports.Criteria.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Models.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Repositories.Implements.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Repositories.Interfaces.TopWinners;
    using Fanex.ApolloParlayRMS.Reports.Resources;
    using Fanex.ApolloParlayRMS.Reports.Services.Interfaces.TopWinners;

    /// <summary>
    /// Top Winners service.
    /// </summary>
    public class TopWinnersService : BasedService, ITopWinnersService
    {
        private readonly ITopWinnersRepository _topWinnersRepository;

        public TopWinnersService() :
            this(null)
        {
        }

        public TopWinnersService(ITopWinnersRepository topWinnersRepository)
        {
            this._topWinnersRepository = topWinnersRepository ?? new TopWinnersRepository();
        }

        /// <summary>
        /// Gets the top winner list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>TopWinner object contains the top winner list and total data of all top winners.</returns>
        public TopWinner GetTopWinners(GetTopWinnersCriteria criteria)
        {
            TopWinner result = null;
            try
            {
                if (!criteria.IsLoadNew)
                {
                    result = SessionManager.Get<TopWinner>(ReportConstants.TopWinnersSessionKey);
                }
                
                if (result == null)
                {
                    BalanceUp balanceUp = BalanceUpService.GetBalanceUp();
                    criteria.BalanceUpDay = balanceUp.BalanceUpDay;
                    criteria.MoveBetsDay = balanceUp.MoveBetsDay;

                    List<TopWinner> records = this._topWinnersRepository.GetTopWinners(criteria).ToList();
                    result = new TopWinner();
                    records.ForEach(obj =>
                    {
                        result.Add(obj);
                    });

                    SessionManager.Set<TopWinner>(ReportConstants.TopWinnersSessionKey, result);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets a top winner's details.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>TopWinnerDetails object represents the detailed data of a top winner.</returns>
        public TopWinnerDetails GetTopWinnerDetails(GetTopWinnerDetailsCriteria criteria)
        {
            BalanceUp balanceUp = BalanceUpService.GetBalanceUp();
            List<TopWinnerDetails> records = new List<TopWinnerDetails>();
            TopWinnerDetails result = new TopWinnerDetails();

            try
            {
                if (balanceUp.BalanceUpDay.CompareTo(criteria.FromDate) <= 0)
                {
                    records.AddRange(this._topWinnersRepository.GetTopWinnerDetails(criteria));
                }
                else if (balanceUp.BalanceUpDay.CompareTo(criteria.ToDate) > 0)
                {
                    var tempCriteria = new GetTopWinnersDetailsOldCriteria
                    {
                        CustId = criteria.CustId,
                        FromDate = criteria.FromDate,
                        ToDate = criteria.ToDate,
                        SportType = criteria.SportType,
                        Selection = criteria.Selection
                    };
                    records.AddRange(this._topWinnersRepository.GetTopWinnerDetails(tempCriteria));
                }
                else
                {
                    var tempOldCriteria = new GetTopWinnersDetailsOldCriteria
                    {
                        CustId = criteria.CustId,
                        FromDate = criteria.FromDate,
                        ToDate = balanceUp.BalanceUpDay.AddDays(-1),
                        SportType = criteria.SportType,
                        Selection = criteria.Selection
                    };
                    records.AddRange(this._topWinnersRepository.GetTopWinnerDetails(tempOldCriteria));

                    var tempCriteria = new GetTopWinnerDetailsCriteria
                    {
                        CustId = criteria.CustId,
                        FromDate = balanceUp.BalanceUpDay,
                        ToDate = criteria.ToDate,
                        MoveBetsDay = balanceUp.MoveBetsDay,
                        SportType = criteria.SportType,
                        Selection = criteria.Selection
                    };
                    records.AddRange(this._topWinnersRepository.GetTopWinnerDetails(tempCriteria));
                }

                records.ForEach(obj =>
                {
                    result.Add(obj);
                });
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }
        
        /// <summary>
        /// Gets the top winner bet details.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>TopWinner object contains the top winner list and total data of all top winners.</returns>
        public IEnumerable<TopWinnerBetDetails> GetTopWinnerBetDetails(GetTopWinnerBetDetailsCriteria criteria)
        {
            IEnumerable<TopWinnerBetDetails> result = null;
            try
            {
                result = this._topWinnersRepository.GetTopWinnerBetDetails(criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        /// <summary>
        /// Exports the top winners report to Excel file.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="templatePath">The template's path.</param>
        public void ExportTopWinners(GetTopWinnersCriteria criteria, string templatePath)
        {
            try
            {
                TopWinner topWinners = GetTopWinners(criteria);
                string fileName = string.Format(
                                             ReportsLang.TopWinnersFileName,
                                             criteria.LimitedRow,
                                             criteria.FromDate.ToShortFormatedDate().Replace("-", string.Empty),
                                             criteria.ToDate.ToShortFormatedDate().Replace("-", string.Empty));
                string reportTitle = criteria.ToString();
                var writer = new ExcelTemplateWriter(templatePath, fileName, ReportsLang.TopWinners);
                int counter = 0;

                // Copy title.
                writer.CopyRows(0, 2);
                writer.WriteRow(2, new { reportTitle });
                writer.CopyRows(3, 2);

                // Prepare content
                var records = topWinners.ChildItems.Select(obj => new
                {
                    No = ++counter,
                    UserName = obj.UserName,
                    Currency = obj.Currency,
                    Stake = obj.Stake,
                    WinLoss = obj.WinLoss,
                    CompanyWinLoss = obj.CompanyWinLoss,
                    BasedStake = obj.StakeBaseRM,
                    BasedWinLoss = obj.WinLossBaseRM,
                    BasedCompanyWinLoss = obj.CompanyWinLossBaseRM,
                    TotalBets = obj.BetCount,
                    AvgOdds = obj.AvgOdds
                });

                // Total row
                var total = new object[]
                {
                    Lang.Total,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    topWinners.StakeBaseRM,
                    topWinners.WinLossBaseRM,
                    topWinners.CompanyWinLossBaseRM,
                    topWinners.BetCount,
                    "-"
                };

                writer.WriteRows(5, records);
                writer.WriteRow(6, total);
                writer.SyncColumnsWidth(11);
                writer.Render();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}