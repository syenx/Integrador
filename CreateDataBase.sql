USE [INTEGRADOR]
GO
/****** Object:  Table [dbo].[TB_REGIAO]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_REGIAO](
	[ID_REGIAO] [int] IDENTITY(1,1) NOT NULL,
	[DESCRICAO] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_UF]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_UF](
	[ID_UF] [int] IDENTITY(1,1) NOT NULL,
	[NOME] [varchar](60) NULL,
	[SIGLA] [varchar](30) NULL,
	[ID_REGIAO] [int] NULL,
	[DIFERENCA_HORARIO] [int] NULL,
	[DATA_EXIGIBILIDADE] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbAluno]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAluno](
	[IdAluno] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](60) NULL,
	[CpfAluno] [varchar](60) NULL,
	[Renach] [varchar](60) NULL,
	[IdSexo] [int] NULL,
	[DtCadastro] [varchar](60) NULL,
	[DtExclusao] [varchar](60) NULL,
	[DtNascimento] [varchar](60) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbAula]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAula](
	[IdAula] [int] IDENTITY(1,1) NOT NULL,
	[IdMatricula] [int] NULL,
	[IdStatusSituacaoAula] [int] NULL,
	[CodigoCfc] [varchar](60) NULL,
	[IdentificadorAula] [varchar](60) NULL,
	[CpfInstrutor] [varchar](60) NULL,
	[DataInicioAula] [datetime] NULL,
	[DataFimAula] [datetime] NULL,
	[TokenInicioAula] [varchar](60) NULL,
	[TokenFimAula] [varchar](60) NULL,
	[DtCadastro] [varchar](60) NULL,
	[DtExclusao] [varchar](60) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbErro]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbErro](
	[IdErro] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](60) NULL,
	[Descricao] [varchar](255) NULL,
	[IdEstado] [int] NULL,
	[CodigoErro] [varchar](60) NULL,
	[DtCadastro] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbEstado]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbEstado](
	[IdEstado] [int] IDENTITY(1,1) NOT NULL,
	[Uf] [varchar](10) NULL,
	[Nome] [varchar](20) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbForcarErro]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbForcarErro](
	[IdForcarErro] [int] IDENTITY(1,1) NOT NULL,
	[IdAluno] [int] NULL,
	[IdTipoErro] [int] NULL,
	[DtCadastro] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbMatricula]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbMatricula](
	[IdMatricula] [int] IDENTITY(1,1) NOT NULL,
	[IdAluno] [int] NULL,
	[IdEstado] [int] NULL,
	[QtdAula] [int] NULL,
	[CodigoCfc] [int] NULL,
	[HoraAula] [varchar](30) NULL,
	[PSA] [varchar](60) NULL,
	[DtCadastro] [varchar](60) NULL,
	[DtExclusao] [varchar](60) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbMenu]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbMenu](
	[IdMenu] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](60) NULL,
	[Ordem] [varchar](60) NULL,
	[Url] [varchar](60) NULL,
	[DtCadastro] [varchar](60) NULL,
	[DtExclusao] [varchar](60) NULL,
	[IdMenuPai] [int] NULL,
	[Admin] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPagina]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPagina](
	[IdPagina] [int] IDENTITY(1,1) NOT NULL,
	[IdMenu] [int] NULL,
	[IdEstado] [int] NULL,
	[Url] [varchar](60) NULL,
	[Ordem] [int] NULL,
	[Nome] [varchar](60) NULL,
	[Icone] [varchar](60) NULL,
	[DtCadastro] [varchar](60) NULL,
	[DtExclusao] [varchar](60) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPerfil]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPerfil](
	[IdPerfil] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](60) NULL,
	[Admin] [bit] NULL,
	[DtCadastro] [varchar](60) NULL,
	[DtExclusao] [varchar](60) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPerfilEstado]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPerfilEstado](
	[IdPerfilEstado] [int] IDENTITY(1,1) NOT NULL,
	[IdEstado] [int] NULL,
	[IdPerfil] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPerfilPagina]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPerfilPagina](
	[IdPerfilPagina] [int] IDENTITY(1,1) NOT NULL,
	[IdPagina] [int] NULL,
	[IdPerfil] [int] NULL,
	[Inserir] [bit] NULL,
	[Atualizar] [bit] NULL,
	[Excluir] [bit] NULL,
	[Consultar] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbSexo]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSexo](
	[IdSexo] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](60) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbStatusSituacaoAula]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbStatusSituacaoAula](
	[IdStatusSituacaoAula] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](60) NULL,
	[Identificador] [int] NULL,
	[IdEstado] [int] NULL,
	[DtCadastro] [varchar](60) NULL,
	[DtExclusao] [varchar](60) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbUsuario]    Script Date: 08/11/2018 17:02:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbUsuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Login] [varchar](30) NULL,
	[Senha] [varchar](30) NULL,
	[Email] [varchar](60) NULL,
	[Bloqueado] [bit] NULL,
	[Hash] [varchar](255) NULL,
	[DtExclusao] [datetime] NULL,
	[DtCadastro] [datetime] NULL,
	[IdPerfil] [int] NULL
) ON [PRIMARY]
GO
