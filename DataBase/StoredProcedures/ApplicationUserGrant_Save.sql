





CREATE PROCEDURE [dbo].[ApplicationUserGrant_Save]
	@ApplicationUserId int,
	@ItemId int,
	@GrantToRead bit,
	@GrantToWrite bit,
	@GrantToDelete bit,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @TOTAL int
    
    SELECT @TOTAL = COUNT(*)
    FROM ApplicationGrant AG WITH(NOLOCK)
    WHERE
		AG.ItemId = @ItemId
	AND AG.UserId = @ApplicationUserId
	
	IF @TOTAL = 0
		BEGIN
			INSERT INTO ApplicationGrant
           ([UserId]
           ,[ItemId]
           ,[GrantToRead]
           ,[GrantToWrite]
           ,[GrantToDelete]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn])
     VALUES
           (@ApplicationUserId
           ,@ItemId
           ,@GrantToRead
           ,@GrantToWrite
           ,@GrantToDelete
           ,@UserId
           ,GETDATE()
           ,@UserId
           ,GETDATE())
		END
	ELSE
		BEGIN
		
		UPDATE ApplicationGrant SET 
		  [GrantToRead] = @GrantToRead,
		  [GrantToWrite] = @GrantToWrite,
		  [GrantToDelete] = @GrantToDelete,
		  [ModifiedBy] = @UserId,
		  [ModifiedOn] = GETDATE()
		WHERE
			UserId = @ApplicationUserId
		AND ItemId = @ItemId		
		END

	SELECT @TOTAL AS TOTAL
END






