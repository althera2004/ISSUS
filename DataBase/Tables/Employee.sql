IF OBJECT_ID('[dbo].[Employee]', 'U')  IS NOT NULL DROP TABLE [dbo].[Employee]


CREATE TABLE [dbo].[Employee](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Name] [nvarchar](50)  NOT NULL,
	[LastName] [nvarchar](50)  NOT NULL,
	[Email] [nvarchar](50)  NULL,
	[Phone] [nvarchar](50)  NULL,
	[NIF] [nvarchar](15)  NULL,
	[Address] [nvarchar](50)  NULL,
	[PostalCode] [nvarchar](10)  NULL,
	[City] [nvarchar](50)  NULL,
	[Province] [nvarchar](50)  NULL,
	[Country] [nvarchar](50)  NULL,
	[Notes] [text]  NULL,
	[Active] [bit]  NULL,
	[FechaBaja] [date]  NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
) ON [PRIMARY]