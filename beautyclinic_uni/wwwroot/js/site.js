document.addEventListener('DOMContentLoaded', function () {

    // نمایش سال جاری
    const yearSpan = document.getElementById('y');
    if (yearSpan) yearSpan.textContent = new Date().getFullYear();

    // فرم تماس
    const form = document.getElementById('contactForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            alert('درخواست شما با موفقیت ثبت شد. به‌زودی با شما تماس خواهیم گرفت.');
            form.reset();
        });
    }

    // چت هوش مصنوعی
    const sendBtn = document.getElementById('send-btn');
    const msgInput = document.getElementById('msg-input');
    const chatBox = document.getElementById('chat-box');

    async function sendMessage() {
        const message = msgInput.value.trim();
        if (!message) return;

        // پیام کاربر
        const userMsgEl = document.createElement('div');
        userMsgEl.textContent = message;
        userMsgEl.style.textAlign = 'right';
        userMsgEl.style.margin = '8px 0';
        userMsgEl.style.fontWeight = 'bold';
        chatBox.appendChild(userMsgEl);
        chatBox.scrollTop = chatBox.scrollHeight;
        msgInput.value = '';

        try {
            const res = await fetch('/AiConsult/Ask', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ message })
            });

            const data = await res.json();

            const botMsgEl = document.createElement('div');
            botMsgEl.textContent = data.reply;
            botMsgEl.style.textAlign = 'left';
            botMsgEl.style.margin = '8px 0';
            botMsgEl.style.color = '#333';
            chatBox.appendChild(botMsgEl);
            chatBox.scrollTop = chatBox.scrollHeight;

        } catch (err) {
            const errorEl = document.createElement('div');
            errorEl.textContent = 'خطا در ارتباط با سرور هوش مصنوعی.';
            errorEl.style.color = 'red';
            chatBox.appendChild(errorEl);
            chatBox.scrollTop = chatBox.scrollHeight;
        }
    }

    sendBtn.addEventListener('click', sendMessage);
    msgInput.addEventListener('keypress', function (e) {
        if (e.key === 'Enter') sendMessage();
    });

});
