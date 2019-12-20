IF OBJECT_ID('[dbo].[ProcessType]', 'U')  IS NOT NULL DROP TABLE [dbo].[ProcessType]


CREATE TABLE [dbo].[ProcessType](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
	[Active] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NULL,
) ON [PRIMARY]