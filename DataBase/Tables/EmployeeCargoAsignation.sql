IF OBJECT_ID('[dbo].[EmployeeCargoAsignation]', 'U')  IS NOT NULL DROP TABLE [dbo].[EmployeeCargoAsignation]


CREATE TABLE [dbo].[EmployeeCargoAsignation](
	[EmployeeId] [int]  NOT NULL,
	[CargoId] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[FechaAlta] [datetime]  NULL,
	[FechaBaja] [datetime]  NULL,
) ON [PRIMARY]