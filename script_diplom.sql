USE [Коммунальные услуги]
GO
/****** Object:  Table [dbo].[Cards]    Script Date: 15.11.2024 16:09:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cards](
	[CardNumber] [nvarchar](50) NULL,
	[ExpiryMonth] [int] NULL,
	[ExpiryYear] [int] NULL,
	[Balance] [money] NULL,
	[CVV] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[client]    Script Date: 15.11.2024 16:09:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[client](
	[cliet_id] [int] IDENTITY(1,1) NOT NULL,
	[Familiy] [nvarchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[firstname] [nvarchar](50) NULL,
	[date] [date] NULL,
	[mobile] [nvarchar](50) NULL,
	[lic_schet] [int] NULL,
	[photo] [varbinary](max) NULL,
	[Login] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Baalance] [money] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeterReadings]    Script Date: 15.11.2024 16:09:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeterReadings](
	[ReadingId] [int] NULL,
	[UserId] [int] NULL,
	[ServiceType] [nvarchar](50) NULL,
	[StartReading] [int] NULL,
	[EndReading] [int] NULL,
	[ReadingDate] [datetime] NULL
) ON [PRIMARY]
GO
