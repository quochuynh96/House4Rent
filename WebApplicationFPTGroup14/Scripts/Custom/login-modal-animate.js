$(document).ready(function () {
    if ($("#userInput").val() !== "") {
        userInputFocus();
    }

    function userInputFocus() {
        $("#userInputLabel").css({
            'fontSize': '14px',
            'top': '-30px',
            'fontStyle': 'italic'
        }, 300);
        $(".userInputLine").animate({
            width: '50%'
        }, 300);
    }

    if ($("#passwordInput").val() !== "") {
        passwordInputFocus();
    }

    function passwordInputFocus() {
        $("#passwordInputLabel").css({
            'fontSize': '14px',
            'top': '-30px',
            'fontStyle': 'italic'
        }, 300);
        $(".passwordInputLine").animate({
            width: '50%'
        }, 300);
    }

    // Animate for user input label
    var isUserInputFocus = true;
    $("#userInput").focus(function (e) {
        if (isUserInputFocus) {
            userInputFocus();
            isUserInputFocus = false;
        }
    });
    $("#userInput").focusout(function (e) {
        if ((!isUserInputFocus) && ($("#userInput").val() === "")) {
            $("#userInputLabel").css({
                'fontSize': '1rem',
                'top': '-5px',
                'fontStyle': 'normal'
            }, 300);
            $(".userInputLine").animate({
                width: '0px'
            }, 300);
            isUserInputFocus = true;
        }
    });
    // Animate for password input label
    var isPasswordInputFocus = true;
    $("#passwordInput").focus(function (e) {
        if (isPasswordInputFocus) {
            passwordInputFocus();
            isPasswordInputFocus = false;
        }
    });
    $("#passwordInput").focusout(function (e) {
        if (!isPasswordInputFocus && ($("#passwordInput").val() === "")) {
            $("#passwordInputLabel").css({
                'fontSize': '1rem',
                'top': '-5px',
                'fontStyle': 'normal'
            }, 400);
            $(".passwordInputLine").animate({
                width: '0px'
            }, 300);
            isPasswordInputFocus = true;
        }
    });
});