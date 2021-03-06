CREATE DATABASE tinder;
GO
USE [tinder]
GO
/****** Object:  Table [dbo].[avatars]    Script Date: 3/6/2017 4:54:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[avatars](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[image] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[locations]    Script Date: 3/6/2017 4:54:55 PM ******/
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
