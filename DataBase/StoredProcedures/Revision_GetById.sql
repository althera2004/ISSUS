



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_GetById]
	@RevisionId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	/*SELECT
		R.Id,
		R.RevisionType,
		R.RevisionMonthSubtype,
		R.PeriodeNumber,
		R.WeekDays,
		R.CreatedBy,
		CB.[Login] AS CreatedByName,
		R.CreatedOn,
		R.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		R.ModifiedOn,
		R.Active
	FROM Revision R WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = R.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = R.ModifiedBy*/



END




