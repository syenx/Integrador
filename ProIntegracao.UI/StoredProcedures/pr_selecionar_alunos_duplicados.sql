USE [ProSimulador]
GO
/****** Object:  StoredProcedure [dbo].[pr_selecionar_alunos_duplicados]    Script Date: 26/04/2016 14:41:04 ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==============================================================
-- AUTHOR:		Fernando Parmezani
-- CREATE DATE: 2016/04/05
-- DESCRIPTION:	Selecionar Alunos Duplicados
-- ==============================================================
-- exec pr_selecionar_alunos_duplicados '','Getulio',

CREATE PROCEDURE [dbo].[pr_selecionar_alunos_duplicados]
	@CPF					VARCHAR(11) = ''
	, @NOME					VARCHAR(255) = ''
	, @RENACH				VARCHAR(255) = ''
AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT
		P.ID_CFC [CFC],
		E.CODIGO,
		E.RAZAO_SOCIAL [RazaoSocial],
		P.CPF [CPF],
		P.NOME [NOME],
		CASE P.ATIVO 
			WHEN '0' THEN 'INATIVO' 
			WHEN '1' THEN 'ATIVO' 
		END AS 'Status'
	FROM TB_PESSOA P (NOLOCK)
		INNER JOIN TB_EMPRESA E (NOLOCK) 
	ON E.ID_EMPRESA = P.ID_CFC
		INNER JOIN TB_ALUNO A (NOLOCK)
	ON P.ID_PESSOA = A.ID_ALUNO
	WHERE 1 = 1 
		 AND (@CPF = ''		OR (P.CPF = @CPF))
		 AND (@NOME = ''	OR (P.NOME LIKE '%' + @NOME + '%'))
		 AND (@RENACH = ''	OR (A.NUMERO_HABILITACAO LIKE '%' + @RENACH + '%'))
END