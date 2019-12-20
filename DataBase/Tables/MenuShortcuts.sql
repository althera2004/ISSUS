IF OBJECT_ID('[dbo].[MenuShortcuts]', 'U')  IS NOT NULL DROP TABLE [dbo].[MenuShortcuts]


CREATE TABLE [dbo].[MenuShortcuts](
	[Id] [int]  NOT NULL,
	[Label] [nvarchar](50)  NOT NULL,
	[Icon] [nvarchar](50)  NOT NULL,
	[Link] [nvarchar](50)  NOT NULL,
	[SecurityGroupId] [int]  NULL,
) ON [PRIMARY]