USE [ArrendamientoInmueble]
GO

/****** Object:  Table [SandBox].[Rel_ConceptoRespValor]    Script Date: 20/01/2020 01:50:18 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SandBox].[LogRel_ConceptoRespValor](
	[FechaCambio] [DATETIME] NOT NULL,
	[IdConceptoRespValor] [INT] NOT NULL,
	[Fk_IdInstitucion] [int] NULL,
	[Fk_IdTema] [tinyint] NOT NULL,
	[Fk_IdConcepto] [int] NOT NULL,
	[Fk_IdRespuesta] [int] NOT NULL,
	[NumOrden] [decimal](5, 2) NOT NULL,
	[EsDeterminante] [bit] NOT NULL,
	[ValorRespuesta] [numeric](6, 2) NULL,
	[ValorMinimo] [decimal](10, 2) NOT NULL,
	[ValorMaximo] [decimal](10, 2) NOT NULL,
	[Comentario] [varchar](150) NULL,
	[EstatusRegistro] [bit] NOT NULL,
	[Fk_IdUsuarioRegistro] [int] NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
)
GO

