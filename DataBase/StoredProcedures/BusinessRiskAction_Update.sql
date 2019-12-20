






CREATE PROCEDURE [dbo].[BusinessRiskAction_Update]
	@IncidentActionId bigint,
	@CompanyId int,
	@Description nvarchar(50),
	@ActionType int,
	@Origin int,
	@Number bigint,
	@ReporterType int,
	@DepartmentId int,
	@ProviderId bigint,
	@CustomerId bigint,
	@BusinessRiskId bigint,
	@WhatHappend nvarchar(155),
	@WhatHappendBy bigint,
	@WhatHappendOn datetime,
	@Causes nvarchar(255),
	@CausesBy bigint,
	@CausesOn datetime,
	@Actions nvarchar(255),
	@ActionsBy bigint,
	@ActionsOn datetime,
	@ActionsExecuter int,
	@ActionsSchedule datetime,
	@Monitoring nvarchar(255),
	@ClosedBy bigint,
	@ClosedOn datetime,
    @ClosedExecutor bigint,
    @ClosedExecutorOn datetime,
	@Notes text,
	@UserId int 
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE IncidentAction SET
		ActionType = @ActionType,
		Description = @Description,
		Origin = @Origin,
		ReporterType = @ReporterType,
		DepartmentId = @DepartmentId,
		ProviderId = @ProviderId,
		CustomerId = @CustomerId,
		Number = @Number,
		BusinessRiskId = @BusinessRiskId,
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
		ClosedBy = @ClosedBy,
		ClosedOn = @ClosedOn,
		ClosedExecutor = @ClosedExecutor,
		ClosedExecutorOn = @ClosedExecutorOn,
		Notes = @Notes,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @IncidentActionId
	AND CompanyId = @CompanyId


END







