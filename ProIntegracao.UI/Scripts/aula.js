//LOAD
$(document).ready(function () {
    //Listar Aulas
    ListarAulas();

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $('#buscaAula').click();
            return false;
        }
    });

    // Click Boãto Busca
    $("#buscaAula").click(function () {
        ListarAulas();
    });

    // Click do Botão Salvar Página
    $("#salvarAula").click(function () {
        var result = false;
        result = SalvarAluno();
        if (result) {
            SalvarAula();
        }
    });

    // Lista de Estados Change
    $("#listaCompletaEstados").change(function () {
        
        var idEstado = $("#listaCompletaEstados option:selected").val();

        $("#IdEstadoConsulta").val(idEstado);

        $("#listaCompletaEstados option").first().attr("selected", "selected");

        $('#myModalEstados').modal('hide');

        $("#listaCompletaEstados")[0].selectedIndex = 0;

        ValidaEstadoPossuiMatriculas(parseInt(idEstado));
    });

    // Limpar Busca
    $("#limparAula").click(function () {
        $("#nomeAluno").val("");
        $("#cpf").val("");
    });

});

//Abrir Modal (Editar Pagina)
function EditarAula(idAula) {

    var url = "/Aula/Edit";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , data: { idAula: idAula }
        , cache: false
        , success: function (data) {
            AbrirModal('Atualizar Aula', data);
            AplicarMascara();
            debugger;
            ChangeMatricula();

            RetornaCPFeGUID("");
        }
        , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception)
        }
    });
}

// Validar Estado POssui Matriculas
function ValidaEstadoPossuiMatriculas(idEstado)
{
    var url = "/Matricula/ValidaEstadoPossuiMatriculas";

    $.ajax({
        url: url
        , data: { idEstado: idEstado }
        , type: "GET"
        , datatype: "json"
        , success: function (data) {
        
            if (data.Resultado) {
                NovaAula();
            } else {
                var html = '<div class="form-horizontal">';
                html += '   <div class="row">';
                html += '       <div class="col-lg-12">';
                html += '               <h5>Não foram encontradas matrículas cadastradas para o Estado selecionado.</h5>';
                html += '               <h5>Para abertura de aulas é necessário o cadastro de uma matrícula por Estado.</h5>';
                html += '<br/><br/>'
                html += '                   <div class="form-group">';
                html += '                       <div class="col-md-offset-8">';
                html += '                           <a href="#" id="cancelar" data-dismiss="modal" class="btn btn-warning btn-lg"><span class="glyphicon glyphicon-arrow-left"></span>Fechar</a>'
                html += '                       </div> ';
                html += '       </div>';
                html += '   </div>';
                html += '</div>';
                html += '</div>';

                AbrirModal('Abertura de Aula', html);
            }
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

//Abrir Modal (Nova Aula)
function NovaAula() {
    
   var idEstado = $("#IdEstadoConsulta").val();

   var url = "/Aula/Create";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , cache: false
        , data: {idEstado : idEstado}
        , success: function (data) {

            AbrirModal('Nova Aula', data);

            AplicarMascara();

            $("#NomeAluno").focusout(function () {
                var nome = $("#NomeAluno").val();
                if (nome != "") {
                    RetornaCPFeGUID(nome);
                }
            });

            ChangeMatricula();
        }
        , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception)
        }
    });
}

//Listar Paginas
function ListarAulas() {

    var cpf = RetornaCPFSemPontos($("#cpf").val());

    var nomeAluno = $("#nomeAluno").val();

    var ativo = $("#ativolistar").is(":checked");

    var url = "/Aula/ListarAulas";

    var div = $("#listaAula");

    div.empty();
    

    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , data: {cpf:cpf,nomeAluno:nomeAluno,ativo:ativo }
        , success: function (data) {
            div.html(data);
            AplicarDataTable('aulas');
        }
        , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception)
        }
    });

}

//Salvar Aula
function SalvarAula() {

    if (ValidarFormulario()) {

        var ativo = $("#ativo").prop('checked');

        var AulaViewModel =
        {
             Id                         : $("#Id").val()
            , CodigoCfc                 : $("#CodigoCfc").val()
            , IdentificadorAula         : $("#IdentificadorAula").val()
            , CpfInstrutor              : $("#CpfInstrutor").val().replace(".", "").replace(".", "").replace("-", "")
            , DataInicioAula            : $("#DataInicioAula").val()
            , DataFimAula               : $("#DataFimAula").val()
            , TokenInicioAula           : $("#TokenInicioAula").val()
            , TokenFimAula              : $("#TokenFimAula").val()
            , NomeAluno                 : $("#NomeAluno").val()
            , IdMatricula               : $("#IdMatricula option:selected").val()
            , IdAluno                   : $("#IdAluno").val()
            , IdStatusSituacaoAula      : $("#IdStatusSituacaoAula option:selected").val()
            , DtCadastro                : $("#DtCadastro").val()
            , DtExclusao                : ativo
        };

        if ($("#DataFimAula").val() == "")
            AulaViewModel.DataFimAula = null;

        if (!ativo) {
            AulaViewModel.DtExclusao = RetornaDataNowToString();
        } else {
            AulaViewModel.DtExclusao = null;
        }

        var url = "/Aula/Create";

        if ($("#Id").val() != "" && $("#Id").val() != "0") {
            url = "/Aula/Edit";
        }

        $.ajax({
            url: url
            , data: { AulaViewModel: AulaViewModel }
            , datatype: "json"
            , cache: false
            , type: "POST"
            , success: function (data) {
                if (data.Resultado) {
                    FecharModalExcluir("myModalContent");
                    Message('Dados Salvos com Sucesso', 'sucesso');
                    ListarAulas();
                } else {
                    Message('Erro ao gravar registro, verifique os dados e tente novamente', 'erro');
                }
            }
            , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception);
            }
        });
    }
}

//Validar Formulario
function ValidarFormulario() {

    
    jQuery.validator.addMethod("greaterStart", function (value, element, params) {
        //console.log(new Date(value));
        //console.log(new Date($(params).val()));
        //return this.optional(element) || new Date(value) >= new Date($(params).val());
        return this.optional(element) || RetornaNewDate(value) >= RetornaNewDate($(params).val());
    }, 'Data Final deve ser maior que Data Inicial.');



    //Efetua a Validação 
    $("#formAula").validate({
        debug: true,
        rules: {
            IdMatricula: {
                required: true
            }
            , CpfAluno: {
                required: true
            }
            , IdStatusSituacaoAula: {
                required : true        
            }
            , IdentificadorAula: {
                required : true
            }
            , DataInicioAula: {
                required: true
                , remote: {
                    url: "/Aula/VerificaMatriculaPorHorario"
                    , type: "GET"
                    , data: {
                        IdMatricula: function () {
                            return $("#IdMatricula option:selected").val();
                        },
                        DataInicioAula: function ()
                        {
                            return $("#DataInicioAula").val();
                        },
                        IdAula: function () {
                            return $("#Id").val();
                        }
                    }
                    , async : false
                }
            },
            DataFimAula: {
                greaterStart: "#DataInicioAula"
            }
        },
        messages: {
            IdMatricula: {
                required: "*Matrícula é obrigatório"
            }
            , CpfAluno: {
                required : "*CPF do Aluno é obrigatório"
            }
            , IdStatusSituacaoAula: {
                required: "*STATUS é obrigatório"
            }
            , IdentificadorAula: {
                required: "*Identificador Aula é obrigatório"
            }
            , DataInicioAula: {
                required: "*Data Inicio é obrigatório"
            }
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        }
        , unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        }
        , errorElement: 'span'
        , errorClass: 'help-block'
        , errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    //Retorna de válido ou não
    return $("#formAula").valid();
}

// Retorna CPF e GUID
function RetornaCPFeGUID() {

    var IdMatricula = $("#IdMatricula option:selected").val();
    debugger;

    var IdAula = $("#Id").val();

    console.log(IdAula);

    var url = "/Aula/RetornaCPF";

    if (IdAula != "") {

        $.ajax({
            url: url
            , datatype: 'json'
            , type: 'GET'
            , cache: false
            , data: {
                IdMatricula: IdMatricula,
                IdAula: IdAula
            }
            , success: function (data) {

                $("#IdentificadorAula").val(data.Identificador);
                $("#TokenInicioAula").val(data.TokenInicial);
                $("#TokenFimAula").val(data.TokenFinal);
                $("#NomeAluno").val(data.NomeAluno);
                $("#CodigoCfc").val(data.CodigoCfc);
                $("#IdAluno").attr("value", data.idAluno);

                AplicarMascara();
            }
            , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception);
            }

        });
    }


}

// Change no Campo Id Matricula
function ChangeMatricula() {

    $("#IdMatricula").change(function () {
        var idMatricula = $("#IdMatricula").val();
        if (idMatricula > 0)
            RetornaCPFeGUID();
    });
}
