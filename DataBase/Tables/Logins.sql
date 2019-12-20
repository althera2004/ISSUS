IF OBJECT_ID('[dbo].[Logins]', 'U')  IS NOT NULL DROP TABLE [dbo].[Logins]


CREATE TABLE [dbo].[Logins](
	[Id] [uniqueidentifier]  NOT NULL,
	[UserName] [nvarchar](50)  NOT NULL,
	[Date] [datetime]  NOT NULL,
	[IP] [nvarchar](50)  NOT NULL,
	[Result] [int]  NOT NULL,
	[UserId] [int]  NULL,
	[CompanyCode] [nvarchar](10)  NOT NULL,
	[CompanyId] [int]  NULL,
) ON [PRIMARY]