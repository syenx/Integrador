//LOAD
$(document).ready(function () {

    //ListaAlunosDuplicados();
    $("#buscaHistoricoAluno").click(function () {
        var cpf = $("#cpf").val();
        var nome = $("#Nome").val();
        var renach = $("#Renach").val();

        if (cpf == "" && nome == "" && renach == "") {
            Message("Informe um parâmetro para realizar a pesquisa", "INFO");
        }
        else {
            ListaHistoricoAlunos();
        }
    });

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaAluno').click();
            return false;
        }
    });

    // Limpar Busca
    $("#limparHistoricoAluno").click(function () {
        $("#cpf").val("");
        $("#Renach").val("");
        $("#Nome").val("");
        var div = $("#listarHistoricoAluno");
        div.empty();
    });

});

// Listar Historico de Alunos
function ListaHistoricoAlunos()
{
    var cpf = RetornaCPFSemPontos($("#cpf").val());
    var nome = $("#Nome").val();
    var renach = $("#Renach").val();
    var ativo = $("#ativolistar").is(":checked");

    var renach = $("#Renach").val();

    var url = "/HistoricoAluno/ListarHistorico";
    var div = $("#listarHistoricoAluno");

    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'GET'
        , data: { cpf: cpf, nome: nome, renach: renach, ativo:ativo }
        , beforeSend: function () {
            waitingDialog.show();
        }
        , complete: function () {
            waitingDialog.hide();
        }
        , success: function (data) {
            div.empty();
            div.html(data);
            AplicarDataTable('historico');
            AplicarMascara();
            waitingDialog.hide();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });



}

// Detalhe Historico
//function DetalheHistorico(nome, dtNascimento,cpf,renach,curso,biometria,status,cep,endereco
//    , bairro,cidade,estado, telefone, celular, idAluno)
//{
//    var url = "/HistoricoAluno/RetornaDetalhe";

//    $.ajax({
//        url: url
//        , datatype: "html"
//        , type: "GET"
//        , success: function (data) {

//            //$("#tallModal").modal();

//            //$(".modal-wide").on("show.bs.modal", function () {
//            //    var height = $(window).height() - 200;
//            //    $(this).find(".modal-body").css("max-height", height);
//            //});
//            // $("#ModalBodyFont").html(data);

//            var dateFormat = '';

//            if (dtNascimento != '') {
//                var dataNascimento = new Date(dtNascimento);
//                dataNascimento.customFormat("#DD#/#MM#/#YYYY#")
//            }

//            $("#nome").html(nome);
//            $("#dtNascimento").html(dateFormat);
//            $("#cpf").html(cpf);
//            $("#renach").html(renach);
//            $("#curso").html(curso);
//            $("#biometria").html(biometria);
//            $("#status").html(status);
//            $("#cep").html(cep);
//            $("#endereco").html(endereco);
//            $("#bairro").html(bairro);
//            $("#cidade").html(cidade);
//            $("#estado").html(estado);
//            $("#telefone").html(telefone);
//            $("#celular").html(celular);

//            ListarMatriculas(idAluno);
            
//        }
//    });
//}

//function ListarMatriculas(idAluno){

//    var url = "/HistoricoAluno/ListarMatriculas";

//    $.ajax({
//        url: url

//        , data: { idAluno: idAluno }
//        , type: "GET"
//        , datatype: "html"
//        , success: function (data) {
//            $("#listaMatriculas").html(data);
//        }
//    });
//}


//function ListarAgendas(idMatricula) {

//    var url = "/HistoricoAluno/ListarAgenda";
//    $.ajax({

//        url: url
//        , data: { idMatricula: idMatricula }
//        , type: "GET"
//        , datatype: "html"
//        , success: function (data) {
//            $("#listaAgendas").html(data);
//            ListarHistoricoAulas(idMatricula);
//        }
//    });

//}

//function ListarHistoricoAulas(idMatricula) {

//    var url = "/HistoricoAluno/ListarHistoricoAula";
//    $.ajax({
//        url: url
//        , data: { idMatricula: idMatricula }
//        , type: "GET"
//        , datatype: "html"
//        , success: function (data) {
//            $("#listaHistoricoAulas").html(data);
//        }
//    });

//}