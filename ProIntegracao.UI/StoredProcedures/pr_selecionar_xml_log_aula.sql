USE [ProSimulador]
GO
/****** Object:  StoredProcedure [dbo].[pr_selecionar_xml_log_aula]    Script Date: 18/04/2016 09:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==============================================================
-- AUTHOR:		Fernando Parmezani
-- CREATE DATE: 2016/04/31
-- DESCRIPTION:	Selecionar XML Log AULA
-- ==============================================================
-- exec pr_selecionar_xml_log_aula '00839476124',2016-04-19, '2016-04-20'
ALTER PROCEDURE [dbo].[pr_selecionar_xml_log_aula]
	@CPF					VARCHAR(50)		
	, @DTCADASTROINICIO     DATETIME		
	, @DTCADASTROFINAL      DATETIME
	, @PSA					VARCHAR(15)

AS
BEGIN
SET NOCOUNT ON;


	SELECT 
		X.ID_EMPRESA,
		X.RAZAO_SOCIAL,
		X.DATA_CADASTRO,
		X.DETALHES,
		X.IP,
		X.XML_ENTRADA,
		X.XML_SAIDA

	FROM(
	
		SELECT
			WSLOG.ID_EMPRESA,
			(SELECT E.RAZAO_SOCIAL FROM TB_EMPRESA E (NOLOCK) WHERE E.ID_EMPRESA = WSLOG.ID_EMPRESA) AS RAZAO_SOCIAL,
			WSLOG.DATA_CADASTRO,
			WSLOG.DETALHES,
			WSLOG.IP,
			WSLOG.XML_ENTRADA,
			WSLOG.XML_SAIDA
		FROM 
			TB_WS_LOG WSLOG   (nolock) 
		WHERE
			WSLOG.DATA_CADASTRO BETWEEN @DTCADASTROINICIO AND @DTCADASTROFINAL
		AND 
			WSLOG.ID_WS IN (1,2,3)
		AND 
			(convert(varchar(max), WSLOG.XML_ENTRADA) LIKE '%' + @CPF +'%' OR convert(varchar(max), WSLOG.XML_SAIDA) LIKE '%' + @CPF +'%')
		AND (
			@PSA = '' OR 
			(
				(convert(varchar(max), WSLOG.XML_ENTRADA) LIKE '%' + @PSA +'%' OR convert(varchar(max), WSLOG.XML_SAIDA) LIKE '%' + @PSA +'%')
			) 
		)
	
		UNION ALL
	
		SELECT
			WSLOGHIST.ID_EMPRESA,
			(SELECT E.RAZAO_SOCIAL FROM TB_EMPRESA E (NOLOCK) WHERE E.ID_EMPRESA = WSLOGHIST.ID_EMPRESA) AS RAZAO_SOCIAL,
			WSLOGHIST.DATA_CADASTRO,
			WSLOGHIST.DETALHES,
			WSLOGHIST.IP,
			WSLOGHIST.XML_ENTRADA,
			WSLOGHIST.XML_SAIDA
		FROM 
			TB_WS_LOG_HIST WSLOGHIST (nolock)
		WHERE 
			WSLOGHIST.DATA_CADASTRO BETWEEN @DTCADASTROINICIO AND @DTCADASTROFINAL
		AND 
			WSLOGHIST.ID_WS IN (1,2,3)
		AND 
			(convert(varchar(max), WSLOGHIST.XML_ENTRADA) LIKE '%' + @CPF +'%' OR convert(varchar(max), WSLOGHIST.XML_SAIDA) LIKE '%' + @CPF +'%')
		AND (
			@PSA = '' OR 
			(
				(convert(varchar(max), WSLOG.XML_ENTRADA) LIKE '%' + @PSA +'%' OR convert(varchar(max), WSLOG.XML_SAIDA) LIKE '%' + @PSA +'%')
			) 
		)


			) as X
	--INNER JOIN 
	--		TB_EMPRESA E (nolock) ON E.ID_EMPRESA = X.ID_EMPRESA
		
END