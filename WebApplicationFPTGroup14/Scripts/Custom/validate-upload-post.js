var price = $('#price'),
    postTitle = $('#postTitle'),
    city = $('#city'),
    district = $('#district'),
    type = $('#type'),
    acreage = $('#acreage'),
    maxPeople = $('#maxPeople'),
    addressRoom = $('#addressRoom'),
    description = $('#description'),
    addressDetail = $('#addressDetail'),
    imageList = $('#imageList');

var formatter = new Intl.NumberFormat('vn-VN', {
    style: 'currency',
    currency: 'VND',
    minimumFractionDigits: 0
});

function currencyToNumber(string) {
    var arr = string.split('.');
    var number = '';
    var index = 0;
    for (index = 0; index < arr.length; index++) {
        number = number + arr[index];
    }
    return number;
}

function showMessage(selector) {
    var root = selector.closest('div');
    root.find('span').addClass('visible');
    selector.css({ "border-color": "#ef1b48" });
}

function hideMessage(selector) {
    var root = selector.closest('div');
    root.find('span').removeClass('visible');
    selector.css({ "border-color": "" });
}

function validateDataUploadPost() {
    var validPostTile = validateTitle();
    var validPrice = validatePrice();
    var validMaxPeople = validateMaxPeople();
    var validCity = validateCity();
    var validDistrict = validateDistrict();
    var validType = validateType();
    var validAcreage = validateAcreage();
    var validAddressRoom = validateAddressRoom();
    var validAddressDetail = validateAddressDetail();
    var validImageList = validateImageList();
    var validDescription = validateDescription();

    var result = validPostTile && validPrice && validMaxPeople() && validCity && validDistrict &&
            validType && validAcreage && validAddressRoom && validAddressDetail && validImageList && validDescription;
    return result;
}

function validateTitle() {
    if (postTitle.val() === '') {
        showMessage(postTitle);
        return false;
    }
    hideMessage(postTitle);
    return true;
}
postTitle.focusout(function () { validateTitle(); });

function validatePrice() {
    if (price.val().length === 0) {
        showMessage(price);
        return false;
    } else if (currencyToNumber(price.val()) < 1000) {
        showMessage(price);
        $('#priceErrorMessage').html('<i class="material-icons">error</i> Giá thuê phải không nhỏ hơn 1,000 VNĐ.');
        return false;
    } else {
        var num = currencyToNumber(price.val());
        var currency = formatter.format(num);
        var priceVal = currency.substring(0, currency.length - 2);
        if (priceVal.includes('NaN')) {
            showMessage(price);
            $('#priceErrorMessage').html('<i class="material-icons">error</i> Giá thuê phải là số.');
            price.val('');
            return false;
        }
    }
    hideMessage(price);
    price.val(priceVal);
    return true;
}
price.focusout(function () { validatePrice(); });

function validateMaxPeople() {
    if (maxPeople.val().length <= 0) {
        showMessage(maxPeople);
        maxPeople.val('');
        return false;
    } else if (maxPeople.val() <= 0 || maxPeople.val().indexOf(".") > 0 || maxPeople.val().indexOf("e") > 0) {
        showMessage(maxPeople);
        $('#maxPeopleErrorMessage').html('<i class="material-icons">error</i> Số người ở tối đa phải là số nguyên dương.');
        maxPeople.val('');
        return false;
    }
    hideMessage(maxPeople);
    return true;
}
maxPeople.focusout(function () { validateMaxPeople(); });

function validateCity() {
    if (city.val() === '0') {
        showMessage(city);
        return false;
    }
    hideMessage(city);
    return true;
}
city.focusout(function () { validateCity(); });

function validateDistrict() {
    if (district.val() === '0') {
        showMessage(district);
        return false;
    }
    hideMessage(district);
    return true;
}
district.focusout(function () { validateDistrict(); });

function validateType() {
    if (type.val() !== '1' && type.val() !== '0') {
        showMessage(type);
        return false;
    }
    hideMessage(type);
    return true;
}
type.focusout(function () { validateType(); });

function validateAcreage() {
    if (acreage.val().length === 0) {
        showMessage(acreage);
        return false;
    } else if (acreage.val() < 5) {
        showMessage(acreage);
        $('#acreageErrorMessage').html('<i class="material-icons">error</i> Diện tích phải lớn hơn 5 m<sup>2</sup>.');
        return false;
    }
    hideMessage(acreage);
    return true;
}
acreage.focusout(function () { validateAcreage(); });

function validateAddressRoom() {
    if (addressRoom.val().length === 0) {
        showMessage(addressRoom);
        return false;
    }
    hideMessage(addressRoom);
    return true;
}
addressRoom.focusout(function () { validateAddressRoom(); });

function validateImageList() {
    if (imageList.val().length === 0) {
        showMessage(imageList);
        return false;
    }
    hideMessage(imageList);
    return true;
}
imageList.change(function () { validateImageList(); });
imageList.focusout(function () { validateImageList(); });

function validateAddressDetail() {
    if (addressDetail.val().length === 0) {
        showMessage(addressDetail);
        return false;
    }
    hideMessage(addressDetail);
    return true;
}
addressDetail.focusout(function () { validateAddressDetail(); });

function validateDescription() {
    if (description.val().length === 0) {
        showMessage(description);
        return false;
    }
    hideMessage(description);
    return true;
}
description.focusout(function () { validateDescription(); });

city.click(function (e) {
    var cID = city.val();
    //console.log(cID);
    $.ajax({
        type: "POST",
        url: "/Home/LoadTownship",
        data: { cityID: cID },
        dataType: "json",
        success: function (response) {
            //console.log(response.result);
            district.html(response.result);
        }
    });
});

city.bind('keyup', function (e) { city.trigger('click'); });