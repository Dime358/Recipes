$(function () {

    var counter = 0;


    $('#addTag').click(function () {

        var tagText = $('#tagName').val();
        var elems = '<div>' + 
            //'<input type="button" class="removebtn btn btn-primary btn-sm" value="x" id="removebtn"' + counter + '"/>' +
            '<button type="button" class="btn btn-cyan btn-sm" id="removebtn">' + tagText + '</button>' +
            '</div>';

        var white = tagText.replace(/\s/g, "").length;

        if (jQuery.inArray(tagText, tagsArray) === -1 && white != 0) {
            $('#tagDiv').append(elems);

            counter++;
            tagsArray.push(tagText);

            var tagsString = tagsArray.join("|");
            $('#tagName').val("");
            $('#tagsInput').val(tagsString);

            return false;

        }

    });

    $('body').on('click', '#removebtn', function () {
        debugger;
        $(this).remove();
        tagsArray.splice(tagsArray.indexOf($(this).val()), 1);
    });
});