IF OBJECT_ID('[dbo].[IndicadorHistorico]', 'U')  IS NOT NULL DROP TABLE [dbo].[IndicadorHistorico]


CREATE TABLE [dbo].[IndicadorHistorico](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[IndicadorId] [int]  NOT NULL,
	[ActionDate] [datetime]  NOT NULL,
	[Reason] [nvarchar](500)  NULL,
	[EmployeeId] [int]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]