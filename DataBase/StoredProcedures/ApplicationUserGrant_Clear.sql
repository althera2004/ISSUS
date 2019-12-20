





CREATE PROCEDURE [dbo].[ApplicationUserGrant_Clear]
	@ApplicationUserId int
AS
BEGIN
	SET NOCOUNT ON;

    
    DELETE FROM ApplicationGrant WHERE UserId = @ApplicationUserId
END






