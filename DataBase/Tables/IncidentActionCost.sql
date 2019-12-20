IF OBJECT_ID('[dbo].[IncidentActionCost]', 'U')  IS NOT NULL DROP TABLE [dbo].[IncidentActionCost]


CREATE TABLE [dbo].[IncidentActionCost](
	[Id] [bigint]  NOT NULL,
	[IncidentActionId] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](100)  NULL,
	[Date] [datetime]  NULL,
	[Amount] [numeric](18,3)  NULL,
	[Quantity] [numeric](18,3)  NULL,
	[Responsable] [int]  NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
	[ModifiedBy] [int]  NULL,
	[ModifiedOn] [datetime]  NULL,
	[Active] [bit]  NULL,
) ON [PRIMARY]