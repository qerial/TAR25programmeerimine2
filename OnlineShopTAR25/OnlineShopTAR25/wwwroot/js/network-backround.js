
(() => {
    "use strict";

    function startNetwork() {
        const canvas = document.getElementById("networkCanvas");
        if (!canvas || canvas.dataset.ready === "true") return;

        const ctx = canvas.getContext("2d", {
            alpha: false,
            desynchronized: true
        });

        if (!ctx) return;
        canvas.dataset.ready = "true";

        const page = canvas.closest(".kindergarten-page, .home-page")
            || canvas.parentElement;

        const reducedMotion = window.matchMedia(
            "(prefers-reduced-motion: reduce)"
        );

        let w = 0, h = 0, dpr = 1, frame = 0, lastTime = 0;
        let radius = 180;
        let dragging = false;
        let activePointerId = null;
        let lastX = 0, lastY = 0, lastPointerTime = 0;

        let rotationY = 0;
        let rotationX = -0.16;
        let velocityX = 0;
        let velocityY = 0;

        const layers = [
            { scale: 1.00, speed: 1.00, tilt: 0.00, alpha: 0.72, lines: 2 },
            { scale: 0.78, speed: -1.35, tilt: 0.48, alpha: 0.52, lines: 3 },
            { scale: 0.56, speed: 1.70, tilt: -0.58, alpha: 0.43, lines: 3 },
            { scale: 0.35, speed: -2.00, tilt: 0.82, alpha: 0.34, lines: 2 },
            { scale: 0.17, speed: 2.50, tilt: -0.95, alpha: 0.27, lines: 2 }
        ];

        const IDLE_SPEED = 0.20;
        const DRAG_SPEED = 0.005;
        const INERTIA = 2.8;

        function center() {
            return {
                x: w <= 650 ? w * 0.72 : w * 0.74,
                y: h * 0.49
            };
        }

        function isNearSphere(x, y) {
            const c = center();
            const dx = x - c.x;
            const dy = y - c.y;

            return Math.hypot(dx, dy) <= radius + 25;
        }

        function resize() {
            const rect = canvas.getBoundingClientRect();
            w = rect.width || innerWidth;
            h = rect.height || innerHeight;

            if (!w || !h) return;

            dpr = Math.min(devicePixelRatio || 1, 1.5);
            canvas.width = Math.round(w * dpr);
            canvas.height = Math.round(h * dpr);
            ctx.setTransform(dpr, 0, 0, dpr, 0, 0);

            radius = Math.min(260, Math.max(100, Math.min(w, h) * 0.30));

            if (frame) cancelAnimationFrame(frame);
            frame = 0;
            lastTime = 0;

            draw();

            if (!reducedMotion.matches || dragging) animate();
        }

        function background() {
            const gradient = ctx.createLinearGradient(0, 0, w, h);
            gradient.addColorStop(0, "#203e49");
            gradient.addColorStop(0.48, "#102f40");
            gradient.addColorStop(1, "#071e30");

            ctx.fillStyle = gradient;
            ctx.fillRect(0, 0, w, h);

            const c = center();
            const glow = ctx.createRadialGradient(
                c.x, c.y, 0, c.x, c.y, radius * 2.5
            );

            glow.addColorStop(0, "rgba(80,190,215,0.17)");
            glow.addColorStop(0.5, "rgba(35,115,150,0.07)");
            glow.addColorStop(1, "rgba(4,25,39,0)");

            ctx.fillStyle = glow;
            ctx.fillRect(0, 0, w, h);
        }

        function drawLayer(layer, time) {
            const r = radius * layer.scale;
            const angle =
                time * IDLE_SPEED * layer.speed +
                rotationY * layer.speed;
            const tilt = layer.tilt + rotationX * 0.45;
            const c = center();

            ctx.save();
            ctx.translate(c.x, c.y);
            ctx.rotate(tilt);

            const fill = ctx.createRadialGradient(
                -r * 0.32, -r * 0.36, r * 0.02,
                0, 0, r
            );

            fill.addColorStop(
                0,
                `rgba(145,230,245,${layer.alpha * 0.12})`
            );
            fill.addColorStop(
                0.55,
                `rgba(40,130,165,${layer.alpha * 0.07})`
            );
            fill.addColorStop(1, "rgba(4,22,39,0.12)");

            ctx.beginPath();
            ctx.arc(0, 0, r, 0, Math.PI * 2);
            ctx.fillStyle = fill;
            ctx.fill();

            ctx.beginPath();
            ctx.arc(0, 0, r, 0, Math.PI * 2);
            ctx.strokeStyle = `rgba(165,232,255,${layer.alpha})`;
            ctx.lineWidth = layer.scale > 0.5 ? 1.35 : 1;
            ctx.stroke();

            const longitudeCount = 4 + layer.lines * 2;

            for (let i = 0; i < longitudeCount; i++) {
                const a = angle + i * Math.PI / longitudeCount;
                const rx = Math.max(1, Math.abs(Math.cos(a)) * r);

                ctx.beginPath();
                ctx.ellipse(0, 0, rx, r, 0, 0, Math.PI * 2);
                ctx.strokeStyle =
                    `rgba(145,225,250,${layer.alpha * 0.58})`;
                ctx.lineWidth = 0.8;
                ctx.stroke();
            }

            for (let i = 1; i <= layer.lines; i++) {
                const latitude = (i / (layer.lines + 1)) * 0.86;
                const y = Math.sin(latitude * Math.PI / 2) * r;
                const rx = Math.cos(latitude * Math.PI / 2) * r;

                for (const sign of [-1, 1]) {
                    ctx.beginPath();
                    ctx.ellipse(
                        0,
                        y * sign,
                        Math.max(1, rx),
                        Math.max(1, rx * 0.18),
                        angle * 0.12,
                        0,
                        Math.PI * 2
                    );

                    ctx.strokeStyle =
                        `rgba(130,215,245,${layer.alpha * 0.40})`;
                    ctx.lineWidth = 0.7;
                    ctx.stroke();
                }
            }

            if (layer.scale > 0.7) {
                ctx.beginPath();
                ctx.ellipse(
                    0, 0, r * 1.09, r * 0.27,
                    angle * 0.24, 0, Math.PI * 2
                );

                ctx.strokeStyle = "rgba(130,225,255,0.28)";
                ctx.lineWidth = 1;
                ctx.stroke();
            }

            ctx.restore();
        }

        function draw(time = performance.now()) {
            background();

            for (const layer of layers) {
                drawLayer(layer, time / 1000);
            }
        }

        function render(time) {
            frame = 0;

            if (document.hidden) return;

            const dt = lastTime
                ? Math.min((time - lastTime) / 1000, 0.05)
                : 1 / 60;

            lastTime = time;

            if (!reducedMotion.matches) {
                if (!dragging) {
                    rotationY += IDLE_SPEED * dt + velocityX * dt;
                    rotationX += velocityY * dt;

                    const damping = Math.exp(-INERTIA * dt);
                    velocityX *= damping;
                    velocityY *= damping;
                }

                rotationX = Math.max(-1.1, Math.min(1.1, rotationX));
            }

            draw(time);

            if (!reducedMotion.matches) {
                frame = requestAnimationFrame(render);
            }
        }

        function animate() {
            if (frame || document.hidden) return;

            lastTime = 0;
            frame = requestAnimationFrame(render);
        }

        // Hover only changes the cursor. It never rotates the sphere.
        canvas.addEventListener("pointermove", event => {
            if (dragging && event.pointerId === activePointerId) {
                const now = performance.now();
                const dt = Math.max(
                    (now - lastPointerTime) / 1000,
                    0.001
                );

                const dx = event.clientX - lastX;
                const dy = event.clientY - lastY;

                rotationY += dx * DRAG_SPEED;
                rotationX += dy * DRAG_SPEED;

                velocityX = (dx * DRAG_SPEED) / dt;
                velocityY = (dy * DRAG_SPEED) / dt;

                velocityX = Math.max(-3, Math.min(3, velocityX));
                velocityY = Math.max(-3, Math.min(3, velocityY));

                rotationX = Math.max(-1.1, Math.min(1.1, rotationX));

                lastX = event.clientX;
                lastY = event.clientY;
                lastPointerTime = now;

                if (reducedMotion.matches) {
                    draw();
                } else {
                    animate();
                }

                return;
            }

            if (dragging) return;

            const rect = canvas.getBoundingClientRect();
            const x = event.clientX - rect.left;
            const y = event.clientY - rect.top;

            canvas.classList.toggle(
                "is-near-sphere",
                isNearSphere(x, y)
            );
        });

        // Dragging starts only on a mouse/touch press over the sphere.
        canvas.addEventListener("pointerdown", event => {
            if (dragging) return;

            // Only respond to the primary mouse button.
            if (event.pointerType === "mouse" && event.button !== 0) {
                return;
            }

            const rect = canvas.getBoundingClientRect();
            const x = event.clientX - rect.left;
            const y = event.clientY - rect.top;

            if (!isNearSphere(x, y)) return;

            dragging = true;
            activePointerId = event.pointerId;

            lastX = event.clientX;
            lastY = event.clientY;
            lastPointerTime = performance.now();

            velocityX = 0;
            velocityY = 0;

            canvas.classList.add("is-dragging");
            canvas.classList.remove("is-near-sphere");

            try {
                canvas.setPointerCapture(event.pointerId);
            } catch (_) {
                // Continue if pointer capture is unavailable.
            }

            event.preventDefault();
        });

        function stopDragging(event) {
            if (!dragging) return;

            if (
                event &&
                activePointerId !== null &&
                event.pointerId !== activePointerId
            ) {
                return;
            }

            dragging = false;
            activePointerId = null;

            canvas.classList.remove("is-dragging");

            if (!reducedMotion.matches) {
                animate();
            } else {
                velocityX = 0;
                velocityY = 0;
                draw();
            }
        }

        canvas.addEventListener("pointerup", stopDragging);
        canvas.addEventListener("pointercancel", stopDragging);
        canvas.addEventListener("lostpointercapture", stopDragging);

        canvas.addEventListener("pointerleave", () => {
            if (!dragging) {
                canvas.classList.remove("is-near-sphere");
            }
        });

        // Prevent the browser from scrolling when interacting by touch.
        canvas.addEventListener("wheel", event => {
            event.preventDefault();

            radius = Math.max(
                100,
                Math.min(300, radius - event.deltaY * 0.15)
            );

            if (reducedMotion.matches && !dragging) {
                draw();
            } else {
                animate();
            }
        }, { passive: false });

        document.addEventListener("visibilitychange", () => {
            if (document.hidden) {
                if (frame) cancelAnimationFrame(frame);
                frame = 0;
                lastTime = 0;
            } else if (!reducedMotion.matches || dragging) {
                animate();
            } else {
                draw();
            }
        });

        window.addEventListener("resize", resize);

        if (page && "ResizeObserver" in window) {
            new ResizeObserver(resize).observe(page);
        }

        reducedMotion.addEventListener?.("change", () => {
            if (frame) cancelAnimationFrame(frame);

            frame = 0;
            lastTime = 0;

            if (!reducedMotion.matches || dragging) {
                animate();
            } else {
                velocityX = 0;
                velocityY = 0;
                draw();
            }
        });

        resize();
    }

    if (document.readyState === "loading") {
        document.addEventListener(
            "DOMContentLoaded",
            startNetwork,
            { once: true }
        );
    } else {
        startNetwork();
    }
})();
