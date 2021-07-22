$(".menu .toggle-menu").on("click", function() {
    $(this).toggleClass("closed");
    $(".menu").toggleClass("minimized");
    $(".menu .menu-item .menu-item-text").toggleClass("isnone");

    $(".content").toggleClass("expanded");
    $(".content .content-header").toggleClass("expanded");
    $(".content .table-wrapper").toggleClass("expanded");
})