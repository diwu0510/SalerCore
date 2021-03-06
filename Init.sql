USE [master]
GO
/****** Object:  Database [Zodo.Saler.Core]    Script Date: 11/16/2018 10:27:08 ******/
CREATE DATABASE [Zodo.Saler.Core] ON  PRIMARY 
( NAME = N'Zodo.Saler.Core', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\Zodo.Saler.Core.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Zodo.Saler.Core_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\Zodo.Saler.Core_log.ldf' , SIZE = 2816KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Zodo.Saler.Core] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Zodo.Saler.Core].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Zodo.Saler.Core] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET ANSI_NULLS OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET ANSI_PADDING OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET ARITHABORT OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [Zodo.Saler.Core] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [Zodo.Saler.Core] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [Zodo.Saler.Core] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET  DISABLE_BROKER
GO
ALTER DATABASE [Zodo.Saler.Core] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [Zodo.Saler.Core] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [Zodo.Saler.Core] SET  READ_WRITE
GO
ALTER DATABASE [Zodo.Saler.Core] SET RECOVERY FULL
GO
ALTER DATABASE [Zodo.Saler.Core] SET  MULTI_USER
GO
ALTER DATABASE [Zodo.Saler.Core] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [Zodo.Saler.Core] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'Zodo.Saler.Core', N'ON'
GO
USE [Zodo.Saler.Core]
GO
/****** Object:  Table [dbo].[MonthReportLog]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonthReportLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Operator] [nvarchar](20) NULL,
	[OperateAt] [datetime] NULL,
	[Role] [nvarchar](20) NULL,
	[OrigReport] [nvarchar](max) NULL,
	[NewReport] [nvarchar](max) NULL,
 CONSTRAINT [PK_MonthReportLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Job]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Job](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
	[IsDel] [bit] NULL,
	[CreateAt] [datetime] NULL,
	[Creator] [nvarchar](20) NULL,
	[UpdateAt] [datetime] NULL,
	[Updator] [nvarchar](20) NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Job', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Job', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Job', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
/****** Object:  Table [dbo].[Base_User]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Base_User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Pw] [varchar](50) NULL,
	[Role] [nvarchar](10) NULL,
	[IsDel] [bit] NULL,
	[CreateAt] [datetime] NULL,
	[CreateBy] [int] NULL,
	[Creator] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[Updator] [nvarchar](50) NULL,
 CONSTRAINT [PK_Base_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_User', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_User', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_User', @level2type=N'COLUMN',@level2name=N'Pw'
GO
/****** Object:  Table [dbo].[Base_Saler]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Base_Saler](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
	[DeptId] [int] NULL,
	[Job] [nvarchar](20) NULL,
	[Salary] [int] NULL,
	[IsDel] [bit] NULL,
	[CreateAt] [datetime] NULL,
	[CreateBy] [int] NULL,
	[Creator] [nvarchar](20) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[Updator] [nvarchar](20) NULL,
 CONSTRAINT [PK_Saler] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Saler', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Saler', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Saler', @level2type=N'COLUMN',@level2name=N'Job'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工资' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Saler', @level2type=N'COLUMN',@level2name=N'Salary'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Saler', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
/****** Object:  Table [dbo].[Base_Role]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Base_Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
	[IsDel] [bit] NULL,
	[CreateAt] [datetime] NULL,
	[CreateBy] [int] NULL,
	[Creator] [nvarchar](20) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[Updator] [nvarchar](20) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Role', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Role', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Role', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
/****** Object:  Table [dbo].[Base_MonthReport]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Base_MonthReport](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SalerId] [int] NULL,
	[DeptId] [int] NULL,
	[Year] [int] NULL,
	[HalfYear] [int] NULL,
	[Quarter] [int] NULL,
	[Month] [int] NULL,
	[FJ] [decimal](18, 2) NULL,
	[HC] [decimal](18, 2) NULL,
	[BS] [decimal](18, 2) NULL,
	[ZJC] [decimal](18, 2) NULL,
	[DSF] [decimal](18, 2) NULL,
	[YWZDF] [decimal](18, 2) NULL,
	[ZSF] [decimal](18, 2) NULL,
	[ZSJYJL] [decimal](18, 2) NULL,
	[JTJT] [decimal](18, 2) NULL,
	[CFJT] [decimal](18, 2) NULL,
	[TXF] [decimal](18, 2) NULL,
	[QTFY] [decimal](18, 2) NULL,
	[XZ] [decimal](18, 2) NULL,
	[HJ]  AS (((((((((((([FJ]+[HC])+[BS])+[ZJC])+[DSF])+[YWZDF])+[ZSF])+[ZSJYJL])+[JTJT])+[CFJT])+[TXF])+[QTFY])+[XZ]),
	[YJ] [decimal](18, 2) NULL,
	[FWF] [decimal](18, 2) NULL,
	[ZMML] [decimal](18, 2) NULL,
	[ML]  AS ([ZMML]-[FWF]),
	[XY]  AS (((((((((((((([ZMML]-[FWF])-[FJ])-[HC])-[BS])-[ZJC])-[DSF])-[YWZDF])-[ZSF])-[ZSJYJL])-[JTJT])-[CFJT])-[TXF])-[QTFY])-[XZ]),
	[LastMonthXY] [decimal](18, 2) NULL,
	[LastYearXY] [decimal](18, 2) NULL,
	[IsDel] [bit] NULL,
	[CreateAt] [datetime] NULL,
	[CreateBy] [int] NULL,
	[Creator] [nvarchar](20) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[Updator] [nvarchar](20) NULL,
 CONSTRAINT [PK_MonthReportItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'SalerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'DeptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'Year'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'Quarter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'Month'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'飞机' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'FJ'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'火车' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'HC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'巴士' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'BS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自驾车' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'ZJC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'的士费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'DSF'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务招待费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'YWZDF'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'住宿费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'ZSF'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'住宿节约奖励' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'ZSJYJL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交通津贴' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'JTJT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'餐费津贴' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'CFJT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通讯费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'TXF'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'其他费用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'QTFY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'薪资' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'XZ'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业绩' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'YJ'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'FWF'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账面毛利' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'ZMML'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'毛利' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_MonthReport', @level2type=N'COLUMN',@level2name=N'ML'
GO
/****** Object:  Table [dbo].[Base_Menu]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Base_Menu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Icon] [varchar](50) NULL,
	[Url] [varchar](200) NULL,
	[Roles] [nvarchar](500) NULL,
	[ParentId] [int] NULL,
	[Sort] [int] NULL,
	[IsDel] [bit] NULL,
	[CreateAt] [datetime] NULL,
	[CreateBy] [int] NULL,
	[Creator] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[Updator] [nvarchar](50) NULL,
 CONSTRAINT [PK_Base_Menu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Base_DeptSaler]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Base_DeptSaler](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeptId] [int] NULL,
	[SalerId] [int] NULL,
	[CreateAt] [datetime] NULL,
	[CreateBy] [int] NULL,
	[Creator] [nvarchar](20) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[Updator] [nvarchar](20) NULL,
 CONSTRAINT [PK_Base_DeptSaler] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Base_Dept]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Base_Dept](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NULL,
	[Sort] [int] NULL,
	[IsDel] [bit] NULL,
	[CreateAt] [datetime] NULL,
	[CreateBy] [int] NULL,
	[Creator] [nvarchar](20) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[Updator] [nvarchar](20) NULL,
 CONSTRAINT [PK_Dept] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Dept', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Dept', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Dept', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Dept', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建于' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Dept', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_Dept', @level2type=N'COLUMN',@level2name=N'Creator'
GO
/****** Object:  Table [dbo].[Base_DataItem]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Base_DataItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[K] [varchar](50) NULL,
	[V] [nvarchar](500) NULL,
	[Cate] [nvarchar](50) NULL,
	[Describe] [nvarchar](200) NULL,
	[Editable] [bit] NULL,
	[IsDel] [bit] NULL,
	[CreateAt] [datetime] NULL,
	[CreateBy] [int] NULL,
	[Creator] [nvarchar](20) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[Updator] [nvarchar](20) NULL,
 CONSTRAINT [PK_DataItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_DataItem', @level2type=N'COLUMN',@level2name=N'K'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_DataItem', @level2type=N'COLUMN',@level2name=N'V'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典项目说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_DataItem', @level2type=N'COLUMN',@level2name=N'Describe'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_DataItem', @level2type=N'COLUMN',@level2name=N'Editable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Base_DataItem', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
/****** Object:  View [dbo].[MonthReportView]    Script Date: 11/16/2018 10:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[MonthReportView]
AS
SELECT     TOP (100) PERCENT dbo.Base_MonthReport.Id, dbo.Base_MonthReport.SalerId, dbo.Base_MonthReport.DeptId, dbo.Base_MonthReport.Year, dbo.Base_MonthReport.Month, 
                      dbo.Base_MonthReport.FJ, dbo.Base_MonthReport.HC, dbo.Base_MonthReport.BS, dbo.Base_MonthReport.ZJC, dbo.Base_MonthReport.DSF, dbo.Base_MonthReport.YWZDF, 
                      dbo.Base_MonthReport.ZSF, dbo.Base_MonthReport.ZSJYJL, dbo.Base_MonthReport.JTJT, dbo.Base_MonthReport.CFJT, dbo.Base_MonthReport.TXF, dbo.Base_MonthReport.QTFY, 
                      dbo.Base_MonthReport.XZ, dbo.Base_MonthReport.YJ, dbo.Base_MonthReport.FWF, dbo.Base_MonthReport.ZMML, dbo.Base_MonthReport.ML, dbo.Base_MonthReport.IsDel, 
                      dbo.Base_MonthReport.CreateAt, dbo.Base_MonthReport.CreateBy, dbo.Base_MonthReport.Creator, dbo.Base_MonthReport.UpdateAt, dbo.Base_MonthReport.UpdateBy, 
                      dbo.Base_MonthReport.Updator, dbo.Base_Dept.Name AS DeptName, dbo.Base_Saler.Name AS SalerName, dbo.Base_Dept.Sort, dbo.Base_Saler.Job, dbo.Base_MonthReport.HJ, 
                      dbo.Base_MonthReport.XY, dbo.Base_MonthReport.LastMonthXY, dbo.Base_MonthReport.LastYearXY, dbo.Base_MonthReport.HalfYear, dbo.Base_MonthReport.Quarter
FROM         dbo.Base_MonthReport INNER JOIN
                      dbo.Base_Saler ON dbo.Base_MonthReport.SalerId = dbo.Base_Saler.Id INNER JOIN
                      dbo.Base_Dept ON dbo.Base_MonthReport.DeptId = dbo.Base_Dept.Id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Base_MonthReport"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 253
               Right = 180
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "Base_Saler"
            Begin Extent = 
               Top = 6
               Left = 398
               Bottom = 310
               Right = 540
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Base_Dept"
            Begin Extent = 
               Top = 137
               Left = 215
               Bottom = 311
               Right = 357
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'MonthReportView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'MonthReportView'
GO
/****** Object:  Default [DF_Job_IsDel]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Job] ADD  CONSTRAINT [DF_Job_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
/****** Object:  Default [DF_Saler_IsDel]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_Saler] ADD  CONSTRAINT [DF_Saler_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
/****** Object:  Default [DF_Base_MonthReport_FJ]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_FJ]  DEFAULT ((0)) FOR [FJ]
GO
/****** Object:  Default [DF_Base_MonthReport_HC]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_HC]  DEFAULT ((0)) FOR [HC]
GO
/****** Object:  Default [DF_Base_MonthReport_BS]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_BS]  DEFAULT ((0)) FOR [BS]
GO
/****** Object:  Default [DF_Base_MonthReport_ZJC]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_ZJC]  DEFAULT ((0)) FOR [ZJC]
GO
/****** Object:  Default [DF_Base_MonthReport_DSF]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_DSF]  DEFAULT ((0)) FOR [DSF]
GO
/****** Object:  Default [DF_Base_MonthReport_YWZDF]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_YWZDF]  DEFAULT ((0)) FOR [YWZDF]
GO
/****** Object:  Default [DF_Base_MonthReport_ZSF]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_ZSF]  DEFAULT ((0)) FOR [ZSF]
GO
/****** Object:  Default [DF_Base_MonthReport_ZSJYJL]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_ZSJYJL]  DEFAULT ((0)) FOR [ZSJYJL]
GO
/****** Object:  Default [DF_Base_MonthReport_JTJT]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_JTJT]  DEFAULT ((0)) FOR [JTJT]
GO
/****** Object:  Default [DF_Base_MonthReport_CFJT]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_CFJT]  DEFAULT ((0)) FOR [CFJT]
GO
/****** Object:  Default [DF_Base_MonthReport_TXF]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_TXF]  DEFAULT ((0)) FOR [TXF]
GO
/****** Object:  Default [DF_Base_MonthReport_QTFY]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_QTFY]  DEFAULT ((0)) FOR [QTFY]
GO
/****** Object:  Default [DF_Base_MonthReport_XZ]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_XZ]  DEFAULT ((0)) FOR [XZ]
GO
/****** Object:  Default [DF_Base_MonthReport_YJ]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_YJ]  DEFAULT ((0)) FOR [YJ]
GO
/****** Object:  Default [DF_Base_MonthReport_FWF]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_FWF]  DEFAULT ((0)) FOR [FWF]
GO
/****** Object:  Default [DF_Base_MonthReport_ZMML]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_ZMML]  DEFAULT ((0)) FOR [ZMML]
GO
/****** Object:  Default [DF_Base_MonthReport_LastMonthXY]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_LastMonthXY]  DEFAULT ((0)) FOR [LastMonthXY]
GO
/****** Object:  Default [DF_Base_MonthReport_LastYearXy]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_LastYearXy]  DEFAULT ((0)) FOR [LastYearXY]
GO
/****** Object:  Default [DF_Base_MonthReport_IsDel]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
/****** Object:  Default [DF_Base_MonthReport_CreateAt]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_CreateAt]  DEFAULT (getdate()) FOR [CreateAt]
GO
/****** Object:  Default [DF_Base_MonthReport_UpdateAt]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_MonthReport] ADD  CONSTRAINT [DF_Base_MonthReport_UpdateAt]  DEFAULT (getdate()) FOR [UpdateAt]
GO
/****** Object:  Default [DF_Base_Menu_CreateAt]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_Menu] ADD  CONSTRAINT [DF_Base_Menu_CreateAt]  DEFAULT (getdate()) FOR [CreateAt]
GO
/****** Object:  Default [DF_Base_Menu_UpdateAt]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_Menu] ADD  CONSTRAINT [DF_Base_Menu_UpdateAt]  DEFAULT (getdate()) FOR [UpdateAt]
GO
/****** Object:  Default [DF_Dept_CreateAt]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_Dept] ADD  CONSTRAINT [DF_Dept_CreateAt]  DEFAULT (getdate()) FOR [CreateAt]
GO
/****** Object:  Default [DF_Base_DataItem_Editable]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_DataItem] ADD  CONSTRAINT [DF_Base_DataItem_Editable]  DEFAULT ((1)) FOR [Editable]
GO
/****** Object:  Default [DF_DataItem_IsDel]    Script Date: 11/16/2018 10:27:09 ******/
ALTER TABLE [dbo].[Base_DataItem] ADD  CONSTRAINT [DF_DataItem_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
