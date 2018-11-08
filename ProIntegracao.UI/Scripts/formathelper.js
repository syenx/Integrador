jQuery(function ($) {
    AplicarMascara();
});

function AplicarMascara()
{
    $(".cpf").mask("999.999.999-99");
    //$(".cfc").mask("999999");
    $(".hora").mask("99:99");
    $(".data").mask("99/99/9999");
    $(".datetime").mask("99/99/9999 99:99:99");
    $(".psa").mask("PSA9999999999");
    $("#PSA").mask("PSA9999999999");

    $(".mesano").mask("99/9999");

}
