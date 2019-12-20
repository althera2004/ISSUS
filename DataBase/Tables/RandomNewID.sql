IF OBJECT_ID('[dbo].[RandomNewID]', 'U')  IS NOT NULL DROP TABLE [dbo].[RandomNewID]


CREATE TABLE [dbo].[RandomNewID](
	[NewID] [uniqueidentifier]  NULL,
) ON [PRIMARY]