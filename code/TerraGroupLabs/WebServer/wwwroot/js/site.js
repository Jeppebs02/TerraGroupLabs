// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



document.addEventListener("DOMContentLoaded", function () {

    console.log("loading-screen div:", document.getElementById("loading-screen"));
    console.log("granting-access div:", document.getElementById("granting-access"));
    
    const textEl = document.getElementById("granting-access");
    const loadingScreen = document.getElementById("loading-screen");

    // Animate "Granting Access...", cycling dots
    const dots = ["", ".", "..", "..."];
    let idx = 0;
    const dotsInterval = setInterval(() => {
        textEl.textContent = "Granting Access" + dots[idx];
        idx = (idx + 1) % dots.length;
    }, 500);

    // After 3 seconds, fade out
    setTimeout(() => {
        // stop the dots animation
        clearInterval(dotsInterval);

        // fade out
        loadingScreen.classList.add("fade-out");

        // remove from DOM after fade completes
        setTimeout(() => {
            loadingScreen.remove();
        }, 1000); // matches the CSS transition time
    }, 3000);
});
