IF OBJECT_ID('[dbo].[AuditoryCuestionarioPregunta]', 'U')  IS NOT NULL DROP TABLE [dbo].[AuditoryCuestionarioPregunta]


CREATE TABLE [dbo].[AuditoryCuestionarioPregunta](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [bigint]  NOT NULL,
	[AuditoryId] [bigint]  NOT NULL,
	[CuestionarioId] [bigint]  NOT NULL,
	[QuestionId] [bigint]  NOT NULL,
	[Question] [nvarchar](2000)  NOT NULL,
	[Compliant] [bit]  NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NULL,
) ON [PRIMARY]