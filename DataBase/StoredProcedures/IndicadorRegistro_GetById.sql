


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IndicadorRegistro_GetById]
	@IndicadorRegistroId int,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		IR.Id,
		IR.IndicadorId,
		I.Descripcion AS IndicadorDescripcion,
		IR.Meta,
		IR.Alarm,
		IR.Value,
		IR.[Date] AS Date,
		IR.ResponsibleId,
		EMP.Name AS ResponsibleName,
		EMP.LastName AS ResponsibleLastName,
		ISNULL(AU.Id,-1) AS EmployeeUserId,
		ISNULL(AU.[Login],'') AS EmployeeUserName,
		IR.CreatedBy,
		CB.[Login] AS CreatedByName,
		IR.CreatedOn,
		IR.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		IR.ModifiedOn,
		IR.Active,
		IR.Comments,
		IR.MetaComparer,
		IR.AlarmComparer
	FROM IndicadorRegistro IR WITH(NOLOCK)
	INNER JOIN Indicador I WITH(NOLOCK)
	ON	I.Id = IR.IndicadorId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = IR.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = IR.ModifiedBy
	INNER JOIN Employee EMP WITH(NOLOCK)
		LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
			INNER JOIN ApplicationUser AU WITH(NOLOCK)
			ON	AU.Id = EUA.UserId
		ON	EUA.EmployeeId = EMP.Id
	ON	EMP.Id = IR.ResponsibleId

	WHERE
		IR.Id = @IndicadorRegistroId
	AND	IR.CompanyId = @CompanyId

END


