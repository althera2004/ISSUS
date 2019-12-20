IF OBJECT_ID('[dbo].[IndicadorObjetivo]', 'U')  IS NOT NULL DROP TABLE [dbo].[IndicadorObjetivo]


CREATE TABLE [dbo].[IndicadorObjetivo](
	[IndicadorId] [int]  NOT NULL,
	[ObjetivoId] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]