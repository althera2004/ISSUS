





CREATE PROCEDURE [dbo].[Process_Update]
	@Id int,
	@Description nvarchar(150),
	@CompanyId int,
	@JobPositionId int,
	@Type int,
	@Start nvarchar(2000),
	@Work nvarchar(2000),
	@End nvarchar(2000),
	@UserId int
AS
BEGIN
	UPDATE Proceso SET
		CargoId = @JobPositionId,
		Type = @Type,
		Inicio = @Start,
		Desarrollo = @Work,
		Fin = @End,
		Description = @Description,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @Id
	AND CompanyId = @CompanyId

END






