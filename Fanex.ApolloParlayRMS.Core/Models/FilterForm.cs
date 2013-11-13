// --------------------------------------------------------------------------
// <copyright file="FilterForm.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.14">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Core.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.ApolloParlayRMS.Core.Constants;
    using Fanex.ApolloParlayRMS.Core.Resources;

    /// <summary>
    /// Contains common fields of filter form.
    /// </summary>
    public static class FilterForm
    {
        private static List<SelectListItem> _sportTypes;
        private static List<SelectListItem> _betTypes;
        private static List<SelectListItem> _selectTops;
        private static List<SelectListItem> _dateRanges;
        private static List<SelectListItem> _combos;
        private static List<SelectListItem> _levels;
        private static List<SelectListItem> _refreshTimes;

        public static List<SelectListItem> Combos
        {
            get
            {
                if (_combos == null)
                {
                    _combos = new List<SelectListItem>();
                    for (int i = 2; i <= 5; i++)
                    {
                        _combos.Add(new SelectListItem
                        {
                            Text = i.ToString(),
                            Value = i.ToString()
                        });
                    }

                    _combos.Add(new SelectListItem { Text = Lang.SixUp, Value = "6" });
                }

                return _combos;
            }
        }

        public static List<SelectListItem> DateRanges
        {
            get
            {
                if (_dateRanges == null)
                {
                    _dateRanges = new List<SelectListItem>()
                    {
                        new SelectListItem { Text = Lang.Yesterday, Value = ((int)CommonEnums.DateRange.Yesterday).ToString() },
                        new SelectListItem { Text = Lang.CurrentWeek, Value = ((int)CommonEnums.DateRange.CurrentWeek).ToString() },
                        new SelectListItem { Text = Lang.CurrentMonth, Value = ((int)CommonEnums.DateRange.CurrentMonth).ToString() },
                        new SelectListItem { Text = Lang.LastWeek, Value = ((int)CommonEnums.DateRange.LastWeek).ToString() },
                        new SelectListItem { Text = Lang.LastMonth, Value = ((int)CommonEnums.DateRange.LastMonth).ToString() },
                        new SelectListItem { Text = Lang.SinceLastMonth, Value = ((int)CommonEnums.DateRange.SinceLastMonth).ToString() },
                        new SelectListItem { Text = Lang.SpecificDate, Value = ((int)CommonEnums.DateRange.SpecificDate).ToString() }
                    };
                }

                return _dateRanges;
            }
        }

        public static List<SelectListItem> SelectTops
        {
            get
            {
                if (_selectTops == null)
                {
                    var values = new List<int> { 10, 20, 50, 100 };
                    _selectTops = new List<SelectListItem>();
                    foreach (var item in values)
                    {
                        _selectTops.Add(new SelectListItem
                        {
                            Text = item.ToString(),
                            Value = item.ToString(),
                            Selected = item == 100 ? true : false
                        });
                    }
                }

                return _selectTops;
            }
        }

        public static List<SelectListItem> SportTypes
        {
            get
            {
                if (_sportTypes == null)
                {
                    _sportTypes = new List<SelectListItem>()
                    {
                        new SelectListItem { Text = Lang.AllSports, Value = "null", Selected = true },
                        new SelectListItem { Text = Lang.SoccerOnly, Value = ((byte)CommonEnums.SportsType.SoccerOnly).ToString() },
                        new SelectListItem { Text = Lang.CombinedSports, Value = ((byte)CommonEnums.SportsType.CombinedSports).ToString() },
                        new SelectListItem { Text = Lang.OtherSports, Value = ((byte)CommonEnums.SportsType.OtherSports).ToString() }
                    };
                }

                return _sportTypes;
            }
        }

        public static List<SelectListItem> BetTypes
        {
            get
            {
                if (_betTypes == null)
                {
                    _betTypes = new List<SelectListItem>()
                    {
                        new SelectListItem { Text = Lang.MixParlay, Value = ((int)CommonEnums.BetType.MixParlay).ToString(), Selected = true }
                    };
                }

                return _betTypes;
            }
        }

        public static List<SelectListItem> LevelList
        {
            get
            {
                if (_levels == null)
                {
                    _levels = new List<SelectListItem>();
                    for (int i = 10; i > 0; i--)
                    {
                        _levels.Add(new SelectListItem
                        {
                            Text = string.Format("{0} {1}", Lang.Level, i),
                            Value = i.ToString()
                        });
                    }
                }

                return _levels;
            }
        }

        public static dynamic Levels
        {
            get
            {
                var allLevel = new SelectListItem
                {
                    Text = Lang.AllLevels,
                    Value = "null"
                };
                var none = new SelectListItem
                {
                    Text = Lang.None,
                    Value = "0"
                };

                return new
                {
                    Levels = LevelList,
                    AllLevel = allLevel,
                    None = none
                };
            }
        }

        public static List<SelectListItem> RefreshTimes
        {
            get
            {
                if (_refreshTimes == null)
                {
                    var values = new List<int> { 0, 30, 60, 120 };
                    _refreshTimes = new List<SelectListItem>();
                    foreach (var value in values)
                    {
                        _refreshTimes.Add(new SelectListItem { Text = value.ToString(), Value = (value * 1000).ToString(), Selected = value == 60 });
                    }
                }

                return _refreshTimes;
            }
        }

        /// <summary>
        /// Gets sport type name from resource by ID.
        /// </summary>
        /// <param name="sportType">Sport type ID.</param>
        /// <returns>Sport type name.</returns>
        public static string GetSportType(byte? sportType)
        {
            switch (sportType)
            {
                case null:
                    return Lang.AllSports;

                case (byte)CommonEnums.SportsType.SoccerOnly:
                    return Lang.SoccerOnly;

                case (byte)CommonEnums.SportsType.CombinedSports:
                    return Lang.OtherSports;

                case (byte)CommonEnums.SportsType.OtherSports:
                    return Lang.CombinedSports;
            }

            return Lang.Unknow;
        }

        /// <summary>
        /// Gets bet type name from resource by ID.
        /// </summary>
        /// <param name="betType">Bet type ID.</param>
        /// <returns>Bet type name.</returns>
        public static string GetBetType(int betType)
        {
            switch (betType)
            {
                case (int)CommonEnums.BetType.MixParlay:
                    return Lang.MixParlay;

                case (int)CommonEnums.BetType.SystemParlay:
                    return Lang.SystemParlay;
            }

            return Lang.Unknow;
        }
    }
}