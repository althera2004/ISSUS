

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RulesHistory_ByRuleId]
	@RuleId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		RH.Id,
		RH.IPR,
		RH.Reason,
		RH.CreatedBy,
		ISNULL(EMP.[Name],''),
		ISNULL(EMP.LastName,''),
		AU.Login,
		RH.CreatedOn
	FROM RulesHistory RH WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = RH.CreatedBy
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
	ON	EUA.UserId = AU.Id
	LEFT JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = EUA.EmployeeId

	WHERE
		RH.RuleId = @RuleId
	AND	RH.Active = 1

	ORDER BY RH.CreatedOn DESc
END

