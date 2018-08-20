//LOAD
$(document).ready(function () {

    //ListaAlunosDuplicados();
    $("#buscaAluno").click(function () {
        var cpf = $("#cpf").val();
        var nome = $("#Nome").val();
        var renach = $("#Renach").val();

        if (cpf == "" && nome == "" && renach == "") {
            Message("Informe um parâmetro para realizar a pesquisa", "INFO");
        }
        else {
            ListaAlunosDuplicados();
        }
    });


    // Limpar Busca
    $("#limparAluno").click(function () {
        $("#cpf").val("");
        $("#Renach").val("");
        $("#Nome").val("");
        var div = $("#listarAlunoDuplicado");
        div.empty();
    });

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaAluno').click();
            return false;
        }
    });

    waitingDialog.hide();

});

//Listar Alunos
function ListaAlunosDuplicados() {

    var cpf = RetornaCPFSemPontos($("#cpf").val());
    var nome = $("#Nome").val();
    var renach = $("#Renach").val();

    var url = "/Aluno/ListarAlunosDuplicados";
    var div = $("#listarAlunoDuplicado");

    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , cache: false
        , data: { cpf: cpf, nome: nome, renach :renach }
        , beforeSend: function () {
            waitingDialog.show();
        }
        , complete: function () {
            waitingDialog.hide();
        }
        , success: function (data) {
            div.empty();
            div.html(data);
            AplicarDataTable('AlunosDuplicados');
            AplicarMascara();
            waitingDialog.hide();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    })
}