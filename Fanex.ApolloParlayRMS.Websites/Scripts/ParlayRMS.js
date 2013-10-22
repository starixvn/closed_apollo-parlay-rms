
var ParlayRMS = function ($) {

	return {
		ClientUrl: null,
		RootUrl: null,
		dialogMsg: null,
		dialogPopup: null,
		headlineMsgBox: null,
		loadingBox: null,
		alertCallback: null,
		confirmCallback: null,
		msieVersion: null,
		agentIsOldIe: null,
		agentIsApple: null,
		agentIsMobile: null,

		commonConstants: {
		    DateFormat: 'mm-dd-yy',
            MomentDateFormat: 'MM-DD-YYYY',
            DecimalForamt: '0,0.00',
            NumberFormat: '0,0'
		},

		initCoreValues: function () {
			ParlayRMS.ClientUrl = $('#ClientURL').val();
			ParlayRMS.RootUrl = $('#RootURL').val();
			ParlayRMS.dialogPopup = $('<div id="dialog-popup"></div>').appendTo('body');

			ParlayRMS.msieVersion = ParlayRMS.getIeVersion();
			ParlayRMS.agentIsOldIe = ParlayRMS.msieVersion != -1 && ParlayRMS.msieVersion < 9.0;
			ParlayRMS.agentIsApple = ParlayRMS.agentIs('apple') != null;
			ParlayRMS.agentIsMobile = ParlayRMS.agentIs('mobile') != null;
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
				ParlayRMS.headlineMsgBox.showErr('An unexpected error has occurred!', { sticky: true });
			}
		},
		showInfoMessage: function (message) {
			ParlayRMS.infoMsgBox.showInfo(message, { duration: 3000 });
		},
		getIeVersion: function () {
			var version = -1; // Return value assumes failure.

			if (window.navigator.userAgent.indexOf("MSIE") > 0) {
				var usrAgent = navigator.userAgent;
				var reg = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");

				if (reg.exec(usrAgent) != null) version = parseFloat(RegExp.$1);
			}

			return version;
		},
		agentIs: function (name) {
			switch (name.toLowerCase()) {
				case 'ipad':
					return navigator.userAgent.match(/iPad/i);
				case 'iphone':
					return navigator.userAgent.match(/iPhone/i);
				case 'ipod':
					return navigator.userAgent.match(/iPod/i);
				case 'apple':
					return navigator.userAgent.match(/iPad/i) ||
						navigator.userAgent.match(/iPhone/i) ||
						navigator.userAgent.match(/iPod/i);
				case 'mobile':
					return
					// Apple
					navigator.userAgent.match(/iPad/i) ||
						navigator.userAgent.match(/iPhone/i) ||
						navigator.userAgent.match(/iPod/i) ||
					// RIM
						navigator.userAgent.match(/BlackBerry/i) ||
						navigator.userAgent.match(/PlayBook/i)
					// Windows Phone
					navigator.userAgent.match(/Windows Phone/i);
					// Android
					navigator.userAgent.match(/Android/i);
					// General
					navigator.userAgent.match(/Touch/i);
				default: return null;
			}
		},
		alert: function (title, msg, callback) {
		},
		confirm: function (title, msg, callback) {
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
		htmlEncode: function (value) {
			return $('<div/>').text(value).html();
		},
		htmlDecode: function (value) {
			return $('<div/>').html(value).text();
		},
		showSuccessMsg: function (msg, url, msec) {
			var msgWrp = $('.validation-summary-valid');

			msgWrp.addClass('success')
				.find('li:first').html(ParlayRMS.htmlDecode(msg)).show();

			ParlayRMS.showSuccessMsgCountDown(url, msec / 1000);
		},
		showSuccessMsgCountDown: function (url, msec) {
			if (msec-- > 0) {
				setTimeout('ParlayRMS.showSuccessMsgCountDown("' + url + '", ' + msec + ')', 1000);
				$('.validation-summary-valid .msec').html(msec);
			} else {
				window.location.replace(NAP.resolveClientUrl(url));
			}
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

		$form.data('validator').settings.errorPlacement = function(error, element) {
			$(element).next().hide();
		};
 
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