IF OBJECT_ID('[dbo].[CostImpactRange]', 'U')  IS NOT NULL DROP TABLE [dbo].[CostImpactRange]


CREATE TABLE [dbo].[CostImpactRange](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
	[Code] [int]  NOT NULL,
	[Type] [int]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]