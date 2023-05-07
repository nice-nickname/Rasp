/// <reference path="../../node_modules/jquery/dist/jquery.js" />
'use strict'

function sum(selector, root) {
    return $(root).find(selector)
                  .toArray()
                  .reduce((s, { value }) => s += Number(value), 0)
}

function suggestInputValueByNamingCase(value, to) {
    
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

(function () {
    $.fn.hideByCompareValue = function (compareAttr) {
        const val = $(this).val()

        $(`[${compareAttr}]`).each(function () {
            const attr = $(this).attr(compareAttr)
            if (attr >= val) {
                $(this).addClass('d-none')
                $(this).find('input').prop('disabled', true)
            }
            else {
                $(this).removeClass('d-none')
                $(this).find('input').prop('disabled', false)
            }
        })
    }

    $.fn.triggerByTimeout = function (event, miliseconds) {
        let timeout = $(this).attr('data-interval')
        clearTimeout(timeout)
        timeout = setTimeout(() => {
            $(this).trigger(event)
        }, miliseconds)
        $(this).attr('data-interval', timeout)
    }

    $.fn.selectpickerval = function (selected = "") {
        $(this).selectpicker('val', JSON.parse(selected))
    }

    $.fn.disciplinePlanReset = function() {
        const table = $(this).closest('table')
        table.find('[role=hours]').each((i, input) => input.value = 0)
        table.find('[role=assigned]').trigger('change')
    }

    $.fn.disciplinePlanCopy = function() {
        let hours = $(this).closest('table')
                           .find('[role=hours]')
                           .toArray()
                           .map(input => Number(input.value))

        $(this).closest('.tab-content')
               .children(':not(:visible)')
               .each(function() {
                   $(this).find('[role=hours]').toArray()
                          .forEach((input, i) => input.value = hours[i])
               })
               .find('[role=assigned]').trigger('change')
    }

    $.fn.disciplinePlanFill = function(order = 'asc') {
        const tr = $(this).closest('tr')
        const table = $(this).closest('table')
        const assigned = table.find('[role=assigned]')
        const total = table.find('[role=total]')

        tr.find('[role=hours]').val(0)
        assigned.trigger('change')

        let hours = Number(total.text()) - Number(assigned.text())

        if (hours <= 0) {
            return;
        }

        let inputs = tr.find('[role=hours]').toArray()
        if (order == 'desc') {
            inputs = inputs.reverse()
        }
        
        do {
            for (let i = 0; i < inputs.length; i++, hours--) {
                if (hours == 0) {
                    return assigned.trigger('change');
                }
                const input = inputs[i];
                input.value = Number(input.value) + 1
            }
        } while (hours > 0);
        assigned.trigger('change')
    }

}(jQuery));

(function() {
    $.fn.search = function(list) {
        let search = $(this).val()

        $(list).find('[filter]').each(function() {
            if ($(this).attr('filter').toLowerCase().includes(search.toLowerCase()))
                $(this).removeClass('hidden')
            else
                $(this).addClass('hidden')
        })
    }
}(jQuery));