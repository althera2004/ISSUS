IF OBJECT_ID('[dbo].[AuditoryCuestionarioFound]', 'U')  IS NOT NULL DROP TABLE [dbo].[AuditoryCuestionarioFound]


CREATE TABLE [dbo].[AuditoryCuestionarioFound](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [bigint]  NOT NULL,
	[AuditoryId] [bigint]  NOT NULL,
	[CuestionarioId] [bigint]  NOT NULL,
	[Text] [nvarchar](2000)  NOT NULL,
	[Requeriment] [nvarchar](2000)  NOT NULL,
	[UnConformity] [nvarchar](2000)  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NULL,
	[Action] [bit]  NULL,
) ON [PRIMARY]