//resCarousel

$(window).resize(function () {
    ResCarouselSize();
});

$(document).ready(function () {
    var itemsMainDiv = ('.resCarouselR');
    var itemsDiv = ('.resCarousel-innerR');
    var itemWidth = "";

    $('.leftLstR, .rightLstR').click(function () {
        debugger;
        var condition = $(this).hasClass("leftLstR");
        if (condition)
            click(0, this);
        else
            click(1, this)
    });

    ResCarouselSize();




    $(window).resize(function () {
        ResCarouselSize();
    });

    
    function ResCarouselSize() {
        var incno = 0;
        var dataItems = ("data-items");
        var itemClass = ('.itemR');
        var id = 0;
        var btnParentSb = '';
        var itemsSplit = '';
        var sampwidth = $(itemsMainDiv).width();
        var bodyWidth = $('body').width();
        $(itemsDiv).each(function () {
            id = id + 1;
            var itemNumbers = $(this).find(itemClass).length;
            btnParentSb = $(this).parent().attr(dataItems);
            itemsSplit = btnParentSb.split(',');
            $(this).parent().attr("id", "ResSlid" + id);


            if (bodyWidth >= 1200) {
                incno = itemsSplit[3];
                itemWidth = sampwidth / incno;
            }
            else if (bodyWidth >= 992) {
                incno = itemsSplit[2];
                itemWidth = sampwidth / incno;
            }
            else if (bodyWidth >= 768) {
                incno = itemsSplit[1];
                itemWidth = sampwidth / incno;
            }
            else {
                incno = itemsSplit[0];
                itemWidth = sampwidth / incno;
            }
            $(this).css({ 'transform': 'translateX(0px)', 'width': itemWidth * itemNumbers });
            $(this).find(itemClass).each(function () {
                $(this).outerWidth(itemWidth);
            });

            //$(".leftLstR").addClass("outtR");
            //$(".rightLstR").removeClass("outtR");

        });
    }


    
    function ResCarousel(e, el, s) {
        debugger;
        var leftBtn = ('.leftLstR');
        var rightBtn = ('.rightLstR');
        var translateXval = '';
        var divStyle = $(el + ' ' + itemsDiv).css('transform');
        var values = divStyle.match(/-?[\d\.]+/g);
        var xds = Math.abs(values[4]);
        var IW = $(el).find('.itemR').outerWidth();
        //alert(IW*s);

        if (e == 0) {
            translateXval = parseInt(xds) - parseInt(IW * s);
            //$(el + ' ' + rightBtn).removeClass("outtR");


            if (translateXval <= IW / 2) {
                translateXval = 0;
                //$(el + ' ' + leftBtn).addClass("outtR");
            }
        }
        else if (e == 1) {
            var itemsCondition = $(el).find(itemsDiv).width() - $(el).width();
            translateXval = parseInt(xds) + parseInt(IW * s);
            //$(el + ' ' + leftBtn).removeClass("outtR");

            if (translateXval >= itemsCondition - IW / 2) {
                translateXval = itemsCondition;
                //$(el + ' ' + rightBtn).addClass("outtR");
            }
        }
        $(el + ' ' + itemsDiv).css('transform', 'translateX(' + -translateXval + 'px)');
    }

    
    function click(ell, ee) {
        debugger;
        var Parent = $('#recentParent').val();
        var slide = $(Parent).attr("data-slide");
        ResCarousel(ell, Parent, slide);
    }

});