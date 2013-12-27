// --------------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.09">Created</change>
// </history>

namespace Apollo.ParlayRMS.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Apollo.ParlayRMS.Core.Constants;
    using Apollo.ParlayRMS.Core.Resources;

    /// <summary>
    /// It contains extension to render common html control.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        private static readonly string NullStr = "null";
        private static readonly string YearPickerClass = "yearpicker";
        private static readonly string DatePickerClass = "datepicker";

        #region Date Picker

        /// <summary>
        ///  This function add class date picker into textbox,
        ///  client script explore all textbox which have class date picker and use jQuery UI
        ///  lib to render date picker control.
        /// </summary>
        /// <param name="htmlHelper">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="name">Name of HTML control.</param>
        /// <param name="date">Initial value.</param>
        /// <param name="minDate">Minimum value of date range.</param>
        /// <param name="maxDate">Maximum value of date range.</param>
        /// <param name="formatDate">Date format.</param>
        /// <param name="unavailDates">List of unavailable dates.</param>
        /// <returns>HTML String that demonstrates the date time picker control.</returns>
        public static MvcHtmlString DatePicker(
                                        this HtmlHelper htmlHelper,
                                        string name,
                                        DateTime? date,
                                        DateTime? minDate = null,
                                        DateTime? maxDate = null,
                                        string formatDate = CommonConstants.ShortDateFormat,
                                        IEnumerable<DateTime> unavailDates = null)
        {
            if (string.IsNullOrWhiteSpace(formatDate))
            {
                throw new ArgumentNullException(Messages.FormatDateError);
            }

            string disabledDate = (unavailDates != null && unavailDates.Count() > 0)
                                  ? string.Join(",", unavailDates.Select(d => d.ToString(CommonConstants.ShortDateFormat)))
                                  : string.Empty;
            string minDateString = (minDate != null && minDate != DateTime.MinValue) ? ((DateTime)minDate).ToYearMonthDateFormatWithSlash() : NullStr;
            string maxDateString = (maxDate != null && maxDate != DateTime.MinValue) ? ((DateTime)maxDate).ToYearMonthDateFormatWithSlash() : NullStr;

            var textbox = htmlHelper.TextBox(
                                        name,
                                        date.HasValue ? date.Value.ToString(formatDate) : string.Empty,
                                        new
                                        {
                                            @class = DatePickerClass,
                                            mindate = minDateString,
                                            maxdate = maxDateString,
                                            disabledDate = disabledDate
                                        });

            return MvcHtmlString.Create(textbox.ToString());
        }

        /// <summary>
        /// <see cref="DatePicker"/>
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="html">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="expression">Metadata expression.</param>
        /// <param name="minDate">Minimum value of date range.</param>
        /// <param name="maxDate">Maximum value of date range.</param>
        /// <param name="formatDate">Date Format.</param>
        /// <param name="unavailDates">List of unavailable dates.</param>
        /// <returns>HTML String that demonstrates the date time picker control.</returns>
        public static MvcHtmlString DatePickerFor<TModel, TValue>(
                                        this HtmlHelper<TModel> html,
                                        Expression<Func<TModel, TValue>> expression,
                                        DateTime? minDate = null,
                                        DateTime? maxDate = null,
                                        string formatDate = CommonConstants.ShortDateFormat,
                                        IEnumerable<DateTime> unavailDates = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            DateTime? value = DateTime.Now;

            if (metaData.Model is DateTime? || metaData.Model is DateTime)
            {
                value = (DateTime?)metaData.Model;
            }
            else
            {
                value = null;
            }

            return html.DatePicker(metaData.PropertyName, value, minDate, maxDate, formatDate, unavailDates);
        }

        #endregion Date Picker

        #region Year Picker

        /// <summary>
        ///  Generates year selection item.
        ///  <para>It adds class year picker to textbox.</para>
        /// </summary>
        /// <param name="htmlHelper">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="name">Name of HTML control.</param>
        /// <param name="fromYear">Minimum year that the year picker supports.</param>
        /// <param name="currentYear">Default is 2000.</param>
        /// <returns>HTML String that demonstrates the year picker control.</returns>
        public static MvcHtmlString YearPicker(this HtmlHelper htmlHelper, string name, int fromYear = 2000, int currentYear = 2000)
        {
            var list = new List<SelectListItem>();
            var endYear = DateTime.Now.Year;

            for (var i = endYear; i >= fromYear; i--)
            {
                list.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = i == currentYear
                });
            }

            var dropDownList = htmlHelper.DropDownList(name, list, new { @class = YearPickerClass });
            return MvcHtmlString.Create(dropDownList.ToString());
        }

        /// <summary>
        /// Generates year selection items.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="html">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="expression">Metadata expression.</param>
        /// <param name="fromYear">Default from year 2000.</param>
        /// <returns>HTML String that demonstrates the year picker control.</returns>
        public static MvcHtmlString YearPickerFor<TModel, TValue>(
                                        this HtmlHelper<TModel> html,
                                        Expression<Func<TModel, TValue>> expression,
                                        int fromYear = 2000)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var currentYear = (int)metaData.Model;

            return html.YearPicker(metaData.PropertyName, fromYear, currentYear);
        }

        #endregion Year Picker

        #region CheckBoxList

        /// <summary>
        /// Generates check-box list control.
        /// </summary>
        /// <param name="htmlHelper">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="name">Name of HTML control.</param>
        /// <returns>HTML String that demonstrates the check-box list control.</returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name)
        {
            return CheckBoxList(htmlHelper, name, null);
        }

        /// <summary>
        /// Generates check-box list control.
        /// </summary>
        /// <param name="htmlHelper">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="name">Name of HTML control.</param>
        /// <param name="selectList">List of SelectListItem objects.</param>
        /// <returns>HTML String that demonstrates the check-box list control.</returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxList(htmlHelper, name, selectList, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Generates check-box list control.
        /// </summary>
        /// <param name="htmlHelper">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="name">Name of HTML control.</param>
        /// <param name="selectList">List of SelectListItem objects.</param>
        /// <param name="htmlAttributes">List of HTML attributes.</param>
        /// <returns>HTML String that demonstrates the check-box list control.</returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return CheckBoxList(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Generates check-box list control.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TProperty">Value type.</typeparam>
        /// <param name="htmlHelper">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="expression">Metadata expression.</param>
        /// <returns>HTML String that demonstrates the check-box list control.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return CheckBoxListFor(htmlHelper, expression, null);
        }

        /// <summary>
        /// Generates check-box list control.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TProperty">Value type.</typeparam>
        /// <param name="htmlHelper">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="expression">Metadata expression.</param>
        /// <param name="selectList">List of SelectListItem objects.</param>
        /// <returns>HTML String that demonstrates the check-box list control.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
                                        this HtmlHelper<TModel> htmlHelper,
                                        Expression<Func<TModel, TProperty>> expression,
                                        IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Generates check-box list control.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TProperty">Value type.</typeparam>
        /// <param name="htmlHelper">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="expression">Metadata expression.</param>
        /// <param name="selectList">List of SelectListItem objects.</param>
        /// <param name="htmlAttributes">List of HTML attributes.</param>
        /// <returns>HTML String that demonstrates the check-box list control.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
                                        this HtmlHelper<TModel> htmlHelper,
                                        Expression<Func<TModel, TProperty>> expression,
                                        IEnumerable<SelectListItem> selectList,
                                        object htmlAttributes)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Generates check-box list control.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TProperty">Value type.</typeparam>
        /// <param name="htmlHelper">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="expression">Metadata expression.</param>
        /// <param name="selectList">List of SelectListItem objects.</param>
        /// <param name="htmlAttributes">List of HTML attributes.</param>
        /// <returns>HTML String that demonstrates the check-box list control.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
                                        this HtmlHelper<TModel> htmlHelper,
                                        Expression<Func<TModel, TProperty>> expression,
                                        IEnumerable<SelectListItem> selectList,
                                        IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(Messages.CheckboxExpressionError);
            }

            return CheckBoxList(htmlHelper, ExpressionHelper.GetExpressionText(expression), selectList, htmlAttributes);
        }

        #endregion CheckBoxList

        #region Dropdownlist

        /// <summary>
        /// Generates drop-down list control.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TProperty">Value type.</typeparam>
        /// <param name="htmlHelper">HTML Helper object that the method needs to be extended to.</param>
        /// <param name="expression">Metadata expression.</param>
        /// <param name="selectList">List of SelectListItem objects.</param>
        /// <param name="firstItem">Default item.</param>
        /// <param name="htmlAttributes">List of HTML attributes.</param>
        /// <returns>HTML String that demonstrates the drop-down list control.</returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(
                                        this HtmlHelper<TModel> htmlHelper,
                                        Expression<Func<TModel, TProperty>> expression,
                                        IEnumerable<SelectListItem> selectList,
                                        SelectListItem firstItem,
                                        object htmlAttributes)
        {
            if (firstItem != null)
            {
                if (firstItem.Value != null)
                {
                    List<SelectListItem> list = new List<SelectListItem>();
                    list.Add(firstItem);

                    if (selectList != null)
                    {
                        foreach (SelectListItem item in selectList)
                        {
                            list.Add(item);
                        }
                    }

                    selectList = list;
                }
            }

            return SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, htmlAttributes);
        }

        #endregion Dropdownlist

        #region Radio button list

        /// <summary>
        /// Creates the radio button list.
        /// </summary>
        /// <param name="helper">The HtmlHelper instance which the method will extend to.</param>
        /// <param name="name">The group name of the radio button list.</param>
        /// <param name="selectList">The list of items which will be used to initialize the radio button list.</param>
        /// <param name="wrapper">Wrapper element.</param>
        /// <returns>MVCHtmlString object represents the created control.</returns>
        public static MvcHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, string wrapper)
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in selectList)
            {
                var id = name + item.Value;
                bool isChecked = item.Selected == true;
                stringBuilder.Append(string.Format(
                                                wrapper,
                                                helper.RadioButton(name, item.Value, isChecked, new { id = id }),
                                                helper.Label(id, item.Text)));
            }

            return new MvcHtmlString(stringBuilder.ToString());
        }

        #endregion Radio button list
    }
}