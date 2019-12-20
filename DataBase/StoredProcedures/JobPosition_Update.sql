





CREATE PROCEDURE [dbo].[JobPosition_Update]
	@JobPositionId bigint,
	@CompanyId int,
    @DepartmentId int,
    @ResponsableId bigint,
    @Description nvarchar(100),
    @Responsabilidades nvarchar(2000),
    @Notas nvarchar(2000),
    @FormacionAcademicaDeseada nvarchar(2000),
    @FormacionEspecificaDesdeada nvarchar(2000),
    @ExperienciaLaboralDeseada nvarchar(2000),
    @HabilidadesDeseadas nvarchar(2000),
    @ModifiedBy int
AS
BEGIN
	UPDATE Cargos SET 
		DepartmentId = @DepartmentId,
		ResponsableId = @ResponsableId,
		Description = @Description,
		Responsabilidades = @Responsabilidades,
		Notas = @Notas,
		FormacionAcademicaDeseada = @FormacionAcademicaDeseada,
		FormacionEspecificaDesdeada = @FormacionEspecificaDesdeada,
		ExperienciaLaboralDeseada = @ExperienciaLaboralDeseada,
		HabilidadesDeseadas = @HabilidadesDeseadas,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = GETDATE()
	WHERE
		Id = @JobPositionId
	AND CompanyId = @CompanyId


END






