IF OBJECT_ID('[dbo].[DocumentsVersion]', 'U')  IS NOT NULL DROP TABLE [dbo].[DocumentsVersion]


CREATE TABLE [dbo].[DocumentsVersion](
	[Id] [bigint]  NOT NULL,
	[DocumentId] [bigint]  NOT NULL,
	[Company] [int]  NOT NULL,
	[Version] [int]  NOT NULL,
	[UserCreate] [int]  NOT NULL,
	[Status] [int]  NOT NULL,
	[Date] [datetime]  NULL,
	[Reason] [nvarchar](100)  NULL,
) ON [PRIMARY]