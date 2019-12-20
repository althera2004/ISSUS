IF OBJECT_ID('[dbo].[CuestionarioPregunta]', 'U')  IS NOT NULL DROP TABLE [dbo].[CuestionarioPregunta]


CREATE TABLE [dbo].[CuestionarioPregunta](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [bigint]  NOT NULL,
	[CuestionarioId] [bigint]  NOT NULL,
	[Question] [nvarchar](2000)  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NULL,
) ON [PRIMARY]