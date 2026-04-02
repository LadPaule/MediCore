window.initializeTheme = () => {
    const savedTheme = localStorage.getItem('theme') || 'light';
    setTheme(savedTheme);
    //Todo: Sync the toggle button's state
    const toggle = document.querySelector('#theme-toggle-icon');
    if (toggle) {
        if (savedTheme === 'dark') {
            toggle.classList.remove('bi-sun');
            toggle.classList.add('bi-moon');
        } else {
            toggle.classList.remove('bi-moon');
            toggle.classList.add('bi-sun');
        }
    }
};

//Todo: Listen for system preference changes
window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
    if (!localStorage.getItem('theme')) {
        setTheme(e.matches ? 'dark' : 'light');
    }
});

window.getCurrentTheme = () => {
    return document.body.getAttribute('data-theme') || 'light';
};

window.setTheme = (theme) => {
    document.body.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
    //Todo: Update toggle icon
    const toggleIcon = document.querySelector('#theme-toggle-icon');
    if (toggleIcon) {
        if (theme === 'dark') {
            toggleIcon.classList.remove('bi-sun');
            toggleIcon.classList.add('bi-moon');
        } else {
            toggleIcon.classList.remove('bi-moon');
            toggleIcon.classList.add('bi-sun');
        }
    }
};

window.initializeTheme = () => {
    const savedTheme = localStorage.getItem('theme') || 'light';
    setTheme(savedTheme);
};


window.getThemeAttribute = () => document.body.getAttribute('data-theme');

