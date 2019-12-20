IF OBJECT_ID('[dbo].[RulesHistory]', 'U')  IS NOT NULL DROP TABLE [dbo].[RulesHistory]


CREATE TABLE [dbo].[RulesHistory](
	[Id] [bigint]  NOT NULL,
	[RuleId] [bigint]  NOT NULL,
	[IPR] [int]  NOT NULL,
	[Reason] [nvarchar](500)  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]