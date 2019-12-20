


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_Update]
	@Id int,
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
	UPDATE [dbo].[Revision] SET
		[Type] = @Type,
		[MonthDay] =@MonthDay,
		[MonthDayOrder] =@MonthDayOrder,
		[MonthDayWeek] = @MonthDayWeek,
		[WeekDays] = @WeekDays,
		[DaysPeriode] = @DaysPeriode,
		[Laboral] = @Laboral,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn]= GETDATE()
	WHERE
		Id = @ID
END



