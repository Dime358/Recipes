$(function () {

    $("#btnSaveComment").on('click', function (e) {
        e.preventDefault();
        var form = $("#frmAddComment");
        var comment = $("#commentBody").val();

        if (form.valid()) {

            $.post("/Comments/Create", form.serialize(), function (data) {
                
                if (data.success) {
                    debugger;
                    var htmlCommentBody = '<li><div class="row" style="padding:15px"><div class="col-md-2">' +
                        '<img src="/Content/images/userImages/' + data.userImage + '" class="img-responsive" /></div><div class="col-md-10">' +
                        '<div class="panel panel-default"><div class="panel-heading"><span class="glyphicon glyphicon glyphicon-user" style="margin-right:3px"></span>' +
                        '<strong>' + data.userName +  '<span class="glyphicon glyphicon glyphicon glyphicon-calendar" style="margin-right:3px;margin-left:5px"></span>' +
                        '<span class="text-muted">' + data.createdDate + '</span>' +
                        ' <span class="glyphicon glyphicon glyphicon glyphicon-time" style="margin-right:3px;margin-left:3px"></span>' +
                        ' <span class="text-muted">' + data.createdTime + '</span>' +
                        '<a class="btn-link btn-sm removeComment pull-right" onclick="modalShow(' + data.commentID + ')">избриши</a>' +
                        '</div><div class="panel-body">' + comment + '</div></div></div></div></li>';
                   
                    $("#commentsList").prepend($(htmlCommentBody).fadeIn('slow'));
                    $("#commentBody").val("");
                    $("#commentsSection").show();
                }
                else {
                    $("#modalBody").html(data.body);
                    $('#myModal').modal('show');
                }
            });


        }
    });
});