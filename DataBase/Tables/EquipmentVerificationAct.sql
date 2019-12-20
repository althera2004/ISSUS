IF OBJECT_ID('[dbo].[EquipmentVerificationAct]', 'U')  IS NOT NULL DROP TABLE [dbo].[EquipmentVerificationAct]


CREATE TABLE [dbo].[EquipmentVerificationAct](
	[Id] [bigint]  NOT NULL,
	[EquipmentId] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[EquipmentVerificationType] [int]  NOT NULL,
	[Date] [datetime]  NOT NULL,
	[Vto] [datetime]  NOT NULL,
	[Result] [numeric](18,3)  NOT NULL,
	[MaxResult] [numeric](18,3)  NOT NULL,
	[Cost] [numeric](18,3)  NULL,
	[ProviderId] [bigint]  NULL,
	[Responsable] [int]  NOT NULL,
	[Active] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Operation] [varchar](100)  NULL,
) ON [PRIMARY]