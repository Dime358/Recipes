$(function () {
    //S na kraj za da ne se povikuva
    $("#btnCreatePosts").on('click', function (e) {
        debugger;
        e.preventDefault();
        //var form = $("#createPost");

        var form = $("#createPost"); // You need to use standard javascript object here
        var formData = new FormData(form);

        //var form = new FormData(document.getElementById('createPost'));
        formData.append('ImagePost', $('input[type=file]')[0].files[0]);


        $.ajax({
            url: '/Posts/Create/',
            type: "POST",
            data: form,
            enctype: 'multipart/form-data',
            mimeType: 'multipart/form-data',
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.success) {
                    $("#modalBody").html(data.body);
                    $('#myModal').modal('show');
                }
            },
            error: function (error) {
                $("#modalBody").html("greska");
                $('#myModal').modal('show');
            }
        });

    });
});

