function setTheme(theme) {
    console.log("Hello!");
    const htmlElement = document.getElementById('html');
    htmlElement.setAttribute('data-bs-theme', theme);
}

function prefersDarkMode() {
    return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
}