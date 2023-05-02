// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function filterSuburbs() {
    /* Filtering adapted from W3 Schools tutorial:
     * How to - filter/search list (no date) How To Create a Filter/Search List. Refsnes Data. 
     * Available at: https://www.w3schools.com/howto/howto_js_filter_lists.asp 
     * (Accessed: April 30, 2023). 
     */
    var $input = $("#SuburbName");
    var $filter = $input.val().toString().toUpperCase();
    var $ul = $("#suburbsList")[0];
    var $li = $("#suburbsList>li")
    var visibleCount = 0;

    var i;
    for (i = 0; i < $li.length; i++) {
        var $button = $($li[i].getElementsByTagName("button")[0]);
        var $buttonName = $button.attr("data-name");

        if ($buttonName.toUpperCase().indexOf($filter) > -1) {
            $li[i].style.display = "";
            visibleCount++;
        } else {
            $li[i].style.display = "none";
        }
    }

    if (visibleCount > 0) {
        $ul.style.display = "";
    }
}

function filterFocusOut() {
    $("#suburbsList")[0].style.display = "none";
}

function suburbClick(event, element) {
    event.preventDefault(); // this should be tested on IE / Firefox
    $("#SuburbName").val($(element).attr("data-name"));
    $("#Postcode").val($(element).attr("data-postcode"));
    $("#suburbsList")[0].style.display = "none";
}
