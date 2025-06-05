// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

async function addToCart(productId) {
    const username = document.getElementById("username");
    if (!username) {
        window.location.href = "/Account/Login";
        return;
    }
    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        data: { productId, quantity: 1 },
        success: () => location.reload(),
        error: () => alert("Not enough stock or error adding to cart.")
    });
}

async function addToWishlist(productId) {
    const username = document.getElementById("username");
    if (!username) {
        window.location.href = "/Account/Login";
        return;
    }

    $.ajax({
        url: '/Wishlist/AddToWishList',
        type: 'POST',
        data: { productId, quantity: 1 },
        success: () => location.reload(),
        error: () => alert("Could not add to wishlist.")
    });
}