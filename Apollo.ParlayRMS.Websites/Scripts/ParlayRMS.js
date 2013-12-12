
var ParlayRMS = function ($) {

    return {
        ClientUrl: null,
        RootUrl: null,
        ServerTimezone: null,
        dialogMsg: null,
        dialogPopup: null,
        headlineMsgBox: null,
        loadingBox: null,

        commonConstants: {
            DateFormat: 'yy-mm-dd',
            MomentDateFormat: 'YYYY-MM-DD',
            MomentDateTimeFormat: 'YYYY-MM-DD hh:mm:ss A',
            DecimalFormat: '0,0.00',
            NumberFormat: '0,0'
        },

        initCoreValues: function () {
            ParlayRMS.ClientUrl = $('#ClientURL').val();
            ParlayRMS.RootUrl = $('#RootURL').val();
            ParlayRMS.ServerTimezone = $('#ServerTimezone').val();
            ParlayRMS.dialogPopup = $('<div id="dialog-popup"></div>').appendTo('body');
        },
        resolveClientUrl: function (relativeUrl) {
            return ParlayRMS.ClientUrl + relativeUrl;
        },
        resolveRootUrl: function (relativeUrl) {
            return ParlayRMS.RootUrl + relativeUrl;
        },
        // Handles common error when making ajax request
        handleAjaxError: function (xhr, status, err) {
            if (xhr.status == 403) {
                ParlayRMS.headlineMsgBox.showErr('Unauthorized request rejected!', { sticky: true });
            } else if (xhr.status == 404) {
                ParlayRMS.headlineMsgBox.showErr('Resources not found!', { sticky: true });
            } else {
                ParlayRMS.headlineMsgBox.showErr('Sorry, an error occurred while processing your request, please notify the administrators.', { sticky: true });
            }
        },
        showInfoMessage: function (message) {
            ParlayRMS.infoMsgBox.showInfo(message, { duration: 3000 });
        },
        modal: function (title, html) {
            ParlayRMS.dialogPopup.html(html).dialog({
                width: 'auto',
                modal: true,
                position: ['center', 160]
            });

            ParlayRMS.dialogPopup.dialog('option', { title: title });
        },
        popup: function (title, html) {
            ParlayRMS.dialogPopup.html(html).dialog({
                width: 'auto',
                modal: false,
                position: ['center', 160]
            });

            ParlayRMS.dialogPopup.dialog('option', { title: title });
        },
        closePopup: function () {
            ParlayRMS.dialogPopup.dialog('close');
        },
        downloadFile: function (url, data, method) {
            //url and data options required
            if (url && data) {
                //data can be string of parameters or array/object
                data = typeof data == 'string' ? data : $.param(data);
                var downloadUrl = url + "?" + data;
                var iframe = $('body iframe');
                if (iframe.length == 0) {
                    iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
                    iframe.appendTo('body');
                }

                iframe.attr('src', downloadUrl);
            }
        },
        redirect: function (url, msec) {
            setTimeout('ParlayRMS.navigateTo("' + url + '")', msec);
        },
        navigateTo: function (url) {
            window.location.replace(ParlayRMS.resolveClientUrl(url));
        },
        navigateToRoot: function (url) {
            window.location.replace(ParlayRMS.resolveRootUrl(url));
        },
        openWindow: function (url) {
            var popH = $(window).height(), popW = $(window).width();
            var formatter = new Array(
                "resizable=no",
                "scrollbars=yes",
                "toolbar=no",
                "menubar=no",
                "location=no",
                "addressbar=no",
                "directories=no",
                "status=yes"
            ).join(',');

            window.open(url, '_blank', '' + ',' + formatter);
        },
        openNewTab: function (url) {
            window.open(url, '_blank');
        },
        htmlEncode: function (value) {
            return $('<div/>').text(value).html();
        },
        htmlDecode: function (value) {
            return $('<div/>').html(value).text();
        },

        encodeURI: function (str) {
            // ref: https://developer.mozilla.org/en-US/docs/JavaScript/Reference/Global_Objects/encodeURI
            return encodeURI(str).replace(/%5B/g, '[').replace(/%5D/g, ']');
        }
    };

} (jQuery);

$(document).ready(function () {
    ParlayRMS.initCoreValues();

    ParlayRMS.headlineMsgBox = $.msgbox({
        message: '',
        //containerId: 'msg-bar',
        type: 'err',
        modal: false,
        position: ['top:130', 'center'],
        theme: '',
        zIndex: 9999,
        useAnimation: false
    });

    ParlayRMS.infoMsgBox = $.msgbox({
        message: '',
        type: 'info',
        modal: false,
        position: ['top:130', 'center'],
        theme: '',
        zIndex: 9999,
        useAnimation: false
    });

    ParlayRMS.loadingBox = $.msgbox({
        message: 'Loading...',
        type: 'warn',
        modal: true,
        position: ['top:5', 'center'],
        closeBtn: false,
        theme: '',
        zIndex: 9999,
        sticky: true,
        useAnimation: false
    });
});

// -- Utilities -- //

// String Format
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined' ? args[number]: match;
    });
};

// Array Remove - By John Resig (MIT Licensed)
Array.prototype.remove = function (from, to) {
    var rest = this.slice((to || from) + 1 || this.length);
    this.length = from < 0 ? this.length + from : from;
    return this.push.apply(this, rest);
};

// Array Remove All elements matched the criteria
Array.prototype.removeAll = function (criteria) {
    var length = this.length;

    for (var i = 0; i < length; i++) {
        var element = this[i];

        if (criteria(element)) {
            this.splice(i--, 1);
            length--;
        }
    }
};

// Reset form helper
(function ($) { 
    //re-set all client validation given a jQuery selected form or child
    $.fn.resetValidation = function () {
        var $form = this.closest('form');
 
        $form.validate().resetForm();
 
        //reset unobtrusive validation summary, if it exists
        $form.find("[data-valmsg-summary=true]")
            .removeClass("validation-summary-errors")
            .addClass("validation-summary-valid")
            .find("ul").empty();
 
        //reset unobtrusive field level, if it exists
        $form.find("[data-valmsg-replace]")
            .removeClass("field-validation-error")
            .addClass("field-validation-valid")
            .empty();

        //remove error marks for fields
        $form.find('.input-validation-error')
            .removeClass('input-validation-error')
            .addClass('valid');

        $form.removeData('validator');
        $form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse($form);

//		$form.data('validator').settings.errorPlacement = function(error, element) {
//			//$(element).next().hide();
//		};
 
        return $form;
    };
 
    //reset a form given a jQuery selected form or a child
    //by default validation is also reset
    $.fn.formReset = function (resetValidation) {
        var $form = this.closest('form');
 
        $form[0].reset();
 
        if (resetValidation == undefined || resetValidation) {
            $form.resetValidation();
        }
 
        return $form;
    }
})(jQuery);