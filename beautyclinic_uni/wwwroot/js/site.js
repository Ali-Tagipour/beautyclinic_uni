document.addEventListener('DOMContentLoaded', function () {

    // =====================
    // سال جاری
    // =====================
    const yearSpan = document.getElementById('y');
    if (yearSpan) {
        yearSpan.textContent = new Date().getFullYear();
    }

    // =====================
    // فرم تماس (ارسال واقعی بدون رفرش)
    // =====================
    const form = document.getElementById('contactForm');
    const resultBox = document.getElementById('contactResult');

    if (form && resultBox) {
        form.addEventListener('submit', async function (e) {
            e.preventDefault();

            resultBox.style.display = 'none';

            const formData = new FormData(form);

            try {
                const res = await fetch(form.action, {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });

                const data = await res.json();

                if (data.ok) {
                    showMessage(
                        '🌸 درخواست شما با موفقیت ثبت شد. به‌زودی برای هماهنگی با شما تماس می‌گیریم.',
                        true
                    );
                    form.reset();
                } else {
                    showMessage(data.msg || 'خطا در ثبت اطلاعات', false);
                }

            } catch (err) {
                showMessage('❌ خطا در ارتباط با سرور. لطفاً دوباره تلاش کنید.', false);
            }
        });
    }

    function showMessage(text, success) {
        resultBox.style.display = 'block';
        resultBox.textContent = text;

        resultBox.style.marginTop = '14px';
        resultBox.style.padding = '12px 14px';
        resultBox.style.borderRadius = '10px';
        resultBox.style.fontSize = '14px';
        resultBox.style.lineHeight = '1.8';
        resultBox.style.textAlign = 'center';

        if (success) {
            resultBox.style.background = '#e9f9f0';
            resultBox.style.color = '#0f5132';
        } else {
            resultBox.style.background = '#fdecec';
            resultBox.style.color = '#842029';
        }
    }

    // =====================
    // چت هوش مصنوعی
    // =====================
    const sendBtn = document.getElementById('send-btn');
    const msgInput = document.getElementById('msg-input');
    const chatBox = document.getElementById('chat-box');

    async function sendMessage() {
        const message = msgInput.value.trim();
        if (!message) return;

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
                headers: { 'Content-Type': 'application/json' },
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

        } catch {
            const errorEl = document.createElement('div');
            errorEl.textContent = '❌ خطا در ارتباط با هوش مصنوعی.';
            errorEl.style.color = 'red';
            chatBox.appendChild(errorEl);
            chatBox.scrollTop = chatBox.scrollHeight;
        }
    }

    if (sendBtn) {
        sendBtn.addEventListener('click', sendMessage);
    }

    if (msgInput) {
        msgInput.addEventListener('keydown', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                sendMessage();
            }
        });
    }

});
