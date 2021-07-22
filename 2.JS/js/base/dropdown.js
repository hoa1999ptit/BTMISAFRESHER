$(function() {
    check_allchoice();
});
//get nth parent
$.fn.nParent = function(n) {
    let p = this;
    for (var i = 0; i < n; i++) p = p.parent();
    return p;
};

//Hiển thị nút clear nếu đã lưa chọn chọn
function check_allchoice() {
    if ($(".dropdown").find(".divtext").text() != "Chưa chọn") {
        $(".dropdown").find(".clearchoice").css("visibility", "visible");
    } else {
        $(".dropdown").find(".clearchoice").css("visibility", "hidden");
    }
}

//Xóa lựa chọn khi bấm vào nút clear
$(".dropdown .clearchoice").on("click", function() {
    $(this).css("visibility", "hidden");
    let thisdropdown = $(this).parent();
    thisdropdown.find(".divtext").text("");
    thisdropdown.find(".item").removeClass("selected")
    thisdropdown.attr('value', '-1');

});

/**
 * 
 * @param {*} mes // thông điệp để kiểm tra
 */

function collapseExceptDrop(thisarrow) {
    $(".arrow").not(thisarrow).removeClass("rot-180");
    $(".arrow").not(thisarrow).addClass("rot-0");
    $(".itemwrapper").slideUp();
}

/**
 * 
 * @param {*} thisdrop :Element dropdown cần mở
 */
function openDrop(thisclick) {
    let thisdrop = thisclick.parent().parent(),
        thisitemwrapper = thisdrop.find(".itemwrapper"),
        thisarrow = thisdrop.find(".arrow");
    collapseExceptDrop(thisarrow);
    if (thisarrow.hasClass('rot-180')) {
        thisitemwrapper.slideUp();
        thisarrow.removeClass('rot-180');
        thisarrow.addClass('rot-0');
    } else {
        thisitemwrapper.slideDown();
        thisarrow.removeClass('rot-0');
        thisarrow.addClass('rot-180');

    }
}
$(".dropdown .divtext,.dropdown .arrow-zone").click(function() {
    openDrop($(this));
});

$(".dropdown .divtext,.dropdown .arrow-zone").on('keypress', function(e) {
    if (e.which == 13) {
        openDrop($(this));
    }
});
/**
 * Hiển thị lựa chọn đã chọn, ẩn ds lựa chọn
 * NH_16/7
 */
$(".itemwrapper").on("click", ".item", function() {
    let item = $(this),
        choice = item.text(),
        thisdrop = item.nParent(2);
    thisdrop.find(".item").removeClass("selected");
    item.addClass("selected");

    thisdrop.find(".divtext").text(choice);
    thisdrop.find(".divtext").attr('value', item.attr('value'));
    thisdrop.find(".clearchoice").css("visibility", "visible");
    thisdrop.attr('value', item.attr('value'));
    // thisdrop.find(".divtext").trigger('contentchanged');
    collapseExceptDrop();
});

/**
 * Đóng các dropdown khi bấm ra ngoài vùng
 */
$(document).click(function(e) {
    if (!$(".dropdown .arrow-zone .arrow, .dropdown .divtext").is(e.target)) {
        collapseExceptDrop();
    }
});