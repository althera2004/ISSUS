IF OBJECT_ID('[dbo].[EquipmentVerificationDefinition]', 'U')  IS NOT NULL DROP TABLE [dbo].[EquipmentVerificationDefinition]


CREATE TABLE [dbo].[EquipmentVerificationDefinition](
	[Id] [bigint]  NOT NULL,
	[EquipmentId] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[VerificationType] [int]  NOT NULL,
	[Periodicity] [int]  NOT NULL,
	[Uncertainty] [numeric](18,6)  NULL,
	[Range] [nvarchar](50)  NOT NULL,
	[Pattern] [nvarchar](50)  NOT NULL,
	[Cost] [numeric](18,3)  NULL,
	[Notes] [text]  NOT NULL,
	[Responsable] [int]  NOT NULL,
	[ProviderId] [bigint]  NULL,
	[Active] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Operation] [nvarchar](100)  NULL,
	[FirstDate] [datetime]  NULL,
) ON [PRIMARY]