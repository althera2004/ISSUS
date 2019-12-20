


CREATE FUNCTION [dbo].[GeneratePassword] ( @PasswordLength INT )
RETURNS VARCHAR(20)
AS
BEGIN


DECLARE @Password     VARCHAR(20)
DECLARE @ValidCharacters   VARCHAR(100)
DECLARE @PasswordIndex    INT
DECLARE @CharacterLocation   INT


SET @ValidCharacters = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890'


SET @PasswordIndex = 1
SET @Password = ''


WHILE @PasswordIndex <= @PasswordLength
BEGIN
 SELECT @CharacterLocation = ABS(CAST(CAST([NewID] AS VARBINARY) AS INT)) % 
LEN(@ValidCharacters) + 1
 FROM [dbo].[RandomNewID]


 SET @Password = @Password + SUBSTRING(@ValidCharacters, @CharacterLocation, 1)


 SET @PasswordIndex = @PasswordIndex + 1
END


RETURN @Password


END


