IF OBJECT_ID('[dbo].[IncidentActionCost_back]', 'U')  IS NOT NULL DROP TABLE [dbo].[IncidentActionCost_back]


CREATE TABLE [dbo].[IncidentActionCost_back](
	[Id] [bigint]  NOT NULL,
	[IncidentActionId] [bigint]  NOT NULL,
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