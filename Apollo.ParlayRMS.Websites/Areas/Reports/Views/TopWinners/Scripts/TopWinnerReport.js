/* global moment, Namespace, Apollo, ParlayRMS, Validator, formatItem, formatSelection, levelUpdater, levels */
/* global dateRanges, labels, levels, messages, hasEditRight, minDate, maxDate */

$(document).ready(function () {
    'use strict';
    var topWinnersReport = new Apollo.ParlayRMS.Reports.TopWinnersReport();

    levelUpdater.Init(levels, messages);
    topWinnersReport.InitPage(labels);
});

Namespace.Register('Apollo.ParlayRMS.Reports.TopWinnersReport');
Apollo.ParlayRMS.Reports.TopWinnersReport = function () {
    'use strict';
    var $this = this;
    var labels = {};
    var urls = {
        GetTopWinners: ParlayRMS.resolveRootUrl('Reports/TopWinners/GetTopWinners'),
        GetTopWinnerDetails: ParlayRMS.resolveRootUrl('Reports/TopWinners/GetTopWinnerDetails'),
        ExportTopWinners: ParlayRMS.resolveRootUrl('Reports/TopWinners/ExportTopWinners')
    };
    var params = {};
    var cacheDOM = function () {
        $this.DOMInstance.ddlSelectTop = $('#ddlSelectTop');
        $this.DOMInstance.ddlCombo = $('#ddlCombo');
        $this.DOMInstance.ddlSportTypes = $('#ddlSportTypes');
        $this.DOMInstance.ddlDateRanges = $('#ddlDateRanges');
        $this.DOMInstance.dpkDateFrom = $('#dpkDateFrom');
        $this.DOMInstance.dpkDateTo = $('#dpkDateTo');
        $this.DOMInstance.btnSubmit = $('#btnSubmit');
        $this.DOMInstance.divFilterForm = $('#filter-bar');
        $this.DOMInstance.divResult = $('#divResult');
        $this.DOMInstance.divTopWinnerDetails = $('#divTopWinnerDetails');
        $this.DOMInstance.divTopWinnerDetailsContainer = $('#divTopWinnerDetailsContainer');
        $this.DOMInstance.icoCalendarUrl = $('#imagePath').val();
    };
    var initSingleDropDownList = function (obj, height) {
        obj.multiselect({
            multiple: false,
            header: false,
            noneSelectedText: 'Select an Option',
            selectedList: 1,
            minWidth: 132,
            height: height
        });
    };
    var initMultiDropDownList = function (obj) {
        obj.multiselect({
            multiple: true,
            minWidth: 132,
            height: 106,
            create: function () {
                obj.multiselect('checkAll');
            },
            selectedText: function (numChecked, numTotal, checkedItems) {
                if (numChecked === 0) {
                    return 'Select options';
                }
                else if (numChecked === 1) {
                    return $(checkedItems).next().text();
                }
                else if (numChecked === numTotal) {
                    return 'All';
                }
                else {
                    return numChecked + ' options';
                }
            }
        });
    };
    var initLevelUpdater = function (obj) {
        obj.multiselect({
            multiple: false,
            header: false,
            noneSelectedText: 'Select an Option',
            selectedList: 1,
            minWidth: 132,
            height: 180,
            format: formatItem,
            onSelectFormat: formatSelection
        });
    };
    var initFields = function () {
        var minDateObj = new Date(minDate);
        var maxDateObj = new Date(maxDate);
        //var currentDate = $.datepicker.formatDate(ParlayRMS.commonConstants.DateFormat, maxDateObj);

        initSingleDropDownList($this.DOMInstance.ddlSelectTop, 86);
        initSingleDropDownList($this.DOMInstance.ddlSportTypes, 86);
        initMultiDropDownList($this.DOMInstance.ddlCombo);
        initSingleDropDownList($this.DOMInstance.ddlDateRanges, 148);

        $this.DOMInstance.dpkDateFrom.datepicker({
            defaultDate: '+1w',
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: ParlayRMS.commonConstants.DateFormat,
            minDate: minDateObj,
            maxDate: maxDateObj,
            showOn: 'button',
            buttonImage: $this.DOMInstance.icoCalendarUrl,
            buttonImageOnly: true,
            buttonText: labels.From,
            onSelect: function (dateText, inst) {
                if (dateText !== inst.lastVal) {
                    $('option[value="7"]', $this.DOMInstance.ddlDateRanges).attr('selected', 1);
                    $this.DOMInstance.ddlDateRanges.multiselect('refresh');
                }
            },
            onClose: function (selectedDate) {
                $this.DOMInstance.dpkDateTo.datepicker('option', 'minDate', selectedDate);
                if ($this.DOMInstance.dpkDateTo.val() === '') {
                    $this.DOMInstance.dpkDateTo.val($(this).val());
                }
            }
        });

        $this.DOMInstance.dpkDateTo.datepicker({
            defaultDate: '+1w',
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: ParlayRMS.commonConstants.DateFormat,
            minDate: minDateObj,
            maxDate: maxDateObj,
            showOn: 'button',
            buttonImage: $this.DOMInstance.icoCalendarUrl,
            buttonImageOnly: true,
            buttonText: labels.From,
            onSelect: function (dateText, inst) {
                if (dateText !== inst.lastVal) {
                    $('option[value="7"]', $this.DOMInstance.ddlDateRanges).attr('selected', 1);
                    $this.DOMInstance.ddlDateRanges.multiselect('refresh');
                }
            },
            onClose: function (selectedDate) {
                $this.DOMInstance.dpkDateFrom.datepicker('option', 'maxDate', selectedDate);
                if ($this.DOMInstance.dpkDateFrom.val() === '') {
                    $this.DOMInstance.dpkDateFrom.val($(this).val());
                }
            }
        });

        $this.DOMInstance.ddlDateRanges.live('change', changeDateRange);
        $this.DOMInstance.ddlDateRanges.change();

        $this.DOMInstance.btnSubmit.bind('click', function () {
            Validator.clearErrorMessages($this.DOMInstance.divFilterForm);
            if (Validator.validateMultiselectRequired($this.DOMInstance.ddlCombo) &&
                Validator.validateDateRange($this.DOMInstance.dpkDateFrom, $this.DOMInstance.dpkDateTo)) {
                var selectTop = $this.DOMInstance.ddlSelectTop.val();
                var sportTypes = $this.DOMInstance.ddlSportTypes.val();
                var dateFrom = $this.DOMInstance.dpkDateFrom.val();
                var dateTo = $this.DOMInstance.dpkDateTo.val();
                var combos = $this.DOMInstance.ddlCombo.multiselect('getChecked').map(function () {
                    return this.value;
                }).get().join(',');

                $this.GetData(selectTop, sportTypes, combos, dateFrom, dateTo);
            }
        });
    };
    var handleGridEvents = function () {
        //var isEnable = hasEditRight ? 'enable' : 'disable';
        $('select[data-type="levelUpdater"]').each(function () {
            initLevelUpdater($(this));
            $(this).multiselect({
                minWidth: 190
            }).multiselect(hasEditRight);
        });
        $('div.grid-link-cell').each(function () {
            var custId = $(this).attr('data-cust-id');
            var username = $(this).attr('data-username');
            $(this).on('click', function () {
                $this.GetTopWinnerDetails(custId, username);
            });
        });
    };
    var changeDateRange = function () {
        var fromDate = null;
        var toDate = null;
        var limitDate = new Date(minDate);
        var selValue = parseInt($('option:selected', $this.DOMInstance.ddlDateRanges).val());
        var nowDate = null;

        switch (selValue) {
            case dateRanges.Yesterday:
                var yesDate = moment(maxDate).subtract('days', 1);

                nowDate = moment(maxDate);
                fromDate = (yesDate < moment(limitDate) ? nowDate : yesDate);
                toDate = fromDate;
                break;

            case dateRanges.CurrentWeek:
                var currWeek = moment(maxDate).day(0);

                fromDate = (currWeek < limitDate ? moment(maxDate) : currWeek);
                toDate = moment(maxDate);
                break;

            case dateRanges.CurrentMonth:
                var currMonth = moment(maxDate).startOf('month');

                nowDate = moment(maxDate);
                fromDate = (currMonth < moment(limitDate) ? nowDate : currMonth);
                toDate = nowDate;
                break;

            case dateRanges.LastWeek:
                var lstWeek = moment(maxDate).subtract('weeks', 1).day(1);

                nowDate = moment(maxDate);
                fromDate = (lstWeek < limitDate ? moment(limitDate) : lstWeek);

                var lstWeekToDate = moment(fromDate).add('days', 6);
                toDate = (lstWeekToDate < limitDate ? nowDate : lstWeekToDate);
                break;

            case dateRanges.LastMonth:
                var lstMonth = moment(maxDate).subtract('months', 1).startOf('month');

                nowDate = moment(maxDate);
                fromDate = (lstMonth < limitDate ? moment(limitDate) : lstMonth);

                var lstMonthToDate = moment(maxDate).startOf('month').subtract('days', 1);
                toDate = (lstMonthToDate < limitDate ? moment(limitDate) : lstMonthToDate);
                break;

            case dateRanges.SinceLastMonth:
                var sinLastMonth = moment(maxDate).subtract('months', 1).startOf('month');

                nowDate = moment(maxDate);
                fromDate = (sinLastMonth < limitDate ? moment(limitDate) : sinLastMonth);
                toDate = nowDate;
                break;
            case dateRanges.SpecificDate:
                fromDate = moment($this.DOMInstance.dpkDateFrom.val(), ParlayRMS.commonConstants.MomentDateFormat);
                toDate = moment($this.DOMInstance.dpkDateTo.val(), ParlayRMS.commonConstants.MomentDateFormat);
                break;
        }

        $this.DOMInstance.dpkDateFrom.val(fromDate.format(ParlayRMS.commonConstants.MomentDateFormat));
        $this.DOMInstance.dpkDateTo.val(toDate.format(ParlayRMS.commonConstants.MomentDateFormat));
    };
    var renderReportData = function (result) {
        var bodyData = [];
        var footerData = [];

        $this.DOMInstance.divResult.addClass('display-none');
        $this.DOMInstance.divResult.html('');

        if (result && result.Data) {
            var records = result.Data.TopWinners;
            var row = null;
            var totalBetCol = '<div class="grid-link-cell link" data-cust-id="{0}" data-username="{1}">{2}</div>';

            $.each(records, function (i, info) {
                row =
                {
                    Columns: [{ Name: 'ColNo', Value: i + 1 },
                              { Name: 'ColUsername', Value: levelUpdater.CreateUpdateField(info.CustId, info.Level, hasEditRight, info.Username) },
                              { Name: 'ColCurrency', Value: info.Currency },
                              { Name: 'ColOriStake', Value: info.Stake },
                              { Name: 'ColOriWinningAmount', Value: info.WinLoss },
                              { Name: 'ColOriCompanyLoseAmount', Value: info.CompanyWinLoss },
                              { Name: 'ColBaseStake', Value: info.StakeBaseRM },
                              { Name: 'ColBaseWinningAmount', Value: info.WinLossBaseRM },
                              { Name: 'ColBaseCompanyLoseAmount', Value: info.CompanyWinLossBaseRM },
                              { Name: 'ColTotalBets', Value: totalBetCol.format(info.CustId, info.Username, info.BetCount) },
                              { Name: 'ColAverageWinOdds', Value: info.AvgOdds}]
                };
                bodyData.push(row);
            });

            row =
            {
                Columns: [{ Name: 'ColNo', Value: labels.Total, MergeColNumber: 6, CellAlign: 'right' },
                          { Name: 'ColBaseStake', Value: result.Data.Total.StakeBaseRM },
                          { Name: 'ColBaseWinningAmount', Value: result.Data.Total.WinLossBaseRM },
                          { Name: 'ColBaseCompanyLoseAmount', Value: result.Data.Total.CompanyWinLossBaseRM },
                          { Name: 'ColTotalBets', Value: result.Data.Total.BetCount },
                          { Name: 'ColAverageWinOdds', Value: '-', CellAlign: 'center'}]
            };
            footerData.push(row);
        }

        var gridOptions =
        {
            customizeHeader: true,
            groupHeaderRows: [{
                Columns: [{ HeaderText: labels.No, Name: 'ColNo', MergeRowNumber: 2 },
                          { HeaderText: labels.Username, Name: 'ColUsername', MergeRowNumber: 2 },
                          { HeaderText: labels.Currency, Name: 'ColCurrency', MergeRowNumber: 2 },
                          { HeaderText: labels.OriginalCurrency, Name: 'ColOriStake', MergeColNumber: 3 },
                          { HeaderText: labels.BaseCurrency, Name: 'ColBaseStake', MergeColNumber: 3 },
                          { HeaderText: labels.TotalBets, Name: 'ColTotalBets', MergeRowNumber: 2 },
                          { HeaderText: labels.AverageWinOdds, Name: 'ColAverageWinOdds', MergeRowNumber: 2}]
            }],
            columns: [{ HeaderText: labels.No, Width: 45, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Username, Width: 190, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Currency, Width: 60, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Stake, Width: 120, Name: 'ColOriStake', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.NumberFormat
                      },
                      { HeaderText: labels.WinningAmount, Width: 120, Name: 'ColOriWinningAmount', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      },
                      { HeaderText: labels.CompanyLoseAmount, Width: 140, Name: 'ColOriCompanyLoseAmount', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      },
                      { HeaderText: labels.Stake, Width: 120, Name: 'ColBaseStake', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      },
                      { HeaderText: labels.WinningAmount, Width: 120, Name: 'ColBaseWinningAmount', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      },
                      { HeaderText: labels.CompanyLoseAmount, Width: 140, Name: 'ColBaseCompanyLoseAmount', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      },
                      { HeaderText: labels.TotalBets, Width: 65, Name: 'ColTotalBets', HeaderAlign: 'center', CellAlign: 'center',
                          FormatNumber: ParlayRMS.commonConstants.NumberFormat
                      },
                      { HeaderText: labels.AverageWinOdds, Width: 62, Name: 'ColAverageWinOdds', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      }],
            showTitle: true,
            showFooter: true,
            gridExpandHeight: 350,
            titleInfo: {
                text: result.Title,
                showExcelButton: true,
                exportExcelFunction: $this.ExportReport
            },
            showNumberColumn: true,
            bodyRows: bodyData,
            footerRows: footerData
        };

        $this.DOMInstance.divResult.removeClass('display-none').fanexGrid(gridOptions);
        handleGridEvents();
    };
    var renderDetailData = function (data, username) {
        var bodyData = [];
        var footerData = [];

        if (data && data.TopWinnerDetailItems.length > 0) {
            var records = data.TopWinnerDetailItems;
            var row = null;
            var betCol = '<div class="grid-link-cell link" data-username="{0}" data-trans-id="{1}">{2}</div>';

            $.each(records, function (i, info) {
                row =
                {
                    Columns: [{ Name: 'ColNo', Value: i + 1 },
                              { Name: 'ColBetId', Value: betCol.format(username, info.RefNo, info.TransId) },
                              { Name: 'ColOriStake', Value: info.Stake },
                              { Name: 'ColOriCustomerWinLoss', Value: info.WinLoss },
                              { Name: 'ColOriCompanyWinLoss', Value: info.CompanyWinLoss },
                              { Name: 'ColBaseStake', Value: info.StakeBaseRM },
                              { Name: 'ColBaseCustomerWinLoss', Value: info.WinLossBaseRM },
                              { Name: 'ColBaseCompanyWinLoss', Value: info.CompanyWinLossBaseRM },
                              { Name: 'ColTotalCombo', Value: info.Combo },
                              { Name: 'ColOdds', Value: (info.Odds === 0) ? '-' : info.Odds}]
                };
                bodyData.push(row);
            });

            row =
            {
                Columns: [{ Name: 'ColNo', Value: labels.Total, MergeColNumber: 2, CellAlign: 'right' },
                          { Name: 'ColOriStake', Value: data.Total.Stake },
                          { Name: 'ColOriCustomerWinLoss', Value: data.Total.WinLoss },
                          { Name: 'ColOriCompanyWinLoss', Value: data.Total.CompanyWinLoss },
                          { Name: 'ColBaseStake', Value: data.Total.StakeBaseRM },
                          { Name: 'ColBaseCustomerWinLoss', Value: data.Total.WinLossBaseRM },
                          { Name: 'ColBaseCompanyWinLoss', Value: data.Total.CompanyWinLossBaseRM },
                          { Name: 'ColTotalCombo', Value: '-', CellAlign: 'center' },
                          { Name: 'ColOdds', Value: '-', CellAlign: 'right'}]
            };
            footerData.push(row);
        }

        var gridOptions =
        {
            customizeHeader: true,
            groupHeaderRows: [{
                Columns: [{ HeaderText: labels.No, Name: 'ColNo', MergeRowNumber: 2 },
                          { HeaderText: labels.BetId, Name: 'ColBetId', MergeRowNumber: 2 },
                          { HeaderText: labels.OriginalCurrency, Name: 'ColOriStake', MergeColNumber: 3 },
                          { HeaderText: labels.BaseCurrency, Name: 'ColBaseStake', MergeColNumber: 3 },
                          { HeaderText: labels.TotalCombo, Name: 'ColTotalCombo', MergeRowNumber: 2 },
                          { HeaderText: labels.Odds, Name: 'ColOdds', MergeRowNumber: 2}]
            }],
            columns: [{ HeaderText: labels.No, Width: 45, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.BetId, Width: 130, Name: 'ColBetId', HeaderAlign: 'center', CellAlign: 'right' },
                      { HeaderText: labels.Stake, Width: 120, Name: 'ColOriStake', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.NumberFormat
                      },
                      { HeaderText: labels.CustomerWinLoss, Width: 120, Name: 'ColOriCustomerWinLoss', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      },
                      { HeaderText: labels.CompanyWinLoss, Width: 120, Name: 'ColOriCompanyWinLoss', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      },
                      { HeaderText: labels.Stake, Width: 120, Name: 'ColBaseStake', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      },
                      { HeaderText: labels.CustomerWinLoss, Width: 120, Name: 'ColBaseCustomerWinLoss', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      },
                      { HeaderText: labels.CompanyWinLoss, Width: 120, Name: 'ColBaseCompanyWinLoss', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      },
                      { HeaderText: labels.TotalCombo, Width: 70, Name: 'ColTotalCombo', HeaderAlign: 'center', CellAlign: 'center',
                          FormatNumber: ParlayRMS.commonConstants.NumberFormat
                      },
                      { HeaderText: labels.Odds, Width: 80, Name: 'ColOdds', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                      }],
            showFooter: true,
            showNumberColumn: true,
            bodyRows: bodyData,
            footerRows: footerData,
            fullHeight: false,
            gridHeight: 600
        };

        $('#dialog-popup .freegrid').fanexGrid(gridOptions);

        $('#dialog-popup .freegrid .grid-link-cell').each(function () {
            var refNo = $(this).attr('data-trans-id');
            var username = $(this).attr('data-username');
            var betId = $(this).text();
            $(this).on('click', function () {
                ParlayRMS.openWindow(ParlayRMS.resolveRootUrl('Reports/TopWinners/BetDetails?RefNo={0}&BetId={1}&Username={2}'.format(refNo, betId, username)));
            });
        });
    };

    $this.DOMInstance = {
        ddlSelectTop: null,
        ddlCombo: null,
        ddlSportTypes: null,
        ddlDateRanges: null,
        dpkDateFrom: null,
        dpkDateTo: null,
        btnSubmit: null,
        divFilterForm: null,
        divResult: null,
        divTopWinnerDetails: null,
        divTopWinnerDetailsContainer: null,
        icoCalendarUrl: null
    };
    $this.SetLang = function (labelValues) {
        if (labelValues) {
            labels = labelValues;
        }
    };
    $this.InitPage = function (labels) {
        cacheDOM();
        initFields();

        $this.SetLang(labels);
    };
    $this.GetData = function (selectTop, sportTypes, combos, dateFrom, dateTo) {
        var data = {
            LimitedRow: selectTop,
            SportType: sportTypes,
            Selection: combos,
            FromDate: dateFrom,
            ToDate: dateTo,
            IsLoadNew: true
        };
        params = data;
        $.ajax({
            type: 'POST',
            url: urls.GetTopWinners,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data),
            beforeSend: function () {
                ParlayRMS.loadingBox.show();
            },
            success: function (records) {
                renderReportData(records);
            },
            error: function (xhr, status, err) {
                ParlayRMS.handleAjaxError(xhr, status, err);
            },
            complete: function () {
                ParlayRMS.loadingBox.close();
            }
        });
    };
    $this.ExportReport = function () {
        params.IsLoadNew = false;
        ParlayRMS.downloadFile(urls.ExportTopWinners, params);
    };

    // For top winner's details popup
    $this.GetTopWinnerDetails = function (custId, username) {
        var data = params;
        data.CustId = custId;

        $.ajax({
            type: 'POST',
            url: urls.GetTopWinnerDetails,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data),
            beforeSend: function () {
                ParlayRMS.loadingBox.show();
            },
            success: function (records) {
                var title = labels.BetListOf.format(username);
                ParlayRMS.popup(title, $this.DOMInstance.divTopWinnerDetailsContainer.html());
                // Hardcode for the popup size.
                $('#dialog-popup').dialog('option', 'width', 1150);
                $('#dialog-popup').dialog('option', 'position', ['center', 160]);
                renderDetailData(records, username);
            },
            error: function (xhr, status, err) {
                ParlayRMS.handleAjaxError(xhr, status, err);
            },
            complete: function () {
                ParlayRMS.loadingBox.close();
            }
        });
    };
};