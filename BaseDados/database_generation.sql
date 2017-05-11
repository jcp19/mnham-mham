USE [master]
GO
/****** Object:  Database [mnham-mnham]    Script Date: 11/05/2017 12:02:08 ******/
CREATE DATABASE [mnham-mnham]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'mnham-mnham', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\mnham-mnham.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'mnham-mnham_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\mnham-mnham_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [mnham-mnham] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [mnham-mnham].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [mnham-mnham] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [mnham-mnham] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [mnham-mnham] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [mnham-mnham] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [mnham-mnham] SET ARITHABORT OFF 
GO
ALTER DATABASE [mnham-mnham] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [mnham-mnham] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [mnham-mnham] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [mnham-mnham] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [mnham-mnham] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [mnham-mnham] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [mnham-mnham] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [mnham-mnham] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [mnham-mnham] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [mnham-mnham] SET  DISABLE_BROKER 
GO
ALTER DATABASE [mnham-mnham] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [mnham-mnham] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [mnham-mnham] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [mnham-mnham] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [mnham-mnham] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [mnham-mnham] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [mnham-mnham] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [mnham-mnham] SET RECOVERY FULL 
GO
ALTER DATABASE [mnham-mnham] SET  MULTI_USER 
GO
ALTER DATABASE [mnham-mnham] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [mnham-mnham] SET DB_CHAINING OFF 
GO
ALTER DATABASE [mnham-mnham] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [mnham-mnham] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [mnham-mnham] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'mnham-mnham', N'ON'
GO
ALTER DATABASE [mnham-mnham] SET QUERY_STORE = OFF
GO
USE [mnham-mnham]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [mnham-mnham]
GO
/****** Object:  Table [dbo].[Alimento]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alimento](
	[id] [int] NOT NULL,
	[designacao] [nvarchar](150) NOT NULL,
	[preco] [decimal](7, 2) NULL,
	[removido] [tinyint] NOT NULL,
	[id_estabelecimento] [int] NOT NULL,
	[foto] [varbinary](max) NULL,
 CONSTRAINT [PK_Alimento] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassificacaoAlimento]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassificacaoAlimento](
	[id_cliente] [int] NOT NULL,
	[id_alimento] [int] NOT NULL,
	[valor] [int] NOT NULL,
	[comentario] [nvarchar](150) NULL,
	[data] [datetime] NULL,
 CONSTRAINT [PK_ClassificacaoAlimento] PRIMARY KEY CLUSTERED 
(
	[id_cliente] ASC,
	[id_alimento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassificacaoEstabelecimento]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassificacaoEstabelecimento](
	[id_estabelecimento] [int] NOT NULL,
	[id_cliente] [int] NOT NULL,
	[valor] [tinyint] NOT NULL,
	[comentario] [nvarchar](150) NULL,
	[data] [datetime] NULL,
 CONSTRAINT [PK_ClassificacaoEstabelecimento] PRIMARY KEY CLUSTERED 
(
	[id_estabelecimento] ASC,
	[id_cliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[id] [int] NOT NULL,
	[genero] [char](1) NOT NULL,
	[email] [nvarchar](50) NOT NULL,
	[nome] [nvarchar](75) NOT NULL,
	[palavra_passe] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Cliente] UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Estabelecimento]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Estabelecimento](
	[id] [int] NOT NULL,
	[contacto_tel] [char](19) NOT NULL,
	[morada] [nvarchar](50) NOT NULL,
	[coords] [nvarchar](30) NOT NULL,
	[horario] [nvarchar](200) NULL,
	[tipo] [nvarchar](50) NULL,
	[ativo] [tinyint] NOT NULL,
	[nome] [nvarchar](50) NOT NULL,
	[id_proprietario] [int] NULL,
	[foto] [varbinary](max) NULL,
	[descricao] [nvarchar](150) NULL,
	[aceita_reservas] [tinyint] NULL,
	[tem_mb] [tinyint] NULL,
	[tem_takeaway] [tinyint] NULL,
	[tem_serv_mesa] [tinyint] NULL,
	[tem_esplanada] [tinyint] NULL,
	[tem_parque_estac] [tinyint] NULL,
	[tem_tv] [tinyint] NULL,
	[tem_wifi] [tinyint] NULL,
	[tem_zona_fum] [tinyint] NULL,
 CONSTRAINT [PK_Establecimento] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ingrediente]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingrediente](
	[id] [int] NOT NULL,
	[designacao] [nvarchar](75) NOT NULL,
 CONSTRAINT [PK_Ingrediente] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Ingrediente] UNIQUE NONCLUSTERED 
(
	[designacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IngredienteAlimento]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IngredienteAlimento](
	[id_alimento] [int] NOT NULL,
	[id_ingrediente] [int] NOT NULL,
 CONSTRAINT [PK_IngredienteAlimento] PRIMARY KEY CLUSTERED 
(
	[id_alimento] ASC,
	[id_ingrediente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NaoPreferencia]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NaoPreferencia](
	[id_cliente] [int] NOT NULL,
	[designacao_ingrediente] [nvarchar](75) NOT NULL,
	[designacao_alimento] [nvarchar](75) NOT NULL,
 CONSTRAINT [PK_NaoPreferencia_1] PRIMARY KEY CLUSTERED 
(
	[id_cliente] ASC,
	[designacao_ingrediente] ASC,
	[designacao_alimento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pedido]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedido](
	[id] [int] NOT NULL,
	[termo] [nvarchar](150) NOT NULL,
	[data] [datetime] NOT NULL,
	[id_cliente] [int] NOT NULL,
 CONSTRAINT [PK_Pedido] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Preferencia]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Preferencia](
	[id_cliente] [int] NOT NULL,
	[designacao_ingrediente] [nvarchar](75) NOT NULL,
	[designcacao_alimento] [nvarchar](75) NOT NULL,
 CONSTRAINT [PK_Preferencia_1] PRIMARY KEY CLUSTERED 
(
	[id_cliente] ASC,
	[designacao_ingrediente] ASC,
	[designcacao_alimento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proprietario]    Script Date: 11/05/2017 12:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proprietario](
	[id] [int] NOT NULL,
	[email] [nvarchar](50) NOT NULL,
	[palavra_passe] [nvarchar](30) NOT NULL,
	[nome] [nvarchar](75) NOT NULL,
	[contacto_tel] [nchar](19) NOT NULL,
	[genero] [char](1) NOT NULL,
 CONSTRAINT [PK_Proprietario] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Proprietario] UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Alimento]  WITH CHECK ADD  CONSTRAINT [FK_Alimento_Establecimento] FOREIGN KEY([id_estabelecimento])
REFERENCES [dbo].[Estabelecimento] ([id])
GO
ALTER TABLE [dbo].[Alimento] CHECK CONSTRAINT [FK_Alimento_Establecimento]
GO
ALTER TABLE [dbo].[ClassificacaoAlimento]  WITH CHECK ADD  CONSTRAINT [FK_ClassificaAlimento_Alimento] FOREIGN KEY([id_alimento])
REFERENCES [dbo].[Alimento] ([id])
GO
ALTER TABLE [dbo].[ClassificacaoAlimento] CHECK CONSTRAINT [FK_ClassificaAlimento_Alimento]
GO
ALTER TABLE [dbo].[ClassificacaoAlimento]  WITH CHECK ADD  CONSTRAINT [FK_ClassificaAlimento_Cliente] FOREIGN KEY([id_cliente])
REFERENCES [dbo].[Cliente] ([id])
GO
ALTER TABLE [dbo].[ClassificacaoAlimento] CHECK CONSTRAINT [FK_ClassificaAlimento_Cliente]
GO
ALTER TABLE [dbo].[ClassificacaoEstabelecimento]  WITH CHECK ADD  CONSTRAINT [FK_ClassificaEstabelecimento_Cliente] FOREIGN KEY([id_cliente])
REFERENCES [dbo].[Cliente] ([id])
GO
ALTER TABLE [dbo].[ClassificacaoEstabelecimento] CHECK CONSTRAINT [FK_ClassificaEstabelecimento_Cliente]
GO
ALTER TABLE [dbo].[ClassificacaoEstabelecimento]  WITH CHECK ADD  CONSTRAINT [FK_ClassificaEstabelecimento_Establecimento] FOREIGN KEY([id_estabelecimento])
REFERENCES [dbo].[Estabelecimento] ([id])
GO
ALTER TABLE [dbo].[ClassificacaoEstabelecimento] CHECK CONSTRAINT [FK_ClassificaEstabelecimento_Establecimento]
GO
ALTER TABLE [dbo].[Estabelecimento]  WITH CHECK ADD  CONSTRAINT [FK_Establecimento_Proprietario] FOREIGN KEY([id_proprietario])
REFERENCES [dbo].[Proprietario] ([id])
GO
ALTER TABLE [dbo].[Estabelecimento] CHECK CONSTRAINT [FK_Establecimento_Proprietario]
GO
ALTER TABLE [dbo].[IngredienteAlimento]  WITH CHECK ADD  CONSTRAINT [FK_IngredienteAlimento_Alimento] FOREIGN KEY([id_alimento])
REFERENCES [dbo].[Alimento] ([id])
GO
ALTER TABLE [dbo].[IngredienteAlimento] CHECK CONSTRAINT [FK_IngredienteAlimento_Alimento]
GO
ALTER TABLE [dbo].[IngredienteAlimento]  WITH CHECK ADD  CONSTRAINT [FK_IngredienteAlimento_Ingrediente] FOREIGN KEY([id_ingrediente])
REFERENCES [dbo].[Ingrediente] ([id])
GO
ALTER TABLE [dbo].[IngredienteAlimento] CHECK CONSTRAINT [FK_IngredienteAlimento_Ingrediente]
GO
ALTER TABLE [dbo].[NaoPreferencia]  WITH CHECK ADD  CONSTRAINT [FK_NaoPreferencia_Cliente] FOREIGN KEY([id_cliente])
REFERENCES [dbo].[Cliente] ([id])
GO
ALTER TABLE [dbo].[NaoPreferencia] CHECK CONSTRAINT [FK_NaoPreferencia_Cliente]
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_Pedido_Cliente] FOREIGN KEY([id_cliente])
REFERENCES [dbo].[Cliente] ([id])
GO
ALTER TABLE [dbo].[Pedido] CHECK CONSTRAINT [FK_Pedido_Cliente]
GO
ALTER TABLE [dbo].[Preferencia]  WITH CHECK ADD  CONSTRAINT [FK_Preferencia_Cliente] FOREIGN KEY([id_cliente])
REFERENCES [dbo].[Cliente] ([id])
GO
ALTER TABLE [dbo].[Preferencia] CHECK CONSTRAINT [FK_Preferencia_Cliente]
GO
ALTER TABLE [dbo].[ClassificacaoAlimento]  WITH CHECK ADD  CONSTRAINT [CK_ClassificaAlimento] CHECK  (([valor]>=(1) AND [valor]<=(5)))
GO
ALTER TABLE [dbo].[ClassificacaoAlimento] CHECK CONSTRAINT [CK_ClassificaAlimento]
GO
ALTER TABLE [dbo].[ClassificacaoEstabelecimento]  WITH CHECK ADD  CONSTRAINT [CK_ClassificaEstabelecimento] CHECK  (([valor]>=(1) AND [valor]<=(5)))
GO
ALTER TABLE [dbo].[ClassificacaoEstabelecimento] CHECK CONSTRAINT [CK_ClassificaEstabelecimento]
GO
ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD  CONSTRAINT [CK_Cliente_genero] CHECK  (([genero]='M' OR [genero]='F'))
GO
ALTER TABLE [dbo].[Cliente] CHECK CONSTRAINT [CK_Cliente_genero]
GO
ALTER TABLE [dbo].[Proprietario]  WITH CHECK ADD  CONSTRAINT [CK_Proprietário_genero] CHECK  (([genero]='M' OR [genero]='F'))
GO
ALTER TABLE [dbo].[Proprietario] CHECK CONSTRAINT [CK_Proprietário_genero]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garante que o género é ''M'' ou ''F''' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Proprietario', @level2type=N'CONSTRAINT',@level2name=N'CK_Proprietário_genero'
GO
USE [master]
GO
ALTER DATABASE [mnham-mnham] SET  READ_WRITE 
GO
