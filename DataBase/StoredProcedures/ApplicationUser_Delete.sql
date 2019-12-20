





CREATE PROCEDURE [dbo].[ApplicationUser_Delete]
	@UserItemId bigint,
	@CompanyId int,
	@UserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ApplicationUser SET
		Status = 0
	WHERE
		Id = @UserItemId
	AND CompanyId = @CompanyId

								   
	  
					  
						   
		
END






