IF OBJECT_ID('[dbo].[EquipmentMaintenanceDefinition_bak]', 'U')  IS NOT NULL DROP TABLE [dbo].[EquipmentMaintenanceDefinition_bak]


CREATE TABLE [dbo].[EquipmentMaintenanceDefinition_bak](
	[Id] [bigint]  NOT NULL,
	[EquipmentId] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Operation] [nvarchar](200)  NOT NULL,
	[Type] [int]  NOT NULL,
	[Periodicity] [int]  NOT NULL,
	[Accessories] [nvarchar](100)  NOT NULL,
	[Cost] [numeric](18,3)  NULL,
	[ProviderId] [bigint]  NULL,
	[ResponsableId] [int]  NOT NULL,
	[Active] [bit]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [date]  NOT NULL,
	[CreatedOn] [date]  NULL,
	[CreatedBy] [int]  NULL,
) ON [PRIMARY]