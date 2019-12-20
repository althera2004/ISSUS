






CREATE PROCEDURE [dbo].[Process_Desactive]
	@Id int out,
	@CompanyId int,
	@UserId int
AS
BEGIN
	UPDATE Proceso SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id
	AND	CompanyId = @CompanyId
END






