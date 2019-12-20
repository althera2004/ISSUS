





CREATE PROCEDURE [dbo].[EmployeeSkills_Update] 
	@Id int,
	@EmployeeId bigint,
	@CompanyId int,
	@Academic text,
	@Specific text,
	@WorkExperience text,
	@Hability text,
	@AcademicValid bit,
	@SpecificValid bit,
	@WorkExperienceValid bit,
	@HabilityValid bit,
	@UserId int
AS
BEGIN
	UPDATE EmployeeSkills SET
		Academic = @Academic,
		AcademicValid = @AcademicValid,
		Specific = @Specific,
		SpecificValid = @SpecificValid,
		WorkExperience = @WorkExperience,
		WorkExperienceValid = @WorkExperienceValid,
		Hability = @Hability,
		HabilityValid = @HabilityValid,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		EmployeeId = @EmployeeId
	AND CompanyId = @CompanyId

END






