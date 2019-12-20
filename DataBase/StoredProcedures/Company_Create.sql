
CREATE PROCEDURE [dbo].[Company_Create]
	@CompanyId int out,
	@Login nvarchar(50) out,
	@Password nvarchar(50) out,
	@Name nvarchar(50),
	@Code nvarchar(10),
	@NIF nvarchar(15),
	@Address nvarchar(50),
	@PostalCode nvarchar(10),
	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(15),
	@Phone nvarchar(15),
	@Mobile nvarchar(15),
	@Fax nvarchar(15),
	@UserName nvarchar(50),
	@Email nvarchar(50),
	@Language nvarchar(2)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO Company
    (
		[Name],
		[SubscriptionStart],
		[SubscriptionEnd],
		[Language],
		[DefaultAddress],
		[NIF-CIF],
		[Code],
		[ModifiedBy],
		[ModifiedOn]
	)
	VALUES
	(
		@Name,
		GETDATE(),
		GETDATE() + 365,
		@language,
		null,
		@NIF,
		@Code,
		1,
		GETDATE()
	)
	
	SET @CompanyId = @@IDENTITY
	DECLARE @CompanyAddressId int
	
	INSERT INTO CompanyAddress
	(
		[CompanyId],
		[Address],
		[PostalCode],
		[City],
		[Province],
		[Country],
		[Phone],
		[Mobile],
		[Email],
		[Notes],
		[Fax],
		[Active],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn]
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
		'',
		@Fax,
		1,
		1,
		GETDATE(),
		1,
		GETDATE()
	)
	
	SET @CompanyAddressId = @@IDENTITY
	
	UPDATE Company SET DefaultAddress = @CompanyAddressId WHERE Id = @CompanyId

	INSERT INTO Document_Category ([CompanyId], [Description], [Editable]) VALUES (@CompanyId, 'Instrucciones', 0)
	INSERT INTO Document_Category ([CompanyId], [Description], [Editable]) VALUES (@CompanyId, 'Procedimientos', 0)
	INSERT INTO Document_Category ([CompanyId], [Description], [Editable]) VALUES (@CompanyId, 'Registros', 0)

	INSERT INTO Procedencia ([CompanyId], [Description], [Editable]) VALUES (@CompanyId, 'Normas', 0)
	INSERT INTO Procedencia ([CompanyId], [Description], [Editable]) VALUES (@CompanyId, 'Legislación internacional', 0)
	INSERT INTO Procedencia ([CompanyId], [Description], [Editable]) VALUES (@CompanyId, 'Legislación nacional', 0)
	INSERT INTO Procedencia ([CompanyId], [Description], [Editable]) VALUES (@CompanyId, 'Legislación autonómica', 0)
	INSERT INTO Procedencia ([CompanyId], [Description], [Editable]) VALUES (@CompanyId, 'Legislación local', 0)
		
	SET @Password = [dbo].GeneratePassword(6)

	INSERT INTO ApplicationUser
	(
		[CompanyId],
		[Login],
		[Password],
		[Status],
		[LoginFailed],
		[MustResetPassword],
		[Language],
		[ShowHelp],
		[Email],
		[PrimaryUser],
		[Admin]


	)
	VALUES
	(
		@CompanyId,
		@UserName,
		@Password,
		1,
		0,
		0,
		'ca',
		1,
		@Email,
		1,
		1
	)
	
	DECLARE @UserId int
	SET @UserId = @@IDENTITY
	
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,1,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,2,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,3,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,4,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,5,1,1,1,1,GETDATE(),1,GETDATE())
	INSERT INTO [ApplicationGrant]([UserId],[ItemId],[GrantToRead],[GrantToWrite],[GrantToDelete],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) VALUES(@UserId,6,1,1,1,1,GETDATE(),1,GETDATE())

	-- Desactivar permisos de trazas
	-- INSERT INTO [Applicat