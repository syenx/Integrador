// LOAD
$(document).ready(function () {

    $("#acesso").click(function () {
        Login();
    });

    $(document).keypress(function (event) {
        if (event.keyCode === 13) {
            $('#acesso').trigger("click");
        }
    });

});

//LOGIN
function Login() {

    var url = "/Login/SigIn";

    var message = $("#message");

    message.removeClass("alert alert-danger");
    message.html("");
    
    var username = $("#username").val();
    var password = $("#password").val();

    $.ajax({
        url: url
        , datatype: 'json'
        , type: 'POST'
        , data: { username: username, password: password }
        , success: function (data) {
            if (data.Resultado) {
                if (data.NovoUsuario) {
                    window.location = "/Usuario/AlterarSenha";
                }
                else {
                    window.location = "/Home/Index";
                }
            } else {
                message.html("Usuário e/ou senha inválidos");
                message.addClass("alert alert-danger");
            }

        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Alterar Senha  | Esqueci Minha Senha
function AlterarSenha()
{
    if (ValidarFormulario())
    {
        var url = "/Login/AlterarSenha";
        var idUsuario = $("#IdUsuario").val();
        var NovaSenha = $("#NovaSenha").val();

        $.ajax({
            url: url
            , datatype: "json"
            , type: "POST"
            , beforeSend: function () { waitingDialog.show(); }
            , complete: function () { waitingDialog.hide(); }
            , data: { idUsuario: idUsuario, NovaSenha: NovaSenha }
            , success: function (data) {
                if (data.Resultado) {
                    Message("Alteração realizada com Sucesso", "INFO");
                    window.location = "/Login/Index";
                } else {
                    Message("Erro Alteração de Senha, verifique os dados e tente nocamente", "ERRO");
                }
            }
            , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception);
            }
        });
    }
}

// Validar Formulário | Esqueci Minha Senha
function ValidarFormulario() {

    $("#formAlterarSenha").validate({
        rules: {
            NovaSenha: {
                required: true
            }
            , ConfirmarSenha: {
                equalTo: "#NovaSenha"
            }
        },
        messages: {
            NovaSenha: {
                required: "*NOVA SENHA é obrigatório"
            }
            , ConfirmarSenha: {
                equalTo: "*CONFIRMAR SENHA não confere"
            }
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    //Retorno validação!
    return $("#formAlterarSenha").valid();
}