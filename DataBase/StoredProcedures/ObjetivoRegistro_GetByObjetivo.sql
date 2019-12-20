



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [dbo].[ObjetivoRegistro_GetByObjetivo]
	@CompanyId int,
	@ObjetivoId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		OBR.Id,
		OBR.CompanyId,
		OBR.ObjetivoId,
		OBR.[Date],
		OBR.Value,
		OBR.Comments,
		OBR.ResponsibleId,
		EMP.Name AS ResponsableFirstName,
		EMP.LastName AS ResponsableLastName,
		OBR.CreatedBy,
		CB.[Login] AS CreatedByName,
		OBR.CreatedOn,
		OBR.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		OBR.ModifiedOn,
		OBR.Active,
		OBR.Meta,
		OBR.MetaComparer
	FROM ObjetivoRegistro OBR WITH(NOLOCK)
	INNER JOIN Objetivo OB WITH(NOLOCK)
	ON	OB.Id = OBR.ObjetivoId
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = OBR.ResponsibleId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = OBR.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = OBR.ModifiedBy

	WHERE
		OBR.ObjetivoId = @ObjetivoId
	AND OBR.Active = 1
	--AND	OBR.CompanyId = @CompanyId
	--AND OBR.[Date] >= StartDate
END




