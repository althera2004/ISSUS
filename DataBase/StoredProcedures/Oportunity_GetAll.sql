

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Oportunity_GetAll]
	@CompanyId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		O.Id,
		O.CompanyId,
		ISNULL(O.Description,'') AS Description,
		CAST(ISNULL(O.Code,0) AS nvarchar(2)) AS Code,
		ISNULL(O.ItemDescription,'') AS ItemDescription,
		ISNULL(O.StartControl,'') AS StartControl,
		ISNULL(O.Notes,'') AS Notes,
		O.ApplyAction,
		O.DateStart,
		ISNULL(O.Causes,'') AS Causes,
		O.Cost,
		O.Impact,
		O.Result,
		O.ProcessId,
		P.Description,
		O.RuleId,
		R.Description,
		O.CreatedBy,
		CB.Login AS CreatedByLastName,
		O.CreatedOn,
		O.ModifiedBy,
		MB.Login AS ModifiedByLastName,
		O.ModifiedOn,
		O.Active


	FROM Oportunity O WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = O.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = O.ModifiedBy
	LEFT JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = O.ProcessId
	LEFT JOIN Rules R WITH(NOLOCK)
	ON	R.Id = O.RuleId

	WHERE
		O.Active = 1
	AND O.CompanyId = @CompanyId

END

