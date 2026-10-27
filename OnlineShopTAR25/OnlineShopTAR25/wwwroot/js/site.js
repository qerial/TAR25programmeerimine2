
"use strict";

(() => {
    function initCustomCursor() {
        if (window.matchMedia("(pointer: coarse)").matches) return;

        let cursor = document.getElementById("customCursor");

        if (!cursor) {
            cursor = document.createElement("div");
            cursor.id = "customCursor";
            cursor.setAttribute("aria-hidden", "true");
            document.body.appendChild(cursor);
        }

        cursor.style.display = "block";

        const canvas = document.getElementById("networkCanvas");

        document.addEventListener("pointermove", event => {
            if (event.pointerType === "touch") return;

            cursor.style.left = event.clientX + "px";
            cursor.style.top = event.clientY + "px";
            cursor.style.display = "block";

            if (!canvas) return;

            const rect = canvas.getBoundingClientRect();
            const x = event.clientX - rect.left;
            const y = event.clientY - rect.top;

            const radius = Math.min(
                260,
                Math.max(100, Math.min(rect.width, rect.height) * 0.30)
            );

            const cx = rect.width <= 650
                ? rect.width * 0.72
                : rect.width * 0.74;

            const cy = rect.height * 0.49;
            const distance = Math.hypot(x - cx, y - cy);

            cursor.classList.toggle(
                "is-near-sphere",
                x >= 0 && x <= rect.width &&
                y >= 0 && y <= rect.height &&
                distance <= radius + 25
            );
        });

        document.addEventListener("pointerdown", () => {
            if (canvas && canvas.classList.contains("is-dragging")) {
                cursor.classList.add("is-dragging");
            }
        });

        document.addEventListener("pointerup", () => {
            cursor.classList.remove("is-dragging");
        });

        document.addEventListener("pointercancel", () => {
            cursor.classList.remove("is-dragging");
        });

        window.addEventListener("blur", () => {
            cursor.classList.remove("is-dragging");
        });

        document.addEventListener("mouseleave", () => {
            cursor.style.display = "none";
        });

        document.addEventListener("mouseenter", () => {
            cursor.style.display = "block";
        });

        if (canvas) {
            const observer = new MutationObserver(() => {
                cursor.classList.toggle(
                    "is-dragging",
                    canvas.classList.contains("is-dragging")
                );
            });

            observer.observe(canvas, {
                attributes: true,
                attributeFilter: ["class"]
            });
        }
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", initCustomCursor);
    } else {
        initCustomCursor();
    }
})();
