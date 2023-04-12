/// <reference path="../../node_modules/jquery/dist/jquery.js" />

function suggestInputValueByNamingCase(value, to) {
    if (!value) {
        return;
    }
    const val = value.split(' ')
        .filter(s => s && s.length > 1)
        .map(s => s[0].toUpperCase())
        .join('')

    $(`[name="${to}"]`).val(val)
}