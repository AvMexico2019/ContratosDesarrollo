/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [IdTipoContratacion]
      ,[DescripcionTipoContratacion]
      ,[EstatusRegistro]
      ,[FechaRegistro]
      ,[Fk_IdUsuarioRegistro]
      ,[Orden]
  FROM [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]

  insert into [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]
  Values ('Excepci�n Art�culo 3 (Acuerdo de montos, aplicable 2020)',1,GETDATE(),30546,12)

  insert into [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]
  Values ('Excepci�n Art�culo 4 (Acuerdo de montos, aplicable 2020)',1,GETDATE(),30546,13)

  insert into [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]
  Values ('Excepci�n Art�culo 5 (Acuerdo de montos, aplicable 2020)',1,GETDATE(),30546,14)

 SELECT TOP (1000) [IdTipoContratacion]
      ,[DescripcionTipoContratacion]
      ,[EstatusRegistro]
      ,[FechaRegistro]
      ,[Fk_IdUsuarioRegistro]
      ,[Orden]
  FROM [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]