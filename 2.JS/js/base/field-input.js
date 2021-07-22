listenInput();
listenClear();

function listenInput() {
    let thisinput = $(".toolbar .filter #searchfield"),
        thisclearchoice = thisinput.parent().find(".clearchoice");

    thisinput.on("input", function() {
        console.log("inputting")
        if (thisinput.val().length > 0) {
            thisclearchoice.css('visibility', 'visible');
        } else thisclearchoice.css('visibility', 'hidden');
    });
}

function listenClear() {
    let thisclearchoice = $(".toolbar .filter .searchbox .clearchoice"),
        thisinput = thisclearchoice.parent().find("#searchfield");
    thisclearchoice.on("click", function() {
        thisinput.val("")
        thisclearchoice.css('visibility', 'hidden');
    });
}