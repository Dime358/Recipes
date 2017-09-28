$(function () {

    $(".reportSpam").on('click', function (e) {
        e.preventDefault();
        debugger;
        
        var commentId = $(this).attr("value");

        $.post("/Comments/addReport", { id: commentId, postId: postIDReport, reason: "spam" }, function (data) {
            $('#modalReport').modal('show');
        });

    });

    $(".reportOffensive").on('click', function (e) {
        e.preventDefault();
        debugger;

        var commentId = $(this).attr("value");

        $.post("/Comments/addReport", { id: commentId, postId: postIDReport, reason: "offensive" }, function (data) {
            $('#modalReport').modal('show');
        });

    });

});