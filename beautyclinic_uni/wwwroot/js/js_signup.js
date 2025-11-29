function togglePass() {
    const p = document.getElementById("password");
    p.type = p.type === "password" ? "text" : "password";
}

function submitReg(e) {
    e.preventDefault();
    const msg = document.getElementById("regMsg");
    msg.textContent = "ثبت‌نام با موفقیت انجام شد (شبیه‌سازی).";
    msg.style.color = "#065f46";
}
