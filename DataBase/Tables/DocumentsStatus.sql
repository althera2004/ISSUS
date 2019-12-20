IF OBJECT_ID('[dbo].[DocumentsStatus]', 'U')  IS NOT NULL DROP TABLE [dbo].[DocumentsStatus]


CREATE TABLE [dbo].[DocumentsStatus](
	[Id] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
) ON [PRIMARY]