window.onscroll = () => {
    document.querySelector('.search-form')?.classList.remove('active');
    document.querySelector('.shopping-cart')?.classList.remove('active');
    document.querySelector('.login-form')?.classList.remove('active');
    document.querySelector('.navbar')?.classList.remove('active');
};

let slides = document.querySelectorAll('.home .slides-container .slide');
let index = 0;

function next() {
    slides[index].classList.remove('active');
    index = (index + 1) % slides.length;
    slides[index].classList.add('active');
}

function prev() {
    slides[index].classList.remove('active');
    index = (index - 1 + slides.length) % slides.length;
    slides[index].classList.add('active');
}
document.addEventListener('DOMContentLoaded', function () {
    bindAddToCartEvents();
    bindRemoveItemEvents();
    setupGlobalEventListeners(); // Thay thế bindCartToggle

    function bindAddToCartEvents() {
        // 1. Với thẻ <a>
        document.querySelectorAll('.add-to-cart').forEach(element => {
            element.addEventListener('click', function (e) {
                e.preventDefault();

                let url = element.getAttribute('href') || element.dataset.url;
                let productId = element.dataset.productId;

                if (!url && productId) {
                    url = `/Cart/AddToCart?productId=${productId}`;
                }

                addToCart(url);
            });
        });

        // 2. Với thẻ <form>
        document.querySelectorAll('.add-to-cart-form').forEach(form => {
            form.addEventListener('submit', function (e) {
                e.preventDefault(); // Ngăn form submit mặc định

                const formData = new FormData(form);
                const productId = formData.get("productId");

                const url = `/Cart/AddToCart?productId=${productId}`;

                addToCart(url);
            });
        });

        function addToCart(url) {
            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ quantity: 1 })
            })
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        showToast("Đã thêm sản phẩm vào giỏ hàng!");
                        refreshCartPopup();
                    } else {
                        showToast("Thêm sản phẩm thất bại!", 'error');
                    }
                });
        }
    }



    function bindRemoveItemEvents() {
        // Sử dụng event delegation để xử lý cả phần tử động
        document.addEventListener('submit', function (e) {
            if (e.target.matches('.remove-item-form')) {
                e.preventDefault();
                const form = e.target;
                const formData = new FormData(form);

                fetch(form.action, {
                    method: 'POST',
                    body: formData
                })
                    .then(res => {
                        if (res.ok) {
                            showToast("Đã xoá sản phẩm khỏi giỏ hàng!");
                            refreshCartPopup();
                        } else {
                            showToast("Xoá sản phẩm thất bại!", 'error');
                        }
                    });
            }
        });
    }

    function refreshCartPopup() {
        fetch('/Cart/CartPopup')
            .then(res => res.text())
            .then(html => {
                document.getElementById('cart-popup-container').innerHTML = html;
                // Không cần gọi lại setupGlobalEventListeners vì đã dùng event delegation
            });
    }

    function setupGlobalEventListeners() {
        // Sử dụng event delegation cho tất cả các nút toggle
        document.addEventListener('click', function (e) {
            const menuBtn = e.target.closest('#menu-btn');
            const searchBtn = e.target.closest('#search-btn');
            const cartBtn = e.target.closest('#cart-btn');
            const loginBtn = e.target.closest('#login-btn');

            if (menuBtn) {
                togglePopup('.navbar');
            } else if (searchBtn) {
                togglePopup('.search-form');
            } else if (cartBtn) {
                togglePopup('#cart-popup-container');
            } else if (loginBtn) {
                const type = loginBtn.dataset.type; // "login" hoặc "account"
                if (type === 'login') {
                    togglePopup('.login-form[data-type="login"]');
                } else if (type === 'account') {
                    togglePopup('.login-form[data-type="account"]');
                }
            }
        });

        // Đóng các popup khi click ra ngoài
        document.addEventListener('click', function (e) {
            if (!e.target.closest('.navbar') && !e.target.closest('#menu-btn')) {
                document.querySelector('.navbar')?.classList.remove('active');
            }
            if (!e.target.closest('.search-form') && !e.target.closest('#search-btn')) {
                document.querySelector('.search-form')?.classList.remove('active');
            }
            if (!e.target.closest('#cart-popup-container') && !e.target.closest('#cart-btn')) {
                document.querySelector('#cart-popup-container')?.classList.remove('active');
            }
            if (
                !e.target.closest('.login-form') &&
                !e.target.closest('#login-btn')
            ) {
                document.querySelectorAll('.login-form').forEach(el => el.classList.remove('active'));
            }
        });
    }

    function togglePopup(selector) {
        // Tắt tất cả popup trước khi bật cái mới
        document.querySelectorAll('.navbar, .search-form, #cart-popup-container, .login-form').forEach(el => {
            el.classList.remove('active');
        });

        // Bật popup được chọn
        const popup = document.querySelector(selector);
        if (popup) {
            popup.classList.add('active');
        }
    }

    function showToast(message, type = 'success') {
        const toast = document.createElement('div');
        toast.textContent = message;
        toast.style.cssText = `
        background-color: ${type === 'success' ? '#28a745' : '#dc3545'};
        color: white;
        padding: 10px 20px;
        margin-bottom: 10px;
        border-radius: 5px;
        font-size: 14px;
        box-shadow: 0 0 10px rgba(0,0,0,0.1);
        opacity: 1;
        transition: opacity 0.5s ease-out;
    `;
        document.getElementById('toast-container').appendChild(toast);

        setTimeout(() => {
            toast.style.opacity = '0';
            setTimeout(() => toast.remove(), 500);
        }, 2000);
    }

});
