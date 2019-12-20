IF OBJECT_ID('[dbo].[ApplicationItem]', 'U')  IS NOT NULL DROP TABLE [dbo].[ApplicationItem]


CREATE TABLE [dbo].[ApplicationItem](
	[Id] [int] IDENTITY(1,1)  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
	[Icon] [nvarchar](50)  NOT NULL,
	[UrlList] [nvarchar](50)  NOT NULL,
	[Parent] [int]  NOT NULL,
	[Container] [bit]  NOT NULL,
	[Order] [int]  NULL,
 CONSTRAINT [PK_ApplicationItem] PRIMARY KEY CLUSTERED  (
  [Id] ASC 
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON [PRIMARY]