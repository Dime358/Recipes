$("#btnCreatePost").on('click', function (e) {
    e.preventDefault();

    if ($("#createPost").valid()) {

        var formData = new FormData($("#createPost")[0]);

        $.ajax({
            url: "/Posts/Create",
            type: 'POST',
            data: formData,
            async: false,
            success: function (data) {
                if (data.success) {
                    $("#modalBody").html(data.body);
                    $('#myModal').modal('show');
                }
                else {
                    $("#modalBody").html(data.body);
                    $('#myModal').modal('show');
                }
            },
            error: function (error) {
                $("#modalBody").html("грешка во креирање на рецептот.Обидете се повторно.");
                $('#myModal').modal('show');
            },
            cache: false,
            contentType: false,
            processData: false
        });

        return false;
    }
});