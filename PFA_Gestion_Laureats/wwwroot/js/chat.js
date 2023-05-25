"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub?userid=" + document.getElementById("receiverInput").value).build();
console.log("Console result");
console.log(document.getElementById("receiverInput").value);
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var p = document.createElement("li");
    document.getElementById("messagesList").append(p);

    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    p.textContent = `${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var receiverId = document.getElementById("receiverInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", receiverId, message).catch(function (err) {
        console.error(err);
    });
    $("input[name=message]").val("");
    event.preventDefault();
});