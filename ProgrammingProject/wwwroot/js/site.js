// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function filterInput(inputFieldID, listID) {
    /* Filtering adapted from W3 Schools tutorial:
     * How to - filter/search list (no date) How To Create a Filter/Search List. Refsnes Data. 
     * Available at: https://www.w3schools.com/howto/howto_js_filter_lists.asp 
     * (Accessed: April 30, 2023). 
     */
    var $input = $("#".concat(inputFieldID));
    var $filter = $input.val().toString().toUpperCase();
    var $ul = $("#".concat(listID))[0];
    var $li = $("#".concat(listID).concat(">li"));
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

function filterFocusOut(listID) {
    $("#".concat(listID))[0].style.display = "none";
}

function suburbClick(event, element) {
    event.preventDefault(); // this should be tested on IE / Firefox
    $("#SuburbName").val($(element).attr("data-name"));
    $("#Postcode").val($(element).attr("data-postcode"));
    $("#suburbsList")[0].style.display = "none";
}

function breedClick(event, element) {
    event.preventDefault(); // again, test on IE / Firefox
    $("#Breed").val($(element).attr("data-name"));
    $("#breedsList")[0].style.display = "none";
}

function radioClick(el) {
    var outlineClass = "bi-star";
    var fillClass = "bi-star-fill";

    var $labels = $(".rating-label");
    var selectedVal = el.value;
    $labels.each(function () {
        this.classList.remove("checked");
        $(this).children()[0].classList.remove(fillClass);
        $(this).children()[0].classList.add(outlineClass);
    })
    $labels.each(function (index) {
        if (index < selectedVal) {
            this.classList.add("checked");
            $(this).children()[0].classList.remove(outlineClass);
            $(this).children()[0].classList.add(fillClass);
        }
    })
    //var $label = $(el).parent();
    //$label.toggleClass("checked")
}

function radioFocus(el) {
    var $label = $(el).parent();
    $label.addClass("focus");
}

function radioFocusOut(el) {
    var $label = $(el).parent();
    $label.removeClass("focus")
}

function clickRate(name, did) {
    var $name = $("#DogName");
    $name.html(name);
    var $dogID = $("#DogID");
    $dogID.val(did);
}
