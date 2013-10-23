$(document).ready(function () {
    var memberWinLossReport = new Fanex.ApolloParlayRMS.Reports.MemberWinLossReport();

    memberWinLossReport.InitPage(labels);
});

Namespace.Register("Fanex.ApolloParlayRMS.Reports.MemberWinLossReport");
Fanex.ApolloParlayRMS.Reports.MemberWinLossReport = function () {
    var $this = this;
    var labels = {};
    var urls = {
        GetWinLoss: ParlayRMS.resolveRootUrl('Reports/MemberWinLoss/GetMemberWinLoss'),
        ExportWinLoss: ParlayRMS.resolveRootUrl('Reports/MemberWinLoss/ExportMemberWinLoss')
    };
    var params = {};
    var cacheDOM = function () {
        $this.DOMInstance.divFilterForm = $('#filter-bar');
        $this.DOMInstance.txtUserName = $('#txtUserName');
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

        $this.DOMInstance.txtUserName.bind('blur', function () {
            var userName = $this.DOMInstance.txtUserName.val().trim();
            $this.DOMInstance.txtUserName.val(userName);
        });

        $this.DOMInstance.txtUserName.bind('keyup', function (event) {
            if (event.keyCode == 13) {
                $this.DOMInstance.txtUserName.blur();
                $this.DOMInstance.btnSearch.click();
            }
        });

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
            if (Validator.validateRequired($this.DOMInstance.txtUserName)
                && Validator.validateMultiselectRequired($this.DOMInstance.ddlCombo)
                && Validator.validateDateRange($this.DOMInstance.dpkDateFrom, $this.DOMInstance.dpkDateTo)) {

                var userName = $this.DOMInstance.txtUserName.val();
                var betType = $this.DOMInstance.ddlBetType.val();
                var sportTypes = $this.DOMInstance.ddlSportTypes.val(); //$('input[name=rbgSportTypes]:checked', '#sportTypes').val();
                var dateFrom = $this.DOMInstance.dpkDateFrom.val();
                var dateTo = $this.DOMInstance.dpkDateTo.val();
                var combos = $this.DOMInstance.ddlCombo.multiselect("getChecked").map(function () {
                    return this.value;
                }).get().join(',');
                console.log(sportTypes);
                $this.GetData(userName, betType, sportTypes, combos, dateFrom, dateTo);
            }
        });
    };

    // Render report script
    var renderAgentReport = function (result) {
        var bodyData = [];
        var footerData = [];

        $this.DOMInstance.divResult.addClass('display-none');

        if (result && result.Data) {
            var records = result.Data.ChildItems;
            var row = null;
            var info = null;

            $.each(records, function (i, info) {
                row =
                {
                    Columns: [{ Name: 'ColTicketWon', Value: info.TicketWon },
                              { Name: 'ColTicketLose', Value: info.TicketLose },
                              { Name: 'ColStakeWon', Value: info.StakeWon },
                              { Name: 'ColStakeLose', Value: info.StakeLose },
                              { Name: 'ColWinLossWon', Value: info.MemberWon },
                              { Name: 'ColWinLossLose', Value: info.MemberLose },
                              { Name: 'ColOurPTWon', Value: info.OurPTWon },
                              { Name: 'ColOurPTLose', Value: info.OurPTLose },
                              { Name: 'ColOurPTNetWinLoss', Value: info.OurPTNetWinLoss}]
                };

                if (info.IsSutotalRow) {
                    row.Columns.unshift({ Name: 'ColMonth', Value: labels.Subtotal, MergeColNumber: 2, CellAlign: 'right' });
                    row.CssClass = "subTotal";
                } else {
                    row.Columns.unshift({ Name: 'ColUserName', Value: info.UserName });
                    if (info.NumOfSubrows > 0) {
                        row.Columns.unshift({ Name: 'ColMonth', Value: info.Month, MergeRowNumber: info.NumOfSubrows });
                    }
                }

                bodyData.push(row);
            });

            // Total row
            row =
            {
                Columns: [{ Name: 'ColMonth', Value: labels.Total, MergeColNumber: 2, CellAlign: 'right' },
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
                Columns: [{ HeaderText: labels.Month, Name: "ColMonth", MergeRowNumber: 2 },
                          { HeaderText: labels.UserName, Name: "ColUserName", MergeRowNumber: 2 },
                          { HeaderText: labels.Ticket, Name: "ColTicketWon", MergeColNumber: 2 },
                          { HeaderText: labels.StakeAfterPT, Name: "ColStakeWon", MergeColNumber: 2 },
                          { HeaderText: labels.WinLoss, Name: "ColWinLossWon", MergeColNumber: 2 },
                          { HeaderText: labels.OurPT, Name: "ColOurPTWon", MergeColNumber: 3}]
            }],
            columns: [{ HeaderText: labels.Month, Width: 100, Name: 'ColMonth', HeaderAlign: 'center', CellAlign: 'center', FormatDateTime: ParlayRMS.commonConstants.MomentDateFormat },
                      { HeaderText: labels.UserName, Width: 150, Name: 'ColUserName', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Won, Width: 80, Name: 'ColTicketWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.NumberFormat },
                      { HeaderText: labels.Lose, Width: 80, Name: 'ColTicketLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.NumberFormat },
                      { HeaderText: labels.Won, Width: 110, Name: 'ColStakeWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt },
                      { HeaderText: labels.Lose, Width: 110, Name: 'ColStakeLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt },
                      { HeaderText: labels.Won, Width: 110, Name: 'ColWinLossWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt, FormatNegative: '$' },
                      { HeaderText: labels.Lose, Width: 110, Name: 'ColWinLossLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt },
                      { HeaderText: labels.Won, Width: 110, Name: 'ColOurPTWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt, FormatNegative: '$' },
                      { HeaderText: labels.Lose, Width: 110, Name: 'ColOurPTLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt },
                      { HeaderText: labels.NetWinLoss, Width: 110, Name: 'ColOurPTNetWinLoss', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt}],
            showTitle: true,
            showFooter: true,
            gridExpandHeight: 220,
            titleInfo: {
                text: result.Title,
                showExcelButton: true,
                exportExcelFunction: $this.ExportReport
            },
            showPager: true,
            pagerOption: {
                itemsPerPage: 20,
                maxItemsPerPage: 100,
                currentPage: 0,
                startPerPage: 10,
                pageStep: 10
            },
            showDistinctRow: false,
            bodyRows: bodyData,
            footerRows: footerData
        };

        $this.DOMInstance.divResult.removeClass('display-none').fanexGrid(gridOptions);
    };
    var renderMemberReport = function (result) {
        var bodyData = [];
        var footerData = [];

        $this.DOMInstance.divResult.addClass('display-none');

        if (result && result.Data) {
            var records = result.Data.ChildItems;
            var row = null;
            var info = null;

            $.each(records, function (i, info) {
                row =
                {
                    Columns: [{ Name: 'ColMonth', Value: info.Month },
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

                bodyData.push(row);
            });

            // Total row
            row =
            {
                Columns: [{ Name: 'ColMonth', Value: labels.Total, CellAlign: 'right' },
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
                Columns: [{ HeaderText: labels.Month, Name: "ColMonth", MergeRowNumber: 2 },
                          { HeaderText: labels.Ticket, Name: "ColTicketWon", MergeColNumber: 2 },
                          { HeaderText: labels.StakeAfterPT, Name: "ColStakeWon", MergeColNumber: 2 },
                          { HeaderText: labels.WinLoss, Name: "ColWinLossWon", MergeColNumber: 2 },
                          { HeaderText: labels.OurPT, Name: "ColOurPTWon", MergeColNumber: 3}]
            }],
            columns: [{ HeaderText: labels.Month, Width: 100, Name: 'ColMonth', HeaderAlign: 'center', CellAlign: 'center', FormatDateTime: ParlayRMS.commonConstants.MomentDateFormat },
                      { HeaderText: labels.Won, Width: 80, Name: 'ColTicketWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.NumberFormat },
                      { HeaderText: labels.Lose, Width: 80, Name: 'ColTicketLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.NumberFormat },
                      { HeaderText: labels.Won, Width: 110, Name: 'ColStakeWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt },
                      { HeaderText: labels.Lose, Width: 110, Name: 'ColStakeLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt },
                      { HeaderText: labels.Won, Width: 110, Name: 'ColWinLossWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt, FormatNegative: '$' },
                      { HeaderText: labels.Lose, Width: 110, Name: 'ColWinLossLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt },
                      { HeaderText: labels.Won, Width: 110, Name: 'ColOurPTWon', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt, FormatNegative: '$' },
                      { HeaderText: labels.Lose, Width: 110, Name: 'ColOurPTLose', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt },
                      { HeaderText: labels.NetWinLoss, Width: 110, Name: 'ColOurPTNetWinLoss', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalForamt}],
            showTitle: true,
            showFooter: true,
            gridExpandHeight: 220,
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

    // Public methods
    $this.DOMInstance = {
        divFilterForm: null,
        txtUserName: null,
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
    $this.GetData = function (userName, betType, sportTypes, combos, dateFrom, dateTo) {
        var data = {
            UserName: userName,
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
            success: function (result) {
                if (result && result.IsMember) {
                    params.IsMember = result.IsMember;
                    renderMemberReport(result);
                } else {
                    renderAgentReport(result);
                }
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