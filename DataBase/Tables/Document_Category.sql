IF OBJECT_ID('[dbo].[Document_Category]', 'U')  IS NOT NULL DROP TABLE [dbo].[Document_Category]


CREATE TABLE [dbo].[Document_Category](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
	[Editable] [bit]  NULL,
) ON [PRIMARY]