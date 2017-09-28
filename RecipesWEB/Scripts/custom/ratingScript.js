$(function () {

    $("#increase").on('click', function (e) {
        
        e.preventDefault();
        var form = $("#increaseFrom");
        var oldRatings = parseInt($("#ratingId").text());

        if (form.valid()) {
            
            $.post("/Ratings/changeRating?state=1", form.serialize(), function (data) {
                if (data.success) {
                    debugger;
                    e.preventDefault();
                    oldRatings = oldRatings + 2;
                    $("#ratingId").text(data.state);

                    $("#increase").removeClass("btnBledo");
                    $("#increase").addClass("btnApproveColor");

                    $("#decrease").removeClass("btnRemoveColor");
                    $("#decrease").addClass("btnBledo");

                }
            });
        }
    });


    $("#decrease").on('click', function (e) {
       
        e.preventDefault();
        var form = $("#decreaseFrom");
        var oldRatings = parseInt($("#ratingId").text());

        if (form.valid()) {
            $.post("/Ratings/changeRating?state=2", form.serialize(), function (data) {
                if (data.success) {
                    debugger;
                    e.preventDefault();
                    oldRatings = oldRatings - 2;
                    $("#ratingId").text(data.state);
                    
                    $("#decrease").removeClass("btnBledo");
                    $("#decrease").addClass("btnRemoveColor");

                    $("#increase").removeClass("btnApproveColor");
                    $("#increase").addClass("btnBledo");

                }
            });
            
        }
    });

});