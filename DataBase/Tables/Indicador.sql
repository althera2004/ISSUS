IF OBJECT_ID('[dbo].[Indicador]', 'U')  IS NOT NULL DROP TABLE [dbo].[Indicador]


CREATE TABLE [dbo].[Indicador](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NULL,
	[Descripcion] [nvarchar](150)  NOT NULL,
	[ResponsableId] [int]  NULL,
	[ProcessId] [int]  NULL,
	[Calculo] [nvarchar](2000)  NOT NULL,
	[MetaComparer] [nvarchar](10)  NOT NULL,
	[Meta] [decimal](18,6)  NOT NULL,
	[AlarmaComparer] [nvarchar](10)  NULL,
	[Alarma] [decimal](18,6)  NULL,
	[Periodicity] [int]  NULL,
	[StartDate] [datetime]  NOT NULL,
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