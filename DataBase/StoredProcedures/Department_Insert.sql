





CREATE PROCEDURE [dbo].[Department_Insert]
	@DepartmentId int out,
	@CompanyId int,
	@Description nvarchar(50),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Id int
	
	SELECT @Id = Id FROM Department D WITH(NOLOCK)
	WHERE
		D.CompanyId = @CompanyId
	AND D.Name = @Description
		
	IF @Id IS NOT NULL 
	BEGIN
		UPDATE Department SET
			Deleted = 0
		WHERE
			CompanyId = @CompanyId
		AND Id = @Id
		
		SET @DepartmentId = @Id
	END
	ELSE
	BEGIN
		INSERT INTO Department
		(
			CompanyId,
			Name,
			Deleted,
			CreatedBy,
			CreatedOn,
			ModifiedBy,
			ModifiedOn
		)
		VALUES
		(
			@CompanyId,
			@Description,
			0,
			@UserId,
			GETDATE(),
			@UserId,
			GETDATE()
		)
		
		SET @DepartmentId = @@IDENTITY
	END
END






