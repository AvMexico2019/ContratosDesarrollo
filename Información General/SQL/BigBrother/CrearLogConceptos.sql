USE [ArrendamientoInmueble]
GO

/****** Object:  Table [SandBox].[Concepto]    Script Date: 20/01/2020 12:34:29 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SandBox].[LogConcepto](
	[FechaCambio] [DATETIME] NOT NULL,
	[IdConcepto] [int] NOT NULL,
	[DescripcionConcepto] [varchar](500) NOT NULL,
	[DescripcionAlternaConcepto] [varchar](500) NULL,
	[FundamentoLegal] [varchar](max) NULL,
	[Observaciones] [varchar](300) NULL,
	[EstatusRegistro] [bit] NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[Fk_IdUsuarioRegistro] [int] NOT NULL
)

GO


