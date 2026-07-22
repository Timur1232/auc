async function load_images(...images_paths) {
    const files = [];
    for (const img of images_paths) {
        try {
            const file = await files_toolbar.url_to_file(img, img.split('/').at(-1));
            files.push(file);
        } catch (e) {
            console.error(`Не удалось загрузить ${item.url}:`, error);
        }
    }
    files_toolbar.add_files(files);
}
