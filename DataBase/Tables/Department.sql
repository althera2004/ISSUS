IF OBJECT_ID('[dbo].[Department]', 'U')  IS NOT NULL DROP TABLE [dbo].[Department]


CREATE TABLE [dbo].[Department](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Name] [nvarchar](50)  NOT NULL,
	[Deleted] [bit]  NULL,
	[ModifiedBy] [int]  NULL,
	[ModifiedOn] [datetime]  NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
) ON [PRIMARY]