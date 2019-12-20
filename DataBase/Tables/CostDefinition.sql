IF OBJECT_ID('[dbo].[CostDefinition]', 'U')  IS NOT NULL DROP TABLE [dbo].[CostDefinition]


CREATE TABLE [dbo].[CostDefinition](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Amount] [numeric](18,3)  NOT NULL,
	[Active] [bit]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[Description] [varchar](100)  NULL,
) ON [PRIMARY]