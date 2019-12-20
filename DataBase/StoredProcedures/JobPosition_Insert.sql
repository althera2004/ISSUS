





CREATE PROCEDURE [dbo].[JobPosition_Insert]
	@JobPositionId bigint out,
	@CompanyId int,
	@DepartmentId int,
	@ResponsableId bigint,
	@Description nvarchar(100),
	@Responsabilidades nvarchar(2000),
	@Notas nvarchar(2000),
	@FormacionAcademicaDeseada nvarchar(2000),
	@FormacionEspecificaDeseada nvarchar(2000),
	@ExperienciaLaboralDeseada nvarchar(2000),
	@HabilidadesDeseadas nvarchar(2000),
	@UserId int
AS
BEGIN
	INSERT INTO Cargos
	(
		CompanyId,
		DepartmentId,
		ResponsableId,
		Description,
		Responsabilidades,
		Notas,
		FormacionAcademicaDeseada,
		FormacionEspecificaDesdeada,
		ExperienciaLaboralDeseada,
		HabilidadesDeseadas,
		Active,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@DepartmentId,
		@ResponsableId,
		@Description,
		@Responsabilidades,
		@Notas,
		@FormacionAcademicaDeseada,
		@FormacionEspecificaDeseada,
		@ExperienciaLaboralDeseada,
		@HabilidadesDeseadas,
		1,
		@UserId,
		GETDATE()
	)
	
	SET @JobPositionId = @@IDENTITY

END






