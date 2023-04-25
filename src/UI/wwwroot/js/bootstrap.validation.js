/// <reference path="../../node_modules/jquery/dist/jquery.js" />

ExecutableValidationRefresh.prototype.internalExecute = function () {
    'use strict'
    
    const inputErrorClass = 'is-invalid';
    const messageErrorClass = 'invalid-feedback';
    const messageValidClass = 'valid-feedback';
    const attrSpan = 'data-valmsg-for';
    const result = ExecutableHelper.IsNullOrEmpty(this.result) ? [] : this.result;

    $(`[${attrSpan}]`, this.target).removeClass(messageErrorClass).addClass(messageValidClass)

    let isWasRefresh = false;
    for (let i = 0; i < result.length; i++) {
        let item = result[i];
        if (!item.hasOwnProperty('isValid'))
            item.isValid = false;

        if (!item.hasOwnProperty('name') || !item.hasOwnProperty('errorMessage')) {
            break;
        }
        if (!ExecutableHelper.IsNullOrEmpty(this.jsonData.prefix)) {
            item.name = "{0}.{1}".f(this.jsonData.prefix, item.name);
        }

        let input = $('[name]', this.target).filter(function () {
            return $(this).attr('name').toLowerCase() == item.name.toString().toLowerCase();
        });
        let span = $('[{0}]'.f(attrSpan), this.target).filter(function () {
            return $(this).attr(attrSpan).toLowerCase() == item.name.toString().toLowerCase();
        });

        if (ExecutableHelper.ToBool(item.isValid)) {
            $(input).removeClass(inputErrorClass);
            $(span).removeClass(messageErrorClass)
                .addClass(messageValidClass)
                .empty();
        }
        else {
            let errorMessage = $('<span/>')
                                .attr({ for: item.name, generated: true })
                                .html(item.errorMessage)

            let error = $('<div/>')
                .addClass(messageErrorClass)
                .attr(attrSpan, item.name)
                .html(errorMessage)

            $(input).addClass(inputErrorClass);
            $(span).replaceWith(error)
        }
        isWasRefresh = true;
    }

    if (!isWasRefresh) {
        this.target.find('.' + inputErrorClass).removeClass(inputErrorClass);
        $('[{0}]'.f(attrSpan), this.target).removeClass(messageErrorClass)
            .addClass(messageValidClass)
            .empty();
        this.target.find('.' + messageErrorClass).addClass(messageValidClass).removeClass(messageErrorClass).empty();
    }

    (this.target.is('form') ? this.target : $('form', this.target))
        .validate()
        .focusInvalid();
};
