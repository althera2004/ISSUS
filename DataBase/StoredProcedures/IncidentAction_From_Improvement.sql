

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IncidentAction_From_Improvement]
	@ImprovementId bigint,
	@Title nvarchar(100),
	@AuditoryId bigint,
	@EmployeeId int,
	@ApplicationUserId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ProviderId bigint
	DECLARE @CustomerId bigint
	DECLARE @ReporterType int
	DECLARE @Text nvarchar(2000)

	SELECT 
		@Text = Text
	FROM AuditoryCuestionarioImprovement WITH(NOLOCK)
	WHERE
		Id = @ImprovementId
		

	SELECT
		@ProviderId = ProviderId,
		@CustomerId = CustomerId
	FROM Auditory WITH(NOLOCK)
	WHERE Id = @AuditoryId

	IF @ProviderId IS NOT NULL
	BEGIN
		SET @ReporterType = 2
	END
	ELSE
		IF @CustomerId IS NOT NULL
		BEGIN
			SET @ReporterType = 3
		END
		ELSE
			SET @ReporterType = 0
		END

    -- Insert statements for procedure here
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
		OportunityId,
		AuditoryId
	)
    VALUES
    (
		@CompanyId,
		1,
		@Title,
		1,
		@ReporterType,
		NULL,
		@ProviderId,
		@CustomerId,
		'',
		NULL,
		@Text,
		@EmployeeId,
		GETDATE(),
		'',
		NULL,
		NULL,
		'',
		NULL,
		NULL,
		NULL,
		NULL,
		'',
		NULL,
		NULL,
		NULL,
		NULL,
		'',
		1,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		NULL,
		NULL,
		NULL,
		@AuditoryId
	)


