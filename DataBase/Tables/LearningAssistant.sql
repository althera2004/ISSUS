IF OBJECT_ID('[dbo].[LearningAssistant]', 'U')  IS NOT NULL DROP TABLE [dbo].[LearningAssistant]


CREATE TABLE [dbo].[LearningAssistant](
	[Id] [int]  NOT NULL,
	[LearningId] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[EmployeeId] [int]  NOT NULL,
	[CargoId] [int]  NULL,
	[Completed] [bit]  NULL,
	[Success] [bit]  NULL,
	[ModifiedBy] [int]  NULL,
	[ModifiedOn] [datetime]  NULL,
	[Active] [bit]  NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
) ON [PRIMARY]