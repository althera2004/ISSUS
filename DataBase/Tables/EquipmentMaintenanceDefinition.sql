IF OBJECT_ID('[dbo].[EquipmentMaintenanceDefinition]', 'U')  IS NOT NULL DROP TABLE [dbo].[EquipmentMaintenanceDefinition]


CREATE TABLE [dbo].[EquipmentMaintenanceDefinition](
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
	[FirstDate] [datetime]  NULL,
	[CreatedOn] [date]  NULL,
	[CreatedBy] [int]  NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [date]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]