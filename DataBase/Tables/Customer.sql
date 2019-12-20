IF OBJECT_ID('[dbo].[Customer]', 'U')  IS NOT NULL DROP TABLE [dbo].[Customer]


CREATE TABLE [dbo].[Customer](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Active] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Description] [varchar](100)  NULL,
) ON [PRIMARY]