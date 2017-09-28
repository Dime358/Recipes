$(function () {

    //$(".removeComment").on('click', function (e) {
        
    //    debugger;

    //    e.preventDefault();
    //    var commentId = $(this).attr("value");
    //    $('#commentRemoveId').val(commentId);
    //    $('#btnGrpComment').show();
    //    $('#myModalComment').modal('show');
        
    //    });

    $("#removeCommentConfirm").on('click', function (e) {
        
        e.preventDefault();
        var commentId = $("#commentRemoveId").val();

        $.post("/Comments/DeleteConfirmed", { id: commentId, postId: postIDReport }, function (data) {

            if (data.success) {
                window.location.href = "/Posts/Details?id=" + postIDReport;
            }
            else {
                $("#modalBodyComment").html("Грешка при бришењето.Ве молиме обидете се повторно.");
                $('#btnGrpComment').hide();
                $('#myModalComment').modal('show');
            }
        });



    });
});