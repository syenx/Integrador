//LOAD
$(document).ready(function () {

    AplicarMascara();

    //Listar Páginas
    ListarSimularErro();

    //Click Boãto Busca
    Cpf: $('#Cpf').val();

    $("#buscaSimularErro").click(function () {
        ListarSimularErro();
    });

    // Click do Botão Salvar Página
    $("#salvarSimularErro").click(function () {
        var result = false;
        result = ValidarFormulario();
        if (result) {
            SalvarSimularErro();
        }
    });

    // Simular Erro
    $("#limparSimulacao").click(function () {
        $("#termo").val("");
    });


    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaSimularErro').click();
            return false;
        }
    });


});

function ListaSimularErros() {

    var termo = $("#termo").val();

    var url = "/SimularErro/ListarSimularErro";

    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , data: { termo: termo }
        , success: function (data) {
            
        }
        , error: function (jqXHR, exception) {
             TratamendodeErro(jqXHR, exception);
        }
    })
}

// Editar SimularErro
function EditarSimularErro(model) {

    var url = "/SimularErro/Excluir";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , data: { idPerfil: idPerfil }
        , success: function (data) {
            AbrirModal('Excluir Erro', data);
        }
        , error: function (jqXHR, exception) {
             TratamendodeErro(jqXHR, exception);
        }
    });
}

// Nova Página
function NovoSimularErro() {

    var url = "/SimularErro/Create";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , success: function (data) {
            AbrirModal('Novo Erro', data);
            AplicarMascara();
        }
        , error: function (jqXHR, exception) {
             TratamendodeErro(jqXHR, exception);
         }
    });
}

// Listar SimularErro
function ListarSimularErro() {

    var termo = $("#termo").val().replace(".", "").replace(".", "").replace("-", "");
    var url = "/SimularErro/ListarSimularErro";
    var div = $("#listaSimularErro");

    div.empty();

    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , data: { termo: termo }
        , beforeSend: function () { waitingDialog.show(); }
        , complete: function () { waitingDialog.hide();}
        , success: function (data) {
            div.html(data);
            AplicarDataTable('SimularErro');
            waitingDialog.hide();
        }
        , error: function (jqXHR, exception) {
             TratamendodeErro(jqXHR, exception);
        }
    });

}

// Salvar Página
function SalvarSimularErro() {

    AplicarMascara();

    if (ValidarFormulario()) {

        var SimularErroViewModel =
        {
            Cpf: RetornaCPFSemPontos($('#Cpf').val())
            , IdTipoErro: $("#IdTipoErro option:selected").val()
        }

        var url = "/SimularErro/Create";

        $.ajax({
            url: url
            , data: { model: SimularErroViewModel }
            , datatype: "json"
            , type: "POST"
            , success: function (data) {
                if (data.Resultado) {
                    FecharModalExcluir("myModalContent");
                    Message('Dados Salvos com Sucesso', 'sucesso');
                    ListarSimularErro();
                }
                else {
                    Message('Erro ao gravar registro, verifique os dados e tente novamente', 'erro');
                }
            }
            , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception);
            }
        });
    }
}

// Validar Formulario
function ValidarFormulario() {

    //Efetua a Validação 
    $("#formSimularErro").validate({
        debug: true,
        rules: {
            Cpf: {
                required: true
                , remote: {
                    url: "/SimularErro/CPFNaoCadastrado"
                    , type: 'GET'
                    , data: {
                        cpf: function () {
                            return $("#Cpf").val();
                        }, id: function () {
                            return $("#Id").val();
                        }
                    }
                    , async: false
                }
            }
            , Url: {
                required: true
                , maxlength: 255
            }
        },
        messages: {
            Nome: {
                required: "*NOME é obrigatório"
                , maxlength: "*NOME deve ser menor que 255"
            }

            , Url: {
                required: "*URL é obrigatório"
                , maxlength: "*URL deve ser menor que 255"
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
    //Retorna de válido ou não
    return $("#formSimularErro").valid();


}