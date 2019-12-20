IF OBJECT_ID('[dbo].[Proceso2]', 'U')  IS NOT NULL DROP TABLE [dbo].[Proceso2]


CREATE TABLE [dbo].[Proceso2](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](150)  NULL,
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
) ON [PRIMARY]