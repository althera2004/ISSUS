IF OBJECT_ID('[dbo].[UserShortCuts]', 'U')  IS NOT NULL DROP TABLE [dbo].[UserShortCuts]


CREATE TABLE [dbo].[UserShortCuts](
	[Id] [int]  NOT NULL,
	[ApplicationUserId] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[ShorcutGreen] [int]  NULL,
	[ShorcutBlue] [int]  NULL,
	[ShortcutYellow] [int]  NULL,
	[ShortcutRed] [int]  NULL,
) ON [PRIMARY]