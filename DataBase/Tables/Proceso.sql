IF OBJECT_ID('[dbo].[Proceso]', 'U')  IS NOT NULL DROP TABLE [dbo].[Proceso]


CREATE TABLE [dbo].[Proceso](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[CargoId] [bigint]  NOT NULL,
	[Type] [int]  NOT NULL,
	[Inicio] [nvarchar](2000)  NULL,
	[Desarrollo] [nvarchar](2000)  NULL,
	[Fin] [nvarchar](2000)  NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
	[Description_bak] [nvarchar](100)  NULL,
	[DisabledOn] [datetime]  NULL,
	[DisabledBy] [int]  NULL,
	[Description] [nvarchar](150)  NULL,
) ON [PRIMARY]