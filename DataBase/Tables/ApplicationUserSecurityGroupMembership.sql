IF OBJECT_ID('[dbo].[ApplicationUserSecurityGroupMembership]', 'U')  IS NOT NULL DROP TABLE [dbo].[ApplicationUserSecurityGroupMembership]


CREATE TABLE [dbo].[ApplicationUserSecurityGroupMembership](
	[ApplicationUserId] [int]  NOT NULL,
	[SecurityGroupId] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
 CONSTRAINT [PK_ApplicationUserSecurityGroupMembership] PRIMARY KEY CLUSTERED  (
  [ApplicationUserId] ASC ,
  [CompanyId] ASC ,
  [SecurityGroupId] ASC 
) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON [PRIMARY]