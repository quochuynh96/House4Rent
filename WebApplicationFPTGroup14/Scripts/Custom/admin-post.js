﻿var postId = 0;
var accountID = "";
$(document).ready(function () {
    $('.post-row').click(function () {
        postId = $(this).attr('id');
        accountID = $(this).attr("accID");

        $('#btnApproval').attr({ 'post-id': postId });
        $('#btnDissApproval').attr({ 'post-id': postId });
        $('#btnDelete').attr({ 'post-id': postId });

        $('#titleModal').html($('#title' + postId).html());
        $('#postDayModal').html($('#postDay' + postId).html());
        $('#addressModal').html($('#address' + postId).html());
        $('#typeModal').html($('#type' + postId).html());
        $('#acreageModal').html($('#acreage' + postId).html() + ' m<sup>2</sup>');
        $('#priceModal').html(formatter.format($('#price' + postId).html()).substring(0) + ' (VND)');
        $('#maxPeopleModal').html($('#maxPeople' + postId).html() + ' người');
        $('#nameModal').html($('#name' + postId).html());
        $('#descriptionModal').html($('#description' + postId).html());
        $('#phoneModal').html($('#phone' + postId).html());
        $('#emailModal').html($('#email' + postId).html());

        var extension = $('#innerBathRoom' + postId).html() === 'True' ? 'Vệ sinh trong' : '';
        extension = $('#parking' + postId).html() === 'True' ? extension + ', có bãi đỗ xe' : extension;
        $('#extensionModal').html(extension);

        var status = $('#status' + postId).attr('status');
        if (status === '1') {
            $('#btnApproval').css({ 'display': 'none' });
            $('#btnDissApproval').css({ 'display':'block' });
        }
        else {
            $('#btnApproval').css({ 'display': 'block' });
            $('#btnDissApproval').css({ 'display': 'none' });
        }

        var imageList = $('#image' + postId).text().split(';');
        $('.carousel-indicators').empty();
        $('.carousel-inner').empty();
        for (var i = 0; i <= imageList.length - 2; i++) {
            var carouselIndi = '';

            var carouselItem = document.createElement('div');

            var image = document.createElement('img');
            $(image).addClass('w-100');
            var url = '../Content/Images/Houserent/' + imageList[i];
            $(image).attr({ 'src': url.trim() });
            $(carouselItem).addClass('carousel-item');
            $(carouselItem).append(image);

            if (i === 0) {
                carouselItem.classList.add('active');
                carouselIndi = '<li data-target="#carouselId" data-slide-to="' + i + '" class="active"></li>';
            }
            else {
                carouselIndi = '<li data-target="#carouselId" data-slide-to="' + i + '"></li>';
            }
            $('.carousel-indicators').append(carouselIndi);
            $('.carousel-inner').append(carouselItem);
        }
    });
});

$('#btnApproval').click(function () {
    //var  prompt('Phê duyệt bài viết ?');
    var postId = $('#btnApproval').attr('post-id');
    $.ajax({
        async: true,
        type: 'POST',
        url: '/Admin/ApprovalPost',
        dataType: 'json',
        data: { postId: postId },
        success: function(response) {
            if (response.result == true) {
                alert('Đã duyệt !!!');
                $('#status' + postId).attr({ 'status': '1' });
                $('#status' + postId).removeClass('text-danger');
                $('#status' + postId).addClass('text-success');
                $('#status' + postId).html('Đã phê duyệt');
                $('#closeModalPostInfo').trigger('click');
            }
        }
    });
});

$('#btnDissApproval').click(function () {
    $.ajax({
        async: true,
        type: 'POST',
        url: '/Admin/DissApprovalPost',
        dataType: 'json',
        data: { postId: postId },
        success: function (response) {
            if (response.result == true) {
                alert('Đã bỏ phê duyệt !!!');
                $('#status' + postId).attr({ 'status': '0' });
                $('#status' + postId).removeClass('text-success');
                $('#status' + postId).addClass('text-danger');
                $('#status' + postId).html('Chưa phê duyệt');
                $('#closeModalPostInfo').trigger('click');
            }
        }
    });
});

$('#btnDelete').click(function () {
    $('#feedbackModalTitle').html('Xác nhận xóa bài viết "' + $('#title' + postId).html() + '"');
    $('#feedback').attr({ 'placeholder': 'Gửi phản hồi đến ' + $('#name' + postId).html() });
});

$('#btnSubmitDelete').click(function () {
    $('#btnCancelSendMessage').trigger('click');
    var message = $('#feedback').val();
    $.ajax({
        async: true,
        type: 'POST',
        url: '/Admin/FeedbackForUser',
        dataType: 'json',
        data: {
            accountID: accountID,
            message: message,
            postId: postId
        },
        success: function (response) {
            if (response.result == true) {
                alert("Đã gửi phản hồi đến chủ trọ");
                $('#' + postId).remove();
                $('#closeModalPostInfo').trigger('click');
            }
        }
    });
});

var formatter = new Intl.NumberFormat('vn-VN', {
    style: 'currency',
    currency: 'VND',
    minimumFractionDigits: 0
});