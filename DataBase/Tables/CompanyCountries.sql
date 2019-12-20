IF OBJECT_ID('[dbo].[CompanyCountries]', 'U')  IS NOT NULL DROP TABLE [dbo].[CompanyCountries]


CREATE TABLE [dbo].[CompanyCountries](
	[CountryId] [int]  NOT NULL,
	[CompanyId] [int]  NOT NULL,
) ON [PRIMARY]