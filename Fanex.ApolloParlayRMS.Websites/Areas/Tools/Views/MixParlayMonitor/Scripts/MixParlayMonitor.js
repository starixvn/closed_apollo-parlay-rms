$(document).ready(function () {
    var mixParlayMonitor = new Fanex.ApolloParlayRMS.Tools.MixParlayMonitor();

    mixParlayMonitor.InitPage();
});

Namespace.Register("Fanex.ApolloParlayRMS.Tools.MixParlayMonitor");
Fanex.ApolloParlayRMS.Tools.MixParlayMonitor = function () {
    var $this = this;
    var urls = {
        GetMixParlayData: ParlayRMS.resolveRootUrl('Tools/MixParlayMonitor/GetMixParlayData'),
        SimilarBets: ParlayRMS.resolveRootUrl('Tools/SimilarBets'),
        TotalOutstanding: ParlayRMS.resolveRootUrl('Tools/TotalOutstanding'),
        GetBetDetails: ParlayRMS.resolveRootUrl('Tools/TotalOutstanding/GetBetDetails')
    };
    var params = {
        Selection: null,
        SportType: null,
        Level: null,
        MaxPayout: null,
        Max_TransId: null,
        LimitRowCount: null
    };
    var cacheDOM = function () {
        $this.DOMInstance.divFilterForm = $('#filter-bar');
        $this.DOMInstance.ddlSelectTop = $('#ddlSelectTop');
        $this.DOMInstance.ddlSportTypes = $('#ddlSportTypes');
        $this.DOMInstance.ddlCombo = $('#ddlCombo');
        $this.DOMInstance.txtMaxPayout = $('#txtMaxPayout');
        $this.DOMInstance.ddlLevel = $('#ddlLevel');
        $this.DOMInstance.ddlRefreshTime = $('#ddlRefreshTime');
        $this.DOMInstance.divRefreshNow = $('#divRefreshNow');
        $this.DOMInstance.divTimer = $('#divTimer');
        $this.DOMInstance.chkPause = $('#chkPause');
        $this.DOMInstance.btnSearch = $('#btnSearch');
        $this.DOMInstance.divResult = $('#divResult');
        $this.DOMInstance.tblHeader = $('#tblHeader');
        $this.DOMInstance.tblResult = $('#tblResult');
        $this.DOMInstance.tblContent = $('#tblResult tbody');
    };
    var initSingleDropDownList = function (obj, height, width) {
        obj.multiselect({
            multiple: false,
            header: false,
            selectedList: 1,
            minWidth: width,
            height: height
        });
    };
    var initMultiDropDownList = function (obj, height, width) {
        obj.multiselect({
            multiple: true,
            minWidth: width,
            height: height,
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
        initSingleDropDownList($this.DOMInstance.ddlSelectTop, 90, 132);
        initMultiDropDownList($this.DOMInstance.ddlCombo, 106, 132);
        initMultiDropDownList($this.DOMInstance.ddlSportTypes, 106, 132);
        initMultiDropDownList($this.DOMInstance.ddlLevel, 106, 132);
        initSingleDropDownList($this.DOMInstance.ddlRefreshTime, 70, 60);
        $this.DOMInstance.ddlRefreshTime.multiselect('option', 'classes', 'width-80');
    };
    var bindEvenHandlers = function () {
        $this.DOMInstance.txtMaxPayout.bind('keyup', function (event) {
            var formatedValue = $.trim($(this).val());
            var rawValue = formatedValue.replace(/,/g, '');

            rawValue = $.isNumeric(rawValue) ? rawValue : '';

            if (rawValue != '') {
                var value = parseFloat(rawValue);

                if (value > 1000000000000) {
                    value = 1000000000000;
                    formatedValue = '1,000,000,000,000';
                } else if (formatedValue.indexOf('.') < 0) {
                    formatedValue = numeral(value).format(ParlayRMS.commonConstants.NumberFormat);
                }
            } else {
                formatedValue = rawValue;
            }

            $(this).val(formatedValue);

            if (event.keyCode == 13) {
                $this.DOMInstance.btnSearch.click();
            }
        });

        $this.DOMInstance.divRefreshNow.bind('click', function () {
            timer.start();
        });

        $this.DOMInstance.ddlRefreshTime.bind('change', function () {
            var ticker = $(this).val();
            timer.setRefreshTime(ticker / 1000);
            $this.DOMInstance.divRefreshNow.val('{0} [{1}]'.format(labels.RefreshNow, ticker / 1000));
        });
        $this.DOMInstance.ddlRefreshTime.change();

        $this.DOMInstance.chkPause.bind('change', function () {
            if ($(this).is(':checked')) {
                timer.pause();
            } else {
                timer.resume();
            }
        });

        $this.DOMInstance.btnSearch.live('click', function () {
            Validator.clearErrorMessages($this.DOMInstance.divFilterForm);
            if (Validator.validateMultiselectRequired($this.DOMInstance.ddlCombo)
                && Validator.validateMultiselectRequired($this.DOMInstance.ddlSportTypes)
                && Validator.validateMultiselectRequired($this.DOMInstance.ddlLevel)
                && Validator.validatePositiveInt($this.DOMInstance.txtMaxPayout)) {

                var maxPayout = $this.DOMInstance.txtMaxPayout.val().replace(/,/g, '');
                var limitRowCount = $this.DOMInstance.ddlSelectTop.val();
                var combo = $this.DOMInstance.ddlCombo.multiselect("getChecked").map(function () {
                    return this.value;
                }).get().join(',');
                var sportType = $this.DOMInstance.ddlSportTypes.multiselect("getChecked").map(function () {
                    return this.value;
                }).get().join(',');
                var level = $this.DOMInstance.ddlLevel.multiselect("getChecked").map(function () {
                    return this.value;
                }).get().join(',');

                maxPayout = (maxPayout == '') ? '0' : maxPayout;
                params.Max_TransId = 0;

                params.Selection = combo;
                params.SportType = sportType;
                params.Level = level;
                params.MaxPayout = parseInt(maxPayout);
                params.LimitRowCount = limitRowCount;

                $this.DOMInstance.tblContent.html('');
                $this.DOMInstance.divRefreshNow.removeAttr('disabled');
                tableSorter.currentOrderCol = null;
                timer.start();
            }
        });

        tableSorter.bind($this.DOMInstance.tblHeader, $this.DOMInstance.tblResult, function () {
            $this.DOMInstance.tblContent.children('tr').each(function (i, obj) {
                $(obj).children('td:first').text(i + 1);
            });
        });
    };
    var createRow = function (order, item) {
        var cells = [];

        cells.push('<tr>');
        cells.push('<td class="align-center">{0}</td>'.format(order));
        cells.push('<td class="align-center">{0}</td>'.format(moment(item.TransDate).format(ParlayRMS.commonConstants.MomentDateTimeFormat)));
        cells.push('<td class="align-right"><div class="link similar-bets">{0}</div></td>'.format(item.TransId));
        cells.push('<td class="align-left"><div class="link pallet-levels total-outstanding"><span class="level{0}"></span>{1}</div></td>'.format(item.Level, item.UserName));
        cells.push('<td class="align-right">{0}</td>'.format(numeral(item.Odds).format(ParlayRMS.commonConstants.DecimalFormat)))
        cells.push('<td class="align-right">{0}</td>'.format(numeral(item.Stake).format(ParlayRMS.commonConstants.DecimalFormat)))
        cells.push('<td class="align-right">{0}</td>'.format(numeral(item.MaxPayout).format(ParlayRMS.commonConstants.DecimalFormat)))
        cells.push('<td class="align-center">{0}</td>'.format(numeral(item.Combo).format(ParlayRMS.commonConstants.NumberFormat)))
        cells.push('<td class="align-center"><div class="link view-details" data-user-name="{0}" data-trans-id="{1}" data-ref-no="{2}">{3}</div></td>'.format(item.UserName, item.TransId, item.RefNo, labels.View));
        cells.push('</tr>')

        return cells.join('');
    };
    var expandTable = function (records) {        
        if (records && records.length > 0) {
            var rows = $this.DOMInstance.tblContent.children('tr');

            // @@TODO: Alert sound here.
            if (rows.length > 0) {
                var currentRow;

                $this.DOMInstance.tblContent.children('tr.noInfo').remove();
                $this.DOMInstance.tblContent.children('tr.highlight').removeClass('highlight');
                tableSorter.sortColumn($this.DOMInstance.tblResult, 1, "asc", 'date');

                $.each(records, function (i, item) {
                    currentRow = $(createRow(i + 1, item)).addClass('highlight'); // Highlight row
                    rows = $this.DOMInstance.tblContent.children('tr');

                    if (params.LimitRowCount == rows.length) {
                        rows.last().remove();
                    }

                    $this.DOMInstance.tblContent.prepend(currentRow);

                    // Re-order by current ordered column.
                    var orderCol = tableSorter.currentOrderCol;
                    if (orderCol) {
                        var type = $(orderCol).attr("data-col-type");
                        tableSorter.sortColumn($this.DOMInstance.tblResult, $(orderCol).index(), orderCol.sortDirection, type);
                    }

                    $this.DOMInstance.tblContent.children('tr').each(function (i, obj) {
                        $(obj).children('td:first').text(i + 1);
                    });
                });
            } else {
                var body = [];
                $.each(records, function (i, item) {
                    body.push(createRow(i + 1, item));
                });
                $this.DOMInstance.tblContent.html(body.join(''));
            }

            handleGridEvents();
        } else if ((params.Max_TransId == 0) && ($('tr.noInfo:first-child', $this.DOMInstance.tblContent).length == 0)) {
            $this.DOMInstance.tblContent.append('<tr class="noInfo"><td colspan="8">{0}</td></tr>'.format(labels.NoData));
        }

        $this.DOMInstance.divResult.removeClass('display-none');
    };
    var handleGridEvents = function () {
        $('.view-details', $this.DOMInstance.tblContent).each(function () {
            $(this).bind('click', function () {
                var transId = $(this).attr('data-trans-id');
                var refNo = $(this).attr('data-ref-no');
                var userName = $(this).attr('data-user-name');
                
                $this.GetBetList(userName, transId, refNo);
            });
        });

        $('.similar-bets', $this.DOMInstance.divResult).each(function () {
            $(this).bind('click', function () {
                var transId = $(this).text();
                var url = urls.SimilarBets + '?betId={0}'.format(transId);

                ParlayRMS.openNewTab(url);
            });
        });

        $('.total-outstanding', $this.DOMInstance.divResult).each(function () {
            $(this).bind('click', function () {
                var userName = $(this).text();
                var url = urls.TotalOutstanding + '?userName={0}'.format(userName);

                ParlayRMS.openNewTab(url);
            });
        });
    };

    var timer = function () {
        var ticker = null;
        var refreshTime = null;
        var timeout = null;

        var clearTimeout = function () {
            if (timeout) {
                window.clearTimeout(timeout);
                timeout = null;
            }
        };
        var runTimer = function () {
            if (ticker > 0) {
                timeout = setTimeout(function () {
                    ticker--;
                    $this.DOMInstance.divRefreshNow.val('{0} [{1}]'.format(labels.RefreshNow, ticker));

                    if (ticker == 0) {
                        $this.GetData();
                        ticker = refreshTime;
                    }

                    runTimer();
                }, 1000);
            }
        };

        return {
            setRefreshTime: function (time) {
                refreshTime = time;
                ticker = refreshTime;
            },
            start: function () {
                clearTimeout();
                $this.GetData();

                if (!$this.DOMInstance.chkPause.is(':checked')) {
                    ticker = refreshTime;
                    runTimer();
                }
            },
            pause: function () {
                clearTimeout();
            },
            resume: function () {
                clearTimeout();
                if (!$this.DOMInstance.chkPause.is(':checked')) {
                    runTimer();
                }
            }
        };
    } ();

    $this.DOMInstance = {
        divFilterForm: null,
        ddlSelectTop: null,
        ddlCombo: null,
        ddlSportTypes: null,
        txtMaxPayout: null,
        ddlLevel: null,
        ddlRefreshTime: null,
        divRefreshNow: null,
        divTimer: null,
        chkPause: null,
        btnSearch: null,
        divResult: null,
        tblHeader: null,
        tblResult: null,
        tblContent: null
    };
    $this.InitPage = function () {
        cacheDOM();
        initFields();
        bindEvenHandlers();
    };
    $this.GetData = function () {
        $.ajax({
            type: "POST",
            url: urls.GetMixParlayData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            beforeSend: function () {
                ParlayRMS.loadingBox.show();
            },
            success: function (result) {
                expandTable(result.Records);
                params.Max_TransId = result.MaxTransId;
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