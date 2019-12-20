

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IncidentActionZombie_ByAuditoryId]
	@CompanyId int,
	@AuditoryId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		Z.Id,
		Z.CompanyId,
		Z.AuditoryId,
		Z.ActionType,
		Z.Description,
		Z.WhatHappend,
		Z.WhatHappendBy,
		ISNULL(E.Name,'') AS WhatHappendByName,
		ISNULL(E.LastName,'') AS WhatHappendByLastName,
		Z.WhatHappendOn
	FROM IncidentActionZombie Z WITH(NOLOCK)
	INNER JOIN Employee E WITH(NOLOCK)
	ON	E.Id = Z.WhatHappendBy

	WHERE
		Z.AuditoryId = @AuditoryId
	AND Z.CompanyId = @CompanyId

END

