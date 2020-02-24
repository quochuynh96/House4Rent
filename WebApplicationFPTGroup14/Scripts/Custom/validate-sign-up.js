var accountId = $('#accountId'),
    password = $('#password'),
    rePassword = $('#rePassword'),
    userName = $('#userName'),
    birthDate = $('#birthDate'),
    phone = $('#phone'),
    email = $('#email'),
    sex = $('#sex'),
    role = $('#role'),
    city = $('#city'),
    district = $('#district'),
    submitted = false;
    
function validateData() {
    var validAccountId = validateAccountId();
    var validPassword = validatePassword();
    var validRePassword = validateRePassword();
    var validName = validateName();
    var validBirthDate = validateBirthDate();
    var validPhone = validatePhone();
    var validEmail = validateEmail();
    var validSex = validateSex();
    var validRole = validateRole();
    var validCity = validateCity();
    var validDistrict = validateDistrict();
    var result = validAccountId && validPassword && validRePassword && validName && validBirthDate &&
         validPhone && validEmail && validSex && validRole && validCity && validDistrict;

    if (result) submitted = true;
    return result;
}

function showMessage(selector) {
    var root = selector.closest('div');
    root.find('span').addClass('visible');
    selector.css({ "border-color": "#ef1b48" });
}

function hideMessage(selector) {
    var root = selector.closest('div');
    root.find('span').removeClass('visible');
    selector.css({ "border-color": "#ddd" });
}

var validAccountId = true;
function validateAccountId() {
    if (accountId.val().length <= 0) {
        $('#accountIdErrorMessage').html('<i class="material-icons">error</i> Phải nhập tên tài khoản');
        showMessage(accountId);
        
        return false;
    } else {
        $.ajax({
            async: false,
            type: "POST",
            url: "/Account/CheckExist",
            data: { accountId: accountId.val() },
            dataType: "json",
            success: function (response) {
                validAccountId = response.result === true ? false : true;
            }
        });
        if (!validAccountId) {
            showMessage(accountId);
            $('#accountIdErrorMessage').html('<i class="material-icons">error</i> Tên tài khoản đã tồn tại');
            
            return false;
        }
    }
    hideMessage(accountId);
    
    return true;
}
accountId.keyup(function () { validateAccountId(); });

function validatePassword() {
    if (password.val().length <= 0) {
        showMessage(password);
        
        return false;
    }
    hideMessage(password);
    
    return true;
}
password.focusout(function () { validatePassword(); });

function validateRePassword() {
    if (rePassword.val().length <= 0) {
        showMessage(rePassword);
        
        return false;
    } else if (rePassword.val() !== password.val()) {
        showMessage(rePassword);
        
        return false;
    }
    hideMessage(rePassword);
    
    return true;
}
rePassword.focusout(function () { validateRePassword(); });

function validateName() {
    if (userName.val().length <= 0) {
        showMessage(userName);
        
        return false;
    }
    hideMessage(userName);
    
    return true;
}
userName.focusout(function () { validateName(); });

function validateBirthDate() {
    if (birthDate.val().length <= 0) {
        showMessage(birthDate);
        
        return false;
    }
    hideMessage(birthDate);
    
    return true;
}
birthDate.focusout(function () { validateBirthDate(); });

function validatePhone() {
    if (phone.val().length === 0) {
        showMessage(phone);
        
        return false;
    } else if (phone.val().length !== 10) {
        $('#phoneErrorMessage').html('<i class="material-icons">error</i>Số bạn nhập không phải số điện thoại');
        showMessage(phone);
        
        return false;
    }
    hideMessage(phone);
    
    return true;
}
phone.focusout(function () { validatePhone(); });

function validateEmail() {
    if (email.val().length <= 0) {
        showMessage(email);
        
        return false;
    }
    hideMessage(email);
    
    return true;
}
email.focusout(function () { validateEmail(); });

function validateSex() {
    if (sex.val() === '-1') {
        showMessage(sex);
        
        return false;
    }
    hideMessage(sex);
    
    return true;
}
sex.focusout(function () { validateSex(); });

function validateRole() {
    if (role.val() === '-1') {
        showMessage(role);
        
        return false;
    }
    hideMessage(role);
    
    return true;
}
role.focusout(function () { validateRole(); });

function validateCity() {
    if(city.val() === null){
        showMessage(city);
        return false;
    }
    hideMessage(city);
    
    return true;
}
city.focusout(function () { validateCity(); });

function validateDistrict(){
    if(city.val()=== null){
        showMessage(district);
        return false;
    }
    hideMessage(district);
    
    return true;
}
district.focusout(function () { validateDistrict(); });