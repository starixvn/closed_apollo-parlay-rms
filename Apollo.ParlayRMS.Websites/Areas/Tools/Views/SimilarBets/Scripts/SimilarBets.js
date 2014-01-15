$(document).ready(function () {
    var similarBets = new Apollo.ParlayRMS.Tools.SimilarBets();

    similarBets.InitPage(labels);
});

Namespace.Register("Apollo.ParlayRMS.Tools.SimilarBets");
Apollo.ParlayRMS.Tools.SimilarBets = function () {
    var $this = this;
    var labels = {};
    var urls = {
        SimilarBets: ParlayRMS.resolveRootUrl('Tools/SimilarBets'),
        GetSimilarBets: ParlayRMS.resolveRootUrl('Tools/SimilarBets/GetSimilarBets'),
        GetBetDetails: ParlayRMS.resolveRootUrl('Tools/SimilarBets/GetBetDetails')
    };
    var originalRefNo = null;
    var cacheDOM = function () {
        $this.DOMInstance.divFilterForm = $('#filter-bar');
        $this.DOMInstance.txtBetId = $('#txtBetId');
        $this.DOMInstance.btnSearch = $('#btnSearch');
        $this.DOMInstance.divResult = $('#divResult');
        $this.DOMInstance.divDetails = $('#divDetails');
        $this.DOMInstance.divDetailContainer = $('.detail-grid');
    };
    var initFields = function () {
        $this.DOMInstance.txtBetId.bind('blur', function (event) {
            var trimValue = $.trim($(this).val());
            $(this).val(trimValue);
        });

        $this.DOMInstance.txtBetId.bind('keyup', function (event) {
            if (event.keyCode == 13) {
                $this.DOMInstance.txtBetId.blur();
                $this.DOMInstance.btnSearch.click();
            }
        });

        $this.DOMInstance.btnSearch.bind('click', function () {
            Validator.clearErrorMessages($this.DOMInstance.divFilterForm);
            if (Validator.validateRequired($this.DOMInstance.txtBetId)) {
                var betId = $this.DOMInstance.txtBetId.val();
                $this.GetData(betId);
            }
        });

        if ($this.DOMInstance.txtBetId.val() != '') {
            $this.DOMInstance.btnSearch.click();
        }
    };
    var handleResultGridEvents = function () {
        $('.view-details', $this.DOMInstance.divResult).each(function () {
            $(this).bind('click', function () {
                var transId = $(this).attr('data-trans-id');
                var refNo = $(this).attr('data-ref-no');
                var username = $(this).attr('data-user-name');

                $this.GetBetList(username, transId, refNo, refNo);
            });
        });

        $('.bet-list', $this.DOMInstance.divResult).each(function () {
            $(this).bind('click', function () {
                $this.DOMInstance.divDetailContainer.toggle();
            });
        });
    };
    var handleDetailsGridEvents = function () {
        $('.bet-details', $this.DOMInstance.divDetails).each(function () {
            $(this).bind('click', function () {
                var transId = $(this).attr('data-trans-id');
                var refNo = $(this).attr('data-ref-no');
                var username = $(this).attr('data-user-name');

                $this.GetBetList(username, transId, refNo, originalRefNo);
            });
        });
    };
    var renderSimilarBets = function (result) {
        var bodyData = [];

        $this.DOMInstance.divResult.addClass('display-none');

        if (result && result.Username) {
            var row =
            {
                Columns: [{ Name: 'ColTransTime', Value: result.TransDate },
                          { Name: 'ColBetId', Value: result.TransId },
                          { Name: 'ColUsername', Value: "<div class='pallet-levels'><span class='level{0}'></span>{1}</div>".format(result.Level, result.Username) },
                          { Name: 'ColOdds', Value: result.Odds },
                          { Name: 'ColStakeAfterPT', Value: result.Stake },
                          { Name: 'ColMaxPayoutAfterPT', Value: result.MaxPayout },
                          { Name: 'ColTotalCombo', Value: result.Combo },
                          { Name: 'ColBetDetails', Value: "<div class='link view-details' data-user-name='{0}' data-trans-id='{1}' data-ref-no='{2}'>{3}</div>".format(result.Username, result.TransId, result.RefNo, labels.View) },
                          { Name: 'ColSimilarBets', Value: result.NumOfBets > 0 ? "<div class='link bet-list'>{0}</div>".format(result.NumOfBets) : result.NumOfBets}]
            };
            bodyData.push(row);
        }
        
        var gridOptions =
        {
            columns: [{ HeaderText: labels.TransTime, Width: 180, Name: 'ColTransTime', HeaderAlign: 'center', CellAlign: 'center', FormatDateTime: ParlayRMS.commonConstants.MomentDateTimeFormat, Timezone: ParlayRMS.ServerTimezone },
                      { HeaderText: labels.BetId, Width: 100, Name: 'ColBetId', HeaderAlign: 'center', CellAlign: 'right' },
                      { HeaderText: labels.Username, Width: 150, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Odds, Width: 80, Name: 'ColOdds', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.StakeAfterPT, Width: 120, Name: 'ColStakeAfterPT', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.MaxPayoutAfterPT, Width: 120, Name: 'ColMaxPayoutAfterPT', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.TotalCombo, Width: 100, Name: 'ColTotalCombo', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: ParlayRMS.commonConstants.NumberFormat },
                      { HeaderText: labels.BetDetails, Width: 100, Name: 'ColBetDetails', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.SimilarBets, Width: 100, Name: 'ColSimilarBets', HeaderAlign: 'center', CellAlign: 'center'}],
            gridExpandHeight: 220,
            showDistinctRow: false,
            bodyRows: bodyData
        };

        $this.DOMInstance.divResult.removeClass('display-none').fanexGrid(gridOptions);
        handleResultGridEvents();
    };
    var renderSimilarBetDetails = function (records) {
        var bodyData = [];

        $this.DOMInstance.divDetails.addClass('display-none');

        if (records && records.length > 0) {
            var row = null;
            var info = null;

            $.each(records, function (i, info) {
                row =
                {
                    Columns: [{ Name: 'ColNo', Value: i + 1 },
                              { Name: 'ColBetId', Value: info.TransId },
                              { Name: 'ColUsername', Value: "<div class='pallet-levels'><span class='level{0}'></span>{1}</div>".format(info.Level, info.Username) },
                              { Name: 'ColMaxPayoutAfterPT', Value: info.MaxPayout },
                              { Name: 'ColSimilarComboFound', Value: info.SimilarCombo },
                              { Name: 'ColBetDetails', Value: "<div class='link bet-details' data-user-name='{0}' data-trans-id='{1}' data-ref-no='{2}'>{3}</div>".format(info.Username, info.TransId, info.RefNo, labels.View)}]
                };
                bodyData.push(row);
            });
        }

        var gridOptions =
        {
            columns: [{ HeaderText: labels.No, Width: 60, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.BetId, Width: 100, Name: 'ColBetId', HeaderAlign: 'center', CellAlign: 'right' },
                      { HeaderText: labels.Username, Width: 180, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.MaxPayoutAfterPT, Width: 150, Name: 'ColMaxPayoutAfterPT', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: ParlayRMS.commonConstants.DecimalFormat },
                      { HeaderText: labels.SimilarComboFound, Width: 150, Name: 'ColSimilarComboFound', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: ParlayRMS.commonConstants.NumberFormat },
                      { HeaderText: labels.BetDetails, Width: 80, Name: 'ColBetDetails', HeaderAlign: 'center', CellAlign: 'center'}],
            gridExpandHeight: 220,
            showTitle: true,
            titleInfo: {
                text: labels.SimilarBetList
            },
            showDistinctRow: false,
            bodyRows: bodyData
        };

        $this.DOMInstance.divDetails.removeClass('display-none').fanexGrid(gridOptions);
        $this.DOMInstance.divDetailContainer.addClass('display-none');
        handleDetailsGridEvents();
    };
    var customizeBetList = function () {
        $('#dialog-popup div[class=different-bet] > div > div[class=match]').prepend('<div class="markDiv"></div>');
    };

    $this.DOMInstance = {
        divFilterForm: null,
        txtBetId: null,
        btnSearch: null,
        divResult: null,
        divDetails: null,
        divDetailContainer: null
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
    $this.GetData = function (transId) {
        var data = {
            transId: transId
        };

        $.ajax({
            type: "POST",
            url: urls.GetSimilarBets,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: function () {
                ParlayRMS.loadingBox.show();
            },
            success: function (result) {
                renderSimilarBets(result);
                if (result) {
                    renderSimilarBetDetails(result.Details);
                    originalRefNo = result.RefNo;
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
    $this.GetBetList = function (username, transId, refNo, originalRefNo) {
        var data = {
            transId: transId,
            refNo: refNo,
            originalRefNo: originalRefNo,
            username: username
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
                ParlayRMS.popup(result.Title, result.TicketDetail);
                customizeBetList();
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