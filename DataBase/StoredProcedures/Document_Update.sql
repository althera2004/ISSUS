
CREATE PROCEDURE [dbo].[Document_Update]
	@DocumentId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@CategoryId int,
	@FechaAlta date,
	--@FechaBaja date,
	@Origen int,
	@ProcedenciaId int,
	@Conservacion int,
	@ConservacionType int,
	@Activo bit,
	@Codigo nvarchar(25),
	@Ubicacion nvarchar(100),
	@UserId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Document SET
		Description = @Description,
		CategoryId = @CategoryId,
		FechaAlta = @Fechaalta,
		--FechaBaja = @FechaBaja,
		Origen = @Origen,
		ProcedenciaId = @ProcedenciaId,
		Conservacion = @Conservacion,
		ConservacionType = @ConservacionType,
		Activo = 1,
		Codigo = @Codigo,
		Ubicacion = @Ubicacion,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE	
		Id = @DocumentId
	AND CompanyId = @CompanyId
END
