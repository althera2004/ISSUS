
CREATE PROCEDURE [dbo].[IncidentAction_Insert]
	@IncidentActionId bigint output,
	@CompanyId int,
	@ActionType int,
	@Description nvarchar(100),
	@Origin int,
	@ReporterType int,
	@DeparmentId int,
	@ProviderId int,
	@CustomerId int,
	@Number bigint,
	@IncidentId bigint,
	@WhatHappend nvarchar(2000),
	@WhatHappendBy bigint,
	@WhatHappendDate datetime,
	@Causes nvarchar(2000),
	@CausesBy bigint,
	@CausesDate datetime,
	@Actions nvarchar(2000),
	@ActionsBy bigint,
	@ActionsExecuter int,
	@ActionsSchedule datetime,
	@ActionsDate datetime,
	@Monitoring nvarchar(2000),
	@ClosedBy bigint,
	@ClosedDate datetime,
	@ClosedExecutor bigint,
	@ClosedExecutorOn datetime,
	@Notes nvarchar(2000),
	@UserId int,
	@BusinessRiskId bigint,
	@ObjetivoId int,
	@OportunityId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT @Number = ISNULL(MAX(Number) ,0) + 1
	FROM IncidentAction I WITH(NOLOCK)
	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1

    INSERT INTO IncidentAction
    (
		CompanyId,
		ActionType,
		Description,
		Origin,
		ReporterType,
		DepartmentId,
		ProviderId,
		CustomerId,
		Number,
		IncidentId,
		WhatHappend,
		WhatHappendBy,
		WhatHappendOn,
		Causes,
		CausesBy,
		CausesOn,
		Actions,
		ActionsBy,
		ActionsOn,
		ActionsExecuter,
		ActionsSchedule,
		Monitoring,
		ClosedBy,
		ClosedOn,
		ClosedExecutor,
		ClosedExecutorOn,
		Notes,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn,
		BusinessRiskId,
		ObjetivoId,
		OportunityId
	)
    VALUES
    (
		@CompanyId,
		@ActionType,
		@Description,
		@Origin,
		@ReporterType,
		@DeparmentId,
		@ProviderId,
		@CustomerId,
		@Number,
		@IncidentId,
		@WhatHappend,
		@WhatHappendBy,
		@WhatHappendDate,
		@Causes,
		@CausesBy,
		@CausesDate,
		@Actions,
		@ActionsBy,
		@ActionsDate,
		@ActionsExecuter,
		@ActionsSchedule,
		@Monitoring,
		@ClosedBy,
		@ClosedDate,
		@ClosedExecutor,
		@ClosedExecutorOn,
		@Notes,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE(),
		@BusinessRiskId,
		@ObjetivoId,
		@OportunityId
	)

	SET @IncidentActionId = @@IDENTITY

END
