





CREATE PROCEDURE [dbo].[DocumentCategory_Update]
	@DocumentCategoryId int,
	@CompanyId int,
	@Description nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Document_Category SET
		Description = @Description
	WHERE
		Id = @DocumentCategoryId
	AND CompanyId = @CompanyId
END






