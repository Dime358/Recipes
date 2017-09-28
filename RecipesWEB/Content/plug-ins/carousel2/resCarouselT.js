//resCarousel

$(window).resize(function () {
    ResCarouselSize();
});
$(document).ready(function(){
var itemsMainDiv = ('.resCarouselT');
var itemsDiv = ('.resCarousel-innerT');
var itemWidth = "";
	
$('.leftLstT, .rightLstT').click(function () {
    debugger;
		var condition = $(this).hasClass("leftLstT");
		if(condition)
			click(0,this);
		else
			click(1,this)
	});

	ResCarouselSize();




$(window).resize(function() {
	ResCarouselSize();
});

//this function define the size of the items
function ResCarouselSize()
{
	var incno = 0;
	var dataItems = ("data-items");
	var itemClass = ('.itemT');	
	var id = 6;
	var btnParentSb = '';
	var itemsSplit = '';
	var sampwidth = $(itemsMainDiv).width();
	var bodyWidth = $('body').width();
	$(itemsDiv).each(function() {
		id=id+1;
		var itemNumbers = $(this).find(itemClass).length;
			btnParentSb = $(this).parent().attr(dataItems);
			itemsSplit = btnParentSb.split(',');
			$(this).parent().attr("id","ResSlid"+id);


		if(bodyWidth>=1200)
		{
			incno=itemsSplit[3];
			itemWidth = sampwidth/incno;		
		}
		else if(bodyWidth>=992)
		{
			incno=itemsSplit[2];
			itemWidth = sampwidth/incno;
		}
		else if(bodyWidth>=768)
		{
			incno=itemsSplit[1];
			itemWidth = sampwidth/incno;
		}
		else
		{
			incno=itemsSplit[0];
			itemWidth = sampwidth/incno;
		}
		$(this).css({'transform':'translateX(0px)','width':itemWidth*itemNumbers});
		$(this).find(itemClass).each(function(){
			$(this).outerWidth(itemWidth);
		});

		//$(".leftLstT").addClass("outt");				
		//$(".rightLstT").removeClass("outt");

	});
}


//this function used to move the items
function ResCarousel(e, el, s){
	var leftBtn = ('.leftLstT');
	var rightBtn = ('.rightLstT');
	var translateXval = '';
	var divStyle = $(el+' '+itemsDiv).css('transform');
	var values = divStyle.match(/-?[\d\.]+/g);
	var xds = Math.abs(values[4]);
	var IW = $(el).find('.itemT').outerWidth();
	//alert(IW*s);
	
		if(e==0){
			translateXval = parseInt(xds)-parseInt(IW*s);				
			//$(el+' '+rightBtn).removeClass("outtT");

			
			if(translateXval<= IW/2){
				translateXval = 0;
				//$(el+' '+leftBtn).addClass("outtT");
			}
		}
		else if(e==1){
			var itemsCondition = $(el).find(itemsDiv).width()-$(el).width();
			translateXval = parseInt(xds)+parseInt(IW*s);
			//$(el+' '+leftBtn).removeClass("outtT");

			if(translateXval>= itemsCondition-IW/2){
				translateXval = itemsCondition;				
				//$(el+' '+rightBtn).addClass("outtT");		
			}
		}
		$(el+' '+itemsDiv).css('transform','translateX('+-translateXval+'px)');
}

//It is used to get some elements from btn
function click(ell, ee) {
    debugger;
    var Parent = $('#topParent').val();
	var slide = $(Parent).attr("data-slide");
	ResCarousel(ell, Parent, slide);
}

});