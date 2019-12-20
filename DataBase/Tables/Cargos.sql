IF OBJECT_ID('[dbo].[Cargos]', 'U')  IS NOT NULL DROP TABLE [dbo].[Cargos]


CREATE TABLE [dbo].[Cargos](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[DepartmentId] [int]  NOT NULL,
	[ResponsableId] [int]  NULL,
	[Description] [nvarchar](100)  NOT NULL,
	[Responsabilidades] [nvarchar](2000)  NULL,
	[Notas] [nvarchar](2000)  NULL,
	[FormacionAcademicaDeseada] [nvarchar](2000)  NULL,
	[FormacionEspecificaDesdeada] [nvarchar](2000)  NULL,
	[ExperienciaLaboralDeseada] [nvarchar](2000)  NULL,
	[HabilidadesDeseadas] [nvarchar](2000)  NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [date]  NOT NULL,
	[Active] [bit]  NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [date]  NULL,
) ON [PRIMARY]