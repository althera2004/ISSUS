IF OBJECT_ID('[dbo].[SecurityGroup]', 'U')  IS NOT NULL DROP TABLE [dbo].[SecurityGroup]


CREATE TABLE [dbo].[SecurityGroup](
	[Id] [int]  NOT NULL,
	[Name] [nvarchar](50)  NOT NULL,
) ON [PRIMARY]