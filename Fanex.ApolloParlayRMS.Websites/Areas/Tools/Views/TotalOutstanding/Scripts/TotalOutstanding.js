$(document).ready(function () {
    var totalOutstanding = new Fanex.ApolloParlayRMS.Tools.TotalOutstanding();

    totalOutstanding.InitPage(labels);
});

Namespace.Register("Fanex.ApolloParlayRMS.Tools.TotalOutstanding");
Fanex.ApolloParlayRMS.Tools.TotalOutstanding = function () {
    var $this = this;
    var labels = {};
    var urls = {
        TotalOutstanding: ParlayRMS.resolveRootUrl('Tools/TotalOutstanding'),
        GetTotalOutstanding: ParlayRMS.resolveRootUrl('Tools/TotalOutstanding/GetTotalOutstanding'),
        SimilarBets: ParlayRMS.resolveRootUrl('Tools/SimilarBets'),
        GetBetDetails: ParlayRMS.resolveRootUrl('Tools/TotalOutstanding/GetBetDetails')
    };
    var params = {
        userName: null,
        combos: null,
        sportTypes: null
    };
    var cacheDOM = function () {
        $this.DOMInstance.divFilterForm = $('#filter-bar');
        $this.DOMInstance.txtUserName = $('#txtUserName');
        $this.DOMInstance.ddlSportTypes = $('#ddlSportTypes');
        $this.DOMInstance.ddlCombo = $('#ddlCombo');
        $this.DOMInstance.btnSearch = $('#btnSearch');
        $this.DOMInstance.divResult = $('#divResult');
    };
    var initMultiDropDownList = function (obj, height, width) {
        obj.multiselect({
            multiple: true,
            minWidth: width,
            height: height,
            create: function () {
                obj.multiselect();
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
        initMultiDropDownList($this.DOMInstance.ddlCombo, 106, 132);
        initMultiDropDownList($this.DOMInstance.ddlSportTypes, 65, 132);

        $this.DOMInstance.txtUserName.bind('blur', function (event) {
            var trimValue = $.trim($(this).val());
            $(this).val(trimValue);
        });

        $this.DOMInstance.txtUserName.bind('keyup', function (event) {
            if (event.keyCode == 13) {
                $this.DOMInstance.txtUserName.blur();
                $this.DOMInstance.btnSearch.click();
            }
        });

        $this.DOMInstance.btnSearch.bind('click', function () {
            Validator.clearErrorMessages($this.DOMInstance.divFilterForm);
            if (Validator.validateMultiselectRequired($this.DOMInstance.ddlCombo)
                && Validator.validateMultiselectRequired($this.DOMInstance.ddlSportTypes)
                && Validator.validateRequired($this.DOMInstance.txtUserName)) {

                var userName = $this.DOMInstance.txtUserName.val();
                var combos = $this.DOMInstance.ddlCombo.multiselect("getChecked").map(function () {
                    return this.value;
                }).get().join(',');
                var sportTypes = $this.DOMInstance.ddlSportTypes.multiselect("getChecked").map(function () {
                    return this.value;
                }).get().join(',');

                $this.GetData(userName, sportTypes, combos);
            }
        });

        if ($this.DOMInstance.txtUserName.val() != '') {
            $this.DOMInstance.btnSearch.click();
        }
    };
    var handleMemberGridEvents = function () {
        $('.view-details', $this.DOMInstance.divResult).each(function () {
            $(this).bind('click', function () {
                var transId = $(this).attr('data-trans-id');
                var refNo = $(this).attr('data-ref-no');

                $this.GetBetList(params.userName, transId, refNo);
            });
        });

        $('.similar-bets', $this.DOMInstance.divResult).each(function () {
            $(this).bind('click', function () {
                var transId = $(this).text();
                var url = urls.SimilarBets + '?betId={0}'.format(transId);

                ParlayRMS.openNewTab(url);
            });
        });
    };
    var handleAgentGridEvents = function () {
        $('.user-name', $this.DOMInstance.divResult).each(function () {
            $(this).bind('click', function () {
                var userName = $(this).text();
                var url = urls.TotalOutstanding + '?userName={0}&combos={1}&sportTypes={2}'.format(userName, params.combos, params.sportTypes);

                ParlayRMS.openNewTab(url);
            });
        });
    };
    var renderMemTotalOutstanding = function (records, total) {
        var bodyData = [];
        var footerData = [];

        $this.DOMInstance.divResult.addClass('display-none');

        if (records && records.length > 0) {
            var row = null;
            var info = null;

            $.each(records, function (i, info) {
                row =
                {
                    Columns: [{ Name: 'ColNo', Value: i + 1 },
                              { Name: 'ColTransTime', Value: info.TransDate },
                              { Name: 'ColBetId', Value: hasSimilarBetRight ? "<div class='link similar-bets'>{0}</div>".format(info.TransId) : info.TransId },
                              { Name: 'ColOdds', Value: info.Odds },
                              { Name: 'ColStakeAfterPT', Value: info.StakePT },
                              { Name: 'ColMaxPayoutAfterPT', Value: info.MaxPayoutPT },
                              { Name: 'ColTotalCombo', Value: info.Combo },
                              { Name: 'ColBetDetails', Value: "<div class='link view-details' data-trans-id='{0}' data-ref-no='{1}'>{2}</div>".format(info.TransId, info.RefNo, labels.View)}]
                };
                bodyData.push(row);
            });

            // Total row
            row =
            {
                Columns: [{ Name: 'ColNo', Value: labels.Total, CellAlign: 'right', MergeColNumber: 4 },
                          { Name: 'ColStakeAfterPT', Value: total.StakePT },
                          { Name: 'ColMaxPayoutAfterPT', Value: total.MaxPayoutPT },
                          { Name: 'ColTotalCombo', Value: '', MergeColNumber: 2}]
            };
            footerData.push(row);
        }

        var gridOptions =
        {
            columns: [{ HeaderText: labels.No, Width: 60, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.TransTime, Width: 180, Name: 'ColTransTime', HeaderAlign: 'center', CellAlign: 'center', FormatDateTime: ParlayRMS.commonConstants.MomentDateTimeFormat },
                      { HeaderText: labels.BetId, Width: 100, Name: 'ColBetId', HeaderAlign: 'center', CellAlign: 'right' },
                      { HeaderText: labels.Odds, Width: 80, Name: 'ColOdds', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.StakeAfterPT, Width: 120, Name: 'ColStakeAfterPT', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.MaxPayoutAfterPT, Width: 150, Name: 'ColMaxPayoutAfterPT', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.TotalCombo, Width: 100, Name: 'ColTotalCombo', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: ParlayRMS.commonConstants.NumberFormat },
                      { HeaderText: labels.BetDetails, Width: 100, Name: 'ColBetDetails', HeaderAlign: 'center', CellAlign: 'center'}],
            showFooter: true,
            gridExpandHeight: 220,
            showDistinctRow: false,
            bodyRows: bodyData,
            footerRows: footerData
        };

        $this.DOMInstance.divResult.removeClass('display-none').fanexGrid(gridOptions);
        handleMemberGridEvents();
    };
    var renderAgTotalOutstanding = function (records, total) {
        var bodyData = [];
        var footerData = [];

        $this.DOMInstance.divResult.addClass('display-none');

        if (records && records.length > 0) {
            var row = null;
            var info = null;

            $.each(records, function (i, info) {
                row =
                {
                    Columns: [{ Name: 'ColNo', Value: i + 1 },
                              { Name: 'ColUserName', Value: "<div class='link user-name'>{0}</div>".format(info.UserName) },
                              { Name: 'ColStakeAfterPT', Value: info.StakePT },
                              { Name: 'ColMaxPayoutAfterPT', Value: info.MaxPayoutPT },
                              { Name: 'ColTotalBets', Value: info.BetCount}]
                };
                bodyData.push(row);
            });

            // Total row
            row =
            {
                Columns: [{ Name: 'ColNo', Value: labels.Total, CellAlign: 'right', MergeColNumber: 2 },
                          { Name: 'ColStakeAfterPT', Value: total.StakePT },
                          { Name: 'ColMaxPayoutAfterPT', Value: total.MaxPayoutPT },
                          { Name: 'ColTotalBets', Value: total.BetCount}]
            };
            footerData.push(row);
        }

        var gridOptions =
        {
            columns: [{ HeaderText: labels.No, Width: 60, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.UserName, Width: 180, Name: 'ColUserName', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.StakeAfterPT, Width: 120, Name: 'ColStakeAfterPT', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.MaxPayoutAfterPT, Width: 150, Name: 'ColMaxPayoutAfterPT', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.TotalBets, Width: 100, Name: 'ColTotalBets', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: ParlayRMS.commonConstants.NumberFormat}],
            showFooter: true,
            gridExpandHeight: 220,
            showDistinctRow: false,
            bodyRows: bodyData,
            footerRows: footerData
        };

        $this.DOMInstance.divResult.removeClass('display-none').fanexGrid(gridOptions);
        handleAgentGridEvents();
    };

    $this.DOMInstance = {
        divFilterForm: null,
        txtUserName: null,
        ddlCombo: null,
        ddlSportTypes: null,
        btnSearch: null,
        divResult: null
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
    $this.GetData = function (userName, sportTypes, combos) {
        var data = {
            userName: userName,
            sportTypes: sportTypes,
            combos: combos
        };

        params = data;
        $.ajax({
            type: "POST",
            url: urls.GetTotalOutstanding,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: function () {
                ParlayRMS.loadingBox.show();
            },
            success: function (result) {
                if (result.IsMember) {
                    renderMemTotalOutstanding(result.Records, result.Total);
                } else {
                    renderAgTotalOutstanding(result.Records, result.Total);
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
    $this.GetBetList = function (userName, transId, refNo) {
        var data = {
            betId: transId,
            refNo: refNo,
            userName: userName
        };

        $.ajax({
            type: "POST",
            url: urls.GetBetDetails,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: function () {
                ParlayRMS.loadingBox.show();
            },
            success: function (result) {
                ParlayRMS.popup(result.Title, result.BetList);
            },
            error: function (xhr, status, err) {
                ParlayRMS.handleAjaxError(xhr, status, err);
            },
            complete: function () {
                ParlayRMS.loadingBox.close();
            }
        });
    };
}