"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();
connection.on("ReloadData", function () {
    location.reload();
    //Refresh();
});

//function Refresh() {

//}

connection.start().then().catch(function (err) {
    return console.log(err.toString());
});