function generate() {
    var url = $("#inp-url").val();
    $.post("/api/Values/Generate", { url: url }, function (result) {
        if (!result || !!result.msg) {
            alert(result.msg);
            return;
        }
        $("#newUrl").html(window.location.origin + "/" + result.shortURL).prop("href", window.location.origin + "/" + result.shortURL);
        $("#oldUrl").html(url).prop("href", url);
        $("#codeImg").prop("src", "/api/Values/Img?url=" + $("#newUrl").html());
    });
}