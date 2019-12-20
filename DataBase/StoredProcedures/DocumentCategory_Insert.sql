





CREATE PROCEDURE [dbo].[DocumentCategory_Insert]
	@DocumentCategoryId int out,
	@CompanyId int,
	@Description nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO Document_Category
	(
		CompanyId,
        Description,
        Editable
    )
    VALUES    
    (
		@CompanyId,
		@Description,
		1
    ) 
    
    SET @DocumentCategoryId = @@IDENTITY
END






