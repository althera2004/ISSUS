IF OBJECT_ID('[dbo].[Rules]', 'U')  IS NOT NULL DROP TABLE [dbo].[Rules]


CREATE TABLE [dbo].[Rules](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
	[Limit] [bigint]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
	[Notes] [varchar](500)  NULL,
) ON [PRIMARY]