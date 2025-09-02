document.addEventListener('click', function (e) {
    const heart = e.target.closest('.favorite-icon');
    if (!heart) return;

    e.preventDefault(); // ngăn chuyển hướng

    // Toggle class "favorited" để đổi màu
    heart.classList.toggle('favorited');

    // Có thể thêm hiệu ứng hoặc gọi API ở đây nếu cần
    const isFavorited = heart.classList.contains('favorited');
    console.log(isFavorited ? 'Đã yêu thích' : 'Đã bỏ yêu thích');
});
