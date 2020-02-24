function setCookie(cname, cvalue) {
    document.cookie = cname + "=" + cvalue + ";";
};

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
};

function checkCookie() {
    var university = getCookie("university");
    if (university != "") {
        $("#school").val(university);
        $("#city").val(getCookie("city"));
        window.setTimeout(function () { $("#city").click(); }, 100);
        window.setTimeout(function () { $("#district").val(getCookie("township")); }, 200);
    };
};

$(document).ready(function () {
    var university = "";
    LoadTownship();
    checkCookie();
    // Display value of slider
    $("#priceSlider").mousemove(function (e) {
        var price = $("#priceSlider").val();
        var format = formatter.format(price);
        $("#notiprice").html(format);
    });
    $("#areaSlider").mousemove(function (e) {
        var area = $("#areaSlider").val();
        $("#notiarea").html(area);
    });
    $("#priceSlider").change(function (e) {
        var price = $("#priceSlider").val();
        if (price > 3500000) {
            $('#house').prop('checked', true);
        }
        var format = formatter.format(price);
        $("#notiprice").html(format);
    });

    $("#areaSlider").change(function (e) {
        var area = $("#areaSlider").val();
        if (area > 100) {
            $('#house').prop('checked', true);
        }
        $("#notiarea").html(area);
    });

    $("#city").click(LoadTownship);
    $('#city').bind('keyup', function () { LoadTownship(); });

    function LoadTownship() {
        var cID = $("#city").val();
        if (cID !== null && cID != 0) {
            $.ajax({
                type: "POST",
                url: "/Home/LoadTownship",
                data: { cityID: cID },
                dataType: "json",
                success: function (response) {
                    //console.log(response.result);
                    $("#district").html(response.result);
                }
            });
        }
    }

    $(".user-type").click(function () {
        if ($(this).val() === $('#studentType').val()) {
            $('#sectionContentStudent').slideDown();
        } else {
            $('#sectionContentStudent').slideUp();
        }
    });

    $('.close').click(function () {
        $('.ask-modal').fadeOut();
    });

    $(document).click(function () {
        var newUnisersity = $("#school").val();
        if (newUnisersity === null) return;
        if (university != newUnisersity) {
            university = newUnisersity;
            $.ajax({
                type: "POST",
                url: "/Home/LoadCityByUniversity",
                data: { university: university },
                dataType: "json",
                success: function (response) {
                    if (response.city != -1) {
                        $("#city").val(response.city);
                        window.setTimeout(function () { $("#city").click(); }, 100);
                        window.setTimeout(function () { $("#district").val(response.township); }, 200);
                        setCookie("university", university);
                        setCookie("city", response.city);
                        setCookie("township", response.township);
                    }
                }
            });
        }
    });
});

$(document).ready(function () {
    var Open = false;
    $("#optionAdvance").click(function () {
        if (Open==false) {
            $("#priceSlider").prop("disabled", false);
            $("#areaSlider").prop("disabled", false);
            $("#innerBathRoom").prop("disabled", false);
            $("#hasPark").prop("disabled", false);
            $("#kitchenshelf").prop("disabled", false);
            $("#mezzanine").prop("disabled", false);
            $("#isSearchAdvance").prop("disabled", false);
            Open = true;
        } else if (Open==true) {
            $("#priceSlider").prop("disabled", true);
            $("#areaSlider").prop("disabled", true);
            $("#innerBathRoom").prop("disabled", true);
            $("#hasPark").prop("disabled", true);
            $("#kitchenshelf").prop("disabled", true);
            $("#mezzanine").prop("disabled", true);
            $("#isSearchAdvance").prop("disabled", true);
            Open = false;
        }
    });
});

var formatter = new Intl.NumberFormat('vn-VN', {
    style: 'currency',
    currency: 'VND',
    minimumFractionDigits: 0
});