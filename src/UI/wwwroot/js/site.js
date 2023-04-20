/// <reference path="../../node_modules/jquery/dist/jquery.js" />

function suggestInputValueByNamingCase(value, to) {
    'use strict'
    
    if (!value) {
        return;
    }

    const val = value.split(' ')
        .filter(s => s)
        .map((v, i, arr) => {
            const c = v[0].toLowerCase()

            if (v.length == 1) {
                if (i + 1 != arr.length && c == arr[i + 1][0].toLowerCase()) {
                    return ''
                }
                else {
                    return c
                }
            }
            else {
                return c.toUpperCase()
            }
        })
        .join('')

    $(`[name="${to}"]`).val(val)
}

//$.fn.extend({
//    hideByCompareValue: function (compareAttr) {
//        const val = $(this).val()
//        $(`[${compareAttr}]`).each(function () {
//            const attr = $(this).attr(compareAttr)
//            attr >= val ? $(this).addClass('d-none') : $(this).removeClass('d-none')
//        })
//    }
//})
(function () {
    $.fn.hideByCompareValue = function (compareAttr) {
        const val = $(this).val()

        $(`[${compareAttr}]`).each(function () {
            const attr = $(this).attr(compareAttr)
            attr >= val ? $(this).addClass('d-none') : $(this).removeClass('d-none')
        })
    }
}(jQuery))
