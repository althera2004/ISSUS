





CREATE PROCEDURE [dbo].[CompanyAddress_Update]
	@CompanyAddressId int,
	@CompanyId int,
	@Address nvarchar(100),
	@PostalCode nvarchar(10),
	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(15),
	@Phone nvarchar(15),
	@Mobile nvarchar(15),
	@Email nvarchar(50),
	@Fax nvarchar(15),
	@UserId int
AS
BEGIN
	UPDATE CompanyAddress SET
		Address = @Address,
		PostalCode = @PostalCode,
		City = @City,
		Province = @Province,
		Country = @Country,
		Phone = @Phone,
		Mobile = @Mobile,
		Email = @Email,
		Fax = @Fax,
		Notes = '',
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @CompanyAddressId
	AND CompanyId = @CompanyId


END






