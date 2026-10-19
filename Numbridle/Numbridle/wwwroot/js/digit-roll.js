(() => {
    const element = document.getElementById("rollNumber");
    const form = document.getElementById("rollForm");
    const button = document.getElementById("rollButton");
    const hint = document.getElementById("rollHint");

    if (!element || !form || !button) return;

    const storageKey = "numbridleRolling";

    form.addEventListener("submit", () => {
        sessionStorage.setItem(storageKey, "1");
        button.disabled = true;
    });

    if (sessionStorage.getItem(storageKey) !== "1") return;

    sessionStorage.removeItem(storageKey);

    const finalNumber = element.textContent.trim();

    if (!/^\d{6}$/.test(finalNumber)) {
        button.disabled = false;
        return;
    }

    const reducedMotion = window.matchMedia(
        "(prefers-reduced-motion: reduce)"
    ).matches;

    const buttonText = button.querySelector(".button-text");
    const buttonIcon = button.querySelector(".button-icon");

    function finishButton() {
        button.disabled = false;

        if (buttonText) buttonText.textContent = "ROLL AGAIN";
        if (buttonIcon) buttonIcon.textContent = "🎲";
    }

    function showFinal() {
        const firstNonZero = finalNumber.search(/[1-9]/);
        const visible = firstNonZero === -1
            ? "0"
            : finalNumber.slice(firstNonZero);

        element.replaceChildren();
        element.textContent = visible;
        element.dataset.fullNumber = finalNumber;
        element.classList.remove("rolling");
        element.classList.add("winner");

        if (hint) {
            hint.textContent = `Discovery complete: ${visible}`;
        }

        finishButton();
    }

    if (reducedMotion) {
        showFinal();
        return;
    }

    button.disabled = true;

    if (buttonText) buttonText.textContent = "ROLLING...";
    if (buttonIcon) buttonIcon.textContent = "✦";
    if (hint) hint.textContent = "Rolling through the void...";

    element.replaceChildren();
    element.classList.remove("winner");
    element.classList.add("rolling");

    const slots = [];

    // Build six independent digit windows.
    for (let i = 0; i < 6; i++) {
        const slot = document.createElement("span");
        slot.className = "digit-slot is-spinning";

        const digit = document.createElement("span");
        digit.className = "rolling-digit";
        digit.textContent = String(
            Math.floor(Math.random() * 10)
        );

        slot.appendChild(digit);
        element.appendChild(slot);

        slots.push({ slot, digit });
    }

    const sleep = ms =>
        new Promise(resolve => setTimeout(resolve, ms));

    function randomDigit() {
        return String(Math.floor(Math.random() * 10));
    }

    async function animate() {
        // Every digit spins independently and very rapidly.
        const timers = slots.map(({ digit }) =>
            window.setInterval(() => {
                digit.textContent = randomDigit();
            }, 35)
        );

        // Let all digits visibly spin before revealing the result.
        await sleep(650);

        // Lock each digit from left to right.
        for (let i = 0; i < 6; i++) {
            await sleep(i === 0 ? 0 : 330);

            window.clearInterval(timers[i]);

            const { slot, digit } = slots[i];
            const finalDigit = finalNumber[i];

            digit.textContent = finalDigit;
            slot.classList.remove("is-spinning");
            slot.classList.add("is-stopped");

            // A leading zero vanishes immediately when it lands.
            if (finalDigit === "0" &&
                finalNumber.slice(0, i + 1)
                    .split("")
                    .every(d => d === "0")) {

                slot.classList.add("is-hidden-zero");
            }

            if (hint) {
                hint.textContent = `Revealing digit ${i + 1} of 6...`;
            }
        }

        // The last digit must remain visible for 000000.
        if (finalNumber === "000000") {
            slots.forEach(({ slot }) => {
                slot.classList.add("is-hidden-zero");
            });

            slots[5].slot.classList.remove("is-hidden-zero");
            slots[5].digit.textContent = "0";
        }

        element.dataset.fullNumber = finalNumber;
        element.classList.remove("rolling");
        element.classList.add("winner");

        const firstNonZero = finalNumber.search(/[1-9]/);
        const visible = firstNonZero === -1
            ? "0"
            : finalNumber.slice(firstNonZero);

        if (hint) {
            hint.textContent = `Discovery complete: ${visible}`;
        }

        finishButton();
    }


    animate().catch(error => {
        console.error("Number animation failed:", error);
        showFinal();
    });
})();
