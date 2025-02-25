﻿$(document).on("click", ".add-to-cart", function (e) {
    e.preventDefault();// Sayfanın yenilenmesini engelle

    var productId = $(this).data("product-id"); // product-id değerini butondan al
    var currentUrl = window.location.pathname

    $.ajax({
        url: "/ShopCart/AddToCart",  // Controller'daki endpoint
        type: "POST",
        data: { productId: productId },
        success: function (response) {
            if (response.success) {
                $("#cart-count").text(response.totalItems); // Sepet sayısını güncelle
                showToast(response.message); // SweetAlert ile Bildirim göster
                window.history.pushState({}, "", currentUrl); // Sayfada kal
                updateCartCount(); // sayfa geçişisnden sonra sepet sayısını güncelle
            }
        },
        error: function () {
           showToast("Ürün sepete eklenirken hata oluştu!", "bg-danger"); // Hata mesajı göster
        }
    });
});

$(document).ready(function () {
    updateCartCount();  // Sayfa yüklendiğinde sepet sayısını güncelle
});

function updateCartCount() {
    $.ajax({
        url: "/ShopCart/GetCartCount",  // Sepet sayısını al
        type: "GET",
        success: function (response) {
            $("#cart-count").text(response.totalItems);  // Sepet sayısını güncelle
        },
        error: function () {
            console.error("Sepet sayısı alınamadı.");
        }
    });
}

function showToast(message, icon = "success") {
    Swal.fire({
        icon: icon, // success veya error
        title: message,
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 1000, // 1 saniye sonra otomatik kaybolur
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer);
            toast.addEventListener('mouseleave', Swal.resumeTimer);
        }
    });
}


