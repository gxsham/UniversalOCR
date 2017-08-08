$(document).ready(function () {

    // get the name of uploaded file
    $('input[type="file"]').change(function () {
        var value = $("input[type='file']").val();
        $('.js-value').text(value);
    });

});
