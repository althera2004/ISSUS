





CREATE PROCEDURE [dbo].[Company_SetDefaultAddress]
	@CompanyId int,
	@AddressId int,
	@UserId int	
AS
BEGIN
	UPDATE Company SET
		DefaultAddress = @AddressId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @CompanyId
END






