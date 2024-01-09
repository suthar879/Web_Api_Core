// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function DisplayLoader() {
    $('.loading').show();
}

function HideLoader() {
    $('.loading').hide();
}

$(window).on('beforeunload', function () {
    DisplayLoader();
});

$(document).on('submit', 'form', function () {
    DisplayLoader();
});

window.setTimeout(function () {
    HideLoader();
}, 2000);