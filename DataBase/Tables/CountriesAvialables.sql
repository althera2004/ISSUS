IF OBJECT_ID('[dbo].[CountriesAvialables]', 'U')  IS NOT NULL DROP TABLE [dbo].[CountriesAvialables]


CREATE TABLE [dbo].[CountriesAvialables](
	[Id] [int]  NOT NULL,
	[Description] [nvarchar](50)  NOT NULL,
) ON [PRIMARY]