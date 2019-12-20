





CREATE PROCEDURE [dbo].[BusinessRisk_Update]
	@Id bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@Code int,
	@RuleId bigint,
	@ProcessId bigint,
	@ItemDescription nvarchar(2000),
	@Notes nvarchar(2000),
	@Causes nvarchar(2000),
	@StartProbability int,
	@StartSeverity int,
	@StartResult int,
	@StartAction int,
	@InitialValue int,
	@DateStart datetime,
	@StartControl nvarchar(2000),
	@FinalProbability int,
	@FinalSeverity int,
	@FinalResult int,
	@FinalAction int,
	@FinalDate datetime,
	@Assumed bit,
	@UserId int
AS
BEGIN
	UPDATE BusinessRisk3 SET
	
		CompanyId = @CompanyId,
		Description = @Description,
		Code = @Code,
		RuleId = @RuleId,
		ItemDescription = @ItemDescription,
		Notes = @Notes,
		Causes = @Causes,
		InitialValue = @InitialValue,
		DateStart = @DateStart,
		ProcessId = @ProcessId,
		StartControl = @StartControl,

		StartProbability = @StartProbability,
		StartSeverity = @StartSeverity,
		StartResult = @StartResult,
		StartAction = @StartAction,

		FinalProbability = @FinalProbability,
		FinalSeverity = @FinalSeverity,
		FinalResult = @FinalResult,
		FinalAction = @FinalAction,
		FinalDate = @FinalDate,
		Assumed = @Assumed,

        ModifiedBy = @UserId,
        ModifiedOn = GETDATE()

	WHERE
		Id = @Id AND CompanyId = @CompanyId
END







