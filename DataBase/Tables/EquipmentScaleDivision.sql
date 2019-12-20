IF OBJECT_ID('[dbo].[EquipmentScaleDivision]', 'U')  IS NOT NULL DROP TABLE [dbo].[EquipmentScaleDivision]


CREATE TABLE [dbo].[EquipmentScaleDivision](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](20)  NOT NULL,
	[Active] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
) ON [PRIMARY]