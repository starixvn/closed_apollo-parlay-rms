/* File Created: January 2, 2014 */
/* global numeral, Namespace, Apollo, ParlayRMS, Validator, labels, tableHeaders, soundManager, hasSimilarBetRight, hasTotalOutstandingRight */
/* jshint camelcase: false */

$(document).ready(function () {
    'use strict';
    var mixParlayMonitor = new Apollo.ParlayRMS.Tools.MixParlayMonitor();

    mixParlayMonitor.InitPage();
});

Namespace.Register('Apollo.ParlayRMS.Tools.MixParlayMonitor');
Apollo.ParlayRMS.Tools.MixParlayMonitor = function () {
    'use strict';
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
    var sortOptions = {};
    var dataStorage = [];
    var betIdStyle = '';
    var usernameStyle = '';

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
    var initFields = function () {
        initSingleDropDownList($this.DOMInstance.ddlSelectTop, 85, 132);
        initMultiDropDownList($this.DOMInstance.ddlCombo, 106, 132);
        initMultiDropDownList($this.DOMInstance.ddlSportTypes, 64, 132);
        initMultiDropDownList($this.DOMInstance.ddlLevel, 106, 132);
        initSingleDropDownList($this.DOMInstance.ddlRefreshTime, 65, 60);
        $this.DOMInstance.ddlRefreshTime.multiselect('option', 'classes', 'width-80');

        if (hasSimilarBetRight) {
            betIdStyle = 'link similar-bets';
        }

        if (hasTotalOutstandingRight) {
            usernameStyle = 'link total-outstanding';
        }
    };
    var bindEvenHandlers = function () {
        $this.DOMInstance.txtMaxPayout.bind('keyup', function (event) {
            var formatedValue = $.trim($(this).val());
            var rawValue = formatedValue.replace(/,/g, '');

            rawValue = $.isNumeric(rawValue) ? rawValue : '';

            if (rawValue !== '') {
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

            if (event.keyCode === 13) {
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

        $this.DOMInstance.btnSearch.bind('click', function () {
            Validator.clearErrorMessages($this.DOMInstance.divFilterForm);
            if (Validator.validateMultiselectRequired($this.DOMInstance.ddlCombo) &&
                Validator.validateMultiselectRequired($this.DOMInstance.ddlSportTypes) &&
                Validator.validateMultiselectRequired($this.DOMInstance.ddlLevel) &&
                Validator.validatePositiveInt($this.DOMInstance.txtMaxPayout)) {
                var maxPayout = $this.DOMInstance.txtMaxPayout.val().replace(/,/g, '');
                var limitRowCount = $this.DOMInstance.ddlSelectTop.val();
                var combo = $this.DOMInstance.ddlCombo.multiselect('getChecked').map(function () {
                    return this.value;
                }).get().join(',');
                var sportType = $this.DOMInstance.ddlSportTypes.multiselect('getChecked').map(function () {
                    return this.value;
                }).get().join(',');
                var level = $this.DOMInstance.ddlLevel.multiselect('getChecked').map(function () {
                    return this.value;
                }).get().join(',');

                maxPayout = (maxPayout === '') ? '0' : maxPayout;
                params.Max_TransId = 0;

                params.Selection = combo;
                params.SportType = sportType;
                params.Level = level;
                params.MaxPayout = parseFloat(maxPayout);
                params.LimitRowCount = parseInt(limitRowCount);

                $this.DOMInstance.divRefreshNow.removeAttr('disabled');
                dataStorage = [];
                sortOptions = {
                    sortColumnName: 'ColTransTime',
                    sortDirection: 'asc'
                };
                timer.start();
            }
        });

        setupSoundManager();
    };

    // Render and expand the table with new data.
    var createGridOptions = function () {
        var usernameCol = '<div class="pallet-levels {0}"><span class="level{1}"></span>{2}</div>';
        var betDetailsCol = '<div class="link view-details" data-user-name="{0}" data-trans-id="{1}" data-ref-no="{2}">{3}</div>';
        var gridOptions = {
            columns: [
                {
                    HeaderText: tableHeaders.No,
                    Width: 100,
                    Name: 'ColNo',
                    HeaderAlign: 'center',
                    CellAlign: 'center'
                },
                {
                    HeaderText: tableHeaders.TransTime,
                    Width: 170,
                    Name: 'ColTransTime',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                    FormatDateTime: ParlayRMS.commonConstants.MomentDateTimeFormat,
                    Timezone: ParlayRMS.ServerTimezone,
                    Sort: true,
                    SortDirection: 'ColTransTime' === sortOptions.sortColumnName ? sortOptions.sortDirection : ''
                },
                {
                    HeaderText: tableHeaders.BetId,
                    Width: 130,
                    Name: 'ColBetId',
                    HeaderAlign: 'center',
                    CellAlign: 'right',
                    CustomCellRender: function (cell) {
                        return '<div class="{0}">{1}</div>'.format(betIdStyle, cell.Value.TransId);
                    }
                },
                {
                    HeaderText: tableHeaders.Username,
                    Width: 170,
                    Name: 'ColUsername',
                    HeaderAlign: 'center',
                    CellAlign: 'left',
                    Sort: true,
                    SortDirection: 'ColUsername' === sortOptions.sortColumnName ? sortOptions.sortDirection : '',
                    CustomCellRender: function (cell) {
                        return usernameCol.format(usernameStyle, cell.Value.Level, cell.Value.Username);
                    },
                    CustomSortFunction: function (value1, value2, sortDirection) {
                        if (sortDirection === 'desc') {
                            return value1.Username >= value2.Username;
                        } else {
                            return value1.Username < value2.Username;
                        }
                    }
                },
                {
                    HeaderText: tableHeaders.Odds,
                    Width: 82,
                    Name: 'ColOdds',
                    HeaderAlign: 'center',
                    CellAlign: 'right',
                    FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                },
                {
                    HeaderText: tableHeaders.StakeAfterPT,
                    Width: 160,
                    Name: 'ColStakeAfterPT',
                    HeaderAlign: 'center',
                    CellAlign: 'right',
                    FormatNumber: ParlayRMS.commonConstants.DecimalFormat
                },
                {
                    HeaderText: tableHeaders.MaxPayoutAfterPT,
                    Width: 180,
                    Name: 'ColMaxPayoutAfterPT',
                    HeaderAlign: 'center',
                    CellAlign: 'right',
                    FormatNumber: ParlayRMS.commonConstants.DecimalFormat,
                    Sort: true,
                    SortDirection: 'ColMaxPayoutAfterPT' === sortOptions.sortColumnName ? sortOptions.sortDirection : ''
                },
                {
                    HeaderText: tableHeaders.TotalCombo,
                    Width: 100,
                    Name: 'ColTotalCombo',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                    FormatNumber: ParlayRMS.commonConstants.NumberFormat
                },
                {
                    HeaderText: tableHeaders.BetDetails,
                    Width: 100,
                    Name: 'ColBetDetails',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                    CustomCellRender: function (cell) {
                        return betDetailsCol.format(cell.Value.Username, cell.Value.TransId, cell.Value.RefNo, labels.View);
                    }
                }],
            gridExpandHeight: 270,
            showDistinctRow: false,
            showNumberColumn: true,
            showHighlightRow: false,
            sortOption: {
                afterGridSorted: function (rows, sortColumnName, sortDirection) {
                    sortOptions.sortColumnName = sortColumnName;
                    sortOptions.sortDirection = sortDirection;
                    handleGridEvents();

                    return rows;
                }
            }
        };

        return gridOptions;
    };
    var renderTable = function (data) {
        var bodyData = [];
        var gridOptions = createGridOptions();

        $this.DOMInstance.divResult.addClass('display-none');

        if ((data && data.length > 0) || (dataStorage && dataStorage.length > 0)) {
            var row = null;
            var numberOfRemovedItems = 0;
            var hasNewRows = data.length > 0 && dataStorage.length > 0;

            if (dataStorage.length + data.length > params.LimitRowCount) {
                numberOfRemovedItems = dataStorage.length + data.length - params.LimitRowCount;
            }

            if (numberOfRemovedItems > 0) {
                dataStorage.splice(bodyData.length - numberOfRemovedItems, numberOfRemovedItems);
            }

            // Push new data to data storage.
            if (data && data.length > 0) {
                dataStorage = data.concat(dataStorage);
                raiseAlert();
            }

            // Push new rows.
            $.each(dataStorage, function (index, info) {
                row =
                {
                    Columns: [{ Name: 'ColNo', Value: index + 1 },
                              { Name: 'ColTransTime', Value: info.TransDate },
                              { Name: 'ColBetId', Value: info },
                              { Name: 'ColUsername', Value: info },
                              { Name: 'ColOdds', Value: info.Odds },
                              { Name: 'ColStakeAfterPT', Value: info.Stake },
                              { Name: 'ColMaxPayoutAfterPT', Value: info.MaxPayout },
                              { Name: 'ColTotalCombo', Value: info.Combo },
                              { Name: 'ColBetDetails', Value: info}]
                };

                if (hasNewRows && index < data.length) {
                    row.CssClass = 'highlight';
                }

                bodyData.push(row);
            });
            gridOptions.bodyRows = bodyData;
        }

        $this.DOMInstance.divResult.removeClass('display-none').fanexGrid(gridOptions);
        handleGridEvents();
    };

    // Handle events occur on grid.
    var handleGridEvents = function () {
        $('div.view-details', $this.DOMInstance.tblContent).each(function () {
            $(this).bind('click', function () {
                var transId = $(this).attr('data-trans-id');
                var refNo = $(this).attr('data-ref-no');
                var username = $(this).attr('data-user-name');

                $this.GetBetList(username, transId, refNo);
            });
        });

        $('div.similar-bets', $this.DOMInstance.divResult).each(function () {
            $(this).bind('click', function () {
                var transId = $(this).text();
                var url = urls.SimilarBets + '?betId={0}'.format(transId);

                ParlayRMS.openNewTab(url);
            });
        });

        $('div.total-outstanding', $this.DOMInstance.divResult).each(function () {
            $(this).bind('click', function () {
                var username = $(this).text();
                var url = urls.TotalOutstanding + '?username={0}'.format(username);

                ParlayRMS.openNewTab(url);
            });
        });
    };

    // Raise sound alert when new tickets come.
    var setupSoundManager = function () {
        soundManager.setup({
            url: ParlayRMS.resolveRootUrl('Scripts/lib/SoundManager2/swf/'),
            flashVersion: 9,
            debugMode: false,
            onready: function () {
                soundManager.createSound({
                    id: 'alertSound',
                    url: ParlayRMS.resolveRootUrl('Sounds/alert.mp3')
                });
            },
            ontimeout: function () {
                alert('sound is disabled');
            }
        });
    };
    var raiseAlert = function () {
        if (params.MaxPayout > 0) {
            console.log(ParlayRMS.resolveRootUrl('Sounds/alert.mp3'));
            soundManager.play('alertSound');
        }
    };

    // Count down timer.
    var timer = (function () {
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

                    if (ticker === 0) {
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
    } ());

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
        divResult: null
    };
    $this.InitPage = function () {
        cacheDOM();
        initFields();
        bindEvenHandlers();
    };
    $this.GetData = function () {
        $.ajax({
            type: 'POST',
            url: urls.GetMixParlayData,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(params),
            beforeSend: function () {
                ParlayRMS.loadingBox.show();
            },
            success: function (result) {
                renderTable(result.Records);
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
    $this.GetBetList = function (username, transId, refNo) {
        var data = {
            betId: transId,
            refNo: refNo,
            username: username
        };

        $.ajax({
            type: 'POST',
            url: urls.GetBetDetails,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
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
};