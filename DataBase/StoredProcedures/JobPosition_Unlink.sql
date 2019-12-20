




CREATE PROCEDURE [dbo].[JobPosition_Unlink] 
	@EmployeeId int,
	@JobPositionId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM EmployeeCargoAsignation
	WHERE
		EmployeeId = @EmployeeId
	AND CargoId = @JobPositionId
END





