IF OBJECT_ID('[dbo].[Provider]', 'U')  IS NOT NULL DROP TABLE [dbo].[Provider]


CREATE TABLE [dbo].[Provider](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Active] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Description] [varchar](100)  NULL,
) ON [PRIMARY]