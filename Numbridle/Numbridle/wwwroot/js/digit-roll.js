(() => {
    const element = document.getElementById("rollNumber");

    if (!element) return;

    const finalNumber = element.textContent.trim();

    // Esimese külastuse korral pole tulemust veel olemas.
    if (!/^\d{6}$/.test(finalNumber)) return;

    // Ära animeeri lehe tavalise uuesti laadimise korral.
    const navigation = performance.getEntriesByType("navigation")[0];

    if (!navigation || navigation.type !== "navigate") return;

    const form = document.getElementById("rollForm");
    const button = document.getElementById("rollButton");
    const hint = document.getElementById("rollHint");

    // Animatsioon käivitub ainult siis, kui mängija vajutas ROLL.
    const shouldAnimate = sessionStorage.getItem("numbridleRolling") === "1";

    if (!shouldAnimate) return;

    sessionStorage.removeItem("numbridleRolling");

    if (form && button) {
        button.disabled = true;
        button.querySelector(".button-text").textContent = "ROLLING...";
        button.querySelector(".button-icon").textContent = "✦";
    }

    if (hint) {
        hint.textContent = "Revealing your discovery...";
    }

    const digits = [...finalNumber];
    element.replaceChildren();
    element.classList.remove("winner");

    const slots = [];

    digits.forEach((digit, index) => {
        const slot = document.createElement("span");
        slot.className = "digit-slot";

        const strip = document.createElement("span");
        strip.className = "digit-strip";

        // Iga rull algab nullist ja teeb mitu ringi.
        const rounds = 3 + index;
        const endPosition = rounds * 10 + Number(digit);

        for (let i = 0; i <= endPosition; i++) {
            const cell = document.createElement("span");
            cell.textContent = String(i % 10);
            strip.appendChild(cell);
        }

        slot.appendChild(strip);
        element.appendChild(slot);

        slots.push({
            strip,
            endPosition,
            index
        });
    });

    // Käivitame animatsiooni pärast elementide mõõtmete arvutamist.
    requestAnimationFrame(() => {
        slots.forEach(({ strip, endPosition, index }) => {
            const duration = 2.2 + index * 0.42;
            const delay = (digits.length - index - 1) * 0.16;

            strip.style.transition = "none";
            strip.style.transform = "translateY(0)";

            strip.getBoundingClientRect();

            strip.style.transition =
                `transform ${duration}s cubic-bezier(.12,.75,.18,1) ${delay}s`;

            strip.style.transform =
                `translateY(-${endPosition * 1.2}em)`;
        });

        // Viimane number lõpetab kõige hiljem.
        const totalDuration = 2.2 + 5 * 0.42 + 5 * 0.16;

        window.setTimeout(() => {
            element.classList.add("winner");

            if (hint) {
                hint.textContent = "Discovery complete. Every number counts.";
            }

            if (button) {
                button.disabled = false;
                button.querySelector(".button-text").textContent = "ROLL AGAIN";
                button.querySelector(".button-icon").textContent = "🎲";
            }
        }, totalDuration * 1000 + 100);
    });

    if (form) {
        form.addEventListener("submit", () => {
            sessionStorage.setItem("numbridleRolling", "1");
        });
    }
})();