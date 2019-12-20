IF OBJECT_ID('[dbo].[Procedencia]', 'U')  IS NOT NULL DROP TABLE [dbo].[Procedencia]


CREATE TABLE [dbo].[Procedencia](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
	[Editable] [bit]  NULL,
) ON [PRIMARY]