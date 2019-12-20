

-- =============================================
-- Author:		Juan Castilla Calderón
-- Create date: 12/02/2019
-- Description:	Inserts auditory planning into database
-- =============================================
CREATE PROCEDURE [dbo].[AuditoryPlanning_ByAuditory]
	@CompanyId int,
	@AuditoryId bigint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		AP.Id,
		AP.CompanyId,
		AP.[Date],
		AP.[Hour],
		AP.Duration,
		AP.ProcessId,
		P.[Description],
		AP.Auditor,
		AUDITOR.[Login],
		'',
		'',
		AP.Audited,
		EMP.[Name],
		ISNULL(EMP.LastName,''),
		ISNULL(EMP.Email,''),
		AP.SendMail,
		ISNULL(AP.ProviderEmail,''),
		AP.CreatedBy,
		CB.[Login],
		AP.CreatedOn,
		AP.ModifiedBy,
		MB.[Login],
		AP.ModifiedOn,
		AP.Active
	FROM AuditoryPlanning AP WITH(NOLOCK)
	INNER JOIN Auditory A WITH(NOLOCK)
	ON	A.Id = AP.AuditoryId
	INNER JOIN ApplicationUser AUDITOR WITH(NOLOCK)
	ON	AUDITOR.Id = AP.Auditor
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = AP.Audited
	INNER JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = AP.ProcessId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON CB.Id = AP.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON MB.Id = AP.ModifiedBy


	WHERE
		AP.Active = 1
	AND AP.CompanyId = @CompanyId
	AND AP.AuditoryId = @AuditoryId

END

