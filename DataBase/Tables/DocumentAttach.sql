IF OBJECT_ID('[dbo].[DocumentAttach]', 'U')  IS NOT NULL DROP TABLE [dbo].[DocumentAttach]


CREATE TABLE [dbo].[DocumentAttach](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[DocumentId] [bigint]  NOT NULL,
	[Version] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
	[Extension] [nvarchar](10)  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]