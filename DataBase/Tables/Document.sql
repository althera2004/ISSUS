IF OBJECT_ID('[dbo].[Document]', 'U')  IS NOT NULL DROP TABLE [dbo].[Document]


CREATE TABLE [dbo].[Document](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[ActualVersion] [int]  NOT NULL,
	[CategoryId] [int]  NULL,
	[FechaAlta] [datetime]  NULL,
	[FechaBaja] [datetime]  NULL,
	[Origen] [int]  NULL,
	[ProcedenciaId] [int]  NULL,
	[Conservacion] [int]  NULL,
	[ConservacionType] [int]  NULL,
	[Activo] [bit]  NULL,
	[Codigo_bak] [nvarchar](10)  NULL,
	[Ubicacion] [nvarchar](100)  NULL,
	[ModifiedBy] [int]  NULL,
	[ModifiedOn] [datetime]  NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
	[Description] [nvarchar](100)  NULL,
	[EndReason] [nvarchar](500)  NULL,
	[Codigo] [nvarchar](25)  NULL,
) ON [PRIMARY]