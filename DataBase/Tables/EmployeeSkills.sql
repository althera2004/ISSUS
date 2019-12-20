IF OBJECT_ID('[dbo].[EmployeeSkills]', 'U')  IS NOT NULL DROP TABLE [dbo].[EmployeeSkills]


CREATE TABLE [dbo].[EmployeeSkills](
	[Id] [int]  NOT NULL,
	[EmployeeId] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Academic] [text]  NULL,
	[AcademicValid] [bit]  NULL,
	[Specific] [text]  NULL,
	[SpecificValid] [bit]  NULL,
	[WorkExperience] [text]  NULL,
	[WorkExperienceValid] [bit]  NULL,
	[Hability] [text]  NULL,
	[HabilityValid] [bit]  NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [date]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [date]  NOT NULL,
) ON [PRIMARY]