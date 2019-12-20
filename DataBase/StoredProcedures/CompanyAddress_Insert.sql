





CREATE PROCEDURE [dbo].[CompanyAddress_Insert] 
	-- Add the parameters for the stored procedure here
	@CompanyAddressId int out,
	@CompanyId int,
	@Address nvarchar(100),
	@PostalCode nvarchar(10),
	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(15),
	@Phone nvarchar(15),
	@Mobile nvarchar(15),
	@Email nvarchar(50),
	@Notes text,
	@Fax nvarchar(15),
	@UserId int
AS
BEGIN
	INSERT INTO CompanyAddress
	(
		CompanyId,
		Address,
		PostalCode,
		City,
		Province,
		Country,
		Phone,
		Mobile,
		Email,
		Notes,
		Fax,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Address,
		@PostalCode,
		@City,
		@Province,
		@Country,
		@Phone,
		@Mobile,
		@Email,
		@Notes,
		@Fax,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @CompanyAddressId = @@IDENTITY
END






