





CREATE PROCEDURE [dbo].[BusinessRisk_Insert]
	@Id bigint out,
	@CompanyId int,
	@Description nvarchar(100),
	@Code int,
	@RuleId bigint,
	@ProcessId bigint,
	@PreviousBusinessRiskId bigint,
	@ItemDescription nvarchar(2000),
	@Notes nvarchar(2000),
	@Causes nvarchar(2000),
	@StartControl nvarchar(2000),
	@StartProbability int,
	@StartSeverity int,
	@StartResult int,
	@StartAction int,
	@Assumed bit,
	@UserId int,
	@DateStart datetime
AS
BEGIN
	If @Code = 0 
	Begin
		SELECT @Code = ISNULL(MAX(I.Code),1) + 1
		FROM BusinessRisk3 I WITH(NOLOCK)
		WHERE
			I.CompanyId = @CompanyId
	End

	INSERT INTO BusinessRisk3
	(
		CompanyId,
		Description,
		Code,
		RuleId,
		PreviousBusinessRiskId,
		ItemDescription,
		Notes,
		Causes,
		StartProbability,
		StartSeverity,
		StartResult,
		StartAction,
        CreatedBy,
        CreatedOn,
        ModifiedBy,
        ModifiedOn,
		Active,
		DateStart,
		StartControl,
		ProcessId,
		ProbabilityId,
		SecurityId,
		Result,
		Assumed,
		ApplyAction
				   
				
			  
			 
	)
    VALUES
	(
		@CompanyId,
        @Description,
		@Code,
		@RuleId,
		@PreviousBusinessRiskId,
		@ItemDescription,
		@Notes,
		@Causes,
		@StartProbability,
		@StartSeverity,
		@StartResult,
        @StartAction,
        @UserId,
        GETDATE(),
        @UserId,
        GETDATE(),
        1,
		@DateStart,
		@StartControl,
		@ProcessId,
		0,
		0,
		0,
		@Assumed,
	
	
	
	
		0
	)

	Set @Id = @@Identity

	/*UPDATE IncidentAction SET
		BusinessRiskId = @Id
	WHERE
		BusinessRiskId = @PreviousBusinessRiskId
	AND @PreviousBusinessRiskId IS NOT NULL
	AND Origin = 4
	AND ClosedOn IS NULL
	AND CompanyId = @CompanyId*/
END







