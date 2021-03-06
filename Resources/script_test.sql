USE [tinder_test]
GO
/****** Object:  Table [dbo].[avatars]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[avatars](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[image] [varbinary](max) NULL,
	[path] [varchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[foods]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[foods](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[food] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[genders]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[genders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[gender] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[hobbies]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hobbies](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[hobby] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[likes]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[likes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userLiking_id] [int] NULL,
	[userLiked_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[locations]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[locations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[location] [geography] NULL,
	[radius]  AS ([location].[STBuffer]((5000)))
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ratings]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ratings](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_rated_id] [int] NULL,
	[rating] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL,
	[description] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users_foods]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users_foods](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[food_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users_genders]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users_genders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[gender_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users_hobbies]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users_hobbies](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[hobby_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users_locations]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users_locations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[location_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users_works]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users_works](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[work_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[works]    Script Date: 3/8/2017 2:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[works](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[work] [varchar](255) NULL
) ON [PRIMARY]

GO
