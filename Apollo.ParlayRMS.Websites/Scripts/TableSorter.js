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
            src.find('span.sortable').each(function () {
                $(this).attr('title', 'Click to sort');
                $(this).bind('click', function () {
                    var currentObj = this;
                    var currentCol = $(currentObj).closest('th');

                    // Change css class of current sortable header.
                    if (currentObj.sortDirection == 'asc') {
                        currentObj.sortDirection = "desc";
                        $(currentObj).removeClass("sortDown").addClass("sortUp");
                    } else {
                        currentObj.sortDirection = "asc";
                        $(currentObj).removeClass("sortUp").addClass("sortDown");
                    }

                    // Reset other sortable headers
                    src.find('.sortable').each(function () {
                        var col = $(this).closest('th');
                        if ($(currentCol).index() != $(col).index()) {
                            $(this).removeClass('sortDown').removeClass('sortUp');
                            this.sortDirection = '';
                        }
                    });

                    // Get type of sorting column.
                    var type = $(currentCol).attr('data-col-type');
                    type = type ? type : 'text';
                    
                    tableSorter.currentOrderCol = currentCol;
                    tableSorter.sortColumn(desc, $(currentCol).index(), currentObj.sortDirection, type);

                    // Call callback function when sorting is finished
                    if (callbackFunc) {
                        callbackFunc(currentCol);
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