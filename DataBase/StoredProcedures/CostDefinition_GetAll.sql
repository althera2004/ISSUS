




CREATE PROCEDURE [dbo].[CostDefinition_GetAll]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		CD.Id,
		CD.CompanyId,
		CD.[Description],
		CD.Amount,
		CD.CreatedBy,
		CB.[Login] AS CreatedByName,
		CD.CreatedOn,
		CD.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		CD.ModifiedOn,
		CD.Active
	FROM CostDefinition CD WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = CD.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = CD.ModifiedBy

	WHERE
		CD.CompanyId = @CompanyId
END





