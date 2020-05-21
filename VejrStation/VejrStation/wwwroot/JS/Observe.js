"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/oHub").build();

//Disable send button until connection is established
document.getElementById("sendBtn").disabled = true;

connection.on("recieveObservation", function (temp) {
    
    var li = document.createElement("li");
    li.textContent = temp;
    document.getElementById("obList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendBtn").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendBtn").addEventListener("click", function (event) {
    var data = {
        "dateObserved": document.getElementById("dateObserved").value,
        "locationName": document.getElementById("locationName").value,
        "locationLat": Number(document.getElementById("locationLat").value),
        "locationLot": Number(document.getElementById("locationLot").value),
        "temperature": Number(document.getElementById("temperature").value),
        "humidity": Number(document.getElementById("humidity").value),
        "airPressure": Number(document.getElementById("airPressure").value)
    }
    connection.invoke("observationUpdate", JSON.stringify(data)).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
    fetch("https://localhost:44309/api/observations", {
        method: 'POST',
        body: JSON.stringify(data),
        headers: {
            'content-Type': 'application/json'
        }
    })
        .then(responseJson => { JSON.parse(responseJson) })
        .catch(error => {alert(error)})

});

