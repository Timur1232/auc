(function() {
    const form = ultra.by_id('lot-create-form');
    const thumbnail_input = form.$find('#thumbnail-input');
    const files_input = form.$find('#files-input');
    form.$on_submit((e) => {
        if (!thumbnail_input.value && files_input.files.length === 0) {
            e.preventDefault();
            const error = form.$find('#error');
            const img_msg = error.$find('#img_msg');
            const msg = ultra.tags.span('Нужно выбрать обложку или хотя бы одно фото.').$class('form-error').$id('img_msg');
            if (img_msg) {
                error.$replace(msg, img_msg);
            } else {
                error.$append(msg);
            }
        }
    });
})();
