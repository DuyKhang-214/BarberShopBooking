document.addEventListener("DOMContentLoaded", function () {

    const toggleButtons = document.querySelectorAll(".toggle-password");

    toggleButtons.forEach(function (icon) {

        icon.addEventListener("click", function () {

            const targetId = icon.dataset.target;
            const input = document.getElementById(targetId);

            if (!input) return;

            if (input.type === "password") {
                input.type = "text";
                icon.classList.remove("fa-eye-slash");
                icon.classList.add("fa-eye");
            }
            else {
                input.type = "password";
                icon.classList.remove("fa-eye");
                icon.classList.add("fa-eye-slash");
            }
        });

    });

});