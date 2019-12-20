IF OBJECT_ID('[dbo].[EquipmentRepair]', 'U')  IS NOT NULL DROP TABLE [dbo].[EquipmentRepair]


CREATE TABLE [dbo].[EquipmentRepair](
	[Id] [bigint]  NOT NULL,
	[EquipmentId] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[RepairType] [int]  NOT NULL,
	[Date] [datetime]  NOT NULL,
	[Description] [text]  NOT NULL,
	[Tools] [text]  NOT NULL,
	[Observations] [text]  NOT NULL,
	[Cost] [numeric](18,3)  NULL,
	[ProviderId] [bigint]  NULL,
	[ResponsableId] [int]  NOT NULL,
	[Active] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
) ON [PRIMARY]