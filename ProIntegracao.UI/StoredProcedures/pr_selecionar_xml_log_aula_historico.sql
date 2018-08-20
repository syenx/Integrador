USE ProSimulador
GO

/****** Object:  StoredProcedure [dbo].[pr_selecionar_xml_log_aula_dia]    Script Date: 27/04/2016 09:52:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==============================================================
-- AUTHOR:		Fernando Parmezani
-- CREATE DATE: 2016/04/31
-- DESCRIPTION:	Selecionar XML Log AULA - HISTORICO
-- ==============================================================
-- exec pr_selecionar_xml_log_aula_historico '129.444.696-73', '', ''


 ALTER PROCEDURE [dbo].[pr_selecionar_xml_log_aula_historico]
	@DETALHES			VARCHAR(255)
	, @DTINICIO			DATETIME = NULL
	, @DTFINAL			DATETIME = NULL
	, @PSA				VARCHAR(15)
	, @ACAO				INT = 0
AS 

BEGIN

	SELECT 
		 U.SIGLA AS UF,
		 WSLOGHIST.ID_EMPRESA,
		 EM.CODIGO,	 
		 EM.RAZAO_SOCIAL,
		 EM.NOME_FANTASIA,
		 WSLOGHIST.DATA_CADASTRO,
		 WS.NOME AS [ACAO],
		 WSLOGHIST.DETALHES,
		 WSLOGHIST.IP,
		 WSLOGHIST.XML_ENTRADA,
		 WSLOGHIST.XML_SAIDA
	FROM 
		TB_WS_LOG_HIST WSLOGHIST	with(nolock)
		INNER JOIN TB_WS WS			with(nolock) ON WS.ID_WS = WSLOGHIST.ID_WS
		INNER JOIN TB_EMPRESA EM	with(nolock) ON EM.ID_EMPRESA = WSLOGHIST.ID_EMPRESA
		INNER JOIN TB_ENDERECO EN	with(nolock) ON EN.ID_ENDERECO = EM.ID_ENDERECO
		INNER JOIN TB_MUNICIPIO M	with(nolock) ON M.ID_MUNICIPIO = EN.ID_MUNICIPIO
		INNER JOIN TB_UF U			with(nolock) ON U.ID_UF = M.ID_UF
	WHERE 1=1
		AND DETALHES LIKE '%'+ @DETALHES +'%' 
		AND (@ACAO = 0 OR WSLOGHIST.ID_WS = @ACAO)
		AND (@DTINICIO IS NULL OR (WSLOGHIST.DATA_CADASTRO BETWEEN @DTINICIO AND @DTFINAL))
		AND (@PSA IS NULL OR (DETALHES LIKE '%'+ @PSA +'%'))
END