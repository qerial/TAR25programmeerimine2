(() => {
    const element = document.getElementById("rollNumber");
    const form = document.getElementById("rollForm");
    const button = document.getElementById("rollButton");
    const hint = document.getElementById("rollHint");

    if (!element || !form || !button) return;

    // Mark a roll before submitting the form.
    form.addEventListener("submit", () => {
        sessionStorage.setItem("numbridleRolling", "1");
        button.disabled = true;
    });

    const shouldAnimate =
        sessionStorage.getItem("numbridleRolling") === "1";

    if (!shouldAnimate) return;

    sessionStorage.removeItem("numbridleRolling");

    const finalNumber = element.textContent.trim();

    // Always preserve the actual six-digit result.
    if (!/^\d{6}$/.test(finalNumber)) {
        button.disabled = false;
        return;
    }

    const reducedMotion = window.matchMedia(
        "(prefers-reduced-motion: reduce)"
    ).matches;

    if (reducedMotion) {
        showFinalNumber(element, finalNumber);
        finish();
        return;
    }

    button.disabled = true;

    const buttonText = button.querySelector(".button-text");
    const buttonIcon = button.querySelector(".button-icon");

    if (buttonText) buttonText.textContent = "ROLLING...";
    if (buttonIcon) buttonIcon.textContent = "✦";
    if (hint) hint.textContent = "Rolling through the void...";

    element.replaceChildren();
    element.classList.remove("winner");
    element.classList.add("rolling");

    // Only remove zeros at the beginning of the final result.
    // Internal and trailing zeros remain visible.
    const firstNonZero = finalNumber.search(/[1-9]/);
    const visibleNumber = firstNonZero === -1
        ? "0"
        : finalNumber.slice(firstNonZero);

    const slots = [];
    const digitHeight = 1.2;

    // A temporary random digit stream, different on every roll.
    for (let index = 0; index < finalNumber.length; index++) {
        const slot = document.createElement("span");
        slot.className = "digit-slot";

        const strip = document.createElement("span");
        strip.className = "digit-strip";

        const rounds = 5 + Math.floor(Math.random() * 5);
        const randomStart = Math.floor(Math.random() * 10);
        const endPosition =
            rounds * 10 + Number(finalNumber[index]);

        // Start with random digits rather than zero.
        for (let i = 0; i <= endPosition; i++) {
            const cell = document.createElement("span");

            cell.textContent = String(
                Math.floor(Math.random() * 10)
            );

            strip.appendChild(cell);
        }

        // The last cell must be the correct final digit.
        strip.lastElementChild.textContent = finalNumber[index];

        // Begin at a random position in the strip.
        const startPosition = randomStart;
        strip.style.transform =
            `translateY(-${startPosition * digitHeight}em)`;

        slot.appendChild(strip);
        element.appendChild(slot);

        slots.push({
            slot,
            strip,
            endPosition,
            index
        });
    }

    requestAnimationFrame(() => {
        // Force initial layout before starting transitions.
        element.getBoundingClientRect();

        slots.forEach(({ strip, endPosition, index }) => {
            const duration = 2.0 + index * 0.28;
            const delay = (5 - index) * 0.12;

            strip.style.filter = "blur(4px)";

            strip.style.transition =
                `transform ${duration}s cubic-bezier(.12,.75,.18,1) ${delay}s, ` +
                `filter ${duration}s ease-out ${delay}s`;

            strip.style.transform =
                `translateY(-${endPosition * digitHeight}em)`;

            strip.addEventListener("transitionend", (event) => {
                if (event.propertyName === "filter") {
                    strip.style.filter = "blur(0)";
                }
            }, { once: true });
        });

        const totalDuration = 2.0 + 5 * 0.28 + 5 * 0.12;

        window.setTimeout(() => {
            // Remove leading zeros from the rendered animation.
            // Do not modify finalNumber or the server result.
            const leadingZeroCount = firstNonZero === -1
                ? 5
                : firstNonZero;

            if (firstNonZero === -1) {
                // Six zeros become one visible zero.
                slots.slice(0, 5).forEach(({ slot }, index) => {
                    slot.style.transition =
                        "opacity .35s ease, transform .35s ease";
                    slot.style.opacity = "0";
                    slot.style.transform = "scale(.5)";
                });

                slots[5].querySelector(".digit-strip")
                    .lastElementChild.textContent = "0";
            } else if (leadingZeroCount > 0) {
                slots.slice(0, leadingZeroCount).forEach(({ slot }) => {
                    slot.style.transition =
                        "opacity .35s ease, transform .35s ease, width .35s ease";

                    slot.style.opacity = "0";
                    slot.style.transform = "translateY(-8px) scale(.6)";
                    slot.style.width = "0";
                    slot.style.minWidth = "0";
                    slot.style.margin = "0";
                });
            }

            // Keep the underlying six-digit result intact.
            element.dataset.fullNumber = finalNumber;
            element.classList.remove("rolling");
            element.classList.add("winner");

            if (hint) {
                hint.textContent =
                    `Discovery complete: ${visibleNumber}`;
            }

            finish();
        }, totalDuration * 1000 + 150);
    });

    function showFinalNumber(target, number) {
        const first = number.search(/[1-9]/);
        const visible = first === -1 ? "0" : number.slice(first);

        target.textContent = visible;
        target.dataset.fullNumber = number;
    }

    function finish() {
        button.disabled = false;

        const text = button.querySelector(".button-text");
        const icon = button.querySelector(".button-icon");

        if (text) text.textContent = "ROLL AGAIN";
        if (icon) icon.textContent = "🎲";
    }
})();