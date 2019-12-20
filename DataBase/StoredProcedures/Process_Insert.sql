






CREATE PROCEDURE [dbo].[Process_Insert]
	@Id int out,
	@CompanyId int,
	@JobPositionId int,
	@Description nvarchar(150),
	@Type int,
	@Start nvarchar(2000),
	@Work nvarchar(2000),
	@End nvarchar(2000),
	@UserId int
AS
BEGIN
	INSERT INTO Proceso
	(
		CompanyId,
		CargoId,
		Type,
		Inicio,
		Desarrollo,
		Fin,
		Description,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn,
		Active
	)
	VALUES
	(
		@CompanyId,
		@JobPositionId,
		@Type,
		@Start,
		@Work,
		@End,
		@Description,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE(),
		1
	)

	SET @Id = @@IDENTITY
END






