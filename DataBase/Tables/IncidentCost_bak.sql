IF OBJECT_ID('[dbo].[IncidentCost_bak]', 'U')  IS NOT NULL DROP TABLE [dbo].[IncidentCost_bak]


CREATE TABLE [dbo].[IncidentCost_bak](
	[Id] [bigint]  NOT NULL,
	[IncidentId] [bigint]  NOT NULL,
	[BusinessRiskId] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Amount] [numeric](18,3)  NULL,
	[Quantity] [numeric](18,3)  NULL,
	[Responsable] [int]  NULL,
	[Active] [bit]  NULL,
	[ModifiedBy] [int]  NULL,
	[ModifiedOn] [datetime]  NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
	[Description] [varchar](100)  NULL,
) ON [PRIMARY]