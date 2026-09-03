(function () {
    const publicNavbar = document.querySelector("[data-public-navbar]");

    if (publicNavbar) {
        const updateNavbar = function () {
            publicNavbar.classList.toggle("is-scrolled", window.scrollY > 12);
        };

        updateNavbar();
        window.addEventListener("scroll", updateNavbar, { passive: true });
    }

    const revealItems = document.querySelectorAll(".reveal-on-scroll");

    if (!revealItems.length) {
        return;
    }

    const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    if (reduceMotion || !("IntersectionObserver" in window)) {
        revealItems.forEach(function (item) {
            item.classList.add("is-visible");
        });
        return;
    }

    const observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add("is-visible");
                observer.unobserve(entry.target);
            }
        });
    }, {
        rootMargin: "0px 0px -8% 0px",
        threshold: 0.12
    });

    revealItems.forEach(function (item) {
        observer.observe(item);
    });
})();