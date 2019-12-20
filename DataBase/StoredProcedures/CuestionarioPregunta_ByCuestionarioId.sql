

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CuestionarioPregunta_ByCuestionarioId]
	@CompanyId int,
	@CuestionarioId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		CP.Id,
		CP.CompanyId,
		CP.Question,
		CP.CuestionarioId,
		CP.CreatedBy,
		CB.[Login],
		CP.CreatedOn,
		CP.ModifiedBy,
		MB.[Login],
		CP.ModifiedOn,
		CP.Active
	FROM CuestionarioPregunta CP WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON CB.Id = CP.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON MB.Id = CP.ModifiedBy

	WHERE
		CP.CuestionarioId = @CuestionarioId
	AND CP.CompanyId = @CompanyId
	AND CP.Active = 1
END

