$("#createNews").submit(function () {
    debugger;

    if ($("#createNews").valid()) {

        var formData = new FormData($(this)[0]);
       

        $.ajax({
            url: "/News/Create",
            type: 'POST',
            data: formData,
            async: false,
            success: function (data) {
                if (data.success) {
                    $("#modalBody").html(data.body);
                    $('#myModal').modal('show');
                }
            },
            error: function (error) {
                $("#modalBody").html("greska");
                $('#myModal').modal('show');
            },
            cache: false,
            contentType: false,
            processData: false
        });
        return false;
    }
    

   
});