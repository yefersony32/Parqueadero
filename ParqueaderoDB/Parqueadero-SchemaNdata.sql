USE [master]
GO
/****** Object:  Database [ParqueaderoDB]    Script Date: 6/03/2025 10:42:17 a. m. ******/
CREATE DATABASE [ParqueaderoDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ParqueaderoDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\ParqueaderoDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ParqueaderoDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\ParqueaderoDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ParqueaderoDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ParqueaderoDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ParqueaderoDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ParqueaderoDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ParqueaderoDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ParqueaderoDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ParqueaderoDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ParqueaderoDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ParqueaderoDB] SET  MULTI_USER 
GO
ALTER DATABASE [ParqueaderoDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ParqueaderoDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ParqueaderoDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ParqueaderoDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ParqueaderoDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ParqueaderoDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ParqueaderoDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [ParqueaderoDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ParqueaderoDB]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 6/03/2025 10:42:17 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[ClienteID] [int] IDENTITY(1,1) NOT NULL,
	[Cedula] [varchar](20) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Telefono] [varchar](15) NULL,
	[Correo] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ClienteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Espacios]    Script Date: 6/03/2025 10:42:17 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Espacios](
	[EspacioID] [int] IDENTITY(1,1) NOT NULL,
	[Nomenclatura] [varchar](10) NOT NULL,
	[Piso] [int] NOT NULL,
	[Zona] [char](1) NOT NULL,
	[Estado] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EspacioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pagos]    Script Date: 6/03/2025 10:42:17 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pagos](
	[PagoID] [int] IDENTITY(1,1) NOT NULL,
	[ReservaID] [int] NOT NULL,
	[MetodoPago] [varchar](20) NOT NULL,
	[Monto] [decimal](10, 2) NOT NULL,
	[FechaPago] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PagoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservas]    Script Date: 6/03/2025 10:42:17 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservas](
	[ReservaID] [int] IDENTITY(1,1) NOT NULL,
	[VehiculoID] [int] NOT NULL,
	[TipoReserva] [varchar](20) NOT NULL,
	[FechaIngreso] [datetime] NOT NULL,
	[FechaSalida] [datetime] NULL,
	[Monto] [decimal](10, 2) NULL,
	[EspacioID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReservaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 6/03/2025 10:42:17 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[UsuarioID] [int] IDENTITY(1,1) NOT NULL,
	[NombreUsuario] [nvarchar](50) NOT NULL,
	[ContraseñaHash] [varchar](64) NOT NULL,
	[Rol] [nvarchar](20) NOT NULL,
	[FechaCreacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UsuarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehiculos]    Script Date: 6/03/2025 10:42:17 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehiculos](
	[VehiculoID] [int] IDENTITY(1,1) NOT NULL,
	[Placa] [varchar](10) NOT NULL,
	[Tipo] [varchar](20) NOT NULL,
	[Marca] [varchar](50) NOT NULL,
	[Color] [varchar](30) NOT NULL,
	[ClienteID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[VehiculoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Clientes] ON 

INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (75, N'58048547', N'Cliente1', N'3002676105', N'cliente1@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (76, N'67741370', N'Cliente2', N'3007853135', N'cliente2@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (77, N'24616255', N'Cliente3', N'3001485062', N'cliente3@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (78, N'87812438', N'Cliente4', N'3005093110', N'cliente4@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (79, N'74309049', N'Cliente5', N'3004726777', N'cliente5@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (80, N'67422661', N'Cliente6', N'3001814295', N'cliente6@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (81, N'38698584', N'Cliente7', N'3005750362', N'cliente7@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (82, N'71917752', N'Cliente8', N'3009161504', N'cliente8@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (83, N'74194065', N'Cliente9', N'3007458601', N'cliente9@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (84, N'88446607', N'Cliente10', N'3001763610', N'cliente10@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (85, N'81795741', N'Cliente11', N'3006548172', N'cliente11@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (86, N'27930226', N'Cliente12', N'3005280507', N'cliente12@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (87, N'89037385', N'Cliente13', N'3004742712', N'cliente13@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (88, N'84362714', N'Cliente14', N'3003572926', N'cliente14@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (89, N'92038633', N'Cliente15', N'3006725226', N'cliente15@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (90, N'51641552', N'Cliente16', N'3006295845', N'cliente16@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (91, N'78446923', N'Cliente17', N'3002162222', N'cliente17@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (92, N'20330654', N'Cliente18', N'3004694767', N'cliente18@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (93, N'80582857', N'Cliente19', N'3004266625', N'cliente19@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (94, N'83425817', N'Cliente20', N'3004945809', N'cliente20@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (95, N'42767756', N'Cliente21', N'3004795637', N'cliente21@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (96, N'52770429', N'Cliente22', N'3003105514', N'cliente22@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (97, N'63960134', N'Cliente23', N'3009861629', N'cliente23@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (98, N'44153181', N'Cliente24', N'3008342247', N'cliente24@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (99, N'78819680', N'Cliente25', N'3002569481', N'cliente25@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (100, N'90982675', N'Cliente26', N'3002893300', N'cliente26@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (101, N'43094384', N'Cliente27', N'3004598976', N'cliente27@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (102, N'86252757', N'Cliente28', N'3007824532', N'cliente28@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (103, N'15572125', N'Cliente29', N'3001648885', N'cliente29@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (104, N'80069679', N'Cliente30', N'3001265036', N'cliente30@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (105, N'68256831', N'Cliente31', N'3005487100', N'cliente31@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (106, N'70943034', N'Cliente32', N'3007093914', N'cliente32@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (107, N'36608720', N'Cliente33', N'3002687647', N'cliente33@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (108, N'90932792', N'Cliente34', N'3007613222', N'cliente34@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (109, N'74045424', N'Cliente35', N'3004534059', N'cliente35@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (110, N'69072051', N'Cliente36', N'3003506673', N'cliente36@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (111, N'27062531', N'Cliente37', N'3003350869', N'cliente37@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (112, N'64617793', N'Cliente38', N'3002230078', N'cliente38@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (113, N'57423785', N'Cliente39', N'3002367898', N'cliente39@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (114, N'33840217', N'Cliente40', N'3003630364', N'cliente40@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (115, N'23823962', N'Cliente41', N'3006804140', N'cliente41@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (116, N'41064239', N'Cliente42', N'3006764220', N'cliente42@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (117, N'46140861', N'Cliente43', N'3008845158', N'cliente43@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (118, N'95454370', N'Cliente44', N'3009659206', N'cliente44@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (119, N'86376379', N'Cliente45', N'3007690526', N'cliente45@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (120, N'46853571', N'Cliente46', N'3009361777', N'cliente46@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (121, N'10258759', N'Cliente47', N'3006291874', N'cliente47@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (122, N'33504899', N'Cliente48', N'3001431645', N'cliente48@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (123, N'65089444', N'Cliente49', N'3004274331', N'cliente49@correo.com')
INSERT [dbo].[Clientes] ([ClienteID], [Cedula], [Nombre], [Telefono], [Correo]) VALUES (124, N'48443127', N'Cliente50', N'3007274924', N'cliente50@correo.com')
SET IDENTITY_INSERT [dbo].[Clientes] OFF
GO
SET IDENTITY_INSERT [dbo].[Espacios] ON 

INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (101, N'1A01', 1, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (102, N'1A02', 1, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (103, N'1A03', 1, N'A', N'Reservado')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (104, N'1A04', 1, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (105, N'1A05', 1, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (106, N'1A06', 1, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (107, N'1A07', 1, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (108, N'1A08', 1, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (109, N'1B01', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (110, N'1B02', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (111, N'1B03', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (112, N'1B04', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (113, N'1B05', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (114, N'1B06', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (115, N'1B07', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (116, N'1B08', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (117, N'1B09', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (118, N'1B10', 1, N'B', N'Ocupado')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (119, N'1B11', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (120, N'1B12', 1, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (121, N'2A01', 2, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (122, N'2A02', 2, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (123, N'2A03', 2, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (124, N'2A04', 2, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (125, N'2A05', 2, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (126, N'2A06', 2, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (127, N'2A07', 2, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (128, N'2A08', 2, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (129, N'2A09', 2, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (130, N'2A10', 2, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (131, N'2B01', 2, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (132, N'2B02', 2, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (133, N'2B03', 2, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (134, N'2B04', 2, N'B', N'Reservado')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (135, N'2B05', 2, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (136, N'2B06', 2, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (137, N'2B07', 2, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (138, N'2B08', 2, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (139, N'2B09', 2, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (140, N'2B10', 2, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (141, N'3A01', 3, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (142, N'3A02', 3, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (143, N'3A03', 3, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (144, N'3A04', 3, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (145, N'3A05', 3, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (146, N'3A06', 3, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (147, N'3A07', 3, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (148, N'3A08', 3, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (149, N'3A09', 3, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (150, N'3A10', 3, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (151, N'3B01', 3, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (152, N'3B02', 3, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (153, N'3B03', 3, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (154, N'3B04', 3, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (155, N'3B05', 3, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (156, N'3B06', 3, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (157, N'3B07', 3, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (158, N'3B08', 3, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (159, N'3B09', 3, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (160, N'3B10', 3, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (161, N'4A01', 4, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (162, N'4A02', 4, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (163, N'4A03', 4, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (164, N'4A04', 4, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (165, N'4A05', 4, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (166, N'4A06', 4, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (167, N'4A07', 4, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (168, N'4A08', 4, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (169, N'4A09', 4, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (170, N'4A10', 4, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (171, N'4B01', 4, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (172, N'4B02', 4, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (173, N'4B03', 4, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (174, N'4B04', 4, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (175, N'4B05', 4, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (176, N'4B06', 4, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (177, N'4B07', 4, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (178, N'4B08', 4, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (179, N'4B09', 4, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (180, N'4B10', 4, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (181, N'5A01', 5, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (182, N'5A02', 5, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (183, N'5A03', 5, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (184, N'5A04', 5, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (185, N'5A05', 5, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (186, N'5A06', 5, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (187, N'5A07', 5, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (188, N'5A08', 5, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (189, N'5A09', 5, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (190, N'5A10', 5, N'A', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (191, N'5B01', 5, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (192, N'5B02', 5, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (193, N'5B03', 5, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (194, N'5B04', 5, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (195, N'5B05', 5, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (196, N'5B06', 5, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (197, N'5B07', 5, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (198, N'5B08', 5, N'B', N'Disponible')
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (199, N'5B09', 5, N'B', N'Disponible')
GO
INSERT [dbo].[Espacios] ([EspacioID], [Nomenclatura], [Piso], [Zona], [Estado]) VALUES (200, N'5B10', 5, N'B', N'Disponible')
SET IDENTITY_INSERT [dbo].[Espacios] OFF
GO
SET IDENTITY_INSERT [dbo].[Pagos] ON 

INSERT [dbo].[Pagos] ([PagoID], [ReservaID], [MetodoPago], [Monto], [FechaPago]) VALUES (16, 36, N'Efectivo', CAST(50000.00 AS Decimal(10, 2)), CAST(N'2025-03-05T13:00:37.297' AS DateTime))
INSERT [dbo].[Pagos] ([PagoID], [ReservaID], [MetodoPago], [Monto], [FechaPago]) VALUES (1016, 1038, N'Efectivo', CAST(5000.00 AS Decimal(10, 2)), CAST(N'2025-03-06T10:19:25.870' AS DateTime))
INSERT [dbo].[Pagos] ([PagoID], [ReservaID], [MetodoPago], [Monto], [FechaPago]) VALUES (1017, 37, N'Tarjeta de Crédito', CAST(200000.00 AS Decimal(10, 2)), CAST(N'2025-03-06T10:33:29.073' AS DateTime))
SET IDENTITY_INSERT [dbo].[Pagos] OFF
GO
SET IDENTITY_INSERT [dbo].[Reservas] ON 

INSERT [dbo].[Reservas] ([ReservaID], [VehiculoID], [TipoReserva], [FechaIngreso], [FechaSalida], [Monto], [EspacioID]) VALUES (36, 116, N'Tiempo', CAST(N'2025-03-05T12:57:39.677' AS DateTime), CAST(N'2025-03-05T13:00:37.297' AS DateTime), CAST(1000.00 AS Decimal(10, 2)), 108)
INSERT [dbo].[Reservas] ([ReservaID], [VehiculoID], [TipoReserva], [FechaIngreso], [FechaSalida], [Monto], [EspacioID]) VALUES (37, 165, N'Reserva', CAST(N'2025-03-05T12:59:33.077' AS DateTime), CAST(N'2025-03-06T10:33:29.073' AS DateTime), CAST(200000.00 AS Decimal(10, 2)), 200)
INSERT [dbo].[Reservas] ([ReservaID], [VehiculoID], [TipoReserva], [FechaIngreso], [FechaSalida], [Monto], [EspacioID]) VALUES (38, 117, N'Reserva', CAST(N'2025-03-05T13:02:47.453' AS DateTime), NULL, CAST(200000.00 AS Decimal(10, 2)), 118)
INSERT [dbo].[Reservas] ([ReservaID], [VehiculoID], [TipoReserva], [FechaIngreso], [FechaSalida], [Monto], [EspacioID]) VALUES (1036, 118, N'Reserva', CAST(N'2025-03-06T10:06:04.010' AS DateTime), NULL, CAST(200000.00 AS Decimal(10, 2)), 134)
INSERT [dbo].[Reservas] ([ReservaID], [VehiculoID], [TipoReserva], [FechaIngreso], [FechaSalida], [Monto], [EspacioID]) VALUES (1037, 119, N'Reserva', CAST(N'2025-03-06T10:18:12.100' AS DateTime), NULL, CAST(200000.00 AS Decimal(10, 2)), 103)
INSERT [dbo].[Reservas] ([ReservaID], [VehiculoID], [TipoReserva], [FechaIngreso], [FechaSalida], [Monto], [EspacioID]) VALUES (1038, 120, N'Tiempo', CAST(N'2025-03-06T10:18:48.360' AS DateTime), CAST(N'2025-03-06T10:19:25.870' AS DateTime), CAST(1000.00 AS Decimal(10, 2)), 140)
SET IDENTITY_INSERT [dbo].[Reservas] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 

INSERT [dbo].[Usuarios] ([UsuarioID], [NombreUsuario], [ContraseñaHash], [Rol], [FechaCreacion]) VALUES (3, N'admins', N'????????????????', N'Admin', CAST(N'2025-03-01T17:08:56.367' AS DateTime))
INSERT [dbo].[Usuarios] ([UsuarioID], [NombreUsuario], [ContraseñaHash], [Rol], [FechaCreacion]) VALUES (5, N'Adminss', N'????????????????', N'Admin', CAST(N'2025-03-01T17:14:31.347' AS DateTime))
INSERT [dbo].[Usuarios] ([UsuarioID], [NombreUsuario], [ContraseñaHash], [Rol], [FechaCreacion]) VALUES (7, N'admin', N'5994471ABB01112AFCC18159F6CC74B4F511B99806DA59B3CAF5A9C173CACFC5', N'Admin', CAST(N'2025-03-01T17:18:25.067' AS DateTime))
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
SET IDENTITY_INSERT [dbo].[Vehiculos] ON 

INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (116, N'IR590I', N'Carro', N'Nissan', N'Gris', 76)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (117, N'YG613B', N'Carro', N'Mazda', N'Rojo', 77)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (118, N'WH706Z', N'Carro', N'Chevrolet', N'Blanco', 78)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (119, N'NM615N', N'Carro', N'Nissan', N'Rojo', 79)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (120, N'NQ985K', N'Carro', N'Nissan', N'Azul', 80)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (123, N'KN574K', N'Carro', N'Chevrolet', N'Azul', 83)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (124, N'IY741R', N'Carro', N'Toyota', N'Rojo', 84)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (125, N'WP463X', N'Moto', N'Nissan', N'Gris', 85)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (127, N'EJ702Y', N'Carro', N'Nissan', N'Azul', 87)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (128, N'YQ771G', N'Carro', N'Chevrolet', N'Gris', 88)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (129, N'NX572Y', N'Moto', N'Toyota', N'Rojo', 89)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (130, N'UM821C', N'Carro', N'Honda', N'Rojo', 90)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (132, N'UA405B', N'Carro', N'Chevrolet', N'Azul', 92)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (134, N'HY960G', N'Moto', N'Nissan', N'Gris', 94)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (135, N'ID500O', N'Moto', N'Nissan', N'Gris', 95)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (136, N'HS926Z', N'Moto', N'Nissan', N'Negro', 96)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (138, N'CS275G', N'Carro', N'Honda', N'Gris', 98)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (139, N'CO367V', N'Moto', N'Nissan', N'Blanco', 99)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (140, N'XQ651E', N'Moto', N'Toyota', N'Gris', 100)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (142, N'HC258J', N'Carro', N'Nissan', N'Gris', 102)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (144, N'CL640O', N'Moto', N'Honda', N'Rojo', 104)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (145, N'ZD453X', N'Carro', N'Honda', N'Negro', 105)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (146, N'RC368Z', N'Carro', N'Nissan', N'Rojo', 106)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (147, N'EU153F', N'Moto', N'Honda', N'Rojo', 107)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (148, N'MZ506X', N'Carro', N'Nissan', N'Rojo', 108)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (150, N'XD516U', N'Carro', N'Honda', N'Gris', 110)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (153, N'XM935L', N'Moto', N'Nissan', N'Negro', 113)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (156, N'TR612R', N'Moto', N'Toyota', N'Gris', 116)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (157, N'AP404U', N'Carro', N'Toyota', N'Azul', 117)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (158, N'JM119I', N'Moto', N'Nissan', N'Gris', 118)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (159, N'HD353S', N'Carro', N'Mazda', N'Azul', 119)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (160, N'IL277B', N'Moto', N'Nissan', N'Gris', 120)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (163, N'UC433R', N'Moto', N'Toyota', N'Negro', 123)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (164, N'LE891W', N'Moto', N'Nissan', N'Gris', 124)
INSERT [dbo].[Vehiculos] ([VehiculoID], [Placa], [Tipo], [Marca], [Color], [ClienteID]) VALUES (165, N'FFY900', N'Moto', N'Nissan', N'Azul', 76)
SET IDENTITY_INSERT [dbo].[Vehiculos] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Clientes__B4ADFE380BF1C2D2]    Script Date: 6/03/2025 10:42:17 a. m. ******/
ALTER TABLE [dbo].[Clientes] ADD UNIQUE NONCLUSTERED 
(
	[Cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Espacios__8F9251A23B6A2136]    Script Date: 6/03/2025 10:42:17 a. m. ******/
ALTER TABLE [dbo].[Espacios] ADD UNIQUE NONCLUSTERED 
(
	[Nomenclatura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Usuarios__6B0F5AE034EC4BA0]    Script Date: 6/03/2025 10:42:17 a. m. ******/
ALTER TABLE [dbo].[Usuarios] ADD UNIQUE NONCLUSTERED 
(
	[NombreUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Vehiculo__8310F99D438BB23B]    Script Date: 6/03/2025 10:42:17 a. m. ******/
ALTER TABLE [dbo].[Vehiculos] ADD UNIQUE NONCLUSTERED 
(
	[Placa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Espacios] ADD  DEFAULT ('Disponible') FOR [Estado]
GO
ALTER TABLE [dbo].[Pagos] ADD  DEFAULT (getdate()) FOR [FechaPago]
GO
ALTER TABLE [dbo].[Reservas] ADD  DEFAULT (getdate()) FOR [FechaIngreso]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ('Admin') FOR [Rol]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD FOREIGN KEY([ReservaID])
REFERENCES [dbo].[Reservas] ([ReservaID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reservas]  WITH CHECK ADD FOREIGN KEY([VehiculoID])
REFERENCES [dbo].[Vehiculos] ([VehiculoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reservas]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Espacios] FOREIGN KEY([EspacioID])
REFERENCES [dbo].[Espacios] ([EspacioID])
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [FK_Reservas_Espacios]
GO
ALTER TABLE [dbo].[Vehiculos]  WITH CHECK ADD FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Clientes] ([ClienteID])
ON DELETE CASCADE
GO
USE [master]
GO
ALTER DATABASE [ParqueaderoDB] SET  READ_WRITE 
GO
