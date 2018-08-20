//LOAD
$(document).ready(function () {

    //Listar Páginas
    ListarConfiguracoes();

    // Click Boãto Busca
    $("#buscaConfiguracao").click(function () {
        ListarConfiguracoes();
    });

    // Click do Botão Salvar Página
    $("#salvarConfiguracao").click(function () {
        var result = false;
        result = ValidarFormulario();
        if (result) {
            SalvarConfiguracao();
        }
    });

    $("#limparConfiguracao").click(function () {
        $("#termo").val("");
    });

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaConfiguracao').click();
            return false;
        }
    });


});

// Editar Configuracao
function EditarConfiguracao(idConfiguracao) {

    var url = "/Configuracao/Edit";
    $.ajax({
        url: url
        , datatype: "html"
        , async: true
        , cache: false
        , type: 'GET'
        , data: { idConfiguracao: idConfiguracao }
        , success: function (data) {
            AbrirModal('Atualizar Configuração', data);
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Nova Configuracao
function NovaConfiguracao() {

    var url = "/Configuracao/Create";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , success: function (data) {
            AbrirModal('Nova Configuração', data);
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Listar Configurações
function ListarConfiguracoes() {

    var termo = $("#termo").val();
    
    var ativo = $("#ativolistar").is(':checked');

    var url = "/Configuracao/ListarConfiguracao";

    var div = $("#listaConfiguracao");

    div.empty();


    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , data: { termo: termo, ativo : ativo }
        , success: function (data) {
            div.html(data);
            AplicarDataTable('configuracao');
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });

}

// Salvar Configuracao
function SalvarConfiguracao() {


    if (ValidarFormulario()) {

        var ativo = $("#ativo").is(':checked');

        var ConfiguracaoViewModel =
        {
            Id              : $("#Id").val()
            , Nome          : $("#Nome").val()
            , Descricao     : $("#Descricao").val()
            , Valor         : $("#Valor").val()
            , DtCadastro    : $("#DtCadastro").val()
            , DtExclusao    : $("#DtExclusao").val()

        }

        if (!ativo) {
           ConfiguracaoViewModel.DtExclusao = RetornaDataNowToString();
        } else {
            ConfiguracaoViewModel.DtExclusao = null;
        }

        var url = "/Configuracao/Create";

        if ($("#Id").val() != "" && $("#Id").val() != "0") {
            url = "/Configuracao/Edit";
        }

        $.ajax({
            url: url
            , data: { model: ConfiguracaoViewModel }
            , datatype: "json"
            , type: "POST"
            , success: function (data) {
                if (data.Resultado) {
                    FecharModalExcluir("myModalContent");
                    Message('Dados Salvos com Sucesso', 'sucesso');
                    ListarConfiguracoes();
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
    $("#formConfiguracao").validate({
        debug: true,
        rules: {
            Nome: {
                required: true
                , maxlength: 50
                , remote: {
                    url: "/Configuracao/ObterConfiguracaoExistente"
                    ,type : "GET"
                    , data: {
                        nome: function () {
                            return $("#Nome").val();
                        }, id: function() {
                            return $("#Id").val();
                        }
                    }
                    , async: false
                }
            }

            , Descricao: {
                required: true
                , maxlength: 255
            }
            , Valor: {
                required: true
                , maxlength: 50
            }
        },
        messages: {
            Nome: {
                required: "*NOME é obrigatório"
                , maxlength: "*NOME deve ser menor que 50 caracteres"
            }

            , Descricao: {
                required: "*DESCRIÇÃO é obrigatório"
                , maxlength: "*DESCRIÇÃO deve ser menor que 255 caracteres"
            }

            , Valor: {
                required: "*VALOR é obrigatório"
                , maxlength: "*VALOR deve ser menor que 50"
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
    return $("#formConfiguracao").valid();


}
