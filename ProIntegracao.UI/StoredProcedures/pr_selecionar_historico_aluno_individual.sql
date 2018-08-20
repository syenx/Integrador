USE [ProSimulador]
GO
/****** Object:  StoredProcedure [dbo].[pr_selecionar_historico_aluno_individual]    
Script Date: 16/05/2016 09:01:36 
******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==============================================================
-- AUTHOR:		Fernando Parmezani
-- CREATE DATE: 2016/04/31
-- DESCRIPTION:	Selecionar Historico Aluno Individual
-- ==============================================================
-- exec pr_selecionar_historico_aluno_individual 686516

CREATE PROCEDURE [dbo].[pr_selecionar_historico_aluno_individual]
	@ID_ALUNO INT
AS BEGIN

	SET NOCOUNT OFF;

	SELECT  top 1
		P.NOME [NOME]
		, ISNULL(P.DATA_NASCIMENTO,'1900-01-01') [DATA_NASCIMENTO]
		, P.CPF [CPF]
		, A.NUMERO_HABILITACAO [RENACH]
		, A.CATEGORIA_PRETENDIDA [CURSO]
		 ,CASE 
			WHEN P.ID_BIOMETRIA IS NULL THEN 'Não validada'
			ELSE 'Validada'
		  END [BIOMETRIA]
		 ,E.CEP [CEP]
		 ,E.LOGRADOURO [ENDERECO]
		 ,E.BAIRRO [BAIRRO]
		 ,MU.NOME [CIDADE]
		 ,UF.NOME [ESTADO]
		 ,('(' + P.DDD_TELEFONE + ') ' +  P.TELEFONE) [TELEFONE]
		 ,('(' + P.DDD_CELULAR	+ ') ' +  P.CELULAR) [CELULAR]
		, CASE P.ATIVO 
			WHEN 0 THEN 'InAtivo'
			WHEN 1 THEN 'Ativo'
		END	 [STATUS]
		, A.ID_ALUNO
		FROM TB_PESSOA P (NOLOCK)
			INNER JOIN TB_ALUNO A (NOLOCK)
				ON P.ID_PESSOA = A.ID_ALUNO
			INNER JOIN TB_ENDERECO E (NOLOCK)
				ON P.ID_ENDERECO = E.ID_ENDERECO
			INNER JOIN TB_MUNICIPIO MU (NOLOCK)
				ON E.ID_MUNICIPIO = MU.ID_MUNICIPIO
			INNER JOIN TB_UF UF (NOLOCK)
				ON MU.ID_UF = UF.ID_UF
		WHERE
			A.ID_ALUNO = @ID_ALUNO
		
END