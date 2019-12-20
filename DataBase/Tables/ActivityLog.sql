IF OBJECT_ID('[dbo].[ActivityLog]', 'U')  IS NOT NULL DROP TABLE [dbo].[ActivityLog]


CREATE TABLE [dbo].[ActivityLog](
	[ActivityId] [uniqueidentifier]  NOT NULL,
	[UserId] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[TargetType] [int]  NOT NULL,
	[TargetId] [int]  NOT NULL,
	[ActionId] [int]  NOT NULL,
	[DateTime] [datetime]  NOT NULL,
	[ExtraData] [nvarchar](2000)  NULL,
) ON [PRIMARY]