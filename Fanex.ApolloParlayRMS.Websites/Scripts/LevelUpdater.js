// This lib must be used with Multislect library.
Namespace.Register("Fanex.ApolloParlayRMS.LevelUpdater");
Fanex.ApolloParlayRMS.LevelUpdater = function () {
    var $this = this;
    var levels = {};
    var messages = {};
    var updateField = null;

    var createLevelUpdater = function (ctrId, hasAll, hasNone, defaultValue, isEnable) {
        var select = $('<select />', { id: ctrId });
        var option = null;
        if (!isEnable) {
            select.attr('disabled', 'true');
        }

        if (hasAll) {
            option = $('<option />', { text: levels.AllLevel.Text, value: levels.AllLevel.Value });
            if (defaultValue == 'null') {
                option.attr('selected', 'true');
            }

            select.append(option);
        }

        $.each(levels.Levels, function (i, el) {
            option = $('<option />', { text: el.Text, value: el.Value });
            if (defaultValue == el.Value) {
                option.attr('selected', 'true');
            }

            select.append(option);
        });

        if (hasNone) {
            option = $('<option />', { text: levels.None.Text, value: levels.None.Value });
            if (defaultValue == '0') {
                option.attr('selected', 'true');
            }

            select.append(option);
        }

        return select;
    };

    $this.Init = function (sourceLevel, sourceMessages) {
        levels = sourceLevel;
        if (sourceMessages) {
            messages = sourceMessages;
        } else {
            messages.ChangeLevelConfirmation = 'Do you want to change the level of {0} to {1}';
            messages.ChangeLevelConfirmation = 'Change level successfully!';
        }
    };
    $this.CreateFilterField = function (obj) {
        var objId = obj.attr('id');
        obj.html(createLevelUpdater(objId, true, true, 'null', true).html());
    };
    $this.CreateAddNewField = function (obj) {
        var objId = obj.attr('id');
        obj.html(createLevelUpdater(objId, false, false, '10', true).html());
    };
    $this.CreateUpdateField = function (id, defaultValue, isEnable, userName) {
        updateField = createLevelUpdater(id, false, true, defaultValue, isEnable);
        updateField.attr('onchange', "levelUpdater.ChangeLevel('{0}')".format(id));
        updateField.attr('data-type', 'levelUpdater');
        updateField.attr('data-username', userName);
        updateField.attr('data-current-value', defaultValue);

        return $('<div>').append(updateField).html();
    };
    $this.ChangeLevel = function (id) {
        var dropDown = $('#' + id);
        var userName = dropDown.attr('data-username');
        var level = dropDown.val();
        var msg = messages.ChangeLevelConfirmation.format(userName, level);

        dropDown.val(dropDown.attr('data-current-value'));
        dropDown.multiselect('refresh'); // Refresh multiselect control
        ParlayRMS.headlineMsgBox.confirm(msg, function () {
            var data = {
                CustId: id,
                UserName: userName,
                Level: level
            };

            $.ajax({
                type: "POST",
                url: ParlayRMS.resolveRootUrl('MemberManagement/MemberManagement/ChangeCustomerLevel'),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                beforeSend: function () {
                    ParlayRMS.loadingBox.show();
                },
                success: function (result) {
                    if (result.Code != 0) {
                        ParlayRMS.headlineMsgBox.showErr(result.Message, { sticky: true });
                    } else {
                        dropDown.val(level);
                        dropDown.multiselect('refresh'); // Refresh multiselect control
                        dropDown.attr('data-current-value', dropDown.val());
                        ParlayRMS.showInfoMessage(messages.ChangeLevelSuccessfully);
                    }
                },
                error: function (xhr, status, err) {
                    ParlayRMS.handleAjaxError(xhr, status, err);
                },
                complete: function () {
                    ParlayRMS.loadingBox.close();
                }
            });
        });
    };
};
var levelUpdater = new Fanex.ApolloParlayRMS.LevelUpdater();

// Format level updater's items
var formatItem = function (obj) {
    var itemClass = 'level' + $(obj).val();
    var itemValue = $(obj).text();

    return "<div class='pallet-levels'><span class='{0}'></span>{1}</div>".format(itemClass, itemValue);
};
var formatSelection = function (obj, value) {
    var itemClass = $('span', value).attr('class');
    var itemValue = $(obj).attr('data-username');

    return "<div class='pallet-levels'><span class='{0}'></span>{1}</div>".format(itemClass, itemValue);
};