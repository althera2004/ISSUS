IF OBJECT_ID('[dbo].[Company]', 'U')  IS NOT NULL DROP TABLE [dbo].[Company]


CREATE TABLE [dbo].[Company](
	[Id] [int]  NOT NULL,
	[Name] [nvarchar](50)  NOT NULL,
	[SubscriptionStart] [datetime]  NULL,
	[SubscriptionEnd] [datetime]  NULL,
	[Language] [nvarchar](2)  NULL,
	[DefaultAddress] [int]  NULL,
	[NIF-CIF] [nvarchar](15)  NULL,
	[Code] [nvarchar](10)  NULL,
	[ModifiedBy] [int]  NULL,
	[ModifiedOn] [datetime]  NULL,
	[Logo] [nvarchar](5)  NULL,
	[DiskQuote] [bigint]  NULL,
	[Agreement] [bit]  NULL,
	[Headquarter] [varchar](100)  NULL,
) ON [PRIMARY]