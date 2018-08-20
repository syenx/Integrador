//LOAD
$(document).ready(function () {
    //Listar Páginas
    ListarPerfil();

    // Click Boãto Busca
    $("#buscaPerfil").click(function () {
        ListarPerfil();
    });

    // Click do Botão Salvar Página
    $("#salvarPerfil").click(function () {
        
        var result = false;
        result = ValidarFormulario();
        if (result) {
            SalvarPerfil();
        }
    });

    // Seleciona todos os CheckBox de Estado
    $("#BR").click(function(){
        var chk = $("#BR").prop("checked");

        if (chk) {
            $(".chkEstado").prop("checked", true);
        } else {
            $(".chkEstado").prop("checked", false);
        }

    });

    // Seleciona todos os checkbox da página d Estado
    $("#Pages").click(function () {

        var chk = $("#Pages").prop("checked");

        if (chk) {
            $("#paginas input:checkbox").prop("checked", true);
        } else {
            $("#paginas input:checkbox").prop("checked", false);
        }
    });

    $("#limparPerfil").click(function () {
        $("#termo").val("");
    });

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaPerfil').click();
            return false;
        }
    });


});

///Editar Perfil
function EditarPerfil(idPerfil)
{
    var url = '/Perfil/Edit';
    var div = $("#per");
    div.empty();

    $.ajax({
        url: url
        , datatype: "html"
        , type: "GET"
        , data: {idPerfil : idPerfil}
        , success : function(data){
            div.html(data);
            AplicarDataTable('perfilpagina');
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Salvar Perfil
function SalvarPerfil() {

    var ativo = $("#ativo").prop('checked');
    var id = $("#Id").val();

    var ListaPerfilPaginasViewModel = CarregaListaPaginaFormulario();

    if (ListaPerfilPaginasViewModel.length == 0 && id == "0" && id == "")
    {
        Message("Selecione uma ou mais Permissões.", "ERRO");
        return;
    }

    var PerfilViewModel = {

        Id                              : $("#Id").val()
        , Nome                          : $("#Nome").val()
        , Admin                         : $("#Admin").is(":checked")     
        , DtCadastro                    : $("#DtCadastro").val()
        , Estados                       : ""
        , ListaEstados                  : null
        , ListaPerfilPaginasViewModel   : ListaPerfilPaginasViewModel
    }


    if (!ativo && ativo != undefined) {
        PerfilViewModel.DtExclusao = RetornaDataNowToString();
    }

    var url = "/Perfil/Create";

    if ($("#Id").val() != "" && $("#Id").val() != "0")
        url = "/Perfil/Edit";
    
    $.ajax({
        url: url
        , datatype: "json"
        , type: "POST"
        , data: { model: PerfilViewModel }
        , success: function (data){
            if (data.Resultado) {
                Message('Dados Salvos com Sucesso', 'sucesso');
                window.setTimeout(function () {
                    window.location = "/Perfil/Index";
                }, 3000);
            } else {
                if (data.Mensagem != "") {
                    Message(data.Mensagem, "erro");
                } else {
                    Message('Erro ao gravar registro, verifique os dados e tente novamente.', 'erro');
                }
            }
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Carregar Lista Estado Formulario
function CarregaListaEstadoFormulario() {
    
    var listaPerfilEstado = [];

    $(".chkEstado").each(function () {
        var id = parseInt($(this).val());
        if ($.isNumeric(id)) {
            var perfilEstado = {
                idPerfil: 0
                , idEstado: id
            };
            listaPerfilEstado.push(perfilEstado);
        };
    });

    return listaPerfilEstado;
}

function CarregaListaPaginaFormulario() {

    var listaPerfilPagina = [];
    var ids = '';
    var inserir = false;
    var atualizar = false;
    var excluir = false;
    var consultar = false;

    $("#tbPerfilPagina tr").each(function (i, v) {

        var PerfilPagina = {
            id: 0
                    , idPerfil: 0
                    , idPagina: 0
                    , Inserir: false
                    , Atualizar: false
                    , Excluir: false
                    , Consultar: false
        };

        var idPerfilPagina = '';
        var idPerfil = '';
        var idPerfilEstado = '';
        var idPagina = '';

        $(this).children('td').each(function (ii, vv) {

            switch (ii) {
                case 0:
                    ids = $(this).text();
                    idPerfilPagina = ids.split(',')[0];
                    idPerfil = ids.split(',')[1];
                    idPagina = ids.split(',')[2];
                    idPerfilEstado = ids.split(',')[3];
                    break;
                case 4:
                    inserir =  ($(this).find('input:checkbox').prop("checked"))
                    break;
                case 5:
                    atualizar = ($(this).find('input:checkbox').prop("checked")) 
                    break;
                case 6:
                    excluir = ($(this).find('input:checkbox').prop("checked")) 
                    break;
                case 7:
                    consultar = ($(this).find('input:checkbox').prop("checked"))
                    break;
            }
        });

        PerfilPaginaViewModel = {
                    idPerfilPagina: idPerfilPagina
                    , idPerfil: idPerfil
                    , idPagina: idPagina
                    , Inserir: inserir
                    , Atualizar: atualizar
                    , Excluir: excluir
                    , Consultar: consultar
        }

        listaPerfilPagina.push(PerfilPaginaViewModel);
    });

   return listaPerfilPagina;
}

//Listar Paginas Perfil por Estado Selecionado
function ListarPaginasPerfilPorEstado() {

    var myArray = new Array();

    var idPerfil = $("#Id").val();
    var postData;
    var url = "";

    $("input:checked").each(function () {
        var idEstado = $(this).val();
        myArray.push(idEstado);
    });


    if (myArray.length == 1)
    {
        Message("Selecione um ou mais Estados.", "erro");
        return;
    }
    
    if (idPerfil == "0" || idPerfil == "") {
        postData = { IdEstado: myArray };
        url = '/Perfil/ListarPaginaPerfilPorEstado';
    } else {
        url = '/Perfil/ListarPaginaPerfilPorIdPerfil';
        postData = { idPerfil: idPerfil, IdEstado: myArray }
    }

    $.ajax({
        url: url
        , datatype: 'json'
        , type: 'POST'
        , data: postData
        , success: function (data) {

            //Remove somente a tabela de páginas
            $("#paginas").remove();

            // Acrescenta a nova tabela de permissões
            $("#permissoes").append(data);
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });

}

// Listar Paginas
function ListarPerfil() {

    var termo = $("#termo").val();
    var ativo = $("#ativolistar").is(":checked");
    var url = "/Perfil/ListarPerfil";
    var div = $("#listaPerfil");
    div.empty();
    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , data: { termo: termo, ativo : ativo }
        , beforeSend: function () { }
        , success: function (data) {
            div.html(data);
            AplicarDataTable('perfilpagina');
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });

}

// Validar Formulario
function ValidarFormulario() {

    //Efetua a Validação 
    $("#formPerfil").validate({
        rules: {
            Nome: {
                required: true
                , maxlength: 255
            }
        },
        messages: {
            Nome: {
                required: "*NOME é obrigatório"
                , maxlength: "*NOME deve ser menor que 255"
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
    return $("#formPerfil").valid();
}

// Selecionar Todods os Estados
function SelecionarTodosEstados() {
    $(".chkEstado").prop("checked", true);
}