IF OBJECT_ID('[dbo].[Unidad]', 'U')  IS NOT NULL DROP TABLE [dbo].[Unidad]


CREATE TABLE [dbo].[Unidad](
	[Id] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]