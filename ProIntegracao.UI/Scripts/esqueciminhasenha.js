$(document).keypress(function (event) {
    if (event.keyCode == 13) {
        $('#enviarSenha').trigger("click");
    }
});

// Esqueci minha Senha
function EsqueciMinhaSenha() {

    var url = "/Login/EsqueciMinhaSenha";
    var email = $("#email").val();

    if (email == "") {
        Message("* Informe o E-mail", "ERRO");
        return true;
    } else {
        $.ajax({
            url: url
            , datatype: "json"
            , type: "POST"
            , data: { Email: email }
            , beforeSend: function () { waitingDialog.show(); }
            , complete: function () { waitingDialog.hide(); }
            , success: function (data) {
                if (data.Resultado) {
                    Message("Se houver uma conta associada a " + data.Email + " você receberá um email com um link para reconfigurar a sua senha.", "info");
                } else {
                    Message(data.Mensagem, "erro");
                }
                setTimeout(
                  function () {
                      window.location = "/Login/Index";
                  }, 3000);

                waitingDialog.hide();
            }
            , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception)
            }
        })
    }
}