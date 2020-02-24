$(document).ready(function () {
    var accountId = 0;
    $('.account-row').click(function () {
        accountId = $(this).attr('id');
        $('#btnBlockAccount').attr({ 'account-id': accountId });
        $('#btnUnlockAccount').attr({ 'account-id': accountId });
        $('#btnApprovalAccount').attr({ 'account-id': accountId });
        $('#btnDeleteAccount').attr({ 'account-id': accountId });
        

        $('#accountIDModal').html($('#AccountID' + accountId).html());
        $('#nameAccountModal').html($('#Name' + accountId).html());
        $('#sexAccountModal').html($('#Sex' + accountId).html());
        $('#birthdayAccountModal').html($('#Birthday' + accountId).html());
        $('#phoneAccountModal').html($('#Phone' + accountId).html());
        $('#emailAccountModal').html($('#Mail' + accountId).html());
        $('#cityAccountModal').html($('#CityName' + accountId).html());
        $('#townshipAccountModal').html($('#TownshipName' + accountId).html());

        var status = $('#accountStatus' + accountId).attr('statusID');
        if (status === '1') {
            $('#btnApprovalAccount').css({ 'display': 'none' });
            $('#btnBlockAccount').css({ 'display': 'block' });
            $('#btnDissApprovalAccount').css({ 'display': 'none' });
            $('#btnUnlockAccount').css({ 'display': 'none' });
        }
        else if (status === '2') {
            $('#btnApprovalAccount').css({ 'display': 'block' });
            $('#btnBlockAccount').css({ 'display': 'none' });
            $('#btnDissApprovalAccount').css({ 'display': 'none' });
            $('#btnUnlockAccount').css({ 'display': 'none' });
        } else if (status === '3') {
            $('#btnBlockAccount').css({ 'display': 'none' });
            $('#btnApprovalAccount').css({ 'display': 'none' });
            $('#btnUnlockAccount').css({ 'display': 'block' });
        }
    });
    $('.account-row-approval').click(function () {
        accountId = $(this).attr('id');
        $('#btnBlockAccount').attr({ 'account-id': accountId });
        $('#btnUnlockAccount').attr({ 'account-id': accountId });
        $('#btnApprovalAccount').attr({ 'account-id': accountId });
        $('#btnDeleteAccount').attr({ 'account-id': accountId });

        $('#accountIDModal').html($('#AccountID' + accountId).html());
        $('#nameAccountModal').html($('#Name' + accountId).html());
        $('#sexAccountModal').html($('#Sex' + accountId).html());
        $('#birthdayAccountModal').html($('#Birthday' + accountId).html());
        $('#phoneAccountModal').html($('#Phone' + accountId).html());
        $('#emailAccountModal').html($('#Mail' + accountId).html());
        $('#cityAccountModal').html($('#CityName' + accountId).html());
        $('#townshipAccountModal').html($('#TownshipName' + accountId).html());
        $('#btnApprovalAccount').css({ 'display': 'block' });
        $('#btnBlockAccount').css({ 'display': 'none' });
        $('#btnDissApprovalAccount').css({ 'display': 'none' });
        $('#btnUnlockAccount').css({ 'display': 'none' });
    });


    $("#btnUnlockAccount").click(function () {
        var accID = $("#btnUnlockAccount").attr("account-id");
        $.ajax({
            async: true,
            type: 'POST',
            url: '/Admin/UnBlockAccount',
            dataType: 'json',
            data: { accountID: accID },
            success: function (response) {
                if (response.result == true) {
                    alert('Đã Mở Khoá !!!');
                    $('#accountStatus' + accID).attr({ 'statusid': '1' });
                    $('#accountStatus' + accID).removeClass('text-danger');
                    $('#accountStatus' + accID).addClass('text-success');
                    $('#accountStatus' + accID).html('Đã phê duyệt');
                    $('#closeModalAccountInfo').trigger('click');
                }
            }
        });
    });

    $("#btnApprovalAccount").click(function () {
        var accID = $("#btnApprovalAccount").attr("account-id");
        $.ajax({
            async: true,
            type: 'POST',
            url: '/Admin/ApprovalAccount',
            dataType: 'json',
            data: { accountID: accID },
            success: function (response) {
                if (response.result == true) {
                    alert('Đã Phê Duyệt !!!');
                    $('#accountStatus' + accID).attr({ 'statusid': '1' });
                    $('#accountStatus' + accID).removeClass('text-danger');
                    $('#accountStatus' + accID).addClass('text-success');
                    $('#accountStatus' + accID).html('Đã phê duyệt');
                    $('#closeModalAccountInfo').trigger('click');
                }
            }
        });
    });

    $("#btnBlockAccount").click(function () {
        var accID = $("#btnBlockAccount").attr("account-id");
        $.ajax({
            async: true,
            type: 'POST',
            url: '/Admin/BlockAccount',
            dataType: 'json',
            data: { accountID: accID },
            success: function (response) {
                if (response.result == true) {
                    alert('Đã Khoá !!!');
                    $('#accountStatus' + accID).attr({ 'statusid': '3' });
                    $('#accountStatus' + accID).removeClass('text-danger');
                    $('#accountStatus' + accID).addClass('text-danger');
                    $('#accountStatus' + accID).html('Bị khoá');
                    $('#closeModalAccountInfo').trigger('click');
                }
            }
        });
    });
});
