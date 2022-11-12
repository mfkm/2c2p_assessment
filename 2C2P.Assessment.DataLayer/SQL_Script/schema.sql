USE [AssessmentDB]
GO
/****** Object:  Table [dbo].[ImportedData]    Script Date: 13/11/2022 4:25:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportedData](
	[TxId] [varchar](150) NOT NULL,
	[Amount] [float] NOT NULL,
	[CurrencyCode] [varchar](10) NOT NULL,
	[TxDate] [datetime] NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[FinalStatus] [varchar](10) NOT NULL,
	[SourceData] [varchar](10) NOT NULL,
 CONSTRAINT [PK_ImportedData] PRIMARY KEY CLUSTERED 
(
	[TxId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
