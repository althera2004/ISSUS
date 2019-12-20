





CREATE PROCEDURE [dbo].[EmployeeSkills_Insert]
	@Id int out,
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
	INSERT INTO EmployeeSkills
	(
		EmployeeId,
		CompanyId,
		Academic,
		Specific,
		WorkExperience,
		Hability,
		AcademicValid,
		SpecificValid,
		WorkExperienceValid,
		HabilityValid,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EmployeeId,
		@CompanyId,
		@Academic,
		@Specific,
		@WorkExperience,
		@Hability,
		@AcademicValid,
		@SpecificValid,
		@WorkExperienceValid,
		@HabilityValid,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @Id = @@IDENTITY
END






