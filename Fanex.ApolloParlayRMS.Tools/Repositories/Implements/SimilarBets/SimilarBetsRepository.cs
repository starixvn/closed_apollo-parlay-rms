// --------------------------------------------------------------------------
// <copyright file="SimilarBetsRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.22">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Repositories.Implements.SimilarBets
{
    using System;
    using System.Data;
    using Fanex.ApolloParlayRMS.Tools.Models.SimilarBets;
    using Fanex.ApolloParlayRMS.Tools.Repositories.Interfaces.SimilarBets;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Similar bet repository.
    /// </summary>
    public class SimilarBetsRepository : ISimilarBetsRepository
    {
        /// <summary>
        /// Gets the similar bet list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>
        /// SimilarBetList object contains the found information about the bet of passed reference number
        /// and list of its similar bets.
        /// </returns>
        public SimilarBetList GetSimilarBetList(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();

                using (var dataset = db.ExecuteDataSet(param))
                {
                    if (dataset.Tables.Count == 2)
                    {
                        DataTable list = dataset.Tables[0];
                        if (list.Rows.Count > 0)
                        {
                            DataRow info = list.Rows[0];
                            DataTable details = dataset.Tables[1];

                            return new SimilarBetList
                            {
                                TransDate = Convert.ToDateTime(info["transdate"]),
                                TransId = Convert.ToInt64(info["transid"]),
                                RefNo = Convert.ToInt64(info["refno"]),
                                Username = Convert.ToString(info["username"]),
                                Odds = Convert.ToDecimal(info["odds"]),
                                Stake = Convert.ToDecimal(info["stake"]),
                                MaxPayout = Convert.ToDecimal(info["max_payout"]),
                                Combo = Convert.ToInt32(info["combo"]),
                                Level = Convert.ToInt32(info["level"]),
                                NumOfBets = Convert.ToInt32(info["cnt_bets"]),
                                Details = details.AsEnumerable().Select(row => new SimilarBetItem
                                {
                                    TransId = Convert.ToInt64(row["transid"]),
                                    RefNo = Convert.ToInt64(row["refno"]),
                                    Username = Convert.ToString(row["username"]),
                                    Level = Convert.ToInt32(row["level"]),
                                    MaxPayout = Convert.ToDecimal(row["max_payout"]),
                                    SimilarCombo = Convert.ToInt32(row["similar_combo"])
                                })
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
