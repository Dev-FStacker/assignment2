document.addEventListener("DOMContentLoaded", function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/categoryHub")
        .build();

    connection.start().catch(err => console.error(err.toString()));

    connection.on("ReceiveCategoryUpdate", function () {
        console.log("Categories updated! Reloading...");

        // Reload category list
        location.reload();
    });
});
