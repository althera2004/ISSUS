IF OBJECT_ID('[dbo].[AuditoryCuestionarioObservations]', 'U')  IS NOT NULL DROP TABLE [dbo].[AuditoryCuestionarioObservations]


CREATE TABLE [dbo].[AuditoryCuestionarioObservations](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [bigint]  NOT NULL,
	[AuditoryId] [bigint]  NOT NULL,
	[CuestionarioId] [bigint]  NOT NULL,
	[Text] [nvarchar](2000)  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]