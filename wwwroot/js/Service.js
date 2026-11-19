document.addEventListener("DOMContentLoaded", function () {
    const serviceItems = document.querySelectorAll(".service-item");

    const observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add("show");
            }
        });
    }, {
        threshold: 0.15
    });

    serviceItems.forEach(function (item) {
        observer.observe(item);
    });

    const searchInput = document.getElementById("searchService");

    if (searchInput) {
        searchInput.addEventListener("keyup", function () {
            const keyword = searchInput.value.toLowerCase().trim();

            serviceItems.forEach(function (card) {
                const serviceName = card.querySelector("h3").innerText.toLowerCase();
                const serviceDescription = card.querySelector("p").innerText.toLowerCase();

                if (serviceName.includes(keyword) || serviceDescription.includes(keyword)) {
                    card.style.display = "block";
                } else {
                    card.style.display = "none";
                }
            });
        });
    }

    const modal = document.getElementById("serviceModal");
    const closeModal = document.getElementById("closeModal");

    const modalName = document.getElementById("modalServiceName");
    const modalDesc = document.getElementById("modalServiceDesc");
    const modalPrice = document.getElementById("modalServicePrice");
    const modalDuration = document.getElementById("modalServiceDuration");

    document.querySelectorAll(".detail-btn").forEach(function (button) {
        button.addEventListener("click", function () {
            modalName.innerText = button.dataset.name;
            modalDesc.innerText = button.dataset.desc;
            modalPrice.innerText = Number(button.dataset.price).toLocaleString("vi-VN") + " VNĐ";
            modalDuration.innerText = button.dataset.duration + " phút";

            modal.classList.add("active");
        });
    });

    if (closeModal) {
        closeModal.addEventListener("click", function () {
            modal.classList.remove("active");
        });
    }

    if (modal) {
        modal.addEventListener("click", function (e) {
            if (e.target === modal) {
                modal.classList.remove("active");
            }
        });
    }
});