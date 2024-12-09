$(document).on("click", ".add-to-favorites", function () {
    const eventId = $(this).data("event-id");

    $.ajax({
        url: "/Concert/AddToFavorites",
        type: "POST",
        data: { eventId: eventId },
        success: function (response) {
            if (response.success) {
                alert(response.message);
            } else {
                alert(response.message);
            }
        },
        error: function () {
            alert("Bir hata oluştu.");
        }
    });
});
