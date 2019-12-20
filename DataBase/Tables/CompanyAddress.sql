IF OBJECT_ID('[dbo].[CompanyAddress]', 'U')  IS NOT NULL DROP TABLE [dbo].[CompanyAddress]


CREATE TABLE [dbo].[CompanyAddress](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[PostalCode] [nvarchar](10)  NULL,
	[City] [nvarchar](50)  NULL,
	[Province] [nvarchar](50)  NULL,
	[Country] [nvarchar](15)  NULL,
	[Phone] [nvarchar](15)  NULL,
	[Mobile] [nvarchar](15)  NULL,
	[Email] [nvarchar](50)  NULL,
	[Notes] [text]  NULL,
	[Fax] [nvarchar](15)  NULL,
	[Active] [bigint]  NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Address] [varchar](100)  NULL,
) ON [PRIMARY]