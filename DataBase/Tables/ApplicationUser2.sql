IF OBJECT_ID('[dbo].[ApplicationUser2]', 'U')  IS NOT NULL DROP TABLE [dbo].[ApplicationUser2]


CREATE TABLE [dbo].[ApplicationUser2](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Login] [nvarchar](50)  NOT NULL,
	[Password] [nvarchar](50)  NOT NULL,
	[Status] [int]  NOT NULL,
	[LoginFailed] [int]  NOT NULL,
	[MustResetPassword] [bit]  NOT NULL,
	[Language] [nvarchar](2)  NULL,
	[ShowHelp] [bit]  NULL,
	[Avatar] [nvarchar](50)  NULL,
 CONSTRAINT [PK_ApplicationUser2] PRIMARY KEY CLUSTERED  (
  [CompanyId] ASC ,
  [Id] ASC 
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON [PRIMARY]