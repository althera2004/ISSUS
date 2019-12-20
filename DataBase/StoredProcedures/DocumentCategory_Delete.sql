





CREATE PROCEDURE [dbo].[DocumentCategory_Delete]
	@DocumentCategoryId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM Document_Category
	WHERE
		Id = @DocumentCategoryId
	AND CompanyId = @CompanyId
END






