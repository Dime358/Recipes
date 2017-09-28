$("form#editNews").submit(function () {
    debugger;

    if ($("#editNews").valid()) {

        var formData = new FormData($(this)[0]);

        $.ajax({
            url: "/News/Edit",
            type: 'POST',
            data: formData,
            async: false,
            success: function (data) {
                if (data.success) {

                    if (data.admin) {
                        var htmlBody = '<div class="btn-group btn-group-justified">' +
                         '<a class="btn btn-sm btn-primary" href="/News/Details?id=' + data.newsID + '">Назад кон советот</a>' +
                         '<a class="btn btn-sm btn-primary" href="/News/Index">Листа на совети</a>' +
                         '<a class="btn btn-sm btn-primary" href="/Posts/approveIndex">Админ страна</a> </div>'
                        $("#modalButtons").html(htmlBody);
                    }
                    $("#modalBody").html(data.body);
                    $('#myModal').modal('show');
                }
                else {
                    $("#modalBody").html(data.body);
                    $('#myModal').modal('show');
                }
            },
            error: function (error) {
                $("#modalBody").html("грешка во промена на веста.Обидете се повторно.");
                $('#myModal').modal('show');
            },
            cache: false,
            contentType: false,
            processData: false
        });

        return false;
    }
});