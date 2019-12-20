
CREATE PROCEDURE [dbo].[IncidentAction_Update]
	@IncidentActionId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@ActionType int,
	@Origin int,
	@Number bigint,
	@ReporterType int,
	@DepartmentId int,
	@ProviderId bigint,
	@CustomerId bigint,
	@IncidentId bigint,
	@WhatHappend nvarchar(2000),
	@WhatHappendBy bigint,
	@WhatHappendOn datetime,
	@Causes nvarchar(2000),
	@CausesBy bigint,
	@CausesOn datetime,
	@Actions nvarchar(2000),
	@ActionsBy bigint,
	@ActionsOn datetime,
	@ActionsExecuter int,
	@ActionsSchedule datetime,
	@Monitoring nvarchar(2000),
	@ClosedBy bigint,
	@ClosedOn datetime,
    @ClosedExecutor bigint,
    @ClosedExecutorOn datetime,
	@Notes nvarchar(2000),
	@UserId int,
	@BusinessRiskId bigint,
	@OportunityId bigint
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE IncidentAction SET
		ActionType = @ActionType,
		[Description] = @Description,
		--Origin = @Origin,
		ReporterType = @ReporterType,
		DepartmentId = @DepartmentId,
		ProviderId = @ProviderId,
		CustomerId = @CustomerId,
		Number = @Number,
		--IncidentId = @IncidentId,
		WhatHappend = @WhatHappend,
		WhatHappendBy = @WhatHappendBy,
		WhatHappendOn = @WhatHappendOn,
		Causes = @Causes,
		CausesBy = @CausesBy,
		CausesOn = @CausesOn,
		Actions = @Actions,
		ActionsBy = @ActionsBy,
		ActionsOn = @ActionsOn,
		ActionsExecuter = @ActionsExecuter,
		ActionsSchedule = @ActionsSchedule,
		Monitoring = @Monitoring,
		--ClosedBy = @ClosedBy,
		--ClosedOn = @ClosedOn,
		--ClosedExecutor = @ClosedExecutor,
		--ClosedExecutorOn = @ClosedExecutorOn,
		Notes = @Notes,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
		--BusinessRiskId = @BusinessRiskId,
		--OportunityId = @OportunityId

	WHERE 
		Id = @IncidentActionId
	AND CompanyId = @CompanyId


END

