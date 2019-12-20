





CREATE PROCEDURE [dbo].[Company_DeleteAddress]
	@CompanyId int,
	@AddressId int,
	@UserId int	
AS
BEGIN
	UPDATE CompanyAddress SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		CompanyId = @CompanyId
	AND Id = @AddressId
END






