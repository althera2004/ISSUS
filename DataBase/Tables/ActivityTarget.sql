IF OBJECT_ID('[dbo].[ActivityTarget]', 'U')  IS NOT NULL DROP TABLE [dbo].[ActivityTarget]


CREATE TABLE [dbo].[ActivityTarget](
	[ActivityTarget] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
) ON [PRIMARY]