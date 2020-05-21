"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/oHub").build();

//Disable send button until connection is established
document.getElementById("send").disabled = true;

connection.on("observationUpdate", function (temp) {
    
    var li = document.createElement("li");
    li.textContent = temp;
    document.getElementById("obList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendBtn").addEventListener("click", function (event) {
    var temp = document.getElementById("dateObserved").value + " " + document.getElementById("locationName").value + " " + document.getElementById("locationLat").value + " " + document.getElementById("locationLot").value;
    temp = temp + " " + document.getElementById("temperature").value + " " + document.getElementById("humidity").value + " " + document.getElementById("airPressure").value;
    connection.invoke("observationUpdate", temp).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

