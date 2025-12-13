const clinics = [
    {
        name: "کلینیک مهر",
        specialty: "پوست",
        specialtyTitle: "پوست و مو",
        address: "تهران، ولیعصر",
        phone: "021-11111111",
        hours: "9 الی 18",
        img: ""
    },
    {
        name: "کلینیک سپید",
        specialty: "دندان",
        specialtyTitle: "دندانپزشکی",
        address: "تهران، صادقیه",
        phone: "021-22222222",
        hours: "10 الی 19",
        img: ""
    },
    {
        name: "کلینیک قلب ایران",
        specialty: "قلب",
        specialtyTitle: "قلب و عروق",
        address: "تهران، ونک",
        phone: "021-33333333",
        hours: "8 الی 16",
        img: ""
    },
    {
        name: "کلینیک مادر",
        specialty: "زنان",
        specialtyTitle: "زنان و زایمان",
        address: "تهران، تجریش",
        phone: "021-44444444",
        hours: "9 الی 17",
        img: ""
    }
];

const box = document.getElementById("cards");

function render(list) {
    box.innerHTML = "";
    list.forEach((c, i) => {
        const imgHtml = c.img
            ? `<img src="${c.img}" alt="عکس ${escapeHtml(c.name)}">`
            : `<span>عکس کلینیک</span>`;

        box.innerHTML += `
      <div class="card" data-index="${i}" onclick="toggle(${i})" role="button" aria-expanded="false">
        <div>
          <div class="card-img" id="img-wrap-${i}">
            ${imgHtml}
          </div>
        </div>
        <div class="card-content">
          <div class="card-name">${escapeHtml(c.name)}</div>
          <div class="card-spec">تخصص: ${escapeHtml(c.specialtyTitle)}</div>
          <div class="clinic-info" id="info-${i}">
            📍 <b>آدرس:</b> ${escapeHtml(c.address)}<br>
            ☎️ <b>تلفن:</b> ${escapeHtml(c.phone)}<br>
            ⏰ <b>ساعات کاری:</b> ${escapeHtml(c.hours)}
          </div>
        </div>
      </div>
    `;
    });
}

function escapeHtml(text) {
    if (!text && text !== 0) return "";
    return String(text)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function toggle(i) {
    document.querySelectorAll(".clinic-info").forEach((e) => e.style.display = "none");
    document.querySelectorAll(".card").forEach((c) => c.setAttribute("aria-expanded", "false"));
    const el = document.getElementById("info-" + i);
    if (!el) return;
    const isVisible = el.style.display === "block";
    el.style.display = isVisible ? "none" : "block";
    const card = document.querySelector(`.card[data-index="${i}"]`);
    if (card) card.setAttribute("aria-expanded", isVisible ? "false" : "true");
}

render(clinics);
