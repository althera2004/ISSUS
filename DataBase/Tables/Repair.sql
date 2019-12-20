IF OBJECT_ID('[dbo].[Repair]', 'U')  IS NOT NULL DROP TABLE [dbo].[Repair]


CREATE TABLE [dbo].[Repair](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[EquipmentId] [bigint]  NOT NULL,
	[ActionDate] [datetime]  NOT NULL,
	[Observations] [nvarchar](250)  NULL,
	[Amount] [numeric](18,3)  NULL,
	[Responsable] [int]  NOT NULL,
	[Vto] [datetime]  NULL,
	[Active] [bit]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
) ON [PRIMARY]