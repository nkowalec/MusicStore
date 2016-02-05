function AddToCart(Id)
{
    var cookie = getCookie("Cart");
    if (cookie == "")
    {
        setCookie("Cart", Id, 1);
    }
    else
    {
        setCookie("Cart", cookie + "," + Id, 1);
    }
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;

    koszykBtnAction();
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function koszykBtnAction() {
    var cookie = getCookie("Cart");
    var splited = cookie.split(",");
    var ile = splited.length;
    if (cookie == "" || cookie == null) ile = 0;
    $("#koszykBtn").text("Koszyk (" + ile + ")");
}

function removeKoszykAction(Id) {

    var reg = new RegExp(Id, "g");
    var cookie = getCookie("Cart");
    cookie = cookie.replace(reg, "");
    cookie = cookie.replace(/,+/g, ",");
    cookie = cookie.replace(/^,+/g, "");
    cookie = cookie.replace(/,+$/g, "");

    setCookie("Cart", cookie, 1);

    location.reload();
}

function validateEmail(elementId) {
    var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var email = document.getElementById(elementId);

    var result = re.test(email.value);
    if (result == false) email.style.backgroundColor = "red";
    else email.style.backgroundColor = "lightgreen";

    return result;
}

function validatePhone(elementId) {
    var re = /^([0-9\+]{8,13})|(\d+([-| ])\d+([-| ])\d+)$/;
    var tel = document.getElementById(elementId);

    var result = re.test(tel.value);
    if (result == false) tel.style.backgroundColor = "red";
    else tel.style.backgroundColor = "lightgreen";

    return result;
}

function validatePostCode(elementId) {
    var re = /^\d{2}-\d{3}$/;
    var tel = document.getElementById(elementId);

    var result = re.test(tel.value);
    if (result == false) tel.style.backgroundColor = "red";
    else tel.style.backgroundColor = "lightgreen";

    return result;
}

$(document).ready(function () {
    $("#TekstBtn").click(function () {
        $("#Tekst").toggle();
    });
    koszykBtnAction();
});

