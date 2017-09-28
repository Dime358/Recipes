$("#btnfavorite").on('click', function (e) {

    e.preventDefault();
    

    $.post("/Posts/manageFavorites?postId=" + postIDReport, function (data) {
        if (data.success) {
            debugger;
            if (data.status == "add") {
                $("#btnfavorite").removeClass(data.remove);
                $("#btnfavorite").addClass(data.add);

                $("#favText").removeClass("glyphicon-star-empty");
                $("#favText").addClass("glyphicon-star");

            }
            else
            {
                $("#btnfavorite").removeClass(data.remove);
                $("#btnfavorite").addClass(data.add);

                $("#favText").removeClass("glyphicon-star");
                $("#favText").addClass("glyphicon-star-empty");
            }

        }
    });

    });