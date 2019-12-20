





CREATE PROCEDURE [dbo].[Employee_Update]
	@EmployeeId bigint,
	@CompanyId int,
	@Name nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Phone nvarchar(50),
	@NIF nvarchar(15),
	@Address nvarchar(50),
	@PostalCode nvarchar(10),
	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(50),
	@Notes text,
	@ModifiedBy int
AS
BEGIN
	UPDATE Employee SET
		Name = @Name,
		LastName = @LastName,
		Email = LOWER(@Email),
		Phone = @Phone,
		NIF = @NIF,
		Address = @Address,
		PostalCode = @PostalCode,
		City = @City,
		Province = @Province,
		Country = @Country,
		Notes = @Notes,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = GETDATE()
	WHERE
		Id = @EmployeeId
	AND	CompanyId = @CompanyId
END






