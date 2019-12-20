
CREATE PROCEDURE [dbo].[Company_Update]
	@CompanyId int,
	@Name nvarchar(50),
	@Nif nvarchar(15),
	@DefaultAddress int,
	@Language nvarchar(2),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Data nvarchar(200)
	
	SELECT 
		@Data = [Name] + '->' + @Name +',' +
		ISNULL([NIF-CIF],'')+ '->' + @Nif + ',' +
		CAST(ISNULL(DefaultAddress,0) AS Nvarchar(6)) + '->' + CAST(@DefaultAddress as nvarchar(6)) +
		CAST(ISNULL([Language],0) AS Nvarchar(2)) + '->' + @Language
	FROM Company WITH(NOLOCK)
	WHERE
		Id = @CompanyId

    UPDATE Company SET
		[NIF-CIF] = @Nif,
		[Name] = @Name,
		DefaultAddress = @DefaultAddress,
		[Language] = @Language,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @CompanyId
		
		
	UPDATE CompanyAddress SET
		@DefaultAddress = 0
	WHERE CompanyId = @CompanyId
		
	UPDATE CompanyAddress SET
		@DefaultAddress = 1
	WHERE 
		Id = @DefaultAddress
	AND	CompanyId = @CompanyId
		
	INSERT INTO ActivityLog
    (
		ActivityId,
		UserId,
		CompanyId,
		TargetType,
		TargetId,
		ActionId,
		[DateTime],
		ExtraData
	)
	VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		1,
		@CompanyId,
		1,
		GETDATE(),
		@Data
	)


END
