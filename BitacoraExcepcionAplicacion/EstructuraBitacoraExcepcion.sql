
CREATE TABLE BitacoraExcepcion (
       IdBitacoraExepcion   int IDENTITY(1,1),
       Aplicacion           varchar(50) NULL,
       Modulo               varchar(50) NOT NULL,
       Funcion              varchar(50) NOT NULL,
       DescExcepcion  varchar(Max) NOT NULL,
       FechaRegistro            datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
       UsurarioRegistro                  varchar(20) NULL
)
go


ALTER TABLE BitacoraExcepcion
       ADD PRIMARY KEY NONCLUSTERED (IdBitacoraExepcion)
go



-- =============================================
-- Author:	 Julio C. Soria
-- Create date: 18/12/201x
-- Description:	regristrar una excepcion de una aplicacion
-- =============================================
alter PROCEDURE dbo.spuInsertBitocaraExcepcionAplicacion
@Aplicacion varchar(50),
@Modulo varchar(50),
@Funcion varchar(50),
@UsuarioRegistro  varchar(20),
@DescExcepcion varchar(max)

AS

BEGIN
		
	Insert into dbo.BitacoraExcepcion
	(Aplicacion, Modulo, Funcion, Usr, DescExcepcion)
	values(@Aplicacion, @Modulo, @Funcion, @UsuarioRegistro, @DescExcepcion)
		
    
END
GO