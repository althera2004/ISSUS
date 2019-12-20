IF OBJECT_ID('[dbo].[ApplicationUserCompany]', 'U')  IS NOT NULL DROP TABLE [dbo].[ApplicationUserCompany]


CREATE TABLE [dbo].[ApplicationUserCompany](
	[UserId] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
) ON [PRIMARY]