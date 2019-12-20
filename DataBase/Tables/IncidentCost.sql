IF OBJECT_ID('[dbo].[IncidentCost]', 'U')  IS NOT NULL DROP TABLE [dbo].[IncidentCost]


CREATE TABLE [dbo].[IncidentCost](
	[Id] [bigint]  NOT NULL,
	[IncidentId] [bigint]  NOT NULL,
	[BusinessRiskId] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](100)  NULL,
	[Amount] [numeric](18,3)  NULL,
	[Quantity] [numeric](18,3)  NULL,
	[Responsable] [int]  NULL,
	[Date] [datetime]  NULL,
	[Active] [bit]  NULL,
	[ModifiedBy] [int]  NULL,
	[ModifiedOn] [datetime]  NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
) ON [PRIMARY]