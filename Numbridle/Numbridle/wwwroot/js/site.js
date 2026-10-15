
(() => {
    // =========================================
    // GUESS FORM
    // =========================================

    const form = document.getElementById("guessForm");
    const input = document.getElementById("combinationInput");
    const button = document.getElementById("guessButton");

    if (input) {
        input.addEventListener("input", () => {
            const cleaned = input.value
                .replace(/[^0-9]/g, "")
                .slice(0, 6);

            if (input.value !== cleaned) {
                input.value = cleaned;
            }
        });
    }

    if (form && button) {
        form.addEventListener("submit", () => {
            if (!form.checkValidity()) return;

            button.disabled = true;

            const label = button.querySelector("span");

            if (label) {
                label.textContent = "Discovering...";
            }
        });
    }

    // =========================================
    // NUMBER ROLL ANIMATION
    // =========================================

    const rollForm = document.getElementById("rollForm");
    const rollButton = document.getElementById("rollButton");

    if (rollForm && rollButton) {
        rollForm.addEventListener("submit", () => {
            sessionStorage.setItem("numbridleRolling", "1");

            rollButton.disabled = true;

            const label = rollButton.querySelector(".button-text");

            if (label) {
                label.textContent = "ROLLING...";
            }
        });
    }

    const rollNumber = document.getElementById("rollNumber");

    if (rollNumber) {
        const navigation =
            performance.getEntriesByType("navigation")[0];

        const shouldRoll =
            sessionStorage.getItem("numbridleRolling") === "1";

        if (shouldRoll) {
            sessionStorage.removeItem("numbridleRolling");

            const finalNumber = rollNumber.textContent.trim();

            if (/^\d{6}$/.test(finalNumber)) {
                animateDigits(rollNumber, finalNumber);
            } else if (rollButton) {
                rollButton.disabled = false;
            }
        }
    }

    function animateDigits(element, finalNumber) {
        const reducedMotion = window.matchMedia(
            "(prefers-reduced-motion: reduce)"
        ).matches;

        const button = document.getElementById("rollButton");
        const hint = document.getElementById("rollHint");

        if (reducedMotion) {
            element.textContent = finalNumber;

            if (button) button.disabled = false;

            return;
        }

        element.replaceChildren();
        element.classList.remove("winner");

        const slots = [...finalNumber].map((digit, index) => {
            const slot = document.createElement("span");
            slot.className = "digit-slot";

            const strip = document.createElement("span");
            strip.className = "digit-strip";

            // Each digit completes several rotations.
            const rounds = 4 + index;
            const endPosition = rounds * 10 + Number(digit);

            for (let i = 0; i <= endPosition; i++) {
                const cell = document.createElement("span");

                cell.textContent = String(i % 10);

                strip.appendChild(cell);
            }

            slot.appendChild(strip);
            element.appendChild(slot);

            return {
                strip,
                endPosition,
                index
            };
        });

        if (hint) {
            hint.textContent = "Revealing your discovery...";
        }

        requestAnimationFrame(() => {
            slots.forEach(({ strip, endPosition, index }) => {
                // Leftmost digit finishes last.
                const duration = 2.2 + index * 0.4;
                const delay = (5 - index) * 0.15;

                strip.style.transform = "translateY(0)";
                strip.getBoundingClientRect();

                strip.style.transition =
                    `transform ${duration}s cubic-bezier(.12,.75,.18,1) ${delay}s`;

                strip.style.transform =
                    `translateY(-${endPosition * 1.2}em)`;
            });

            const totalDuration = 2.2 + 5 * 0.4 + 5 * 0.15;

            window.setTimeout(() => {
                element.classList.add("winner");

                if (hint) {
                    hint.textContent =
                        "Discovery complete. Every number counts.";
                }

                if (button) {
                    button.disabled = false;

                    const label =
                        button.querySelector(".button-text");

                    const icon =
                        button.querySelector(".button-icon");

                    if (label) label.textContent = "ROLL AGAIN";
                    if (icon) icon.textContent = "🎲";
                }
            }, totalDuration * 1000 + 100);
        });
    }

    // =========================================
    // DISCOVERY RESULT ANIMATIONS
    // =========================================

    const result = document.getElementById("discoveryResult");

    if (result && result.classList.contains("result-success")) {
        const tier = Number(result.dataset.zeroTier || 0);
        const stage = document.getElementById("revealStage");

        if (stage && tier > 0 && tier < 6) {
            stage.classList.add("zero-discovery");

            stage.animate(
                [
                    { opacity: 0, transform: "scale(.94)" },
                    { opacity: 1, transform: "scale(1.025)" },
                    { opacity: 1, transform: "scale(1)" }
                ],
                {
                    duration: 650 + tier * 100,
                    easing: "cubic-bezier(.2,.8,.2,1)"
                }
            );
        }

        if (tier === 6) {
            document.body.classList.add("void-discovered");

            result.animate(
                [
                    {
                        filter: "brightness(1)",
                        transform: "scale(1)"
                    },
                    {
                        filter: "brightness(1.8)",
                        transform: "scale(1.008)"
                    },
                    {
                        filter: "brightness(1)",
                        transform: "scale(1)"
                    }
                ],
                {
                    duration: 1100,
                    easing: "ease-out"
                }
            );

            if (stage) {
                const ring = stage.querySelector(".void-ring");

                if (ring) {
                    ring.animate(
                        [
                            {
                                transform: "scale(.55)",
                                opacity: 0
                            },
                            {
                                transform: "scale(1.35)",
                                opacity: .8
                            },
                            {
                                transform: "scale(1.8)",
                                opacity: 0
                            }
                        ],
                        {
                            duration: 1400,
                            easing: "ease-out"
                        }
                    );
                }
            }
        }

        // Count reward points without changing the server result.
        const reward = document.getElementById("rewardValue");

        if (
            reward &&
            !window.matchMedia(
                "(prefers-reduced-motion: reduce)"
            ).matches
        ) {
            const target = Number(reward.dataset.points || 0);

            if (Number.isSafeInteger(target) && target > 0) {
                const duration = tier === 6 ? 1600 : 700;
                const start = performance.now();

                const tick = (now) => {
                    const progress = Math.min(
                        (now - start) / duration,
                        1
                    );

                    const eased = 1 - Math.pow(1 - progress, 4);
                    const current = Math.floor(target * eased);

                    reward.textContent =
                        "+" + current.toLocaleString();

                    if (progress < 1) {
                        requestAnimationFrame(tick);
                    } else {
                        reward.textContent =
                            "+" + target.toLocaleString();
                    }
                };

                reward.textContent = "+0";
                requestAnimationFrame(tick);
            }
        }
    }
})();