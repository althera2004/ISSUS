


CREATE PROCEDURE [dbo].[Auditory_GetById]
	@AuditoryId bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		A.Id,
		A.CompanyId,
		A.[Type],
		A.CustomerId,
		ISNULL(C.[Description],'') AS CustomerName,
		A.ProviderId,
		ISNULL(P.[Description],'') AS ProviderName,
		A.Nombre,
		A.NormaId,
		A.Amount,
		A.InternalResponsible,
		ISNULL(IR.[Name],'') AS InternalResponsibleName,
		ISNULL(IR.LastName,'') AS InternalResponsibleLastName,
		ISNULL(A.Description,'') AS Description,
		ISNULL(A.Scope,'') AS Scope,
		ISNULL(A.CompanyAddressId,-1) AS CompanyAddressId,
		ISNULL(A.EnterpriseAddress,'') AS EnterpriseAddress,
		ISNULL(A.Notes,'') AS Notes,
		A.PlannedBy,
		ISNULL(PB.[Name],'') AS PlannedByName,
		ISNULL(PB.LastName,'') AS PlannedByLastName,
		A.PlannedOn,
		A.ValidatedBy,
		ISNULL(VB.[Name],'') AS ValidatedByName,
		ISNULL(VB.LastName,'') AS ValidatedByLastName,
		A.ValidatedOn,
		A.ValidatedUserBy,
		ISNULL(VUB.[Login],'') AS ValidatedUserByName,
		A.ValidatedUserOn,
		A.Status,
		ISNULL(A.AuditorTeam,'') AS AuditorTeam,
		A.ClosedBy,
		ISNULL(ClB.[Login],'') AS ClosedByName,
		'',--ISNULL(ClB.[LastName],'') AS ClosedByLastName,
		A.ClosedOn,
		A.CreatedBy,
		CB.[Login] AS CreatedByName,
		A.CreatedOn,
		A.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		A.ModifiedOn,
		A.Active,
		A.ReportStart,
		A.ReportEnd,
		A.PreviewDate,
		ISNULL(A.PuntosFuertes,'') AS PuntosFuertes
	FROM Auditory A WITH(NOLOCK)
	LEFT JOIN Employee IR WITH(NOLOCK)
	ON	IR.Id = A.InternalResponsible
	LEFT JOIN Employee PB WITH(NOLOCK)
	ON	PB.Id = A.PlannedBy
	LEFT JOIN Employee VB WITH(NOLOCK)
	ON	VB.Id = A.ValidatedBy
	LEFT JOIN ApplicationUser VUB WITH(NOLOCK)
	ON	VUB.Id = A.ValidatedUserBy
	LEFT JOIN ApplicationUser ClB WITH(NOLOCK)
	ON	ClB.Id = A.ClosedBy
	LEFT JOIN Customer C WITH(NOLOCK)
	ON	C.Id = A.CustomerId
	LEFT JOIN Provider P WITH(NOLOCK)
	ON	P.Id = A.ProviderId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = A.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = A.ModifiedBy

	WHERE
		A.Id = @AuditoryId
	AND A.CompanyId = @CompanyId

END

