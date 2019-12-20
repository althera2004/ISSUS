IF OBJECT_ID('[dbo].[IncidentActionType]', 'U')  IS NOT NULL DROP TABLE [dbo].[IncidentActionType]


CREATE TABLE [dbo].[IncidentActionType](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
	[Editable] [bit]  NOT NULL,
	[Active] [bit]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
) ON [PRIMARY]