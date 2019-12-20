IF OBJECT_ID('[dbo].[ApplicationGrant]', 'U')  IS NOT NULL DROP TABLE [dbo].[ApplicationGrant]


CREATE TABLE [dbo].[ApplicationGrant](
	[UserId] [int]  NOT NULL,
	[ItemId] [int]  NOT NULL,
	[GrantToRead] [bit]  NOT NULL,
	[GrantToWrite] [bit]  NOT NULL,
	[GrantToDelete] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NULL,
	[ModifiedOn] [datetime]  NULL,
 CONSTRAINT [PK_ApplicationGrant] PRIMARY KEY CLUSTERED  (
  [ItemId] ASC ,
  [UserId] ASC 
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON [PRIMARY]