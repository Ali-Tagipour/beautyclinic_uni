// کلیدهای ذخیره‌سازی
const STORAGE = {
    USER: 'clinic_user_info_v3',
    DOCTOR: 'clinic_doctor_info_v3',
    APPTS: 'clinic_appts_v7',
    PATIENTS: 'clinic_patients_v7',
    TRACK: 'clinic_track_v7',
    PAYMENTS_CURRENT: 'payments_current_v7',
    PAYMENTS_PREVIOUS: 'payments_previous_v7',
    PAYMENTS: 'clinic_payments_v7',
    CONSULTS: 'clinic_consults_v7'
};
const load = (key, def) => { try { const v = localStorage.getItem(key); return v ? JSON.parse(v) : def; } catch (e) { return def } };
const save = (key, data) => localStorage.setItem(key, JSON.stringify(data));
const remove = (key) => localStorage.removeItem(key);

// داده‌ها
let services = [
    { id: 1, title: 'تزریق بوتاکس', desc: 'کاهش چین و چروک', price: 2000000 },
    { id: 2, title: 'لیزر پوست', desc: 'رفع لک و موهای زائد', price: 1500000 },
    { id: 3, title: 'پاکسازی صورت', desc: 'پاکسازی و جوان‌سازی', price: 800000 },
    { id: 4, title: 'کاشت مو', desc: 'ترمیم طاسی و کم‌پشتی', price: 10000000 }
];
let appointments = load(STORAGE.APPTS, [
    { id: 1, name: 'مریم احمدی', date: '2025-12-10', time: '10:00', service: 'لیزر پوست', status: 'در انتظار', note: '' },
    { id: 2, name: 'علی رضایی', date: '2025-12-11', time: '12:00', service: 'تزریق بوتاکس', status: 'تایید شده', note: '' }
]);
let patients = load(STORAGE.PATIENTS, [
    { id: 1, name: 'سارا محمدی', age: 40, service: 'کاشت مو', status: 'جلسه اول', details: 'توضیحات پرونده سارا' },
    { id: 2, name: 'رضا کریمی', age: 35, service: 'لیزر پوست', status: 'در حال درمان', details: 'توضیحات پرونده رضا' }
]);
let tracking = load(STORAGE.TRACK, [
    { id: 1, session: 1, date: '2025-12-01', status: 'انجام شد' },
    { id: 2, session: 2, date: '2025-12-15', status: 'در انتظار' }
]);
let payments = load(STORAGE.PAYMENTS, []);

// نمونه گزارش ماه جاری و قبل
const SAMPLE_CURRENT = [
    { id: 1, date: '2025-11-05', patientName: 'مریم احمدی', service: 'لیزر پوست', doctor: 'دکتر حسینی', amount: 1500000 },
    { id: 2, date: '2025-11-07', patientName: 'علی رضایی', service: 'تزریق بوتاکس', doctor: 'دکتر موسوی', amount: 2000000 },
    { id: 3, date: '2025-11-10', patientName: 'سارا محمدی', service: 'کاشت مو', doctor: 'دکتر حسینی', amount: 10000000 },
    { id: 4, date: '2025-11-12', patientName: 'نرگس رضایی', service: 'پاکسازی صورت', doctor: 'دکتر موسوی', amount: 800000 },
    { id: 5, date: '2025-11-18', patientName: 'رضا کریمی', service: 'لیزر پوست', doctor: 'دکتر حسینی', amount: 1500000 },
    { id: 6, date: '2025-11-20', patientName: 'مهدی احمدی', service: 'تزریق بوتاکس', doctor: 'دکتر موسوی', amount: 2000000 }
];
const SAMPLE_PREVIOUS = [
    { id: 101, date: '2025-10-03', patientName: 'مریم احمدی', service: 'لیزر پوست', doctor: 'دکتر حسینی', amount: 1200000 },
    { id: 102, date: '2025-10-06', patientName: 'علی رضایی', service: 'تزریق بوتاکس', doctor: 'دکتر موسوی', amount: 2000000 },
    { id: 103, date: '2025-10-09', patientName: 'سارا محمدی', service: 'کاشت مو', doctor: 'دکتر حسینی', amount: 9000000 },
    { id: 104, date: '2025-10-11', patientName: 'نرگس رضایی', service: 'پاکسازی صورت', doctor: 'دکتر موسوی', amount: 700000 },
    { id: 105, date: '2025-10-22', patientName: 'رضا کریمی', service: 'لیزر پوست', doctor: 'دکتر حسینی', amount: 1500000 }
];
if (!localStorage.getItem(STORAGE.PAYMENTS_CURRENT)) save(STORAGE.PAYMENTS_CURRENT, SAMPLE_CURRENT);
if (!localStorage.getItem(STORAGE.PAYMENTS_PREVIOUS)) save(STORAGE.PAYMENTS_PREVIOUS, SAMPLE_PREVIOUS);

// المان‌ها
const btnUser = document.getElementById('btnUser');
const btnDoctor = document.getElementById('btnDoctor');
const userMenu = document.getElementById('userMenu');
const doctorMenu = document.getElementById('doctorMenu');
const sidebarTitle = document.getElementById('sidebarTitle');

const apServiceSelect = document.getElementById('ap_service');
const myApptsBody = document.getElementById('myApptsBody');
const docApptsBody = document.getElementById('docApptsBody');
const patientsBody = document.getElementById('patientsBody');
const trackingBody = document.getElementById('trackingBody');
const servicesBody = document.getElementById('servicesBody');

const payPatientSelect = document.getElementById('pay_patient');
const payDoctorSelect = document.getElementById('pay_doctor');

const periodSelect = document.getElementById('periodSelect');
const doctorFilter = document.getElementById('doctorFilter');
const serviceFilter = document.getElementById('serviceFilter');
const paymentsTableBody = document.getElementById('paymentsTableBody');
const doctorSummaryDiv = document.getElementById('doctorSummary');
const serviceBreakBody = document.getElementById('serviceBreakBody');
const refreshBtn = document.getElementById('refreshBtn');
const dateStart = document.getElementById('dateStart');
const dateEnd = document.getElementById('dateEnd');

// فرم‌های اطلاعات کاربر و پزشک
const userForm = document.getElementById('userForm');
const doctorForm = document.getElementById('doctorForm');
const userView = document.getElementById('userView');
const doctorView = document.getElementById('doctorView');

// نمودارها
let doctorBarChart = null;
let servicePieChart = null;

// نقش‌ها
btnUser.addEventListener('click', () => {
    userMenu.style.display = 'flex'; doctorMenu.style.display = 'none';
    sidebarTitle.innerHTML = '<h3>منو</h3>';
    showSection('userInfo'); renderAll();
});
btnDoctor.addEventListener('click', () => {
    doctorMenu.style.display = 'flex'; userMenu.style.display = 'none';
    sidebarTitle.innerHTML = '<h3>منو</h3>';
    showSection('doctorInfo'); renderAll();
});

// نمایش بخش
function showSection(id) {
    document.querySelectorAll('.panel').forEach(p => { p.classList.remove('active'); p.setAttribute('aria-hidden', 'true'); });
    const sec = document.getElementById(id);
    if (sec) { sec.classList.add('active'); sec.setAttribute('aria-hidden', 'false'); sec.scrollIntoView({ behavior: 'smooth', block: 'start' }); }
    renderAll();
}

// حالت نمایش/ویرایش اطلاعات کاربر
function toggleUserEdit(edit) {
    userForm.style.display = edit ? 'block' : 'none';
    userView.style.display = edit ? 'none' : 'block';
    if (edit) {
        document.getElementById('userName').value = safeText('v_userName');
        document.getElementById('userPhone').value = safeText('v_userPhone');
        document.getElementById('userEmail').value = safeText('v_userEmail');
        document.getElementById('userAddress').value = safeText('v_userAddress');
    }
}
function resetUserInfo() {
    const defaults = { name: '', phone: '', email: '', address: '' };
    save(STORAGE.USER, defaults);
    loadUserIntoView();
    toggleUserEdit(false);
    alert('اطلاعات کاربر به حالت اولیه بازنشانی شد.');
}
function clearUserInfo() {
    remove(STORAGE.USER);
    loadUserIntoView();
    toggleUserEdit(false);
    alert('اطلاعات کاربر پاک‌سازی شد.');
}
function resetUserForm() {
    const u = load(STORAGE.USER, { name: '', phone: '', email: '', address: '' });
    document.getElementById('userName').value = u.name || '';
    document.getElementById('userPhone').value = u.phone || '';
    document.getElementById('userEmail').value = u.email || '';
    document.getElementById('userAddress').value = u.address || '';
}

// حالت نمایش/ویرایش اطلاعات پزشک
function toggleDoctorEdit(edit) {
    doctorForm.style.display = edit ? 'block' : 'none';
    doctorView.style.display = edit ? 'none' : 'block';
    if (edit) {
        document.getElementById('doctorName').value = safeText('v_doctorName');
        document.getElementById('doctorSpecialty').value = safeText('v_doctorSpecialty');
        document.getElementById('doctorPhone').value = safeText('v_doctorPhone');
        document.getElementById('doctorEmail').value = safeText('v_doctorEmail');
    }
}
function resetDoctorInfo() {
    const defaults = { name: '', specialty: '', phone: '', email: '' };
    save(STORAGE.DOCTOR, defaults);
    loadDoctorIntoView();
    toggleDoctorEdit(false);
    alert('اطلاعات پزشک به حالت اولیه بازنشانی شد.');
}
function clearDoctorInfo() {
    remove(STORAGE.DOCTOR);
    loadDoctorIntoView();
    toggleDoctorEdit(false);
    alert('اطلاعات پزشک پاک‌سازی شد.');
}
function resetDoctorForm() {
    const d = load(STORAGE.DOCTOR, { name: '', specialty: '', phone: '', email: '' });
    document.getElementById('doctorName').value = d.name || '';
    document.getElementById('doctorSpecialty').value = d.specialty || '';
    document.getElementById('doctorPhone').value = d.phone || '';
    document.getElementById('doctorEmail').value = d.email || '';
}

function safeText(id) {
    const t = document.getElementById(id).textContent;
    return t === '—' ? '' : t;
}

// ذخیره و بارگذاری اطلاعات کاربر/پزشک
userForm.addEventListener('submit', (e) => {
    e.preventDefault();
    const data = {
        name: document.getElementById('userName').value.trim(),
        phone: document.getElementById('userPhone').value.trim(),
        email: document.getElementById('userEmail').value.trim(),
        address: document.getElementById('userAddress').value.trim()
    };
    save(STORAGE.USER, data);
    loadUserIntoView();
    toggleUserEdit(false);
    alert('اطلاعات کاربر ذخیره شد');
});
doctorForm.addEventListener('submit', (e) => {
    e.preventDefault();
    const data = {
        name: document.getElementById('doctorName').value.trim(),
        specialty: document.getElementById('doctorSpecialty').value.trim(),
        phone: document.getElementById('doctorPhone').value.trim(),
        email: document.getElementById('doctorEmail').value.trim()
    };
    save(STORAGE.DOCTOR, data);
    loadDoctorIntoView();
    toggleDoctorEdit(false);
    alert('اطلاعات پزشک ذخیره شد');
    populateDoctorFilterFromInfo();
});

function loadUserIntoView() {
    const u = load(STORAGE.USER, null);
    const hasData = !!(u && (u.name || u.phone || u.email || u.address));
    userView.style.display = hasData ? 'block' : 'none';
    userForm.style.display = hasData ? 'none' : 'block';
    setText('v_userName', u?.name);
    setText('v_userPhone', u?.phone);
    setText('v_userEmail', u?.email);
    setText('v_userAddress', u?.address);
    if (!hasData) { resetUserForm(); }
}
function loadDoctorIntoView() {
    const d = load(STORAGE.DOCTOR, null);
    const hasData = !!(d && (d.name || d.specialty || d.phone || d.email));
    doctorView.style.display = hasData ? 'block' : 'none';
    doctorForm.style.display = hasData ? 'none' : 'block';
    setText('v_doctorName', d?.name);
    setText('v_doctorSpecialty', d?.specialty);
    setText('v_doctorPhone', d?.phone);
    setText('v_doctorEmail', d?.email);
    if (!hasData) { resetDoctorForm(); }
}
function setText(id, val) { document.getElementById(id).textContent = val && val.trim() ? val : '—'; }

// خدمات و پرداخت
function renderServices() {
    servicesBody.innerHTML = '';
    services.forEach(s => {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td>${s.title}</td><td>${s.desc}</td><td>${s.price.toLocaleString()} تومان</td>
      <td><button class="pay-link" onclick="payFor(${s.id})">پرداخت ${s.price.toLocaleString()} تومان</button></td>`;
        servicesBody.appendChild(tr);
    });
    apServiceSelect.innerHTML = '';
    services.forEach(s => apServiceSelect.innerHTML += `<option value="${s.title}">${s.title}</option>`);
}

function populatePatientDoctorSelects() {
    // بیماران
    payPatientSelect.innerHTML = '';
    if (patients.length === 0) {
        const u = load(STORAGE.USER, {});
        if (u.name) payPatientSelect.innerHTML = `<option value="${u.name}">${u.name}</option>`;
        else payPatientSelect.innerHTML = '<option value="مراجع ناشناس">مراجع ناشناس</option>';
    } else {
        patients.forEach(p => payPatientSelect.innerHTML += `<option value="${p.name}">${p.name}</option>`);
    }
    // پزشکان
    const doctorSet = new Set();
    const cur = load(STORAGE.PAYMENTS_CURRENT, []);
    const prev = load(STORAGE.PAYMENTS_PREVIOUS, []);
    cur.concat(prev).forEach(x => doctorSet.add(x.doctor));
    payments.forEach(x => doctorSet.add(x.doctor));
    const dInfo = load(STORAGE.DOCTOR, {});
    if (dInfo.name) doctorSet.add(dInfo.name);
    if (doctorSet.size === 0) { doctorSet.add('دکتر حسینی'); doctorSet.add('دکتر موسوی'); }
    payDoctorSelect.innerHTML = '';
    Array.from(doctorSet).forEach(d => payDoctorSelect.innerHTML += `<option value="${d}">${d}</option>`);
    // فیلتر پزشک
    doctorFilter.innerHTML = '<option value="">همه پزشکان</option>';
    Array.from(doctorSet).forEach(d => doctorFilter.innerHTML += `<option value="${d}">${d}</option>`);
}

function populateDoctorFilterFromInfo() {
    const dInfo = load(STORAGE.DOCTOR, {});
    if (dInfo.name) {
        const exists = Array.from(doctorFilter.options).some(o => o.value === dInfo.name);
        if (!exists) doctorFilter.innerHTML += `<option value="${dInfo.name}">${dInfo.name}</option>`;
    }
}

function payFor(serviceId) {
    const s = services.find(x => x.id === serviceId); if (!s) return;
    document.getElementById('pay_amount').value = s.price;
    document.getElementById('pay_desc').value = s.title + ' - ' + s.desc;
    populatePatientDoctorSelects();
    showSection('payment');
}

document.getElementById('paymentForm').addEventListener('submit', function (e) {
    e.preventDefault();
    const patientName = document.getElementById('pay_patient').value;
    const doctor = document.getElementById('pay_doctor').value;
    const amount = Number(document.getElementById('pay_amount').value);
    const desc = document.getElementById('pay_desc').value;
    const id = payments.length ? Math.max(...payments.map(p => p.id)) + 1 : 1;
    const date = new Date().toISOString().split('T')[0];
    payments.push({ id, date, patientName, service: desc.split(' - ')[0], doctor, amount });
    save(STORAGE.PAYMENTS, payments);
    document.getElementById('paymentMsg').style.display = 'block';
    document.getElementById('paymentMsg').textContent = 'پرداخت با موفقیت ثبت شد.';
    setTimeout(() => document.getElementById('paymentMsg').style.display = 'none', 2500);
    this.reset();
    renderAll();
    showSection('services');
});

// نوبت‌ها، بیماران، پیگیری
function renderAppointmentsTables() {
    // پزشک
    docApptsBody.innerHTML = '';
    appointments.forEach(ap => {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td>${ap.name}</td><td>${ap.date}</td><td>${ap.time}</td><td>${ap.service}</td><td id="status-${ap.id}">${ap.status}</td>
      <td class="inline-actions">
        <button class="small-btn" onclick="changeApptStatus(${ap.id},'تایید شده')">تایید</button>
        <button class="small-btn muted-btn" onclick="changeApptStatus(${ap.id},'لغو شده')">لغو</button>
        <button class="small-btn" onclick="openEditAppt(${ap.id})">ویرایش</button>
        <button class="small-btn danger" onclick="deleteAppointment(${ap.id})">حذف</button>
      </td>`;
        docApptsBody.appendChild(tr);
    });

    // نوبت‌های من
    myApptsBody.innerHTML = '';
    appointments.forEach(ap => {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td>${ap.name}</td><td>${ap.date}</td><td>${ap.time}</td><td>${ap.service}</td><td id="mystatus-${ap.id}">${ap.status}</td>
      <td class="inline-actions">
        <button class="small-btn" onclick="openEditAppt(${ap.id})">ویرایش</button>
        <button class="small-btn muted-btn" onclick="changeApptStatus(${ap.id},'لغو شده')">لغو</button>
        <button class="small-btn danger" onclick="deleteAppointment(${ap.id})">حذف</button>
      </td>`;
        myApptsBody.appendChild(tr);
    });
}

function changeApptStatus(id, status) {
    const ap = appointments.find(a => a.id === id); if (!ap) return;
    ap.status = status; save(STORAGE.APPTS, appointments); renderAll();
    alert('وضعیت نوبت تغییر کرد: ' + status);
}

function deleteAppointment(id) {
    if (!confirm('آیا از حذف این نوبت مطمئن هستید؟')) return;
    appointments = appointments.filter(a => a.id !== id); save(STORAGE.APPTS, appointments); renderAll();
}

function openEditAppt(id) {
    const ap = appointments.find(a => a.id === id); if (!ap) return;
    const modal = document.createElement('div'); modal.style.position = 'fixed'; modal.style.inset = '0'; modal.style.background = 'rgba(0,0,0,0.4)'; modal.style.display = 'flex'; modal.style.alignItems = 'center'; modal.style.justifyContent = 'center'; modal.style.zIndex = 9999;
    modal.innerHTML = `<div style="background:#fff;padding:16px;border-radius:10px;width:90%;max-width:520px;">
    <h4 style="margin:0 0 8px 0;color:#7a2f4d">ویرایش نوبت</h4>
    <label>نام</label><input id="edit_name" value="${ap.name}" />
    <div style="display:flex;gap:8px;margin-top:8px;">
      <div style="flex:1"><label>تاریخ</label><input id="edit_date" type="date" value="${ap.date}" /></div>
      <div style="flex:1"><label>ساعت</label><input id="edit_time" type="time" value="${ap.time}" /></div>
    </div>
    <label style="margin-top:8px">خدمت</label><input id="edit_service" value="${ap.service}" />
    <label style="margin-top:8px">توضیحات</label><textarea id="edit_note">${ap.note || ''}</textarea>
    <div style="display:flex;gap:8px;justify-content:flex-end;margin-top:12px;">
      <button id="saveEdit" class="btn-primary">ذخیره</button>
      <button id="cancelEdit" class="btn-ghost">انصراف</button>
    </div>
  </div>`;
    document.body.appendChild(modal);
    document.getElementById('cancelEdit').addEventListener('click', () => modal.remove());
    document.getElementById('saveEdit').addEventListener('click', () => {
        ap.name = document.getElementById('edit_name').value.trim();
        ap.date = document.getElementById('edit_date').value;
        ap.time = document.getElementById('edit_time').value;
        ap.service = document.getElementById('edit_service').value.trim();
        ap.note = document.getElementById('edit_note').value.trim();
        save(STORAGE.APPTS, appointments); renderAll(); modal.remove();
    });
}

function renderPatientsTable() {
    patientsBody.innerHTML = '';
    patients.forEach(p => {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td>${p.name}</td><td>${p.age}</td><td>${p.service}</td><td id="pstatus-${p.id}">${p.status}</td>
      <td class="inline-actions">
        <button class="small-btn" onclick="viewPatient(${p.id})">مشاهده</button>
        <button class="small-btn" onclick="editPatient(${p.id})">ویرایش</button>
        <button class="small-btn danger" onclick="deletePatient(${p.id})">حذف</button>
      </td>`;
        patientsBody.appendChild(tr);
    });
}

function viewPatient(id) {
    const p = patients.find(x => x.id === id); if (!p) return;
    document.getElementById('pdName').textContent = p.name + ' — ' + p.age + ' سال';
    document.getElementById('pdInfo').textContent = 'خدمت: ' + p.service + ' | وضعیت: ' + p.status + ' | ' + p.details;
    document.getElementById('patientEditArea').innerHTML = '';
    document.getElementById('patientDetail').style.display = 'block';
    document.getElementById('patientDetail').scrollIntoView({ behavior: 'smooth', block: 'center' });
}

function editPatient(id) {
    const p = patients.find(x => x.id === id); if (!p) return;
    document.getElementById('pdName').textContent = 'ویرایش پرونده ' + p.name;
    const area = document.getElementById('patientEditArea');
    area.innerHTML = `
    <label>نام</label><input id="pat_name" value="${p.name}" />
    <label>سن</label><input id="pat_age" type="number" value="${p.age}" />
    <label>خدمت</label><input id="pat_service" value="${p.service}" />
    <label>وضعیت</label><input id="pat_status" value="${p.status}" />
    <label>جزئیات</label><textarea id="pat_details">${p.details}</textarea>
    <div style="display:flex;gap:8px;justify-content:flex-end;margin-top:8px;">
      <button id="savePatient" class="btn-primary">ذخیره</button>
      <button id="cancelPatient" class="btn-ghost">انصراف</button>
    </div>`;
    document.getElementById('savePatient').addEventListener('click', () => {
        p.name = document.getElementById('pat_name').value.trim();
        p.age = parseInt(document.getElementById('pat_age').value) || p.age;
        p.service = document.getElementById('pat_service').value.trim();
        p.status = document.getElementById('pat_status').value.trim();
        p.details = document.getElementById('pat_details').value.trim();
        save(STORAGE.PATIENTS, patients); renderAll(); viewPatient(p.id);
    });
    document.getElementById('cancelPatient').addEventListener('click', () => viewPatient(p.id));
}

function deletePatient(id) {
    if (!confirm('آیا از حذف این پرونده مطمئن هستید؟')) return;
    patients = patients.filter(p => p.id !== id); save(STORAGE.PATIENTS, patients); renderAll(); document.getElementById('patientDetail').style.display = 'none';
}

// گزارش مالی
function loadPayments(period) {
    const key = period === 'current' ? STORAGE.PAYMENTS_CURRENT : STORAGE.PAYMENTS_PREVIOUS;
    return load(key, []);
}
function summarizeByDoctor(paymentsArr) {
    const map = {};
    paymentsArr.forEach(p => {
        if (!map[p.doctor]) map[p.doctor] = { total: 0, payments: [] };
        map[p.doctor].total += Number(p.amount);
        map[p.doctor].payments.push(p);
    });
    return map;
}
function computeDoctorChanges(curPayments, prevPayments) {
    const cur = summarizeByDoctor(curPayments);
    const prev = summarizeByDoctor(prevPayments);
    const doctors = new Set([...Object.keys(cur), ...Object.keys(prev)]);
    const result = [];
    doctors.forEach(doc => {
        const current = cur[doc]?.total || 0;
        const previous = prev[doc]?.total || 0;
        const diff = current - previous;
        const pct = previous === 0 ? (current === 0 ? 0 : (current > 0 ? 100 : 0)) : (diff / previous) * 100;
        result.push({ doctor: doc, current, previous, diff, pct, payments: cur[doc]?.payments || [] });
    });
    result.sort((a, b) => b.diff - a.diff);
    return result;
}
function renderPaymentsTable(paymentsArr) {
    paymentsTableBody.innerHTML = '';
    paymentsArr.forEach((p, idx) => {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td>${idx + 1}</td><td>${p.date}</td><td>${p.patientName}</td><td>${p.service}</td><td>${p.doctor}</td><td>${Number(p.amount).toLocaleString()}</td>`;
        paymentsTableBody.appendChild(tr);
    });
}
function renderDoctorSummary(changes) {
    doctorSummaryDiv.innerHTML = '';
    changes.forEach(item => {
        const box = document.createElement('div');
        box.style.background = 'linear-gradient(180deg,var(--accent-purple-soft),#ffffff)';
        box.style.border = '1px solid rgba(0,0,0,0.06)'; box.style.borderRadius = '10px'; box.style.padding = '12px'; box.style.marginBottom = '10px';
        const changeColor = item.diff >= 0 ? 'green' : '#e74c3c';
        const sign = item.diff >= 0 ? '+' : '';
        box.innerHTML = `
      <div style="display:flex;justify-content:space-between;align-items:center">
        <div>
          <div style="font-weight:700;color:#3f2b7a">${item.doctor}</div>
          <div class="muted-text">جمع درآمد فعلی: <strong>${item.current.toLocaleString()}</strong> تومان</div>
          <div class="muted-text">جمع درآمد قبلی: <strong>${item.previous.toLocaleString()}</strong> تومان</div>
        </div>
        <div style="text-align:center">
          <div style="color:${changeColor};font-size:18px">${sign}${item.diff.toLocaleString()} تومان</div>
          <div style="color:${changeColor};font-size:13px">${item.pct.toFixed(1)}%</div>
        </div>
      </div>
      <div style="margin-top:8px">
        <div class="muted-text">نمونه پرداخت‌های اخیر:</div>
        <table style="width:100%;margin-top:6px;font-size:12px">
          <thead><tr><th>بیمار</th><th>خدمت</th><th>تاریخ</th><th>مبلغ</th></tr></thead>
          <tbody>
            ${item.payments.slice(0, 4).map(p => `<tr><td>${p.patientName}</td><td>${p.service}</td><td>${p.date}</td><td>${Number(p.amount).toLocaleString()}</td></tr>`).join('')}
          </tbody>
        </table>
      </div>`;
        doctorSummaryDiv.appendChild(box);
    });
}
function renderServiceBreakdown(paymentsArr) {
    const map = {};
    paymentsArr.forEach(p => {
        if (!map[p.service]) map[p.service] = { count: 0, total: 0, doctors: new Set() };
        map[p.service].count += 1;
        map[p.service].total += Number(p.amount);
        map[p.service].doctors.add(p.doctor);
    });
    serviceBreakBody.innerHTML = '';
    Object.keys(map).forEach(s => {
        const row = document.createElement('tr');
        row.innerHTML = `<td>${s}</td><td>${map[s].count}</td><td>${map[s].total.toLocaleString()}</td><td>${Array.from(map[s].doctors).join('، ')}</td>`;
        serviceBreakBody.appendChild(row);
    });
}
function populateFilters(paymentsArr) {
    const doctors = new Set(paymentsArr.map(p => p.doctor));
    const servicesSet = new Set();
    serviceFilter.innerHTML = '<option value="">همه خدمات</option>';
    paymentsArr.forEach(p => servicesSet.add(p.service));
    doctorFilter.innerHTML = '<option value="">همه پزشکان</option>';
    doctors.forEach(d => doctorFilter.innerHTML += `<option value="${d}">${d}</option>`);
    servicesSet.forEach(s => serviceFilter.innerHTML += `<option value="${s}">${s}</option>`);
    populateDoctorFilterFromInfo();
}
function applyFilters(paymentsArr) {
    const doc = doctorFilter.value;
    const svc = serviceFilter.value;
    const start = dateStart.value;
    const end = dateEnd.value;
    return paymentsArr.filter(p => {
        if (doc && p.doctor !== doc) return false;
        if (svc && p.service !== svc) return false;
        if (start && (new Date(p.date) < new Date(start))) return false;
        if (end && (new Date(p.date) > new Date(end))) return false;
        return true;
    });
}

// نمودارها با پالت جدید
function renderCharts(currentPayments, previousPayments) {
    const changes = computeDoctorChanges(currentPayments, previousPayments);
    const labels = changes.map(c => c.doctor);
    const currentData = changes.map(c => c.current);
    const previousData = changes.map(c => c.previous);

    const barCtx = document.getElementById('doctorBarChart').getContext('2d');
    if (doctorBarChart) doctorBarChart.destroy();
    doctorBarChart = new Chart(barCtx, {
        type: 'bar',
        data: {
            labels,
            datasets: [
                { label: 'ماه جاری', data: currentData, backgroundColor: 'rgba(249,168,212,0.9)', borderColor: '#f4b8dc', borderWidth: 1, borderRadius: 4 },
                { label: 'ماه قبل', data: previousData, backgroundColor: 'rgba(196,181,253,0.6)', borderColor: '#c4b5fd', borderWidth: 1, borderRadius: 4 }
            ]
        },
        options: {
            responsive: true, maintainAspectRatio: false,
            scales: { y: { beginAtZero: true, ticks: { callback: v => v.toLocaleString() } } },
            plugins: { legend: { position: 'top', labels: { boxWidth: 10, boxHeight: 6, font: { size: 11 }, color: '#3f2b7a' } } }
        }
    });

    const serviceMap = {};
    currentPayments.forEach(p => { serviceMap[p.service] = (serviceMap[p.service] || 0) + Number(p.amount); });
    const svcLabels = Object.keys(serviceMap);
    const svcValues = svcLabels.map(k => serviceMap[k]);
    const pieCtx = document.getElementById('servicePieChart').getContext('2d');
    if (servicePieChart) servicePieChart.destroy();
    servicePieChart = new Chart(pieCtx, {
        type: 'pie',
        data: {
            labels: svcLabels,
            datasets: [{
                data: svcValues,
                backgroundColor: ['rgba(249,168,212,0.9)', 'rgba(196,181,253,0.9)', 'rgba(249,168,212,0.6)', 'rgba(196,181,253,0.6)', 'rgba(233,213,255,0.9)'],
                borderColor: ['#f4b8dc', '#c4b5fd', '#f4b8dc', '#c4b5fd', '#e9d5ff'], borderWidth: 1
            }]
        },
        options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { position: 'bottom', labels: { boxWidth: 10, font: { size: 11 }, color: '#3f2b7a' } } } }
    });
}

function updateReport() {
    const period = periodSelect.value;
    const currentPayments = loadPayments(period);
    const previousPayments = loadPayments(period === 'current' ? 'previous' : 'current');

    const filtered = applyFilters(currentPayments);
    renderPaymentsTable(filtered);
    populateFilters(currentPayments);

    const changes = computeDoctorChanges(loadPayments('current'), loadPayments('previous'));
    renderDoctorSummary(changes);

    renderServiceBreakdown(currentPayments);
    renderCharts(loadPayments('current'), loadPayments('previous'));
}

// رویدادهای گزارش
refreshBtn.addEventListener('click', updateReport);
periodSelect.addEventListener('change', updateReport);
doctorFilter.addEventListener('change', updateReport);
serviceFilter.addEventListener('change', updateReport);
dateStart.addEventListener('change', updateReport);
dateEnd.addEventListener('change', updateReport);

// رزرو نوبت
document.getElementById('appointmentForm').addEventListener('submit', function (e) {
    e.preventDefault();
    const name = document.getElementById('ap_name').value.trim();
    const date = document.getElementById('ap_date').value;
    const time = document.getElementById('ap_time').value;
    const service = document.getElementById('ap_service').value;
    const note = document.getElementById('ap_note').value.trim();
    const id = appointments.length ? Math.max(...appointments.map(a => a.id)) + 1 : 1;
    appointments.push({ id, name, date, time, service, status: 'در انتظار', note });
    save(STORAGE.APPTS, appointments);
    renderAll();
    alert('نوبت شما ثبت شد.');
    this.reset();
    showSection('appointment');
});

// جدول پیگیری
function renderTracking() {
    trackingBody.innerHTML = '';
    tracking.forEach(t => {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td>${t.session}</td><td>${t.date}</td><td>${t.status}</td>`;
        trackingBody.appendChild(tr);
    });
}

// رندر کلی
function renderAll() {
    loadUserIntoView();
    loadDoctorIntoView();
    renderServices();
    renderAppointmentsTables();
    renderPatientsTable();
    renderTracking();
    populatePatientDoctorSelects();
    updateReport();
}

// مقداردهی اولیه
renderAll();

// دسترسی عمومی
window.payFor = payFor;
window.changeApptStatus = changeApptStatus;
window.openEditAppt = openEditAppt;
window.deleteAppointment = deleteAppointment;
window.viewPatient = viewPatient;
window.editPatient = editPatient;
window.deletePatient = deletePatient;
window.toggleUserEdit = toggleUserEdit;
window.toggleDoctorEdit = toggleDoctorEdit;
window.resetUserForm = resetUserForm;
window.resetDoctorForm = resetDoctorForm;
window.resetUserInfo = resetUserInfo;
window.resetDoctorInfo = resetDoctorInfo;
window.clearUserInfo = clearUserInfo;
window.clearDoctorInfo = clearDoctorInfo;
