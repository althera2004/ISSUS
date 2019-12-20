IF OBJECT_ID('[dbo].[EmployeeUserAsignation]', 'U')  IS NOT NULL DROP TABLE [dbo].[EmployeeUserAsignation]


CREATE TABLE [dbo].[EmployeeUserAsignation](
	[UserId] [int]  NOT NULL,
	[EmployeeId] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
) ON [PRIMARY]