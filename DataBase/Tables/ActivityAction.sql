IF OBJECT_ID('[dbo].[ActivityAction]', 'U')  IS NOT NULL DROP TABLE [dbo].[ActivityAction]


CREATE TABLE [dbo].[ActivityAction](
	[ActivityTarget] [int]  NOT NULL,
	[ActivityAction] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
) ON [PRIMARY]