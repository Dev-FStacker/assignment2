document.addEventListener("DOMContentLoaded", function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/newsHub")
        .build();

    connection.start().catch(err => console.error(err.toString()));

    connection.on("ReceiveNewsUpdate", function () {
        console.log("News updated! Reloading...");

        // Reload category list
        location.reload();
    });
});
