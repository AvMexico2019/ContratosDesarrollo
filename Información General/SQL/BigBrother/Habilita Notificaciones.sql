USE [master]
GO

EXEC dbo.sp_dbcmptlevel @dbname=N'ArrendamientoInmueble', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ArrendamientoInmueble].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [ArrendamientoInmueble] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET ARITHABORT OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ArrendamientoInmueble] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ArrendamientoInmueble] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ArrendamientoInmueble] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET  ENABLE_BROKER   --   IMPORTANTE
GO
ALTER DATABASE [ArrendamientoInmueble] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ArrendamientoInmueble] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ArrendamientoInmueble] SET  READ_WRITE 
GO
ALTER DATABASE [ArrendamientoInmueble] SET RECOVERY FULL 
GO
ALTER DATABASE [ArrendamientoInmueble] SET  MULTI_USER 
GO
ALTER DATABASE [ArrendamientoInmueble] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ArrendamientoInmueble] SET DB_CHAINING OFF 

/***************************************************/
