/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [IdTipoContratacion]
      ,[DescripcionTipoContratacion]
      ,[EstatusRegistro]
      ,[FechaRegistro]
      ,[Fk_IdUsuarioRegistro]
      ,[Orden]
  FROM [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]

begin try
	begin tran

		insert into [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]
		Values ('Excepción Artículo 3 (Acuerdo de montos, aplicable 2020)',1,GETDATE(),30546,12)

		insert into [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]
		Values ('Excepción Artículo 4 (Acuerdo de montos, aplicable 2020)',1,GETDATE(),30546,13)

		insert into [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]
		Values ('Excepción Artículo 5 (Acuerdo de montos, aplicable 2020)',1,GETDATE(),30546,14)
	commit tran
end try
begin catch
	rollback tran
end catch

 SELECT TOP (1000) [IdTipoContratacion]
      ,[DescripcionTipoContratacion]
      ,[EstatusRegistro]
      ,[FechaRegistro]
      ,[Fk_IdUsuarioRegistro]
      ,[Orden]
  FROM [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]