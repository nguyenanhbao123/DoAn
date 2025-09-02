document.querySelectorAll('.quantity-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        const input = this.parentElement.querySelector('.quantity-input');
        let value = parseInt(input.value);

        if (this.classList.contains('minus')) {
            if (value > 1) input.value = value - 1;
        } else {
            input.value = value + 1;
        }
    });
});

// Xử lý nút xóa sản phẩm
document.querySelectorAll('.remove-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        const cartItem = this.closest('.cart-item');
        cartItem.style.opacity = '0';
        setTimeout(() => {
            cartItem.remove();

            // Kiểm tra nếu giỏ hàng trống thì hiển thị trang trống
            if (document.querySelectorAll('.cart-item').length === 0) {
                document.getElementById('cart-with-items').style.display = 'none';
                document.getElementById('empty-cart').style.display = 'block';
            }
        }, 300);
    });
});
document.addEventListener('DOMContentLoaded', async function () {

    let provinces = {}, districts = {}, wards = {};

    const provinceSelect = document.getElementById("province");
    const districtSelect = document.getElementById("district");
    const wardSelect = document.getElementById("ward");

    const provinceNameInput = document.getElementById("ProvinceName");
    const districtNameInput = document.getElementById("DistrictName");
    const wardNameInput = document.getElementById("WardName");

    try {
        const [provinceRes, districtRes, wardRes] = await Promise.all([
            fetch("/dist/tinh_tp.json"),
            fetch("/dist/quan_huyen.json"),
            fetch("/dist/xa_phuong.json")
        ]);

        provinces = await provinceRes.json();
        districts = await districtRes.json();
        wards = await wardRes.json();

        // Khởi tạo dropdown tỉnh
        provinceSelect.innerHTML = '<option selected disabled>Chọn tỉnh/thành phố</option>';
        for (const code in provinces) {
            const name = provinces[code].name_with_type;
            provinceSelect.innerHTML += `<option value="${code}">${name}</option>`;
        }

    } catch (error) {
        console.error("Không thể tải dữ liệu địa phương:", error);
    }

    // Khi chọn tỉnh → lọc quận
    provinceSelect.addEventListener("change", function () {
        const selectedProvince = this.value;
        const name = provinces[selectedProvince].name_with_type;
        provinceNameInput.value = name;

        districtSelect.innerHTML = '<option selected disabled>Chọn quận/huyện</option>';
        wardSelect.innerHTML = '<option selected disabled>Chọn quận trước</option>';

        for (const code in districts) {
            if (districts[code].parent_code === selectedProvince) {
                const dName = districts[code].name_with_type;
                districtSelect.innerHTML += `<option value="${code}">${dName}</option>`;
            }
        }

        districtNameInput.value = '';
        wardNameInput.value = '';
    });

    // Khi chọn quận → lọc phường
    districtSelect.addEventListener("change", function () {
        const selectedDistrict = this.value;
        const name = districts[selectedDistrict].name_with_type;
        districtNameInput.value = name;

        wardSelect.innerHTML = '<option selected disabled>Chọn phường/xã</option>';
        for (const code in wards) {
            if (wards[code].parent_code === selectedDistrict) {
                const wName = wards[code].name_with_type;
                wardSelect.innerHTML += `<option value="${code}">${wName}</option>`;
            }
        }

        wardNameInput.value = '';
    });

    // Khi chọn phường
    wardSelect.addEventListener("change", function () {
        const selectedWard = this.value;
        const name = wards[selectedWard].name_with_type;
        wardNameInput.value = name;
    });

});