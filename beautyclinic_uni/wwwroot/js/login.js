function togglePassword() {
    const input = document.getElementById("password");
    input.type = input.type === "password" ? "text" : "password";
}

function handleSubmit(e) {
    e.preventDefault();
    const msg = document.getElementById("formMsg");
    msg.textContent = "ورود با موفقیت شبیه‌سازی شد.";
    msg.style.color = "#065f46";
}
