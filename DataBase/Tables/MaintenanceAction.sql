IF OBJECT_ID('[dbo].[MaintenanceAction]', 'U')  IS NOT NULL DROP TABLE [dbo].[MaintenanceAction]


CREATE TABLE [dbo].[MaintenanceAction](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[EquipmentId] [bigint]  NOT NULL,
	[MaintenanceId] [bigint]  NOT NULL,
	[ActionDate] [datetime]  NOT NULL,
	[Observations] [nvarchar](50)  NOT NULL,
	[Responsable] [int]  NOT NULL,
	[Amount] [numeric](18,3)  NOT NULL,
	[Vto] [datetime]  NULL,
	[Active] [bit]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
) ON [PRIMARY]