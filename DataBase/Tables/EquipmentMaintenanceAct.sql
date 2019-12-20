IF OBJECT_ID('[dbo].[EquipmentMaintenanceAct]', 'U')  IS NOT NULL DROP TABLE [dbo].[EquipmentMaintenanceAct]


CREATE TABLE [dbo].[EquipmentMaintenanceAct](
	[Id] [bigint]  NOT NULL,
	[EquipmentId] [bigint]  NOT NULL,
	[EquipmentMaintenanceDefinitionId] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Date] [datetime]  NOT NULL,
	[Observations] [text]  NOT NULL,
	[ProviderId] [bigint]  NULL,
	[ResponsableId] [int]  NOT NULL,
	[Cost] [numeric](18,3)  NULL,
	[Vto] [date]  NOT NULL,
	[Active] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [date]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Operation] [varchar](200)  NULL,
) ON [PRIMARY]