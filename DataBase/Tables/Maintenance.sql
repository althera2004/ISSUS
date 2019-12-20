IF OBJECT_ID('[dbo].[Maintenance]', 'U')  IS NOT NULL DROP TABLE [dbo].[Maintenance]


CREATE TABLE [dbo].[Maintenance](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[EquipmentId] [bigint]  NOT NULL,
	[Type] [int]  NOT NULL,
	[Periodicity] [int]  NOT NULL,
	[Accessories] [nvarchar](150)  NOT NULL,
	[Amount] [numeric](18,3)  NOT NULL,
	[Active] [bit]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
) ON [PRIMARY]