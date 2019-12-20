


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Objetivo_GetAvailable]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		O.Id,
		O.Name,
		O.[Description],
		O.Methodology,
		O.Resources,
		O.Notes,
		O.VinculatedToIndicator,
		O.IndicatorId,
		O.RevisionId,
		O.StartDate,
		O.PreviewEndDate,
		O.EndDate,
		O.ResponsibleId,
		O.ResponsibleClose,
		O.CreatedBy,
		CB.[Login] AS CreatedByName,
		O.CreatedOn,
		O.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		O.ModifiedOn,
		O.Active,
		ISNULL(R.Name,'') AS ResponsibleName,
		ISNULL(R.LastName,'') AS ResponsibleLastname,
		ISNULL(RC.Name,'') AS ResponsibleCloseName,
		ISNULL(RC.LastName,'') AS ResponsibleCloseLastame,
		'',
		0,
		ISNULL(O.EndReason,'') AS EndReason
	FROM Objetivo O WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = O.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = O.ModifiedBy
	INNER JOIN Employee R WITH(NOLOCK)
	ON	R.Id = O.ResponsibleId
	LEFT JOIN Employee RC WITH(NOLOCK)
	ON	RC.Id = O.ResponsibleClose

	/*LEFT JOIN Indicador I WITH(NOLOCK)
	ON	I.ObjetivoId = O.Id
	AND	I.Active = 1*/

	WHERE
		O.CompanyId = @CompanyId
	AND O.Active = 1
	--AND I.Id IS NULL
END



