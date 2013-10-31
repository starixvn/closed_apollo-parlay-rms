var Validator = function ($) {
    //var dateFormat = 'MM-DD-YYYY';
    var errorMessages = {};

    var getErrorMessages = function () {
        $.ajax({
            type: "POST",
            url: '/Home/GetValidationErrorMessages', // ParlayRMS.resolveRootUrl('Home/GetValidationErrorMessages'),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                //ParlayRMS.loadingBox.show();
            },
            success: function (data) {
                errorMessages = data;
            },
            error: function (xhr, status, err) {
                //ParlayRMS.handleAjaxError(xhr, status, err);
            },
            complete: function () {
                //ParlayRMS.loadingBox.close();
            }
        });
    };
    var showTooltip = function (obj, message) {
        var $parent = $(obj).parent();
        $parent.attr('title', message);
        $parent.tipsy(
        {
            trigger: 'manual'
        });
        $parent.tipsy('show');
        $(obj).focus();
    };

    getErrorMessages();

    return {
        clearErrorMessages: function (form) {
            form.find('[original-title]').each(function () {
                //$(this).removeClass('.field-validation-error');
                $(this).tipsy('hide');
            });
        },
        validateRequired: function (element) {
            var value = $.trim($(element).val());

            if (!value) {
                //$(element).addClass('field-validation-error');
                showTooltip(element, errorMessages.RequiredField);
                return false;
            } else {
                return true;
            }
        },
        validateMultiselectRequired: function (element) {
            var value = $(element).multiselect("getChecked");

            if (!value || value.length <= 0) {
                //$(element).next().addClass('field-validation-error');
                showTooltip(element, errorMessages.RequiredField);
                return false;
            } else {
                return true;
            }
        },
        validateNumber: function (element) {
            var value = $.trim($(element).val().replace(/,/g, ''));

            if ((value != '') && (!$.isNumeric(value))) {
                showTooltip(element, errorMessages.InvalidNumberField);
                return false;
            } else {
                return true;
            }
        },
        validatePositiveInt: function (element) {
            var value = $.trim($(element).val().replace(/,/g, ''));

            if ((value != '') && (!$.isNumeric(value) || parseInt(value) < 0)) {
                //$(element).addClass('field-validation-error');
                showTooltip(element, errorMessages.PositiveField);
                return false;
            } else {
                return true;
            }
        },
        validateDate: function (element) {
            var isValid = Validator.validateRequired(element);

            if (isValid) {
                var date = $(element).val();

                if (!moment(date, ParlayRMS.commonConstants.MomentDateFormat).isValid()) {
                    //$(element).attr('title', errorMessages.InvalidDateField);
                    showTooltip(element, errorMessages.InvalidDateField);
                    isValid = false;
                }
            }

            return isValid;
        },
        validateDateRange: function (fromDate, toDate) {
            var isValid = Validator.validateDate(fromDate) && Validator.validateDate(toDate);

            if (isValid) {
                var fDate = new Date(moment($(fromDate).val()));
                var tDate = new Date(moment($(toDate).val()));

                if (fDate.getTime() > tDate.getTime()) {
                    //$(element).addClass('field-validation-error');
                    showTooltip(toDate, errorMessages.DateRangeField);
                    isValid = false;
                }
            }

            return isValid;
        }
    };
} (jQuery);