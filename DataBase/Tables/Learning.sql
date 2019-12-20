IF OBJECT_ID('[dbo].[Learning]', 'U')  IS NOT NULL DROP TABLE [dbo].[Learning]


CREATE TABLE [dbo].[Learning](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](100)  NOT NULL,
	[Status] [int]  NOT NULL,
	[DateStimatedDate] [date]  NOT NULL,
	[RealStart] [date]  NULL,
	[RealFinish] [date]  NULL,
	[Master] [nvarchar](100)  NULL,
	[Hours] [bigint]  NULL,
	[Amount] [numeric](18,3)  NULL,
	[Notes] [nvarchar](2000)  NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Year] [int]  NULL,
	[Active] [bit]  NULL,
	[Objetivo] [nvarchar](2000)  NULL,
	[Metodologia] [nvarchar](2000)  NULL,
) ON [PRIMARY]