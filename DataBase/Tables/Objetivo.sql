IF OBJECT_ID('[dbo].[Objetivo]', 'U')  IS NOT NULL DROP TABLE [dbo].[Objetivo]


CREATE TABLE [dbo].[Objetivo](
	[Id] [int]  NOT NULL,
	[Name] [nvarchar](100)  NOT NULL,
	[Description] [nvarchar](2000)  NULL,
	[ResponsibleId] [int]  NOT NULL,
	[StartDate] [datetime]  NOT NULL,
	[VinculatedToIndicator] [bit]  NOT NULL,
	[IndicatorId] [int]  NULL,
	[RevisionId] [int]  NOT NULL,
	[Methodology] [nvarchar](2000)  NULL,
	[Resources] [nvarchar](2000)  NULL,
	[Notes] [nvarchar](2000)  NULL,
	[PreviewEndDate] [datetime]  NULL,
	[EndDate] [datetime]  NULL,
	[EndReason] [nvarchar](500)  NULL,
	[ResponsibleClose] [int]  NULL,
	[CompanyId] [int]  NOT NULL,
	[MetaComparer] [nvarchar](10)  NULL,
	[Meta] [decimal](18,6)  NULL,
	[CreatedBy] [int]  NOT NULL,
	[CreatedOn] [datetime]  NOT NULL,
	[ModifiedBy] [int]  NOT NULL,
	[ModifiedOn] [datetime]  NOT NULL,
	[Active] [bit]  NOT NULL,
) ON [PRIMARY]