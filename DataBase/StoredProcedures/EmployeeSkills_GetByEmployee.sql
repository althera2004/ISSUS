





CREATE PROCEDURE [dbo].[EmployeeSkills_GetByEmployee]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		ES.Id,
		ES.EmployeeId,
		ES.CompanyId,
		ES.Academic,
		ES.AcademicValid,
		ES.Specific,
		ES.SpecificValid,
		ES.WorkExperience,
		ES.WorkExperienceValid,
		ES.Hability,
		ES.HabilityValid,
		ES.ModifiedOn,
		ES.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName
	FROM EmployeeSkills ES WITH(NOLOCK)
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = Es.ModifiedBy
	WHERE
		ES.EmployeeId = @EmployeeId
	AND ES.CompanyId = @CompanyId
END






