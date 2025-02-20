

$(document).on("click", ".add-to-cart", function (e) {
    e.preventDefault();// Sayfanın yenilenmesini engelle

        var productId = $(this).data("product-id"); // product-id değerini butondan al
        var currentUrl = window.location.pathname

        $.ajax({
            url: "/ShopCart/AddToCart",  // Controller'daki endpoint
            type: "POST",
            data: { productId: productId },
            success: function (response) {
                if (response.success) {
                    alert(response.message); // Başarı mesajını göster
                    $("#cart-count").text(response.totalItems); // Sepet içindeki toplam ürün sayısını güncelle
                    ViewBag.CartItemsCount = text(response.totalItems)

                    // Kullanıcıyı bulunduğu sayfada tut
                    window.history.pushState({}, "", currentUrl);
                }
            },
            error: function () {
                alert("Ürün sepete eklenirken hata oluştu!");
            }
        });
    });

$(document).ready(function () {
    $.ajax({
        url: "/Cart/GetCartCount", // Sepet ürün sayısını döndüren bir endpoint
        type: "GET",
        success: function (response) {
            $("#cart-count").text(response.totalItems); // Sepet ürün sayısını güncelle
        }
    });
});

