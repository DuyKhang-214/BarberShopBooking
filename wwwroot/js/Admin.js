document.addEventListener("DOMContentLoaded", function () {
    const cards = document.querySelectorAll(".admin-card");

    cards.forEach(function (card, index) {
        card.style.opacity = "0";
        card.style.transform = "translateY(25px)";

        setTimeout(function () {
            card.style.transition = ".4s ease";
            card.style.opacity = "1";
            card.style.transform = "translateY(0)";
        }, index * 120);
    });
});