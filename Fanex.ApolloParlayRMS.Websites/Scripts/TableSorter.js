// Elements sorter
jQuery.fn.sortElements = (function () {
    var sort = [].sort;

    return function (comparator, getSortable) {
        getSortable = getSortable || function () { return this; };

        var placements = this.map(function () {
            var sortElement = getSortable.call(this);
            var parentNode = sortElement.parentNode;
            var nextSibling = parentNode.insertBefore(document.createTextNode(''), sortElement.nextSibling);

            return function () {
                if (parentNode === this) {
                    throw new Error("You can't sort elements if any one is a descendant of another.");
                }

                // Insert before flag:
                parentNode.insertBefore(this, nextSibling);
                // Remove flag:
                parentNode.removeChild(nextSibling);
            };
        });

        return sort.call(this, comparator).each(function (i) {
            placements[i].call(getSortable.call(this));
        });
    };
})();

// Table sorter
var tableSorter = function () {
    return {
        currentOrderCol: null,
        bind: function (src, desc, callbackFunc) {
            src.find('th.sortable').each(function () {
                $(this).attr('title', 'Click to sort');
                $(this).bind('click', function () {
                    var currentObj = this;

                    // Change css class of current sortable header.
                    if (currentObj.sortDirection == 'asc') {
                        currentObj.sortDirection = "desc";
                        $(currentObj).removeClass("headerSortDown").addClass("headerSortUp");
                    } else {
                        currentObj.sortDirection = "asc";
                        $(currentObj).removeClass("headerSortUp").addClass("headerSortDown");
                    }

                    // Reset other sortable headers
                    src.find('.sortable').each(function () {
                        if ($(currentObj).index() != $(this).index()) {
                            $(this).removeClass('headerSortDown').removeClass('headerSortUp');
                            this.sortDirection = '';
                        }
                    });

                    // Get type of sorting column.
                    var type = $(currentObj).attr('data-col-type');
                    type = type ? type : 'text';

                    tableSorter.currentOrderCol = currentObj;
                    tableSorter.sortColumn(desc, $(currentObj).index(), currentObj.sortDirection, type);

                    // Call callback function when sorting is finished
                    if (callbackFunc) {
                        callbackFunc(currentObj);
                    }
                });
            });
        },
        sortColumn: function (table, colIndex, sortDirection, type) {
            var selector = $(table).find('td').filter(function () {
                return $(this).index() == colIndex;
            }).sortElements(function (fParam, sParam) {
                var fValue = $.trim($(fParam).text());
                var sValue = $.trim($(sParam).text());

                if (type == 'float') {
                    fValue = parseFloat(fValue.replace(/,/g, ''));
                    sValue = parseFloat(sValue.replace(/,/g, ''));
                } else if (type == 'int') {
                    fValue = parseInt(fValue.replace(/,/g, ''));
                    sValue = parseInt(sValue.replace(/,/g, ''));
                } else if (type == 'date') {
                    console.log(new Date(moment(fValue, ParlayRMS.commonConstants.MomentDateTimeFormat)));
                    fValue = (new Date(moment(fValue, ParlayRMS.commonConstants.MomentDateTimeFormat))).getTime();
                    sValue = (new Date(moment(sValue, ParlayRMS.commonConstants.MomentDateTimeFormat))).getTime();
                } else if (type == 'ignoreCase') {
                    fValue = fValue.toLowerCase();
                    sValue = sValue.toLowerCase();
                }

                if (sortDirection == 'asc') {
                    return fValue > sValue ? -1 : 1;
                }
                else {
                    return fValue > sValue ? 1 : -1;
                }
            }, function () {
                return this.parentNode;
            });
        }
    }
} ();