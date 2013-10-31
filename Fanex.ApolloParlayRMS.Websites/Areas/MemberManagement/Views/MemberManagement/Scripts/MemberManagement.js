$(document).ready(function () {
    var memberManagement = new Fanex.ApolloParlayRMS.MemberManagement.MemberManagement();
        
    levelUpdater.Init(levels, messages);
    memberManagement.InitPage(labels, messages);
});

Namespace.Register("Fanex.ApolloParlayRMS.MemberManagement.MemberManagement");
Fanex.ApolloParlayRMS.MemberManagement.MemberManagement = function () {
    var $this = this;
    var labels = {};
    var messages = {};
    var urls = {
        GetSpecialCustomers: ParlayRMS.resolveRootUrl('MemberManagement/MemberManagement/GetSpecialCustomers'),
        ExportCustomerList: ParlayRMS.resolveRootUrl('MemberManagement/MemberManagement/ExportCustomerList'),
        AddNewCustomer: ParlayRMS.resolveRootUrl('MemberManagement/MemberManagement/AddSpecialCustomer')
    };
    var params = {};
    var cacheDOM = function () {
        $this.DOMInstance.lnkAddNew = $('#lnkAddNew');
        $this.DOMInstance.txtUserName = $('#txtUserName');
        $this.DOMInstance.ddlLevel = $('#ddlLevel');
        $this.DOMInstance.btnSearch = $('#btnSearch');
        $this.DOMInstance.btnShowAll = $('#btnShowAll');
        $this.DOMInstance.divResult = $('#divResult');
        $this.DOMInstance.divAddMemberForm = $('#divAddMemberForm');

        // Add new member form's instance
        $this.DOMInstance.divAddMemberFormMembers.UserName = $('#UserName');
        $this.DOMInstance.divAddMemberFormMembers.Level = $('#Level');
        $this.DOMInstance.divAddMemberFormMembers.Submit = $('#Submit');
    };
    var bindEvents = function () {
        $this.DOMInstance.lnkAddNew.live('click', function () {
            ParlayRMS.modal(labels.AddSpecialMember, $this.DOMInstance.divAddMemberForm);
            $this.DOMInstance.divAddMemberFormMembers.Form = $('#dialog-popup form');
            $this.DOMInstance.divAddMemberFormMembers.Form.formReset(true);
            initDropDownList($this.DOMInstance.divAddMemberFormMembers.Level);

            $this.DOMInstance.divAddMemberFormMembers.Form.live('submit', function (event) {
                $this.AddMember();
                event.preventDefault();
            });
        });

        $this.DOMInstance.btnSearch.live('click', function () {
            var userName = $this.DOMInstance.txtUserName.val();
            var level = $this.DOMInstance.ddlLevel.val();

            $this.GetData(userName, level);
        });

        $this.DOMInstance.btnShowAll.live('click', function () {
            $this.GetData(null, null);
        });
    };
    var initDropDownList = function (obj) {
        obj.multiselect({
            multiple: false,
            header: false,
            noneSelectedText: "Select an Option",
            selectedList: 1,
            minWidth: 132,
            height: 180,
            format: formatItem
        });
    };
    var initFields = function () {
        levelUpdater.CreateFilterField($this.DOMInstance.ddlLevel);
        initDropDownList($this.DOMInstance.ddlLevel);
        levelUpdater.CreateAddNewField($this.DOMInstance.divAddMemberFormMembers.Level);
        initDropDownList($this.DOMInstance.divAddMemberFormMembers.Level);
    };
    var initLevelUpdaterFields = function () {
        var isEnable = hasEditRight ? 'enable' : 'disable';
        $("[data-type='levelUpdater']").each(function () {
            initDropDownList($(this));
            $(this).multiselect(isEnable);
        });
    };
    var renderData = function (data) {
        var bodyData = [];

        $this.DOMInstance.divResult.addClass('display-none');

        if (data && data.length > 0) {
            var row = null;
            var info = null;

            $.each(data, function (i, info) {
                row =
                {
                    Columns: [{ Name: 'ColNo', Value: i + 1 },
                              { Name: 'ColUserName', Value: info.UserName },
                              { Name: 'ColLevel', Value: levelUpdater.CreateUpdateField(info.CustId, info.Level, hasEditRight, info.UserName)}]
                };
                bodyData.push(row);
            });
        }

        var gridOptions =
        {
            columns: [{ HeaderText: labels.No, Width: 100, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.UserName, Width: 270, Name: 'ColUserName', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Level, Width: 150, Name: 'ColLevel', HeaderAlign: 'center', CellAlign: 'center'}],
            showTitle: true,
            showFooter: true,
            gridExpandHeight: 300,
            titleInfo: {
                showExcelButton: true,
                exportExcelFunction: $this.ExportReport
            },
            showNumberColumn: true,
            showPager: true,
            pagerOption: {
                itemsPerPage: 20,
                maxItemsPerPage: 100,
                currentPage: 0,
                startPerPage: 10,
                pageStep: 10,
                afterPagedFunction: initLevelUpdaterFields
            },
            bodyRowHeight: 24,
            bodyRows: bodyData
        };

        $this.DOMInstance.divResult.removeClass('display-none').fanexGrid(gridOptions);
        initLevelUpdaterFields();
    };

    $this.DOMInstance = {
        lnkAddNew: null,
        txtUserName: null,
        ddlLevel: null,
        btnSearch: null,
        btnShowAll: null,
        divResult: null,
        divAddMemberForm: null,
        divAddMemberFormMembers: {
            Form: null,
            UserName: null,
            Level: null,
            Submit: null
        }
    };
    $this.SetLang = function (labelValues, messageValues) {
        if (labelValues) {
            labels = labelValues;
        }

        if (messageValues) {
            messages = messageValues;
        }
    };
    $this.InitPage = function (labels, messages) {
        cacheDOM();
        initFields();
        bindEvents();

        $this.SetLang(labels, messages);
        $this.GetData(null, null);
    };
    $this.GetData = function (userName, level) {
        var data = {
            UserName: userName,
            Level: level,
            IsLoadNew: true
        };

        params = data;
        $.ajax({
            type: "POST",
            url: urls.GetSpecialCustomers,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: function () {
                ParlayRMS.loadingBox.show();
            },
            success: function (records) {
                renderData(records);
            },
            error: function (xhr, status, err) {
                ParlayRMS.handleAjaxError(xhr, status, err);
            },
            complete: function () {
                ParlayRMS.loadingBox.close();
            }
        });
    };
    $this.AddMember = function () {
        var userName = $this.DOMInstance.divAddMemberFormMembers.UserName.val();
        var level = $this.DOMInstance.divAddMemberFormMembers.Level.val();
        var data = {
            UserName: userName,
            Level: level
        };
        
        $this.DOMInstance.divAddMemberFormMembers.Form.resetValidation();
        $.ajax({
            type: "POST",
            url: urls.AddNewCustomer,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: function () {
                ParlayRMS.loadingBox.show();
            },
            success: function (result) {
                if (result.Code == 0) {
                    ParlayRMS.showInfoMessage(messages.AddNewCustomerSuccessfully);
                    ParlayRMS.closePopup();
                } else {
                    ParlayRMS.headlineMsgBox.showErr(result.Message);
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
        ParlayRMS.downloadFile(urls.ExportCustomerList, params);
    };
}