



CREATE PROCEDURE [dbo].[DocumentAttach_GetByDoumentId]
	@DocumentId bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		DA.Id,
		DA.CompanyId,
		DA.DocumentId,
		DA.[Description],
		DA.Extension,
		DA.[Version],
		DA.CreatedBy,
		CB.[Login] AS CreatedByName,
		DA.CreatedOn,
		DA.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		DA.ModifiedOn,
		DA.Active
	FROM DocumentAttach DA WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = DA.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = DA.ModifiedBy
	WHERE
		DA.CompanyId = @CompanyId
	AND DA.DocumentId = @DocumentId
END



