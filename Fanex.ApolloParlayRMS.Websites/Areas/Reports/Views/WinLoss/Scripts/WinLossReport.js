$(document).ready(function () {
    var winLossReport = new Fanex.ApolloParlayRMS.Reports.WinLossReport();

    winLossReport.InitPage(labels);
});

Namespace.Register("Fanex.ApolloParlayRMS.Reports.WinLossReport");
Fanex.ApolloParlayRMS.Reports.WinLossReport = function () {
    var $this = this;
    var labels = {};
    var urls = {
        GetWinLoss: ParlayRMS.resolveRootUrl('Reports/WinLoss/GetWinLoss'),
        ExportWinLoss: ParlayRMS.resolveRootUrl('Reports/WinLoss/ExportWinLoss')
    };
    var params = {};
    var cacheDOM = function () {
        $this.DOMInstance.divFilterForm = $('#filter-bar');
        $this.DOMInstance.ddlBetType = $('#ddlBetType');
        $this.DOMInstance.ddlSportTypes = $('#ddlSportTypes');
        $this.DOMInstance.ddlCombo = $('#ddlCombo');
        $this.DOMInstance.dpkDateFrom = $('#dpkDateFrom');
        $this.DOMInstance.dpkDateTo = $('#dpkDateTo');
        $this.DOMInstance.btnSearch = $('#btnSearch');
        $this.DOMInstance.divResult = $('#divResult');
        $this.DOMInstance.icoCalendarUrl = $('#imagePath').val();
    };
    var initSingleDropDownList = function (obj, height) {
        obj.multiselect({
            multiple: false,
            header: false,
            noneSelectedText: "Select an Option",
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
                obj.multiselect("checkAll");
            },
            selectedText: function (numChecked, numTotal, checkedItems) {
                if (numChecked == 0) {
                    return "Select options";
                }
                else if (numChecked == 1) {
                    return $(checkedItems).next().text();
                }
                else if (numChecked == numTotal) {
                    return "All";
                }
                else {
                    return numChecked + ' options';
                }
            }
        });
    };
    var initFields = function () {
        var minDateObj = new Date(minDate);
        var maxDateObj = new Date(maxDate);
        var currentDate = $.datepicker.formatDate(ParlayRMS.commonConstants.DateFormat, maxDateObj);

        initSingleDropDownList($this.DOMInstance.ddlBetType, 30);
        initMultiDropDownList($this.DOMInstance.ddlCombo);
        initSingleDropDownList($this.DOMInstance.ddlSportTypes, 85);

        $this.DOMInstance.dpkDateFrom.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            numberOfMonths: 1,
            dateFormat: ParlayRMS.commonConstants.DateFormat,
            minDate: minDateObj,
            maxDate: maxDateObj,
            showOn: "button",
            buttonImage: $this.DOMInstance.icoCalendarUrl,
            buttonImageOnly: true,
            buttonText: labels.From,
            onClose: function (selectedDate) {
                $this.DOMInstance.dpkDateTo.datepicker("option", "minDate", selectedDate);
                if ($this.DOMInstance.dpkDateTo.val() == '') {
                    $this.DOMInstance.dpkDateTo.val($(this).val());
                }
            }
        });

        $this.DOMInstance.dpkDateTo.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            numberOfMonths: 1,
            dateFormat: ParlayRMS.commonConstants.DateFormat,
            minDate: minDateObj,
            maxDate: maxDateObj,
            showOn: "button",
            buttonImage: $this.DOMInstance.icoCalendarUrl,
            buttonImageOnly: true,
            buttonText: labels.From,
            onClose: function (selectedDate) {
                $this.DOMInstance.dpkDateFrom.datepicker("option", "maxDate", selectedDate);
                if ($this.DOMInstance.dpkDateFrom.val() == '') {
                    $this.DOMInstance.dpkDateFrom.val($(this).val());
                }
            }
        });

        $this.DOMInstance.dpkDateFrom.val(currentDate);
        $this.DOMInstance.dpkDateTo.val(currentDate);

        $this.DOMInstance.btnSearch.live('click', function () {
            Validator.clearErrorMessages($this.DOMInstance.divFilterForm);
            if (Validator.validateMultiselectRequired($this.DOMInstance.ddlCombo)
                && Validator.validateDateRange($this.DOMInstance.dpkDateFrom, $this.DOMInstance.dpkDateTo)) {
                var betType = $this.DOMInstance.ddlBetType.val();
                var sportTypes = $('input[name=rbgSportTypes]:checked', '#sportTypes').val();
                var dateFrom = $this.DOMInstance.dpkDateFrom.val();
                var dateTo = $this.DOMInstance.dpkDateTo.val();
                var combos = $this.DOMInstance.ddlCombo.multiselect("getChecked").map(function () {
                    return this.value;
                }).get().join(',');

                $this.GetData(betType, sportTypes, combos, dateFrom, dateTo);
            }
        });
    };
    var renderReportData = function (result) {
        var bodyData = [];
        var footerData = [];

        $this.DOMInstance.divResult.addClass('display-none');

        if (result && result.Data) {
            var records = result.Data.ChildItems;
            var row = null;
            var info = null;

            $.each(records, function (i, dateData) {
                $.each(dateData.ChildItems, function (j, info) {
                    row =
                    {
                        Columns: [{ Name: 'ColCombo', Value: info.Combo == '6' ? labels.SixUp : info.Combo },
                                  { Name: 'ColTicketWon', Value: info.TicketWon },
                                  { Name: 'ColTicketLose', Value: info.TicketLose },
                                  { Name: 'ColStakeWon', Value: info.StakeWon },
                                  { Name: 'ColStakeLose', Value: info.StakeLose },
                                  { Name: 'ColWinLossWon', Value: info.MemberWon },
                                  { Name: 'ColWinLossLose', Value: info.MemberLose },
                                  { Name: 'ColOurPTWon', Value: info.OurPTWon },
                                  { Name: 'ColOurPTLose', Value: info.OurPTLose },
                                  { Name: 'ColOurPTNetWinLoss', Value: info.OurPTNetWinLoss}]
                    };

                    if (j == 0) {
                        row.Columns.unshift({ Name: 'ColDate', Value: info.WinLossDate, MergeRowNumber: dateData.ChildItems.length });
                    }

                    bodyData.push(row);
                });

                // Subtotal row
                row =
                {
                    Columns: [{ Name: 'ColDate', Value: labels.Subtotal, MergeColNumber: 2, CellAlign: 'right' },
                              { Name: 'ColTicketWon', Value: dateData.TicketWon },
                              { Name: 'ColTicketLose', Value: dateData.TicketLose },
                              { Name: 'ColStakeWon', Value: dateData.StakeWon },
                              { Name: 'ColStakeLose', Value: dateData.StakeLose },
                              { Name: 'ColWinLossWon', Value: dateData.MemberWon },
                              { Name: 'ColWinLossLose', Value: dateData.MemberLose },
                              { Name: 'ColOurPTWon', Value: dateData.OurPTWon },
                              { Name: 'ColOurPTLose', Value: dateData.OurPTLose },
                              { Name: 'ColOurPTNetWinLoss', Value: dateData.OurPTNetWinLoss}],
                    CssClass: "subTotal"
                };

                bodyData.push(row);
            });

            // Total row
            row =
            {
                Columns: [{ Name: 'ColDate', Value: labels.Total, CellAlign: 'right', MergeColNumber: 2 },
                          { Name: 'ColTicketWon', Value: result.Data.TicketWon },
                          { Name: 'ColTicketLose', Value: result.Data.TicketLose },
                          { Name: 'ColStakeWon', Value: result.Data.StakeWon },
                          { Name: 'ColStakeLose', Value: result.Data.StakeLose },
                          { Name: 'ColWinLossWon', Value: result.Data.MemberWon },
                          { Name: 'ColWinLossLose', Value: result.Data.MemberLose },
                          { Name: 'ColOurPTWon', Value: result.Data.OurPTWon },
                          { Name: 'ColOurPTLose', Value: result.Data.OurPTLose },
                          { Name: 'ColOurPTNetWinLoss', Value: result.Data.OurPTNetWinLoss}]
            };
            footerData.push(row);
        }

        var gridOptions =
        {
            customizeHeader: true,
            groupHeaderRows: [{
                Columns: [{ HeaderText: labels.Date, Name: "ColDate", MergeRowNumber: 2 },
                          { HeaderText: labels.Combo, Name: "ColCombo", MergeRowNumber: 2 },
                          { HeaderText: labels.Ticket, Name: "ColTicketWon", MergeColNumber: 2 },
                          { HeaderText: labels.StakeAfterPT, Name: "ColStakeWon", MergeColNumber: 2 },
                          { HeaderText: labels.WinLoss, Name: "ColWinLossWon", MergeColNumber: 2 },
                          { HeaderText: labels.OurPT, Name: "ColOurPTWon", MergeColNumber: 3}]
            }],
            columns: [{ HeaderText: labels.Date, Width: 120, Name: 'ColDate', HeaderAlign: 'center', CellAlign: 'center', FormatDateTime: ParlayRMS.commonConstants.MomentDateFormat },
                      { HeaderText: labels.Combo, Width: 83, Name: 'ColCombo', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Won, Width: 110, Name: 'ColTicketWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.NumberFormat },
                      { HeaderText: labels.Lose, Width: 110, Name: 'ColTicketLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.NumberFormat },
                      { HeaderText: labels.Won, Width: 110, Name: 'ColStakeWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.Lose, Width: 110, Name: 'ColStakeLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.Won, Width: 110, Name: 'ColWinLossWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat, FormatNegative: '$' },
                      { HeaderText: labels.Lose, Width: 110, Name: 'ColWinLossLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.Won, Width: 110, Name: 'ColOurPTWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat, FormatNegative: '$' },
                      { HeaderText: labels.Lose, Width: 110, Name: 'ColOurPTLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.NetWinLoss, Width: 100, Name: 'ColOurPTNetWinLoss', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat}],
            showTitle: true,
            showFooter: true,
            gridExpandHeight: 260,
            titleInfo: {
                text: result.Title,
                showExcelButton: true,
                exportExcelFunction: $this.ExportReport
            },
            showDistinctRow: false,
            bodyRows: bodyData,
            footerRows: footerData
        };

        $this.DOMInstance.divResult.removeClass('display-none').fanexGrid(gridOptions);
    };

    $this.DOMInstance = {
        divFilterForm: null,
        ddlBetType: null,
        ddlSportTypes: null,
        ddlCombo: null,
        dpkDateFrom: null,
        dpkDateTo: null,
        btnSearch: null,
        divResult: null,
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
    $this.GetData = function (betType, sportTypes, combos, dateFrom, dateTo) {
        var data = {
            BetType: betType,
            SportType: sportTypes,
            Selection: combos,
            FromDate: dateFrom,
            ToDate: dateTo,
            IsLoadNew: true
        };
        params = data;
        $.ajax({
            type: "POST",
            url: urls.GetWinLoss,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
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
        ParlayRMS.downloadFile(urls.ExportWinLoss, params);
    };
}