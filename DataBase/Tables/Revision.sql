IF OBJECT_ID('[dbo].[Revision]', 'U')  IS NOT NULL DROP TABLE [dbo].[Revision]


CREATE TABLE [dbo].[Revision](
	[Id] [int]  NOT NULL,
	[Type] [int]  NOT NULL,
	[MonthDay] [int]  NULL,
	[MonthDayOrder] [int]  NULL,
	[MonthDayWeek] [int]  NULL,
	[WeekDays] [nvarchar](7)  NULL,
	[DaysPeriode] [int]  NULL,
	[Laboral] [bit]  NULL,
	[CreatedBy] [int]  NULL,
	[CreatedOn] [datetime]  NULL,
	[ModifiedBy] [int]  NULL,
	[ModifiedOn] [datetime]  NULL,
	[Active] [bit]  NULL,
) ON [PRIMARY]