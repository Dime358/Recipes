$(function () {

    $("#btnSetAsSeen").on('click', function (e) {

        e.preventDefault();

        var checkedSeen = $("#notificationsRow input:checkbox:checked").map(function () {
            return $(this).val();
        }).get();
        debugger;
        if (checkedSeen.length > 0) {
            $.post("/Partial/markAsSeen", { checkData: checkedSeen }, function (data) {
                window.location.href = data.Url
            });
        }
    });

    $("#btnDeleteSelected").on('click', function (e) {

        e.preventDefault();

        var checkedDeleted = $("#notificationsRow input:checkbox:checked").map(function () {
            return $(this).val();
        }).get();
        debugger;
        if (checkedDeleted.length > 0) {
            $.post("/Partial/deleteNotifications", { checkData: checkedDeleted }, function (data) {
                window.location.href = data.Url
            });
        }
    });

    $("#btnCheck").on('click', function (e) {

        e.preventDefault();

        if (checkedState) {
            $(".notificationCheckbox").prop('checked', false)
            checkedState = false;
            $("#selectbtn").html("селектирај се");
        }
        else {
            $(".notificationCheckbox").prop('checked', true)
            checkedState = true;
            $("#selectbtn").html("деселектирај се");
        }

    });


});