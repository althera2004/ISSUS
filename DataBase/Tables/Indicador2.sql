IF OBJECT_ID('[dbo].[Indicador2]', 'U')  IS NOT NULL DROP TABLE [dbo].[Indicador2]


CREATE TABLE [dbo].[Indicador2](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NULL,
	[Descripcion] [nvarchar](150)  NOT NULL,
	[ResponsableId] [int]  NULL,
	[ProcessId] [int]  NULL,
	[Calculo] [nvarchar](500)  NOT NULL,
	[MetaComparer] [nvarchar](10)  NOT NULL,
	[Meta] [decimal](18,6)  NOT NULL,
	[AlarmaComparer] [nvarchar](10)  NULL,
	[Alarma] [decimal](18,6)  NULL,
	[Periodicity] [int]  NULL,
	[EndDate] [datetime]  NULL,
	[EndReason] [nvarchar](500)  NULL,
	[EndResponsible] [int]  NULL,
	[UnidadId] [int]  NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]