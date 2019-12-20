

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Oportunity_Update]
	@Id int,
	@CompanyId int,
	@Description nvarchar(100),
	@Code bigint,
	@ItemDescription nvarchar(2000),
	@StartControl nvarchar(2000),
	@Notes nvarchar(2000),
	@ApplyAction bit,
	@DateStart datetime,
	@Causes nvarchar(2000),
	@Cost int,
	@Impact int,
	@Result int,
	@ProcessId bigint,
	@RuleId bigint,
	@FinalCost int,
	@FinalImpact int,
	@FinalResult int,
	@FinalDate datetime,
	@FinalApplyAction bit,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF YEAR(@FinalDate) < 2000 
	BEGIN
		SET @FinalDate = null
	END

    -- Insert statements for procedure here
	UPDATE Oportunity SET
		  [Description] = @Description,
		  [Code] = @Code,
		  [ItemDescription] = @ItemDescription,
		  [StartControl] = @StartControl,
		  [Notes] = @Notes,
		  [ApplyAction] = @ApplyAction,
		  [DateStart] = @DateStart,
		  [Causes] = @Causes,
		  [Cost] = @Cost,
		  [Impact] = @Impact,
		  [Result] = @Result,
		  [ProcessId] = @ProcessId,
		  [RuleId] = @RuleId,
		  [FinalCost] = @FinalCost,
		  [FinalImpact] = @FinalImpact,
		  [FinalResult] = @FinalResult,
		  [FinalDate] = @FinalDate,
		  [FinalApplyAction] = @FinalApplyAction,
		  [ModifiedBy] = @ApplicationUserId,
		  [ModifiedOn] = GETDATE()
	WHERE 
		Id = @Id
	AND	CompanyId = @CompanyId


END

