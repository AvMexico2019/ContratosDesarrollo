USE [ArrendamientoInmueble]
GO

/****** Object:  StoredProcedure [SandBox].[GetINFOChanged]    Script Date: 21/01/2020 05:14:56 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		Desa21-Raymundo Peralta
-- Create date: 13 dic 2019
-- Description:	
-- =============================================
CREATE PROCEDURE [SandBox].[GetINFOChanged]
@Ahora DATETIME
AS
BEGIN
	Create Table #INFO
	(
		INFO NVARCHAR(MAX)
	)

	insert into #INFO
	SELECT CONCAT('Concepto', '>'
	  ,[Causa], '>'
      ,[FechaCambio], '>'
      ,[IdConcepto], '>'
      ,[DescripcionConcepto]) as INFO
	FROM [ArrendamientoInmueble].[SandBox].[LogConcepto]  where FechaCambio > @Ahora

	insert into #INFO
	SELECT CONCAT('Pregunta', '>'
      ,[Causa], '>'
      ,[FechaCambio], '>'
	  ,[IdConceptoRespValor], '>'
      ,[Fk_IdTema], '>'
      ,[NumOrden], '>'
      ,[Fk_IdConcepto]) as INFO
	FROM [ArrendamientoInmueble].[SandBox].[LogRel_ConceptoRespValor]  where FechaCambio > @Ahora

	select * from #INFO


END

GO


