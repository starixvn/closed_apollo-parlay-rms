var Layout = function ($) {

	return {
		filterBar: null,
		pageBody: null,
		page: null,
		frmPage: null
	};

} (jQuery);

$(document).ready(function () {
	Layout.page = $('#page');
	Layout.frmPage = $('#frm-page');

	Layout.pageBody = $('#body');
	Layout.filterBar = $('#filter-bar');

	// #region Menu
	var menuEvent = 'click';

	$('nav').on(menuEvent, '.menu li', function (e) {
		var $this = $(this),
			_SELECTED = 'selected';

		if ($this.hasClass(_SELECTED)) {
			$this.removeClass(_SELECTED).find('li').removeClass(_SELECTED);
		} else {
			$this.addClass(_SELECTED).siblings('li').removeClass(_SELECTED);
		}

		if (!$this.hasClass('has-kids')) {
			$.cookie('menu-id', $this.attr('mid'), { path: '/' });
		}

		e.stopPropagation();
	}).find('li').each(function () {
		var $this = $(this),
			menuId = $.cookie('menu-id');

		if ($this.attr('mid') == menuId) {
			$this.addClass('current').parents('li').addClass('current');
		}
	});

	$('header').on(menuEvent, '.greeting', function (e) {
		$(this).toggleClass('selected');
		e.stopPropagation();
	});

	$('body').on('click', function () {
		$('nav li, header .greeting').removeClass('selected');
	});
	// #endregion Menu

	$('.pnl-header-toggle').live('click', function () {
		var $this = $(this),
			pnlWrapper = $this.parents('.pnl-wrp'),
			pnlHeader = pnlWrapper.find('.pnl-header'),
			pnlBody = pnlWrapper.find('.pnl-body');

		$this.toggleClass('off');
		pnlHeader.toggleClass('off');
		pnlBody.slideToggle('fast');
	});
});