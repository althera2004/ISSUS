
CREATE PROCEDURE [dbo].[Document_Insert]
	@DocumentId bigint out,
	@CompanyId int,
	@Description nvarchar(100),
	@CategoryId int,
	@Origen int,
	@FechaAlta datetime,
	@ProcedenciaId int,
	@Conservacion int,
	@ConservacionType int,
	@Activo bit,
	@Codigo nvarchar(25),
	@Ubicacion nvarchar(100),
	@Version int,
	@RevisionDate datetime,
	@UserId int
	
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO Document
    (
		CompanyId,
		ActualVersion,
		Description,
		CategoryId,
		FechaAlta,
		FechaBaja,
		Origen,
		ProcedenciaId,
		Conservacion,
		ConservacionType,
		Activo,
		Codigo,
		Ubicacion,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		1,
		LTRIM(RTRIM(@Description)),
		@CategoryId,
		@FechaAlta,
		null,
		@Origen,
		@ProcedenciaId,
		@Conservacion,
		@ConservacionType,
		@Activo,
		LTRIM(RTRIM(@Codigo)),
		@Ubicacion,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()		
	)
	
	SET @DocumentId = @@IDENTITY
	
	INSERT INTO DocumentsVersion
    (
		DocumentId,
		Company,
		Version,
		UserCreate,
		Status,
		Date,
		Reason
	)
    VALUES
    (
		@DocumentId,
        @CompanyId,
        @Version,
        @UserId,
        1,
        @RevisionDate,
        'Creation'
    )


END
