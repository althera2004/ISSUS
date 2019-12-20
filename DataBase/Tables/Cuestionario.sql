IF OBJECT_ID('[dbo].[Cuestionario]', 'U')  IS NOT NULL DROP TABLE [dbo].[Cuestionario]


CREATE TABLE [dbo].[Cuestionario](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NULL,
	[Description] [nvarchar](150)  NULL,
	[NormaId] [bigint]  NULL,
	[ProcessId] [bigint]  NULL,
	[ApartadoNorma] [nvarchar](50)  NULL,
	[Notes] [nvarchar](2000)  NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NULL,
	[DisabledOn] [datetime]  NULL,
	[DisabledBy] [int]  NULL,
) ON [PRIMARY]