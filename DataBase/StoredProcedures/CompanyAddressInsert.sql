





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CompanyAddressInsert]
	@CompanyId int,
	@Address nvarchar(100),
	@PostalCode nvarchar(10),
	@City nvarchar(50),
	@Province nvarchar(50),
	@Country nvarchar(15),
	@Phone nvarchar(15),
	@Mobile nvarchar(15),
	@Fax nvarchar(15),
	@Email nvarchar(50),
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
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
		Fax
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
		@Fax
	)
     
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
		6,
		@@IDENTITY,
		1,
		GETDATE(),
		''
	)
		
END






