IF OBJECT_ID('[dbo].[UploadFiles]', 'U')  IS NOT NULL DROP TABLE [dbo].[UploadFiles]


CREATE TABLE [dbo].[UploadFiles](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[ItemLinked] [int]  NOT NULL,
	[ItemId] [bigint]  NOT NULL,
	[FileName] [nvarchar](250)  NOT NULL,
	[Description] [nvarchar](100)  NOT NULL,
	[Extension] [nchar](10)  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]