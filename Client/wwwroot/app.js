window.previewMediaFile = (inputElem, htmlElem) => {
    const url = URL.createObjectURL(inputElem.files[0]);
    htmlElem.addEventListener('load', () => URL.revokeObjectURL(url), { once: true });
    htmlElem.src = url;
}

window.clearSrc = (element, isVideo) => {
    if (isVideo == true) {
        element.pause();
    }
    element.removeAttribute("src");
}