IF OBJECT_ID('[dbo].[ObjetivoRegistro]', 'U')  IS NOT NULL DROP TABLE [dbo].[ObjetivoRegistro]


CREATE TABLE [dbo].[ObjetivoRegistro](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[ObjetivoId] [int]  NOT NULL,
	[MetaComparer] [nvarchar](10)  NULL,
	[Meta] [decimal](18,6)  NULL,
	[AlarmComparer] [nvarchar](10)  NULL,
	[Alarm] [decimal](18,6)  NULL,
	[Date] [datetime]  NOT NULL,
	[Value] [decimal](18,6)  NOT NULL,
	[ResponsibleId] [int]  NOT NULL,
	[Comments] [nvarchar](500)  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]