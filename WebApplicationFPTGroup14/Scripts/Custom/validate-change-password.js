var accountID = "";
var oldPassword = "";
var newPassword = "";
var newPasswordConfirm = "";

function validateOldPassword() {
    var flag = true;
    if (oldPassword.length <= 0) {
        $("#error-old-password").html('<i class="material-icons">error</i>Vui lòng nhập mật khẩu cũ');
        $("#oldPassword").focus();
    } else {
        $.ajax({
            type: "POST",
            url: "/Account/CheckOldPassword",
            data: {
                accountID: accountID,
                oldPassword: oldPassword
            },
            dataType: "Json",
            success: function (response) {
                if (response.result == false) {
                    flag = false;
                    $("#error-old-password").html('<i class="material-icons">error</i>Mật khẩu cũ không đúng');
                    $("#oldPassword").focus();
                }
            }
        });
    }
    return flag;
}

function validateNewPassword(pass) {
    var resultValidNewPassword = false;
    if (pass.length == 0) {
        $("#error-new-password").html('<i class="material-icons">error</i>Vui lòng nhập mật khẩu mới');
        $('#newPassword').focus();
    } else {
        resultValidNewPassword = true;
    }
    return resultValidNewPassword;
}

function validateNewPasswordConfirm(pass,passConfirm) {
    var resultValidNewPasswordConfirm = false;
    if (passConfirm.length == 0) {
        $("#error-new-password-confirm").html('<i class="material-icons">error</i>Vui lòng nhập mật xác nhận');
        $('#newPasswordConfirm').focus();
    }
    else {
        if (pass === passConfirm) {
            resultValidNewPasswordConfirm = true;
        } else {
            $("#error-new-password-confirm").html('<i class="material-icons">error</i>Mật khẩu xác nhận không trùng khớp');
            $('#newPasswordConfirm').focus();
        }
    }
    return resultValidNewPasswordConfirm;
}

function validateChangePass() {
    accountID = $('#accountID').val();
    oldPassword = $('#oldPassword').val();
    newPassword = $('#newPassword').val();
    newPasswordConfirm = $('#newPasswordConfirm').val();

    var validOldPass = validateOldPassword();
    var validNewPass = validateNewPassword(newPassword);
    var validnewPassConfirm = validateNewPasswordConfirm(newPassword, newPasswordConfirm);
    var resultValid = validOldPass && validNewPass && validnewPassConfirm;
    return resultValid;
}

