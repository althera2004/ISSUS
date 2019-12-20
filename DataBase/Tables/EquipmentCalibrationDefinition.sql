IF OBJECT_ID('[dbo].[EquipmentCalibrationDefinition]', 'U')  IS NOT NULL DROP TABLE [dbo].[EquipmentCalibrationDefinition]


CREATE TABLE [dbo].[EquipmentCalibrationDefinition](
	[Id] [bigint]  NOT NULL,
	[EquipmentId] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Type] [int]  NOT NULL,
	[Periodicity] [int]  NOT NULL,
	[Uncertainty] [numeric](18,6)  NOT NULL,
	[Range] [nvarchar](50)  NOT NULL,
	[Pattern] [nvarchar](50)  NOT NULL,
	[Cost] [numeric](18,3)  NULL,
	[Notes] [text]  NOT NULL,
	[Responsable] [int]  NOT NULL,
	[ProviderId] [bigint]  NULL,
	[Active] [bit]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [date]  NULL,
	[Operation] [varchar](100)  NULL,
	[FirstDate] [datetime]  NULL,
) ON [PRIMARY]