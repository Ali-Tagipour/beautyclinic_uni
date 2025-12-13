const doctors = [
    {
        name: "دکتر سارا محمدی",
        degree: "متخصص پوست و مو - دانشگاه تهران",
        experience: "۱۰ سال تجربه",
        specialty: "پوست، زیبایی و لیزر",
        image: "https://s34.picofile.com/file/8488589318/dr6.png"
    },
    {
        name: "دکتر علی کریمی",
        degree: "جراح عمومی - دانشگاه شهید بهشتی",
        experience: "۱۲ سال تجربه",
        specialty: "جراحی زیبایی و ترمیمی",
        image: "https://s34.picofile.com/file/8488589268/dr1.png"
    },
    {
        name: "دکتر فاطمه رضایی",
        degree: "متخصص دندانپزشکی زیبایی - دانشگاه علوم پزشکی ایران",
        experience: "۸ سال تجربه",
        specialty: "ایمپلنت و لمینت",
        image: "https://s34.picofile.com/file/8488589276/dr2.png"
    },
    {
        name: "دکتر حسن سلطانی",
        degree: "متخصص قلب و عروق - دانشگاه مشهد",
        experience: "۱۵ سال تجربه",
        specialty: "تشخیص و درمان بیماری‌های قلبی",
        image: "https://s34.picofile.com/file/8488589284/dr3.png"
    },
    {
        name: "دکتر نگین آقاجانی",
        degree: "متخصص زنان و زایمان - دانشگاه شیراز",
        experience: "۹ سال تجربه",
        specialty: "زنان، زایمان، نازایی",
        image: "https://s34.picofile.com/file/8488589292/dr4.png"
    }
];

const container = document.getElementById("doctorsContainer");

function renderDoctors(list) {
    container.innerHTML = "";

    list.forEach(d => {
        const card = document.createElement("div");
        card.className = "doctor-card";

        card.innerHTML = `
            <div class="doctor-info">
                <h2>${d.name}</h2>
                <p><strong>مدرک:</strong> ${d.degree}</p>
                <p><strong>تجربه:</strong> ${d.experience}</p>
                <p><strong>تخصص:</strong> ${d.specialty}</p>
            </div>
            <img src="${d.image}" alt="${d.name}">
        `;

        container.appendChild(card);
    });
}

function filterDoctors(keyword, btn) {
    document.querySelectorAll(".service-btn")
        .forEach(b => b.classList.remove("active"));

    btn.classList.add("active");

    if (keyword === "همه") {
        renderDoctors(doctors);
        return;
    }

    const filtered = doctors.filter(d =>
        d.specialty.includes(keyword)
    );

    renderDoctors(filtered);
}

renderDoctors(doctors);
