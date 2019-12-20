IF OBJECT_ID('[dbo].[MeasureUnit]', 'U')  IS NOT NULL DROP TABLE [dbo].[MeasureUnit]


CREATE TABLE [dbo].[MeasureUnit](
	[Id] [bigint]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
	[Active] [bit]  NOT NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
) ON [PRIMARY]