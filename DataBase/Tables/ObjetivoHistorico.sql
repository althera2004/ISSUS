IF OBJECT_ID('[dbo].[ObjetivoHistorico]', 'U')  IS NOT NULL DROP TABLE [dbo].[ObjetivoHistorico]


CREATE TABLE [dbo].[ObjetivoHistorico](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[ObjetivoId] [int]  NOT NULL,
	[ActionDate] [datetime]  NOT NULL,
	[Reason] [nvarchar](500)  NULL,
	[EmployeeId] [int]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]