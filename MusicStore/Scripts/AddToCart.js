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

$(document).ready(function () {
    $("#TekstBtn").click(function () {
        $("#Tekst").toggle();
    });
    koszykBtnAction();
});

