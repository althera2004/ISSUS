IF OBJECT_ID('[dbo].[EquipmentCalibrationAct]', 'U')  IS NOT NULL DROP TABLE [dbo].[EquipmentCalibrationAct]


CREATE TABLE [dbo].[EquipmentCalibrationAct](
	[Id] [bigint]  NOT NULL,
	[EquipmentId] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[EquipmentCalibrationType] [int]  NOT NULL,
	[Date] [datetime]  NOT NULL,
	[Vto] [datetime]  NOT NULL,
	[Result] [numeric](18,6)  NOT NULL,
	[MaxResult] [numeric](18,6)  NOT NULL,
	[Cost] [numeric](18,3)  NULL,
	[ProviderId] [bigint]  NULL,
	[Responsable] [int]  NOT NULL,
	[Active] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Uncetainty] [numeric](18,6)  NULL,
	[Operation] [varchar](100)  NULL,
) ON [PRIMARY]