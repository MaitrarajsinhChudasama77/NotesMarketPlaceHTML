USE [master]
GO
/****** Object:  Database [NotesMarketPlace]    Script Date: 28-03-2021 11:16:19 ******/
CREATE DATABASE [NotesMarketPlace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NotesMarketPlace', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\NotesMarketPlace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NotesMarketPlace_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\NotesMarketPlace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [NotesMarketPlace] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NotesMarketPlace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ARITHABORT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NotesMarketPlace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NotesMarketPlace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET  ENABLE_BROKER 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NotesMarketPlace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET RECOVERY FULL 
GO
ALTER DATABASE [NotesMarketPlace] SET  MULTI_USER 
GO
ALTER DATABASE [NotesMarketPlace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NotesMarketPlace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NotesMarketPlace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NotesMarketPlace] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [NotesMarketPlace] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'NotesMarketPlace', N'ON'
GO
ALTER DATABASE [NotesMarketPlace] SET QUERY_STORE = OFF
GO
USE [NotesMarketPlace]
GO
/****** Object:  Table [dbo].[AddEditCategory]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddEditCategory](
	[ID] [int] NOT NULL,
	[CategoryName] [varchar](100) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AddEditCountry]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddEditCountry](
	[ID] [int] NOT NULL,
	[CountryName] [varchar](100) NOT NULL,
	[CountryCode] [varchar](10) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AddEditType]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddEditType](
	[ID] [int] NOT NULL,
	[Type] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactUs]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactUs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](50) NOT NULL,
	[EmailId] [varchar](50) NOT NULL,
	[Subject] [varchar](100) NOT NULL,
	[Comments_Questions] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK__ContactU__3214EC279F108BED] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DownloadedNotes]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DownloadedNotes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[Downloader] [int] NOT NULL,
	[HasSellerAllowedDownload] [bit] NOT NULL,
	[AttachmentPath] [varchar](max) NULL,
	[IsAttachmentDownloaded] [bit] NOT NULL,
	[AttachmentDownloadedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[Category] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ManageSystemConfiguration]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ManageSystemConfiguration](
	[ID] [int] NOT NULL,
	[SupportEmail] [varchar](100) NOT NULL,
	[SupportContactNumber] [varchar](15) NOT NULL,
	[EmailAddress_es] [varchar](100) NOT NULL,
	[FacebookURL] [varchar](100) NULL,
	[TwitterURL] [varchar](100) NULL,
	[LinkedInURL] [varchar](100) NULL,
	[DefaultNoteDisplayImage] [varbinary](1) NOT NULL,
	[DefaultProfilePicture] [varbinary](1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteDetails]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[TypeID] [int] NOT NULL,
	[CountryID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[DisplayPicture] [varchar](max) NULL,
	[NumberOfPages] [varchar](20) NULL,
	[Description] [varchar](max) NOT NULL,
	[University] [varchar](200) NULL,
	[InstituitionName] [varchar](200) NULL,
	[Course] [varchar](100) NULL,
	[CourseCode] [varchar](100) NULL,
	[Professor] [varchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice_USD] [decimal](20, 0) NOT NULL,
	[NotesPreview] [varchar](max) NULL,
	[ActionBy] [int] NOT NULL,
	[Remark] [varchar](200) NULL,
	[Spam] [int] NULL,
	[Review] [int] NULL,
	[Star] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__NoteDeta__3214EC27E770FE64] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteReview]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteReview](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewedByID] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Ratings] [int] NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__NoteRevi__3214EC27D7BFB370] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesAttachment]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesAttachment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__SellerNo__3214EC27F9735701] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SpamReportsTable]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpamReportsTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReportedByID] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatusTable]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusTable](
	[ID] [int] NOT NULL,
	[StatusName] [varchar](100) NOT NULL,
	[Description] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[SecondaryEmail] [varchar](100) NULL,
	[DateOfBirth] [datetime] NULL,
	[Gender] [varchar](6) NULL,
	[PhoneNumber_CountryCode] [varchar](10) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[ProfilePicture] [varchar](max) NULL,
	[AddressLine_1] [varchar](100) NOT NULL,
	[AddressLine_2] [varchar](100) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[ZipCode] [varchar](50) NOT NULL,
	[CountryID] [int] NOT NULL,
	[University] [varchar](100) NULL,
	[College] [varchar](100) NULL,
	[SubmittedDate] [datetime] NULL,
	[SubmittedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK__UserProf__3214EC27ACA3034F] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[ID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 28-03-2021 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailID] [varchar](100) NOT NULL,
	[Password] [varchar](24) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[ActivationCode] [uniqueidentifier] NULL,
 CONSTRAINT [PK__Users__3214EC27A227E60D] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[AddEditCategory] ([ID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, N'Engineering', N'This is engineering books', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCategory] ([ID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, N'MBA', N'This is MBA category', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCategory] ([ID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, N'Science', N'This is Science category', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCategory] ([ID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, N'Medical', N'This is Medical category', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCategory] ([ID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, N'School', N'This is School books', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCategory] ([ID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, N'Commerce', N'This is Commerce category', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCategory] ([ID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, N'Social', N'This is Social category', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCategory] ([ID], [CategoryName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (8, N'IT', N'This is IT category', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AddEditCountry] ([ID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, N'India', N'+91', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCountry] ([ID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, N'Australia', N'+61', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCountry] ([ID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, N'Austria', N'+43', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCountry] ([ID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, N'Brazil', N'+55', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCountry] ([ID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, N'Canada', N'+1', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCountry] ([ID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, N'China', N'+86', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCountry] ([ID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, N'United Kingdom', N'+44', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCountry] ([ID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (8, N'United States', N'+1', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCountry] ([ID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (9, N'Vietnam', N'+84', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditCountry] ([ID], [CountryName], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (10, N'Zimbabwe', N'+263', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[AddEditType] ([ID], [Type], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, N'Hand-Written', N'This is hand written type', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditType] ([ID], [Type], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, N'University', N'This is University type', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditType] ([ID], [Type], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, N'Academic', N'This is Academic type', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditType] ([ID], [Type], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, N'Reference', N'This is Reference type', NULL, 1, NULL, NULL)
INSERT [dbo].[AddEditType] ([ID], [Type], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, N'Novel', N'This is novel type', NULL, 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ContactUs] ON 

INSERT [dbo].[ContactUs] ([ID], [FullName], [EmailId], [Subject], [Comments_Questions], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, N'Maitraraj Chudasama', N'abc3@gmail.com', N'About Purchase', N'Expensive...', NULL, NULL, NULL, NULL)
INSERT [dbo].[ContactUs] ([ID], [FullName], [EmailId], [Subject], [Comments_Questions], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, N'Maitra Chudasama', N'abc4@gmail.com', N'About Purchase', N'hahahahahah..', NULL, NULL, NULL, NULL)
INSERT [dbo].[ContactUs] ([ID], [FullName], [EmailId], [Subject], [Comments_Questions], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, N'Leonardo Decaprio', N'leo@gmil.com', N'About Notes', N'y', NULL, NULL, NULL, NULL)
INSERT [dbo].[ContactUs] ([ID], [FullName], [EmailId], [Subject], [Comments_Questions], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1003, N'Leonardo Decaprio', N'leo@hotmail.com', N'About Notes', N'etc', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[ContactUs] OFF
GO
SET IDENTITY_INSERT [dbo].[DownloadedNotes] ON 

INSERT [dbo].[DownloadedNotes] ([ID], [NoteID], [Seller], [Downloader], [HasSellerAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [Category], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, 2012, 39, 37, 1, N'/Members/39/2012/Attachment/Attachment1_26-03-21.pdf', 1, CAST(N'2021-03-26T12:02:03.237' AS DateTime), 1, CAST(10 AS Decimal(18, 0)), N'Data Science', N'Science', CAST(N'2021-03-25T14:03:25.390' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[DownloadedNotes] ([ID], [NoteID], [Seller], [Downloader], [HasSellerAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [Category], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, 2012, 39, 40, 1, N'/Members/39/2012/Attachment/Attachment1_26-03-21.pdf', 1, CAST(N'2021-03-26T11:39:13.070' AS DateTime), 1, CAST(10 AS Decimal(18, 0)), N'Data Science', N'Science', CAST(N'2021-03-25T15:14:08.080' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[DownloadedNotes] ([ID], [NoteID], [Seller], [Downloader], [HasSellerAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [Category], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, 4012, 39, 42, 1, N'/Members/39/4012/Attachment/Attachment1_26-03-21.pdf', 1, CAST(N'2021-03-26T12:15:49.053' AS DateTime), 1, CAST(100 AS Decimal(18, 0)), N'Test 4', N'Engineering', CAST(N'2021-03-25T16:08:34.020' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[DownloadedNotes] ([ID], [NoteID], [Seller], [Downloader], [HasSellerAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [Category], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, 2012, 39, 42, 1, N'/Members/39/2012/Attachment/Attachment1_26-03-21.pdf', 1, CAST(N'2021-03-26T12:11:10.330' AS DateTime), 1, CAST(10 AS Decimal(18, 0)), N'Data Science', N'Science', CAST(N'2021-03-25T16:09:47.720' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[DownloadedNotes] ([ID], [NoteID], [Seller], [Downloader], [HasSellerAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [Category], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, 4014, 37, 39, 1, N'/Members/37/4014/Attachment/Attachment1_26-03-21.pdf', 1, CAST(N'2021-03-26T18:56:37.710' AS DateTime), 1, CAST(50 AS Decimal(18, 0)), N'IELTS', N'Engineering', CAST(N'2021-03-25T16:44:41.267' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[DownloadedNotes] ([ID], [NoteID], [Seller], [Downloader], [HasSellerAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [Category], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, 2013, 39, 40, 1, N'/Members/39/2013/Attachment/Attachment1_25-03-21.pdf', 1, CAST(N'2021-03-25T17:20:58.100' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Artificial Intelligence', N'IT', CAST(N'2021-03-25T17:20:58.103' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[DownloadedNotes] ([ID], [NoteID], [Seller], [Downloader], [HasSellerAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [Category], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (8, 2013, 39, 37, 1, N'/Members/39/2013/Attachment/Attachment1_25-03-21.pdf', 1, CAST(N'2021-03-25T18:39:10.313' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Artificial Intelligence', N'IT', CAST(N'2021-03-25T18:39:10.313' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[DownloadedNotes] ([ID], [NoteID], [Seller], [Downloader], [HasSellerAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [Category], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (10, 4014, 37, 39, 1, N'/Members/37/4014/Attachment/Attachment1_26-03-21.pdf', 0, NULL, 1, CAST(50 AS Decimal(18, 0)), N'IELTS', N'Engineering', CAST(N'2021-03-27T16:58:39.093' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[DownloadedNotes] ([ID], [NoteID], [Seller], [Downloader], [HasSellerAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [Category], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (11, 4014, 37, 43, 0, NULL, 0, NULL, 1, CAST(50 AS Decimal(18, 0)), N'IELTS', N'Engineering', CAST(N'2021-03-27T17:02:45.777' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[DownloadedNotes] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteDetails] ON 

INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2012, 39, 3, 2, 1, 1, N'Data Science', N'~/Members/39/2012/DP_26-03-21.jpg', N'450', N'Data Science', NULL, N'Gec Bhavnagar', N'Data Science', N'0112', N'KP Kandoriya', 1, CAST(10 AS Decimal(20, 0)), N'~/Members/39/2012/Preview_26-03-21.pdf', 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-23T11:30:22.900' AS DateTime), NULL, CAST(N'2021-03-26T11:49:57.337' AS DateTime), 39, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2013, 39, 8, 2, 1, 1, N'Artificial Intelligence', NULL, N'300', N'AI', NULL, N'Gec Bhavnagar', N'Artificial Intelligence', N'0113', N'KP Kandoriya', 0, CAST(0 AS Decimal(20, 0)), NULL, 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-23T11:31:41.363' AS DateTime), NULL, CAST(N'2021-03-25T17:16:03.997' AS DateTime), 39, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2014, 39, 6, 1, 1, 1, N'Accounts', NULL, N'1000', N'Accounts', NULL, N'Gec Bhavnagar', N'Accounts', N'1121', N'KP Kandoriya', 0, CAST(100 AS Decimal(20, 0)), NULL, 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-23T11:32:54.557' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2015, 39, 7, 3, 1, 1, N'Social Studies', NULL, N'450', N'Social Studies', NULL, N'Gec Bhavnagar', N'Social Studies', N'1210', N'A.V Nimavat', 0, CAST(450 AS Decimal(20, 0)), NULL, 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-23T11:34:49.967' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2016, 39, 1, 2, 1, 1, N'Auto-CAD', NULL, N'300', N'Auto-CAD', NULL, N'Gec Bhavnagar', N'Auto-CAD', N'0112', N'KP Kandoriya', 0, CAST(250 AS Decimal(20, 0)), NULL, 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-23T11:36:12.850' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2017, 39, 1, 2, 1, 1, N'Thermodynamics', NULL, N'450', N'Thermodynamics', NULL, N'Gec Bhavnagar', N'Thermodynamics', N'0113', N'KP Kandoriya', 0, CAST(350 AS Decimal(20, 0)), NULL, 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-23T11:37:28.137' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2018, 39, 1, 2, 1, 1, N'Solidworks', NULL, N'780', N'Solidworks', NULL, N'Gec Bhavnagar', N'Solidworks', N'0117', N'KP Kandoriya', 0, CAST(650 AS Decimal(20, 0)), NULL, 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-23T11:46:39.607' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2019, 39, 5, 3, 1, 1, N'Spoken English', NULL, N'650', N'Spoken English', NULL, N'Gec Bhavnagar', N'Spoken English', N'0114', N'KP Kandoriya', 0, CAST(0 AS Decimal(20, 0)), NULL, 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-23T11:49:14.653' AS DateTime), NULL, CAST(N'2021-03-25T14:19:56.137' AS DateTime), 39, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2021, 39, 2, 5, 1, 2, N'Test', N'C:/Users/Maitrarajsinh/source/repos/Notes-MarketPlace/Default/1.jpg', N'650', N'Test', NULL, N'Gec Bhavnagar', N'Test', N'0001', N'KP Kandoriya', 0, CAST(100 AS Decimal(20, 0)), NULL, 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-23T11:53:16.460' AS DateTime), NULL, CAST(N'2021-03-24T08:45:51.493' AS DateTime), NULL, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4012, 39, 1, 2, 1, 1, N'Test 4', N'~/Members/39/4012/DP_26-03-21.jpg', N'1000', N't', NULL, N'Gec Bhavnagar', N'Test 4', N'0112', N'A.V Nimavat', 1, CAST(100 AS Decimal(20, 0)), N'~/Members/39/4012/Preview_26-03-21.pdf', 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-24T16:55:15.027' AS DateTime), 39, CAST(N'2021-03-26T12:15:28.373' AS DateTime), 39, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4013, 42, 7, 5, 1, 1, N'The Art of Living', N'~/Members/42/4013/DP_26-03-21.jpg', N'650', N'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sapiente aspernatur harum libero et nostrum sequi consequatur, tempore accusamus ab esse, voluptatem laudantium voluptas rerum? Laborum id ad alias saepe nemo,alias saepe ne consequatur.', NULL, N'Gec Bhavnagar', N'Art of Living', N'01211', N'Shri Shri Ravi Shankarji', 0, CAST(0 AS Decimal(20, 0)), N'~/Members/42/4013/Preview_26-03-21.pdf', 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-25T16:15:28.140' AS DateTime), 42, CAST(N'2021-03-26T11:51:58.317' AS DateTime), 42, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4014, 37, 1, 2, 1, 1, N'IELTS', N'~/Members/37/4014/DP_26-03-21.jpg', N'780', N'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sapiente aspernatur harum libero et nostrum sequi consequatur, tempore accusamus ab esse, voluptatem laudantium voluptas rerum? Laborum id ad alias saepe nemo,alias saepe ne consequatur.', NULL, N'Gec Bhavnagar', N'IELTS', N'11212', N'A.V Nimavat', 1, CAST(50 AS Decimal(20, 0)), N'~/Members/37/4014/Preview_26-03-21.pdf', 3, NULL, NULL, 2, 10, CAST(N'2021-03-25T16:42:36.253' AS DateTime), 37, CAST(N'2021-03-26T11:53:27.377' AS DateTime), 37, 1)
INSERT [dbo].[NoteDetails] ([ID], [UserID], [CategoryID], [TypeID], [CountryID], [Status], [NoteTitle], [DisplayPicture], [NumberOfPages], [Description], [University], [InstituitionName], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice_USD], [NotesPreview], [ActionBy], [Remark], [Spam], [Review], [Star], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4015, 38, 1, 1, 1, 1, N'Test 6', NULL, N'450', N'test', NULL, N'Gec Bhavnagar', N'Test 6', N'01141', N'KP Kandoriya', 1, CAST(650 AS Decimal(20, 0)), N'~/Members/38/4015/Preview_25-03-21.pdf', 3, NULL, NULL, NULL, NULL, CAST(N'2021-03-25T17:00:07.200' AS DateTime), 38, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[NoteDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteReview] ON 

INSERT [dbo].[NoteReview] ([ID], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, 4014, 39, 6, 5, N'Excellent book.', CAST(N'2021-03-27T10:33:12.853' AS DateTime), 39, NULL, NULL, 1)
INSERT [dbo].[NoteReview] ([ID], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1002, 4014, 43, 11, 5, N'etc5', CAST(N'2021-03-27T19:13:13.760' AS DateTime), 43, CAST(N'2021-03-27T19:16:52.523' AS DateTime), 43, 1)
SET IDENTITY_INSERT [dbo].[NoteReview] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesAttachment] ON 

INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2010, 2012, N'Attachment1_26-03-21.pdf;', N'/Members/39/2012/Attachment/Attachment1_26-03-21.pdf', CAST(N'2021-03-23T11:30:22.917' AS DateTime), 39, CAST(N'2021-03-26T11:49:57.400' AS DateTime), 39, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2011, 2013, N'Attachment1_25-03-21.pdf;', N'/Members/39/2013/Attachment/Attachment1_25-03-21.pdf', CAST(N'2021-03-23T11:31:41.377' AS DateTime), 39, CAST(N'2021-03-25T17:16:04.057' AS DateTime), 39, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2012, 2014, N'TS_SRS_Notes_marketplace_v1.0.pdf;', N'C:\Users\Maitrarajsinh\source\repos\Notes-MarketPlace\Members\39\2014\Attachment\Attachment_1_23-03-2021_11-32-54_TS_SRS_Notes_marketplace_v1.0.pdf;', CAST(N'2021-03-23T11:32:54.570' AS DateTime), 39, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2013, 2015, N'TS_SRS_Notes_marketplace_v1.0.pdf;', N'C:\Users\Maitrarajsinh\source\repos\Notes-MarketPlace\Members\39\2015\Attachment\Attachment_1_23-03-2021_11-34-49_TS_SRS_Notes_marketplace_v1.0.pdf;', CAST(N'2021-03-23T11:34:49.977' AS DateTime), 39, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2014, 2016, N'TS_SRS_Notes_marketplace_v1.0.pdf;', N'C:\Users\Maitrarajsinh\source\repos\Notes-MarketPlace\Members\39\2016\Attachment\Attachment_1_23-03-2021_11-36-12_TS_SRS_Notes_marketplace_v1.0.pdf;', CAST(N'2021-03-23T11:36:12.863' AS DateTime), 39, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2015, 2017, N'TS_SRS_Notes_marketplace_v1.0.pdf;', N'C:\Users\Maitrarajsinh\source\repos\Notes-MarketPlace\Members\39\2017\Attachment\Attachment_1_23-03-2021_11-37-28_TS_SRS_Notes_marketplace_v1.0.pdf;', CAST(N'2021-03-23T11:37:28.150' AS DateTime), 39, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2016, 2018, N'TS_SRS_Notes_marketplace_v1.0.pdf;', N'C:\Users\Maitrarajsinh\source\repos\Notes-MarketPlace\Members\39\2018\Attachment\Attachment_1_23-03-2021_11-46-39_TS_SRS_Notes_marketplace_v1.0.pdf;', CAST(N'2021-03-23T11:46:39.617' AS DateTime), 39, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2017, 2019, N'TS_SRS_Notes_marketplace_v1.0.pdf;', N'C:\Users\Maitrarajsinh\source\repos\Notes-MarketPlace\Members\39\2019\Attachment\Attachment_1_25-03-21_TS_SRS_Notes_marketplace_v1.0.pdf;', CAST(N'2021-03-23T11:49:14.667' AS DateTime), 39, CAST(N'2021-03-25T14:19:56.173' AS DateTime), 39, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2019, 2021, N'TS_SRS_Notes_marketplace_v1.0.pdf;', N'C:\Users\Maitrarajsinh\source\repos\Notes-MarketPlace\Members\39\2021\Attachment\Attachment_1_24-03-2021_08-45-51_TS_SRS_Notes_marketplace_v1.0.pdf;', CAST(N'2021-03-23T11:53:16.473' AS DateTime), 39, CAST(N'2021-03-24T08:45:51.567' AS DateTime), NULL, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4010, 4012, N'Attachment1_26-03-21.pdf;', N'/Members/39/4012/Attachment/Attachment1_26-03-21.pdf', CAST(N'2021-03-24T16:55:15.097' AS DateTime), 39, CAST(N'2021-03-26T12:15:28.453' AS DateTime), 39, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4011, 4013, N'Attachment1_26-03-21.pdf;', N'/Members/42/4013/Attachment/Attachment1_26-03-21.pdf', CAST(N'2021-03-25T16:15:28.160' AS DateTime), 42, CAST(N'2021-03-26T11:51:58.343' AS DateTime), 42, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4012, 4014, N'Attachment1_26-03-21.pdf;', N'/Members/37/4014/Attachment/Attachment1_26-03-21.pdf', CAST(N'2021-03-25T16:42:36.283' AS DateTime), 37, CAST(N'2021-03-26T11:53:27.400' AS DateTime), 37, 1)
INSERT [dbo].[SellerNotesAttachment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4013, 4015, N'TS_SRS_Notes_marketplace_v1.0;', N'C:\Users\Maitrarajsinh\source\repos\Notes-MarketPlace\Members\38\4015\Attachment\Attachment_1_25-03-21;', CAST(N'2021-03-25T17:00:07.253' AS DateTime), 38, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[SellerNotesAttachment] OFF
GO
SET IDENTITY_INSERT [dbo].[SpamReportsTable] ON 

INSERT [dbo].[SpamReportsTable] ([ID], [NoteID], [ReportedByID], [AgainstDownloadsID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, 4014, 39, 6, N'Book is too expensive.', CAST(N'2021-03-27T10:33:43.350' AS DateTime), 39, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SpamReportsTable] OFF
GO
INSERT [dbo].[StatusTable] ([ID], [StatusName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Draft', N'When user click on Save', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[StatusTable] ([ID], [StatusName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Submitted For Review', N'When user click on Publish', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[StatusTable] ([ID], [StatusName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'In Review', N'When Admin click on Review', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[StatusTable] ([ID], [StatusName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Published', N'When Admin click on Approve', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[StatusTable] ([ID], [StatusName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Rejected', N'When Admin click on Reject', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[StatusTable] ([ID], [StatusName], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Removed', N'When Admin click on Delete', NULL, NULL, NULL, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

INSERT [dbo].[UserProfile] ([ID], [UserID], [SecondaryEmail], [DateOfBirth], [Gender], [PhoneNumber_CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine_1], [AddressLine_2], [City], [State], [ZipCode], [CountryID], [University], [College], [SubmittedDate], [SubmittedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, 39, NULL, CAST(N'1999-01-14T00:00:00.000' AS DateTime), N'Male', N'+91', N'9664917245', N'Members/39/DP_27-03-2021.jpg', N'Shiv Shakti', N'Bhavnagar', N'Bhavnagar', N'Gujarat', N'364001', 1, N'GTU', N'GEC Bhavnagar', CAST(N'2021-03-16T18:57:38.530' AS DateTime), 39, CAST(N'2021-03-27T16:09:47.737' AS DateTime), 39)
INSERT [dbo].[UserProfile] ([ID], [UserID], [SecondaryEmail], [DateOfBirth], [Gender], [PhoneNumber_CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine_1], [AddressLine_2], [City], [State], [ZipCode], [CountryID], [University], [College], [SubmittedDate], [SubmittedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, 38, NULL, CAST(N'2006-02-02T00:00:00.000' AS DateTime), N'Male', N'+91', N'9664917245', N'Default/Joker.jpg', N'Shiv Shakti', N'Bhavnagar', N'Bhavnagar', N'Gujarat', N'364001', 1, N'GTU', N'Gec Bvn', CAST(N'2021-03-17T11:29:00.963' AS DateTime), 38, NULL, NULL)
INSERT [dbo].[UserProfile] ([ID], [UserID], [SecondaryEmail], [DateOfBirth], [Gender], [PhoneNumber_CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine_1], [AddressLine_2], [City], [State], [ZipCode], [CountryID], [University], [College], [SubmittedDate], [SubmittedBy], [ModifiedDate], [ModifiedBy]) VALUES (8, 37, NULL, CAST(N'2005-07-20T00:00:00.000' AS DateTime), N'Male', N'+91', N'9664917245', N'Members/37/DP_27-03-2021.jpg', N'Nilkanthnagar', N'Gaytrinagar', N'Bhavnagar', N'Gujarat', N'364001', 1, N'GTU', N'GEC Bhavnagar', CAST(N'2021-03-19T10:41:00.923' AS DateTime), 37, CAST(N'2021-03-27T16:24:18.727' AS DateTime), 37)
INSERT [dbo].[UserProfile] ([ID], [UserID], [SecondaryEmail], [DateOfBirth], [Gender], [PhoneNumber_CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine_1], [AddressLine_2], [City], [State], [ZipCode], [CountryID], [University], [College], [SubmittedDate], [SubmittedBy], [ModifiedDate], [ModifiedBy]) VALUES (10, 40, NULL, CAST(N'2007-03-25T00:00:00.000' AS DateTime), N'Male', N'+91', N'9664917245', N'Members/40/DP_27-03-2021.jpg', N'Nilkanthnagar', N'Gaytrinagar', N'Bhavnagar', N'Gujarat', N'364001', 1, N'GTU', N'GEC Bhavnagar', CAST(N'2021-03-25T16:06:29.800' AS DateTime), 40, CAST(N'2021-03-27T16:28:22.043' AS DateTime), 40)
INSERT [dbo].[UserProfile] ([ID], [UserID], [SecondaryEmail], [DateOfBirth], [Gender], [PhoneNumber_CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine_1], [AddressLine_2], [City], [State], [ZipCode], [CountryID], [University], [College], [SubmittedDate], [SubmittedBy], [ModifiedDate], [ModifiedBy]) VALUES (11, 42, NULL, CAST(N'2001-06-14T00:00:00.000' AS DateTime), N'Male', N'+91', N'9664917245', N'Members/42/DP_27-03-2021.jpg', N'Nilkanthnagar', N'Gaytrinagar', N'Bhavnagar', N'Gujarat', N'364001', 1, N'GTU', N'GEC BVN', CAST(N'2021-03-25T16:08:09.167' AS DateTime), 42, CAST(N'2021-03-27T16:29:34.983' AS DateTime), 42)
INSERT [dbo].[UserProfile] ([ID], [UserID], [SecondaryEmail], [DateOfBirth], [Gender], [PhoneNumber_CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine_1], [AddressLine_2], [City], [State], [ZipCode], [CountryID], [University], [College], [SubmittedDate], [SubmittedBy], [ModifiedDate], [ModifiedBy]) VALUES (12, 43, NULL, CAST(N'2021-03-02T00:00:00.000' AS DateTime), N'Male', N'+91', N'9664917245', N'Default/Joker.jpg', N'Nilkanthnagar', N'Gaytrinagar', N'Bhavnagar', N'Gujarat', N'364001', 1, N'GTU', N'GEC Bhavnagar', CAST(N'2021-03-26T19:49:31.730' AS DateTime), 43, NULL, NULL)
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Super Admin', N'This role is for super admin', NULL, 1, NULL, NULL, 1)
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Admin', N'This role is for admin', NULL, 1, NULL, NULL, 1)
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'User', N'This role is for user', NULL, 1, NULL, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [ActivationCode]) VALUES (37, 3, N'user166', N'lastname166', N'chudasamamaitraraj@gmail.com', N'Market@1234', 0, NULL, NULL, NULL, NULL, 1, N'dc0c073e-d33d-4f05-87d2-209ba4b441e2')
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [ActivationCode]) VALUES (38, 3, N'user122', N'lastname122', N'maitrarajsinhchudasama05799@gmail.com', N'Market@1234', 0, NULL, NULL, NULL, NULL, 1, N'2dd062ea-4af4-4f28-bc8e-d687407b816a')
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [ActivationCode]) VALUES (39, 3, N'user30', N'lastname30', N'mgc0071999@gmail.com', N'Market@1234', 0, NULL, NULL, NULL, NULL, 1, N'a0298869-5dd0-41da-a188-c060f64711d7')
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [ActivationCode]) VALUES (40, 3, N'user31', N'lastname31', N'therebel01999@gmail.com', N'Market@1234', 0, NULL, NULL, NULL, NULL, 1, N'c24b693a-e9f3-4cff-a782-6e9fb246a2ef')
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [ActivationCode]) VALUES (42, 3, N'user1', N'lastname1', N'maitrarajchudasama@gmail.com', N'Market@1234', 0, NULL, NULL, NULL, NULL, 1, N'a24a5748-1fe9-4c72-8502-9ad1f3d8d161')
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive], [ActivationCode]) VALUES (43, 3, N'user167', N'lastname167', N'testyfoe00777@gmail.com', N'Market@1234', 0, NULL, NULL, NULL, NULL, 1, N'2c30a6cc-495c-4af3-b50c-a08e7a357477')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__7ED91AEEB09A568E]    Script Date: 28-03-2021 11:16:19 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ__Users__7ED91AEEB09A568E] UNIQUE NONCLUSTERED 
(
	[EmailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[NoteDetails] ADD  CONSTRAINT [DF__NoteDetai__IsAct__403A8C7D]  DEFAULT ('TRUE') FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotesAttachment] ADD  CONSTRAINT [DF__SellerNot__IsAct__440B1D61]  DEFAULT ('TRUE') FOR [IsActive]
GO
ALTER TABLE [dbo].[StatusTable] ADD  DEFAULT ('TRUE') FOR [IsActive]
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT ('TRUE') FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__IsEmailVe__29572725]  DEFAULT ('FALSE') FOR [IsEmailVerified]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__IsActive__2A4B4B5E]  DEFAULT ('TRUE') FOR [IsActive]
GO
ALTER TABLE [dbo].[DownloadedNotes]  WITH CHECK ADD FOREIGN KEY([Downloader])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[DownloadedNotes]  WITH CHECK ADD  CONSTRAINT [FK__Downloade__NoteI__75A278F5] FOREIGN KEY([NoteID])
REFERENCES [dbo].[NoteDetails] ([ID])
GO
ALTER TABLE [dbo].[DownloadedNotes] CHECK CONSTRAINT [FK__Downloade__NoteI__75A278F5]
GO
ALTER TABLE [dbo].[DownloadedNotes]  WITH CHECK ADD FOREIGN KEY([Seller])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteDetails]  WITH CHECK ADD  CONSTRAINT [FK__NoteDetai__Categ__3D5E1FD2] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[AddEditCategory] ([ID])
GO
ALTER TABLE [dbo].[NoteDetails] CHECK CONSTRAINT [FK__NoteDetai__Categ__3D5E1FD2]
GO
ALTER TABLE [dbo].[NoteDetails]  WITH CHECK ADD  CONSTRAINT [FK__NoteDetai__Count__3F466844] FOREIGN KEY([CountryID])
REFERENCES [dbo].[AddEditCountry] ([ID])
GO
ALTER TABLE [dbo].[NoteDetails] CHECK CONSTRAINT [FK__NoteDetai__Count__3F466844]
GO
ALTER TABLE [dbo].[NoteDetails]  WITH CHECK ADD  CONSTRAINT [FK__NoteDetai__TypeI__3E52440B] FOREIGN KEY([TypeID])
REFERENCES [dbo].[AddEditType] ([ID])
GO
ALTER TABLE [dbo].[NoteDetails] CHECK CONSTRAINT [FK__NoteDetai__TypeI__3E52440B]
GO
ALTER TABLE [dbo].[NoteDetails]  WITH CHECK ADD  CONSTRAINT [FK__NoteDetai__UserI__3C69FB99] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteDetails] CHECK CONSTRAINT [FK__NoteDetai__UserI__3C69FB99]
GO
ALTER TABLE [dbo].[NoteDetails]  WITH CHECK ADD  CONSTRAINT [FK_NoteDetails_StatusTable] FOREIGN KEY([Status])
REFERENCES [dbo].[StatusTable] ([ID])
GO
ALTER TABLE [dbo].[NoteDetails] CHECK CONSTRAINT [FK_NoteDetails_StatusTable]
GO
ALTER TABLE [dbo].[NoteReview]  WITH CHECK ADD  CONSTRAINT [FK__NoteRevie__Again__7C4F7684] FOREIGN KEY([AgainstDownloadsID])
REFERENCES [dbo].[DownloadedNotes] ([ID])
GO
ALTER TABLE [dbo].[NoteReview] CHECK CONSTRAINT [FK__NoteRevie__Again__7C4F7684]
GO
ALTER TABLE [dbo].[NoteReview]  WITH CHECK ADD  CONSTRAINT [FK__NoteRevie__NoteI__7A672E12] FOREIGN KEY([NoteID])
REFERENCES [dbo].[NoteDetails] ([ID])
GO
ALTER TABLE [dbo].[NoteReview] CHECK CONSTRAINT [FK__NoteRevie__NoteI__7A672E12]
GO
ALTER TABLE [dbo].[NoteReview]  WITH CHECK ADD  CONSTRAINT [FK__NoteRevie__Revie__7B5B524B] FOREIGN KEY([ReviewedByID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteReview] CHECK CONSTRAINT [FK__NoteRevie__Revie__7B5B524B]
GO
ALTER TABLE [dbo].[SellerNotesAttachment]  WITH CHECK ADD  CONSTRAINT [FK__SellerNot__NoteI__4316F928] FOREIGN KEY([NoteID])
REFERENCES [dbo].[NoteDetails] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesAttachment] CHECK CONSTRAINT [FK__SellerNot__NoteI__4316F928]
GO
ALTER TABLE [dbo].[SpamReportsTable]  WITH CHECK ADD FOREIGN KEY([AgainstDownloadsID])
REFERENCES [dbo].[DownloadedNotes] ([ID])
GO
ALTER TABLE [dbo].[SpamReportsTable]  WITH CHECK ADD  CONSTRAINT [FK__SpamRepor__NoteI__7F2BE32F] FOREIGN KEY([NoteID])
REFERENCES [dbo].[NoteDetails] ([ID])
GO
ALTER TABLE [dbo].[SpamReportsTable] CHECK CONSTRAINT [FK__SpamRepor__NoteI__7F2BE32F]
GO
ALTER TABLE [dbo].[SpamReportsTable]  WITH CHECK ADD FOREIGN KEY([ReportedByID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK__UserProfi__UserI__2D27B809] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK__UserProfi__UserI__2D27B809]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_AddEditCountry] FOREIGN KEY([CountryID])
REFERENCES [dbo].[AddEditCountry] ([ID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_AddEditCountry]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK__Users__RoleID__286302EC] FOREIGN KEY([RoleID])
REFERENCES [dbo].[UserRoles] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK__Users__RoleID__286302EC]
GO
USE [master]
GO
ALTER DATABASE [NotesMarketPlace] SET  READ_WRITE 
GO
