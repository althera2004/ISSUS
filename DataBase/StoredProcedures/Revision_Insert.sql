


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_Insert]
	@Id int output,
	@Type int,
    @MonthDay int,
    @MonthDayOrder int,
    @MonthDayWeek int,
    @WeekDays nvarchar(7),
    @DaysPeriode int,
    @Laboral bit,
    @ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Revision]
	(
		[Type],
		[MonthDay],
		[MonthDayOrder],
		[MonthDayWeek],
		[WeekDays],
		[DaysPeriode],
		[Laboral],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@Type,
        @MonthDay,
        @MonthDayOrder,
        @MonthDayWeek,
        @WeekDays,
        @DaysPeriode,
        @Laboral,
        @ApplicationUserId,
        GETDATE(),
        @ApplicationUserId,
        GETDATE(),
		1
	)

	SET @Id = @@IDENTITY
END



