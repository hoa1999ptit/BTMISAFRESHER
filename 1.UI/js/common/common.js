$(document).ready(function() {

    // close data-site when click select control
    $('.dropdown').click(function() {
            var data_site = $(this).find('.drop-data-site');
            var expanded = data_site.hasClass('checked');
            if (expanded) {
                data_site.css('display', 'none')
                data_site.removeClass('checked')
            } else {
                data_site.css('display', 'block')
                data_site.addClass('checked')
            }
        })
        // end event


    // close data-site when click outside screen
    $(document).on('click', function(event) {
            if (!$(event.target).closest('.dropdown').length) {
                var data_site = $(".dropdown").find('.drop-data-site')
                data_site.css('display', 'none')
                data_site.removeClass('checked')
            }
        })
        // end event 

    // change text when select option combo box
    $('.drop-item').click(function() {
            var drop_text = $(this).parent().parent().find('.drop-text')
            var selected_text = $(this).find('.item-text').text()
            drop_text.text(selected_text)

            var data_site = $(this).parent().find('.drop-item')
            data_site.removeClass("selected")
            data_site.find('.checked-icon').html('')

            $(this).addClass('selected');
            $(this).find('.checked-icon').html('<i class="fa fa-check"></i>')
        })
        // end event



    // close modal
    $('.close, .cancle').click(function() {
            $('.modal').css('display', 'none')
        })
        // end event
    function isNullOrUndefined(par) {
        return (typeof(variable) != "undefined" && variable != null) ? false : true
    }
})