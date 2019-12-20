





CREATE PROCEDURE [dbo].[Employee_Insert]
	@EmployeeId bigint out,
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
	@UserId int,
	@UserName nvarchar(50),
	@Password nvarchar(6) out
AS
	SET NOCOUNT ON;
	INSERT INTO Employee
	(
		CompanyId,
		Name,
		LastName,
		Email,
		Phone,
		NIF,
		Address,
		PostalCode,
		City,
		Province,
		Country,
		Notes,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn,
		Active
	)
    VALUES
    (
		@CompanyId,
		@Name,
		@LastName,
		@Email,
		@Phone,
		@NIF,
		@Address,
		@PostalCode,
		@City,
		@Province,
		@Country,
		@Notes,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE(),
		1
    )

	SET @EmployeeId = @@IDENTITY
	/*SELECT @Password = [dbo].GeneratePassword(6)
	
	INSERT INTO ApplicationUser
	(
		CompanyId,
		Login,
		Password,
		Status,
		LoginFailed,
		MustResetPassword,
		Language,
		ShowHelp
	)
	VALUES
	(
		@CompanyId,
		@UserName,
		'root',--@Password,
		1,
		0,
		1,
		'es',
		1
	)
	
	DECLARE @newUser int
	SET @newUser = @@IDENTITY
	
	INSERT INTO EmployeeUserAsignation
	(
		UserId,
		EmployeeId,
		CompanyId
	)
    VALUES
    (
		@newUser,
		@EmployeeId,
		@CompanyId
    )*/
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		8,
		@EmployeeId,
		1,
		GETDATE(),
		''
    )







