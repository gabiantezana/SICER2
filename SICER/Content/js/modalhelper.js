$(document).on('click', '[data-type=modal-link]', function (e) {
    e.preventDefault();

    var $link = $(this);

    var beforeInit = $link.data('before-callback');
    var resultBefore = false;

    if (beforeInit != null) {
        var fnct = eval(beforeInit);
        if (typeof window[beforeInit] == 'function') {
            resultBefore = window[beforeInit].call();
        }
    } else {
        resultBefore = true;
    }

    if (resultBefore == false)
        return false;

    var sourceUrl = $link.attr('data-source-url');
    var onClose = $link.attr('data-on-close');
    var modalSize = $link.attr('data-modal-size');

    var $modalLoading = $('#default-modal-loading');
    var $modal = $('<div class="modal fade modal-fade-in-scale-up" role="dialog"><div class="modal-dialog ' + modalSize + '"><div class="modal-content"></div></div></div>');
    var $modalContent = $modal.find('.modal-content');

    $modalContent.html($modalLoading.html());

    var $modalContainer = $('#default-modal-container');
    $modalContainer.append($modal);

    var bootModal = $modal.modal('show');

    $modalContent.load(sourceUrl, function (response, status, xhr) {
        if (status == 'error')
            $modalContent.html($('#default-modal-loading-error').html());

        $('.select2').css('width', '');
    });
});

$(".modal").on('hidden.bs.modal', function () {

    $(this).remove();

    $('.pac-container').remove();
    $('iframe[name=gm-master]').remove();
    console.log('modal close');

});
