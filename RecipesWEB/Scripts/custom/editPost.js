$("form#editPost").submit(function () {
    debugger;

    if ($("#editPost").valid()) {

        var formData = new FormData($(this)[0]);

        $.ajax({
            url: "/Posts/Edit",
            type: 'POST',
            data: formData,
            async: false,
            success: function (data) {
                if (data.success) {
                    debugger;
                    if(data.admin)
                    {
                        var htmlBody = '<div class="btn-group btn-group-justified">' +
                         '<a class="btn btn-sm btn-primary" href="/Posts/Details?id=' + data.postID + '">Назад кон рецептот</a>' +
                         '<a class="btn btn-sm btn-primary" href="/Posts/Index">Листа на рецепти</a>' + 
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
                $("#modalBody").html("грешка во промена на рецептот.Обидете се повторно.");
                $('#myModal').modal('show');
            },
            cache: false,
            contentType: false,
            processData: false
        });

        return false;
    }
});